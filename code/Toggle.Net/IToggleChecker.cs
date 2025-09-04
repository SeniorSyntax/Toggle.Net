namespace Toggle.Net;

/// <summary>
/// Main interface at runtime.
/// </summary>
public interface IToggleChecker
{
	/// <summary>
	/// Returns <code>true</code> if feature with specified toggle name is enabled,
	/// otherwise <code>false</code>.
	/// </summary>
	/// <param name="toggleName">Name of the toggle</param>
	bool IsEnabled(string toggleName);
}