using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toggle.Net.Internal;
using Toggle.Net.Specifications;

namespace Toggle.Net.Providers.TextFile;

/// <summary>
/// Reads features and their <see cref="IToggleSpecification"/> from a text file.
/// 
/// Format
/// [feature]=[specification]
/// [feature].[specification].[param]=[value]
/// 
/// It's also possible to use shorter syntax for specifications with one parameter
/// [feature].[specification].[param]=[value]
/// 
/// To remark, use "#" sign.
/// 
/// <example>
/// # This is an example
/// 
/// NewFeature = false
/// Logon = true
/// TheThing = myspecification
/// TheThing.myspecification.MyParam = 13
/// TheThing.myspecification.MyOtherParam = 13
/// </example>
/// </summary>
public class FileParser(IFileReader fileReader, ISpecificationMappings specificationMappings)
	: IFeatureProviderFactory
{
	public const string MustContainEqualSign = "Missing equal sign at line {0}.";
	public const string MustOnlyContainOneEqualSign = "More than one equal sign at line {0}.";
	public const string MustHaveValidSpecification = "Unknown specification '{0}' at line {1}.";
	public const string MustHaveTwoDotsIfParameterUse =
		"Wrong parameter usage at line {0}. Use format [feature].[specification].[parametername] = [parametervalue].";
	public const string MustOnlyContainSameParameterOnce = "Parameter '{0}' declared twice at line {1}.";
	public const string MustOnlyBeDeclaredOnce = "Feature '{0}' is declared twice at line {1}. This is not allowed when you've set ThrowIfFeatureIsDeclaredTwice to true.";
	public const string NotAllowedFeature = "Feature '{0}' is not in AllowedFeatures collection.";

	public bool ThrowIfFeatureIsDeclaredTwice { get; set; }

	public IEnumerable<string> AllowedFeatures { get; set; }

	public IFeatureProvider Create()
	{
		var exOutput = new StringBuilder();
		var featureSettings = parseFile(exOutput);
		foreach (var feature in featureSettings)
		{
			try
			{
				feature.Value.Validate(feature.Key);
			}
			catch (InvalidSpecificationParameterException ex)
			{
				exOutput.AppendLine(ex.Message);
			}
		}
		if (exOutput.Length > 0)
			throw new IncorrectTextFileException(exOutput.ToString());
		return new StaticFeatureProvider(featureSettings);
	}

	private IDictionary<string, Feature> parseFile(StringBuilder exOutput)
	{
		var readFeatures = new Dictionary<string, Feature>(StringComparer.OrdinalIgnoreCase);
		var content = fileReader.Content();
		for (var index = 0; index < content.Length; index++)
		{
			var row = content[index];
			var rowNumber = index + 1;
			var indexOfCommentSign = row.IndexOf("#", StringComparison.OrdinalIgnoreCase);
			var rowWithoutComments = indexOfCommentSign > -1 ? row.Remove(indexOfCommentSign) : row;
			var splitByEqualSign = rowWithoutComments.Split('=');
			var leftOfEqualSign = splitByEqualSign[0].Trim();
			if (leftOfEqualSign == string.Empty)
				continue;
			switch (splitByEqualSign.Length)
			{
				case 1:
					exOutput.AppendLine(string.Format(MustContainEqualSign, rowNumber));
					break;
				case 2:
					var rightOfEqualSign = splitByEqualSign[1].Trim();
					parseRow(readFeatures, leftOfEqualSign, rightOfEqualSign, rowNumber, exOutput);
					break;
				default:
					exOutput.AppendLine(string.Format(MustOnlyContainOneEqualSign, rowNumber));
					break;
			}
		}
		return readFeatures;
	}

	private void parseRow(IDictionary<string, Feature> readFeatures,
		string leftOfEqualSign, 
		string rightOfEqualSign, 
		int rowNumber, 
		StringBuilder exOutput)
	{
		var splitLeftByDots = leftOfEqualSign.Split('.');
		string toggleName;
		string specificationName;

		switch (splitLeftByDots.Length)
		{
			case 1:
				toggleName = leftOfEqualSign;
				specificationName = rightOfEqualSign;

				addSpecificationToFeature(readFeatures, rowNumber, exOutput, specificationName, toggleName);
				break;
			case 3:
				toggleName = splitLeftByDots[0];
				specificationName = splitLeftByDots[1];
				var paramName = splitLeftByDots[2].Trim();
				var paramValue = rightOfEqualSign;
				try
				{
					if (!readFeatures.TryGetValue(toggleName, out var feature))
					{
						feature = addSpecificationToFeature(readFeatures, rowNumber, exOutput, specificationName, toggleName);
					}
					feature?.AddParameter(specificationMappings.GetSpecification(specificationName), paramName, paramValue);
				}
				catch (ArgumentException)
				{
					exOutput.AppendLine(string.Format(MustOnlyContainSameParameterOnce, paramName, rowNumber));
				}
				break;
			default:
				exOutput.AppendLine(string.Format(MustHaveTwoDotsIfParameterUse, rowNumber));
				break;
		}
	}

	private Feature addSpecificationToFeature(IDictionary<string, Feature> readFeatures, int rowNumber,
		StringBuilder exOutput, string specificationName, string toggleName)
	{
		makeSureToggleNameIsAllowed(exOutput, toggleName);
		Feature feature = null;
		var foundSpecification = specificationMappings.GetSpecification(specificationName);
		if(foundSpecification != null)
		{
			if (readFeatures.TryGetValue(toggleName, out feature))
			{
				if (ThrowIfFeatureIsDeclaredTwice)
				{
					exOutput.AppendLine(string.Format(MustOnlyBeDeclaredOnce, toggleName, rowNumber));
				}
				else
				{
					feature.AddSpecification(foundSpecification);
				}
			}
			else
			{
				feature = new Feature(foundSpecification);
				readFeatures.Add(toggleName, feature);
			}
		}
		else
		{
			exOutput.AppendLine(string.Format(MustHaveValidSpecification, specificationName, rowNumber));
		}
		return feature;
	}

	private void makeSureToggleNameIsAllowed(StringBuilder exOutput, string toggleName)
	{
		if (AllowedFeatures!=null && !AllowedFeatures.Any(x => string.Equals(x.Trim(), toggleName, StringComparison.CurrentCultureIgnoreCase)))
		{
			exOutput.AppendLine(string.Format(NotAllowedFeature, toggleName));
		}
	}
}