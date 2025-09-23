using System;
using NUnit.Framework;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;

namespace Toggle.Net.Test.TextFile;

public class UniqueSpecificationNameTest
{
	[NUnit.Framework.Test]
	public void ShouldThrowIfAddingSpecificationsWithSameName()
	{
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("double", new BoolSpecification(true));
		Assert.Throws<ArgumentException>(() => 
			mappings.AddMapping("double", new BoolSpecification(true)));
	}

	[NUnit.Framework.Test]
	public void ShouldThrowIfAddingSpecificationsWithSameNameAsDefaultOne()
	{
		var mappings = new DefaultSpecificationMappings();
		Assert.Throws<ArgumentException>(() =>
			mappings.AddMapping("false", new BoolSpecification(true)));
	}
	
	[NUnit.Framework.Test]
	public void ShouldThrowIfAddingMultipleSpecificationDifferOnlyInCasing()
	{
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("DOUBle", new BoolSpecification(true));
		Assert.Throws<ArgumentException>(() =>
			mappings.AddMapping("double", new BoolSpecification(true)));
	}
}