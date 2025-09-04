using Toggle.Net.Configuration;

namespace Toggle.Net.Tests.Stubs;

public class UserProviderStub(string currentUser) : IUserProvider
{
	public string CurrentUser()
	{
		return currentUser;
	}
}