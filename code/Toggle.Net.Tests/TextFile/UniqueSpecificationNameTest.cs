using System;
using NUnit.Framework;
using Toggle.Net.Providers.TextFile;
using Toggle.Net.Specifications;

namespace Toggle.Net.Tests.TextFile;

public class UniqueSpecificationNameTest
{
	[Test]
	public void ShouldThrowIfAddingSpecificationsWithSameName()
	{
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("double", new BoolSpecification(true));
		Assert.Throws<ArgumentException>(() => 
			mappings.AddMapping("double", new BoolSpecification(true)));
	}

	[Test]
	public void ShouldThrowIfAddingSpecificationsWithSameNameAsDefaultOne()
	{
		var mappings = new DefaultSpecificationMappings();
		Assert.Throws<ArgumentException>(() =>
			mappings.AddMapping("false", new BoolSpecification(true)));
	}
	
	[Test]
	public void ShouldThrowIfAddingMultipleSpecificationDifferOnlyInCasing()
	{
		var mappings = new DefaultSpecificationMappings();
		mappings.AddMapping("DOUBle", new BoolSpecification(true));
		Assert.Throws<ArgumentException>(() =>
			mappings.AddMapping("double", new BoolSpecification(true)));
	}
}