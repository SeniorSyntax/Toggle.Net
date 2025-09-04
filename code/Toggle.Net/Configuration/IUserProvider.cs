namespace Toggle.Net.Configuration;

/// <summary>
/// Returns current user as a string.
/// </summary>
public interface IUserProvider
{
	string CurrentUser();
}