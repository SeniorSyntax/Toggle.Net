using System;

namespace Toggle.Net.Providers.InCode;

public class NotExistingToggleException(string toggleName) : Exception($"Toggle {toggleName} doesn't exist!");