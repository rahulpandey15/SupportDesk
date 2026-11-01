using Microsoft.EntityFrameworkCore;
using SupportDesk.Data.Entities;

namespace SupportDesk.Data.Store;
public interface IOrderStore
{ 
    Task<List<Order>> GetRecentOrderByCustomerAsync(int customerId, int count, CancellationToken ct);

    public Task<Order?> GetAsync(int customerId, int orderId, CancellationToken ct);
}


public class OrderStore : IOrderStore
{
    private readonly SupportDeskDbContext _dbContext;

    public OrderStore(SupportDeskDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<List<Order>> GetRecentOrderByCustomerAsync(
            int customerId, int count, CancellationToken ct)
    {
        var orders =  await _dbContext.Orders.Where(
                            x => x.CustomerId == customerId)
                .OrderByDescending(x => x.OrderDate)
                .Take(count).ToListAsync(ct);


        return orders;
    }

    public async Task<Order?> GetAsync(int customerId, int orderId, CancellationToken ct)
    {
        var orders =  await _dbContext.Orders.Where(
                x => x.CustomerId == customerId && x.OrderId == orderId)
            .FirstOrDefaultAsync(ct);
        
        return orders;
    }
}