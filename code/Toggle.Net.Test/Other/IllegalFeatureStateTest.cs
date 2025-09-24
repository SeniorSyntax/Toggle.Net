using System;
using NUnit.Framework;
using Toggle.Net.Providers;
using Toggle.Net.Specifications;

namespace Toggle.Net.Test.Other;

public class IllegalFeatureStateTest
{
	[Test]
	public void ShouldNotAcceptNullAsFeature()
	{
		Assert.Throws<ArgumentNullException>(() => 
			new Feature(null)
		);
	}

	[Test]
	public void ShouldNotAcceptNullWhenAddingFeature()
	{
		var feature = new Feature(new BoolSpecification(false));
		Assert.Throws<ArgumentNullException>(() =>
			feature.AddSpecification(null)
		);
	}
}