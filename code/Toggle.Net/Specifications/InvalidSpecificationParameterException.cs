using System;

namespace Toggle.Net.Specifications;

public class InvalidSpecificationParameterException : Exception
{
	public InvalidSpecificationParameterException(string message) : base(message)
	{
	}
}