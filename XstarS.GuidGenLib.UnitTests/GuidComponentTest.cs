﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Guids;

[TestClass]
public partial class GuidComponentTest
{
    [TestMethod]
    public void Deconstruct_GuidByFields_GetInputFields()
    {
        var guid = new Guid(0x00112233, 0x4455, 0x6677,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF);
        var (a, b, c, d, e, f, g, h, i, j, k) = guid;
        Assert.AreEqual(0x00112233, a);
        Assert.AreEqual(0x4455, b); Assert.AreEqual(0x6677, c);
        Assert.AreEqual(0x88, d); Assert.AreEqual(0x99, e);
        Assert.AreEqual(0xAA, f); Assert.AreEqual(0xBB, g);
        Assert.AreEqual(0xCC, h); Assert.AreEqual(0xDD, i);
        Assert.AreEqual(0xEE, j); Assert.AreEqual(0xFF, k);
    }

    [TestMethod]
    public void Deconstruct_GuidByFieldsAndArray_GetInputFieldsAndArray()
    {
        var lowerBytes =
            new byte[] { 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        var guid = new Guid(0x00112233, 0x4455, 0x6677, lowerBytes);
        var (a, b, c, guidLowerBytes) = guid;
        Assert.AreEqual(0x00112233, a);
        Assert.AreEqual(0x4455, b); Assert.AreEqual(0x6677, c);
        CollectionAssert.AreEqual(lowerBytes, guidLowerBytes);
    }

    [TestMethod]
    public void GetVersion_KnownVersionGuids_GetExpectedGuidVersion()
    {
        foreach (var (version, guidText) in new Dictionary<GuidVersion, string>()
        {
            [GuidVersion.Empty] = "00000000-0000-0000-0000-000000000000",
            [GuidVersion.Version1] = "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            [GuidVersion.Version2] = "6ba7b810-9dad-21d1-b402-00c04fd430c8",
            [GuidVersion.Version3] = "a9ec4420-7252-3c11-ab70-512e10273537",
            [GuidVersion.Version4] = "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            [GuidVersion.Version5] = "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            [GuidVersion.Version6] = "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            [GuidVersion.Version7] = "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            [GuidVersion.Version8] = "05db6c94-bba6-8702-88aa-548f4d6cd700",
            [GuidVersion.MaxValue] = "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var guid = Guid.Parse(guidText);
            Assert.AreEqual(version, guid.GetVersion());
        }
    }

    [TestMethod]
    public void GetVersion_UnknownVersionGuids_GetExpectedVersionNumber()
    {
        foreach (var (version, guidText) in new Dictionary<GuidVersion, string>()
        {
#if UUIDREV_DISABLE
            [(GuidVersion)0x6] = "00000000-0000-6000-bfff-ffffffffffff",
            [(GuidVersion)0x7] = "00000000-0000-7000-bfff-ffffffffffff",
            [(GuidVersion)0x8] = "00000000-0000-8000-bfff-ffffffffffff",
#endif
            [(GuidVersion)0x9] = "00000000-0000-9000-bfff-ffffffffffff",
            [(GuidVersion)0xA] = "00000000-0000-a000-bfff-ffffffffffff",
            [(GuidVersion)0xB] = "00000000-0000-b000-bfff-ffffffffffff",
            [(GuidVersion)0xC] = "00000000-0000-c000-bfff-ffffffffffff",
            [(GuidVersion)0xD] = "00000000-0000-d000-bfff-ffffffffffff",
            [(GuidVersion)0xE] = "00000000-0000-e000-bfff-ffffffffffff",
#if UUIDREV_DISABLE
            [(GuidVersion)0xF] = "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var guid = Guid.Parse(guidText);
            Assert.AreEqual(version, guid.GetVersion());
        }
    }

    [TestMethod]
    public void GetVariant_Version5Guid_GetGuidRfc4122Variant()
    {
        var guid = Guid.Parse("768a7b1b-ae51-5c0a-bc9d-a85a343f2c24");
        Assert.AreEqual(GuidVariant.Rfc4122, guid.GetVariant());
#if !UUIDREV_DISABLE
        Assert.AreEqual(GuidVariant.Rfc9562, guid.GetVariant());
#endif
        Assert.AreEqual(GuidVariant.OsfDce, guid.GetVariant());
    }

    [TestMethod]
    public void GetVariant_ComInterfaceGuid_GetGuidMicrosoftVariant()
    {
        var guid = Guid.Parse("00000001-0000-0000-C000-000000000046");
        Assert.AreEqual(GuidVariant.Microsoft, guid.GetVariant());
    }

    [TestMethod]
    public void GetVariant_SpecialGuids_GetExpectedGuidVariant()
    {
        Assert.AreEqual(GuidVariant.Ncs, Guid.Empty.GetVariant());
#if !UUIDREV_DISABLE
        Assert.AreEqual(GuidVariant.Reserved, Uuid.MaxValue.GetVariant());
#endif
    }

    [TestMethod]
    public void TryGetDomainAndLocalId_Version2Guid_GetExpectedDoaminAndLocalId()
    {
        var guid = Guid.Parse("6ba7b810-9dad-21d1-b402-00c04fd430c8");
        var result = guid.TryGetDomainAndLocalId(out var domain, out var localId);
        Assert.IsTrue(result);
        Assert.AreEqual(DceSecurityDomain.Org, domain);
        Assert.AreEqual(0x6ba7b810, localId);
    }

    [TestMethod]
    public void TryGetDomainAndLocalId_OtherVersionGuids_GetAllFalseResults()
    {
        foreach (var guidText in new[]
        {
            "00000000-0000-0000-0000-000000000000",
            "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            "a9ec4420-7252-3c11-ab70-512e10273537",
            "2502f1d5-c2a9-47d3-b6d8-d7670094ace2",
            "768a7b1b-ae51-5c0a-bc9d-a85a343f2c24",
#if !UUIDREV_DISABLE
            "1d19dad6-ba7b-6810-80b4-00c04fd430c8",
            "017f22e2-79b0-7cc3-98c4-dc0c0c07398f",
            "05db6c94-bba6-8702-88aa-548f4d6cd700",
            "ffffffff-ffff-ffff-ffff-ffffffffffff",
#endif
        })
        {
            var guid = Guid.Parse(guidText);
            var result = guid.TryGetDomainAndLocalId(out _, out _);
            Assert.IsFalse(result);
        }
    }
}
