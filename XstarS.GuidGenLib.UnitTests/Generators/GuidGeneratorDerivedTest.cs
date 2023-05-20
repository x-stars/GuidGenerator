using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public class GuidGeneratorDerivedTest
{
    [TestMethod]
    public void FillVersionField_AllPossibleVersion_GetInputVersion()
    {
        var versions = Enum.GetValues(typeof(GuidVersion));
        foreach (var version in (GuidVersion[])versions)
        {
            var guidGen = new TestGuidGenerator(version: version);
            var guid = guidGen.NewGuid();
            var guidVersion = guid.GetVersion();
            Assert.AreEqual(version, guidVersion);
        }
    }

    [TestMethod]
    public void FillVariantField_AllPossibleVariant_GetInputVariant()
    {
        var variants = Enum.GetValues(typeof(GuidVariant));
        foreach (var variant in (GuidVariant[])variants)
        {
            var guidGen = new TestGuidGenerator(variant: variant);
            var guid = guidGen.NewGuid();
            var guidVariant = guid.GetVariant();
            Assert.AreEqual(variant, guidVariant);
        }
    }

    private sealed class TestGuidGenerator : GuidGenerator
    {
        public TestGuidGenerator(
            GuidVersion version = default,
            GuidVariant variant = default)
        {
            this.Version = version;
            this.Variant = variant;
        }

        public override GuidVersion Version { get; }

        public override GuidVariant Variant { get; }

        public override Guid NewGuid()
        {
            var guid = Guid.Empty;
            this.FillVersionField(ref guid);
            this.FillVariantField(ref guid);
            return guid;
        }
    }
}
