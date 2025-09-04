using Toggle.Net.Internal;

namespace Toggle.Net.Providers;

/// <summary>
/// Gets features settings from the source
/// </summary>
public interface IFeatureProvider
{
	/// <summary>
	/// Gets the feature from the repository.
	/// If not present, this method must return <code>null</code>.
	/// </summary>
	/// <param name="toggleName"></param>
	/// <returns><see cref="Feature"/></returns>
	Feature Get(string toggleName);
}