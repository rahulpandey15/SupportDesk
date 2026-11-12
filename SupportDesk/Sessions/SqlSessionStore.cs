using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SupportDesk.Data;
using SupportDesk.Data.Entities;

namespace SupportDesk.Sessions;


public readonly record struct SessionKey(int CustomerId,string ConversationId)
{
     public string Value => $"{CustomerId}:{ConversationId}";
     }


public interface ISessionStore
{
    Task<JsonElement?> GetAsync(SessionKey key, CancellationToken ct);
    Task SaveAsync(SessionKey key, JsonElement state, CancellationToken ct);
}


public sealed class SqlSessionStore(SupportDeskDbContext db) : ISessionStore
{
    public async Task<JsonElement?> GetAsync(SessionKey key, CancellationToken ct)
    {
        var json = await db.AgentSessions
            .AsNoTracking()
            .Where(s => s.SessionKey == key.Value && s.CustomerId == key.CustomerId)
            .Select(s => s.StateJson)
            .FirstOrDefaultAsync(ct);

        if (json is null)
        {
            return null;
        }

        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone(); // Clone: the element must outlive the document
    }

    public async Task SaveAsync(SessionKey key, JsonElement state, CancellationToken ct)
    {
        var json = state.GetRawText();

        // Existing conversation (the common case): update without loading the old JSON.
        if (await TryUpdateAsync(key, json, ct))
        {
            return;
        }

        var now = DateTime.UtcNow;
        db.AgentSessions.Add(new AgentSession
        {
            SessionKey = key.Value,
            CustomerId = key.CustomerId,
            AgentName = "SupportDeskAgent", // need to change at later stage
            ConversationId = key.ConversationId,
            StateJson = json,
            CreatedAt = now,
            UpdatedAt = now,
        });

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            db.ChangeTracker.Clear();
            if (!await TryUpdateAsync(key, json, ct))
            {
                throw;
            }
        }
    }

    private async Task<bool> TryUpdateAsync(SessionKey key, string json, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var rows = await db.AgentSessions
            .Where(s => s.SessionKey == key.Value && s.CustomerId == key.CustomerId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(s => s.StateJson, json)
                .SetProperty(s => s.UpdatedAt, now), ct);
        return rows > 0;
    }
}