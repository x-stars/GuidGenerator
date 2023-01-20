using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids.Generators;

[TestClass]
public class GuidGeneratorDerivedTest
{
    private sealed class TestGuidGenerator : GuidGenerator
    {
        public TestGuidGenerator(GuidVersion version, GuidVariant variant)
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

    [TestMethod]
    public void FillVersionField_AllPossibleVersion_GetInputVersion()
    {
        foreach (var verNum in ..(int)GuidVersion.Version5)
        {
            var version = (GuidVersion)verNum;
            var guidGen = new TestGuidGenerator(version, default);
            var guid = guidGen.NewGuid();
            var guidVersion = guid.GetVersion();
            Assert.AreEqual(version, guidVersion);
        }
    }

    [TestMethod]
    public void FillVariantField_AllPossibleVariant_GetInputVariant()
    {
        foreach (var varNum in ..(int)GuidVariant.Reserved)
        {
            var variant = (GuidVariant)varNum;
            var guidGen = new TestGuidGenerator(default, variant);
            var guid = guidGen.NewGuid();
            var guidVariant = guid.GetVariant();
            Assert.AreEqual(variant, guidVariant);
        }
    }
}
