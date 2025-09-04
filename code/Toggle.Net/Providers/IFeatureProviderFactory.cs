namespace Toggle.Net.Providers;

/// <summary>
/// Creates an <see cref="IFeatureProvider"/>.
/// </summary>
public interface IFeatureProviderFactory
{
	IFeatureProvider Create();
}