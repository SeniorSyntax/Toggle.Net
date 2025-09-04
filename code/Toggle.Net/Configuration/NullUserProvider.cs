namespace Toggle.Net.Configuration;

public class NullUserProvider : IUserProvider
{
	public string CurrentUser()
	{
		return string.Empty;
	}
}