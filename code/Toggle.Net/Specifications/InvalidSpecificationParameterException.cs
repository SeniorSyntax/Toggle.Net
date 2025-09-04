using System;

namespace Toggle.Net.Specifications;

public class InvalidSpecificationParameterException(string message) : Exception(message);