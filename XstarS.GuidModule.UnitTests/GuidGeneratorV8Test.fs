namespace XNetEx.FSharp.Core

#if !FEATURE_DISABLE_UUIDREV
open System
open System.Security.Cryptography
open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidGeneratorV8Test() =

    [<TestMethod>]
    member _.NewVersion8_WithoutInput_GetGuidOfVersion8() =
        Guid.newV8 ()
        |> Guid.version
        |> Assert.equalTo Guid.Version.Version8

    [<TestMethod>]
    member _.NewVersion8N_Sha256HashingEmptyName_GetExpectedGuid() =
        use sha256Hashing = SHA256.Create()
        Guid.newV8N Guid.hsSha256 sha256Hashing
                    Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "1133a3b1-c2ec-87f2-ade0-dfef9f3cd913")

    [<TestMethod>]
    member _.NewVersion8NByString_Sha256HashingExampleDns_GetExpectedGuid() =
        use sha256Hashing = SHA256.Create()
        Guid.newV8NS Guid.hsSha256 sha256Hashing
                     Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "401835fd-a627-870a-873f-ed73f2bc5b2c")

    [<TestMethod>]
    member _.NewVersion8NByEncoding_Sha256HashingExampleDns_GetExpectedGuid() =
        use sha256Hashing = SHA256.Create()
        Guid.newV8NEnc Guid.hsSha256 sha256Hashing
                       Guid.nsDns Text.Encoding.Unicode "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "909371f1-5575-8bf9-82e4-0a96ce6c1f90")

    [<TestMethod>]
    member _.NewVersion8NSha256_EmptyName_GetExpectedGuid() =
        Guid.newV8NSha256 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "1133a3b1-c2ec-87f2-ade0-dfef9f3cd913")

    [<TestMethod>]
    member _.NewVersion8NSha256ByString_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha256S Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "401835fd-a627-870a-873f-ed73f2bc5b2c")

    [<TestMethod>]
    member _.NewVersion8NSha256ByEncoding_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha256Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "909371f1-5575-8bf9-82e4-0a96ce6c1f90")

    [<TestMethod>]
    member _.NewVersion8NSha384_EmptyName_GetExpectedGuid() =
        Guid.newV8NSha384 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "d1799304-22a5-8cd7-911d-0c1b82c5123c")

    [<TestMethod>]
    member _.NewVersion8NSha384ByString_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha384S Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "cbf4bd9c-024c-849c-bb0d-8bffc5be3afe")

    [<TestMethod>]
    member _.NewVersion8NSha512_EmptyName_GetExpectedGuid() =
        Guid.newV8NSha512 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "6db9934e-1903-80bb-bfaa-75577a22b99a")

    [<TestMethod>]
    member _.NewVersion8NSha512ByString_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha512S Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "da65f7c3-21ae-82e0-a654-97eea9af6ed6")
#endif
