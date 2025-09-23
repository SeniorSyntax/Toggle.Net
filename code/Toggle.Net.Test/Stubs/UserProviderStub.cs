using Toggle.Net.Configuration;

namespace Toggle.Net.Test.Stubs;

public class UserProviderStub(string currentUser) : IUserProvider
{
	public string CurrentUser() => currentUser;
}