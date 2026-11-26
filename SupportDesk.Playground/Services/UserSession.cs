namespace SupportDesk.Playground.Services;

public sealed class UserSession
{
	public event Action? Changed;

	public string UserName { get; private set; } = string.Empty;
	public string Role { get; private set; } = string.Empty;

	public bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserName);
	public bool IsAdmin => Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

	public void SignIn(string userName)
	{
		UserName = userName;
		Role = userName.Equals("admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
		Changed?.Invoke();
	}

	public void SignOut()
	{
		UserName = string.Empty;
		Role = string.Empty;
		Changed?.Invoke();
	}
}
