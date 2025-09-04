using System;

namespace Toggle.Net.Providers.TextFile;

public class IncorrectTextFileException : Exception
{
	public IncorrectTextFileException(string message) :base(message)
	{
	}
}