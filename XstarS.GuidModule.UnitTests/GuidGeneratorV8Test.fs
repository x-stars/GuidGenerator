namespace XNetEx.FSharp.Core

#if !UUIDREV_DISABLE
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
        Guid.newV8N sha256Hashing
                    Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "374708ff-f771-8dd5-979e-c875d56cd228")

    [<TestMethod>]
    member _.NewVersion8NByString_Sha256HashingExampleDns_GetExpectedGuid() =
        use sha256Hashing = SHA256.Create()
        Guid.newV8NS sha256Hashing
                     Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "5c146b14-3c52-8afd-938a-375d0df1fbf6")

    [<TestMethod>]
    member _.NewVersion8NByEncoding_Sha256HashingExampleDns_GetExpectedGuid() =
        use sha256Hashing = SHA256.Create()
        Guid.newV8NEnc sha256Hashing
                       Guid.nsDns Text.Encoding.Unicode "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "55e88991-1eda-876a-8ff8-65f5a9fbd5db")

    [<TestMethod>]
    member _.NewVersion8NSha256_EmptyName_GetExpectedGuid() =
        Guid.newV8NSha256 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "374708ff-f771-8dd5-979e-c875d56cd228")

    [<TestMethod>]
    member _.NewVersion8NSha256ByString_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha256S Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "5c146b14-3c52-8afd-938a-375d0df1fbf6")

    [<TestMethod>]
    member _.NewVersion8NSha256ByEncoding_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha256Enc Guid.nsDns Text.Encoding.Unicode "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "55e88991-1eda-876a-8ff8-65f5a9fbd5db")

    [<TestMethod>]
    member _.NewVersion8NSha384_EmptyName_GetExpectedGuid() =
        Guid.newV8NSha384 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "ae40659d-a119-8cde-88df-474b5e36416a")

    [<TestMethod>]
    member _.NewVersion8NSha384ByString_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha384S Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "3df00ae4-42a7-8066-88ad-1f925b8b8e54")

    [<TestMethod>]
    member _.NewVersion8NSha512_EmptyName_GetExpectedGuid() =
        Guid.newV8NSha512 Guid.empty Array.empty
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "0b6cbac8-38df-87f4-bea1-bd0df00ec282")

    [<TestMethod>]
    member _.NewVersion8NSha512ByString_ExampleDns_GetExpectedGuid() =
        Guid.newV8NSha512S Guid.nsDns "www.example.com"
        |> tee (Guid.version
                >> Assert.equalTo Guid.Version.Version8)
        |> Assert.equalTo (Guid.parse "94ee4ddb-9f36-8018-9ccf-86a4441691e0")
#endif
