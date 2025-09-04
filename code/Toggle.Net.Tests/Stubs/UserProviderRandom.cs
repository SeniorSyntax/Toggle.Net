using System;
using Toggle.Net.Configuration;

namespace Toggle.Net.Tests.Stubs;

public class UserProviderRandom : IUserProvider
{
	private readonly Random random = new();

	public string CurrentUser()
	{
		return random.Next(1000).ToString();
	}
}