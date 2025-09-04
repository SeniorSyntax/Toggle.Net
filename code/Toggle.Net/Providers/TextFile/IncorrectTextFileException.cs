using System;

namespace Toggle.Net.Providers.TextFile;

public class IncorrectTextFileException(string message) : Exception(message);