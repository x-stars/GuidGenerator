namespace XNetEx.FSharp.Core

#if !UUIDREV_DISABLE && NET8_0_OR_GREATER
open System
open System.Security.Cryptography
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidGeneratorV8NSha3Test() =

    [<TestMethod>]
    member _.NewVersion8NSha3D256_EmptyName_GetExpectedGuid() =
        if not SHA3_256.IsSupported then ()
        else
            Guid.newV8NSha3D256 Guid.empty Array.empty
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "61664696-888a-8102-b8ff-672620c85217")

    [<TestMethod>]
    member _.NewVersion8NSha3D256ByString_ExampleDns_GetExpectedGuid() =
        if not SHA3_256.IsSupported then ()
        else
            Guid.newV8NSha3D256S Guid.nsDns "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "fc506eca-a1f4-8315-87c8-c71449dfd324")

    [<TestMethod>]
    member _.NewVersion8NSha3D256ByEncoding_ExampleDns_GetExpectedGuid() =
        if not SHA3_256.IsSupported then ()
        else
            Guid.newV8NSha3D256Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "8c561cb3-c8f8-8c31-9ab2-39941a72bf0f")

    [<TestMethod>]
    member _.NewVersion8NSha3D384_EmptyName_GetExpectedGuid() =
        if not SHA3_384.IsSupported then ()
        else
            Guid.newV8NSha3D384 Guid.empty Array.empty
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "a78e349c-372b-8ed0-abcb-0d141600cc2d")

    [<TestMethod>]
    member _.NewVersion8NSha3D384ByString_ExampleDns_GetExpectedGuid() =
        if not SHA3_384.IsSupported then ()
        else
            Guid.newV8NSha3D384S Guid.nsDns "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "ab4f9412-4e4c-87a5-be26-dc72f9adf6ed")

    [<TestMethod>]
    member _.NewVersion8NSha3D384ByEncoding_ExampleDns_GetExpectedGuid() =
        if not SHA3_384.IsSupported then ()
        else
            Guid.newV8NSha3D384Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "d17324d4-ea31-89d4-b380-7621517aeee0")

    [<TestMethod>]
    member _.NewVersion8NSha3D512_EmptyName_GetExpectedGuid() =
        if not SHA3_512.IsSupported then ()
        else
            Guid.newV8NSha3D512 Guid.empty Array.empty
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "f0140e31-4ee3-8d44-b239-3680e7a72a81")

    [<TestMethod>]
    member _.NewVersion8NSha3D512ByString_ExampleDns_GetExpectedGuid() =
        if not SHA3_512.IsSupported then ()
        else
            Guid.newV8NSha3D512S Guid.nsDns "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "83120d41-2935-8110-964c-6bd77c735fbc")

    [<TestMethod>]
    member _.NewVersion8NSha3D512ByEncoding_ExampleDns_GetExpectedGuid() =
        if not SHA3_512.IsSupported then ()
        else
            Guid.newV8NSha3D512Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "938092d7-d841-8578-87ff-58ced503501c")

    [<TestMethod>]
    member _.NewVersion8NShake128_EmptyName_GetExpectedGuid() =
        if not Shake128.IsSupported then ()
        else
            Guid.newV8NShake128 Guid.empty Array.empty
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "8f8e4f61-2e61-8fb9-978c-3ea707e37768")

    [<TestMethod>]
    member _.NewVersion8NShake128ByString_ExampleDns_GetExpectedGuid() =
        if not Shake128.IsSupported then ()
        else
            Guid.newV8NShake128S Guid.nsDns "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "54e7e64f-0dba-8913-8b69-cd3dbf220c7a")

    [<TestMethod>]
    member _.NewVersion8NShake128ByEncoding_ExampleDns_GetExpectedGuid() =
        if not Shake128.IsSupported then ()
        else
            Guid.newV8NShake128Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "9e1643f5-74e7-804c-b079-2b6a969aa69a")

    [<TestMethod>]
    member _.NewVersion8NShake256_EmptyName_GetExpectedGuid() =
        if not Shake256.IsSupported then ()
        else
            Guid.newV8NShake256 Guid.empty Array.empty
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "d570b23c-455f-8f43-84bf-34aa6f2b7628")

    [<TestMethod>]
    member _.NewVersion8NShake256ByString_ExampleDns_GetExpectedGuid() =
        if not Shake256.IsSupported then ()
        else
            Guid.newV8NShake256S Guid.nsDns "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "10e1aa77-4f8c-8a0f-be35-354cc01a76ac")

    [<TestMethod>]
    member _.NewVersion8NShake256ByEncoding_ExampleDns_GetExpectedGuid() =
        if not Shake256.IsSupported then ()
        else
            Guid.newV8NShake256Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
            |> tee (Guid.version
                    >> Assert.equalTo Guid.Version.Version8)
            |> Assert.equalTo (Guid.parse "5790fa5e-c8c3-8d6d-b61c-7b1d21e3cdb7")
#endif
