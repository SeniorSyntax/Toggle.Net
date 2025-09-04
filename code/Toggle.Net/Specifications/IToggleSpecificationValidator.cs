using System.Collections.Generic;
using Toggle.Net.Internal;

namespace Toggle.Net.Specifications;

/// <summary>
/// Validates that an implementation of <see cref="IToggleSpecification"/> is correct
/// </summary>
public interface IToggleSpecificationValidator
{
    /// <summary>
    /// Called at startup with parameters for every <see cref="Feature"/> using this specification.
    /// If not valid, throw a <see cref="InvalidSpecificationParameterException"/>
    /// with suitable message.
    /// </summary>
    /// <param name="toggleName"></param>
    /// <param name="parameters"></param>
    void Validate(string toggleName, IDictionary<string, string> parameters);
}