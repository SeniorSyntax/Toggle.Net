using System.Collections.Generic;

namespace Toggle.Net.Autofac.Test;

public class ToggleState
{
    private readonly Dictionary<string, bool> _toggleStates = new();
	
    public bool IsEnabled(string toggleName) => _toggleStates[toggleName];

    public void Set(string theToggle, bool toggleState) => 
        _toggleStates[theToggle] = toggleState;
}