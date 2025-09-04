namespace Toggle.Net.Configuration;

public class NullUserProvider : IUserProvider
{
	public string CurrentUser() => 
		string.Empty;
}