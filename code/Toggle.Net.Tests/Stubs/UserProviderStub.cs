using Toggle.Net.Configuration;

namespace Toggle.Net.Tests.Stubs;

public class UserProviderStub : IUserProvider
{
	private readonly string _currentUser;

	public UserProviderStub(string currentUser)
	{
		_currentUser = currentUser;
	}

	public string CurrentUser()
	{
		return _currentUser;
	}
}