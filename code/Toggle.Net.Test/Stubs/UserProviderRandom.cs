using System;
using Toggle.Net.Configuration;

namespace Toggle.Net.Test.Stubs;

public class UserProviderRandom : IUserProvider
{
	private readonly Random _random = new();

	public string CurrentUser() => 
		_random.Next(1000).ToString();
}