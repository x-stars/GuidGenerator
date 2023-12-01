namespace XNetEx.FSharp.Core

open Microsoft.VisualStudio.TestTools.UnitTesting
open XNetEx.FSharp.UnitTesting.MSTest

[<TestClass>]
type GuidDataComponentTest() =

    [<TestMethod>]
    member _.TryGetHashData_Version3Guid_GetExpectedDataAndMask() =
        Guid.parse "a9ec4420-7252-3c11-ab70-512e10273537"
        |> Guid.tryGetHashData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> tee (fun struct (_, mask) -> mask
                >> Assert.Seq.equalTo (Array.map byte
                    [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                       0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]))
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0xa9; 0xec; 0x44; 0x20; 0x72; 0x52; 0x0c; 0x11
               0x2b; 0x70; 0x51; 0x2e; 0x10; 0x27; 0x35; 0x37 |])

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.TryGetRandomData_Version7Guid_GetExpectedDataAndMask() =
        Guid.parse "017f22e2-79b0-7cc3-98c4-dc0c0c07398f"
        |> Guid.tryGetRandomData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> tee (fun struct (_, mask) -> mask
                >> Assert.Seq.equalTo (Array.map byte
                    [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0f; 0xff
                       0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]))
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0c; 0xc3
               0x18; 0xc4; 0xdc; 0x0c; 0x0c; 0x07; 0x39; 0x8f |])
#else
    [<TestMethod>]
    member _.TryGetRandomData_Version4Guid_GetExpectedDataAndMask() =
        Guid.parse "2502f1d5-c2a9-47d3-b6d8-d7670094ace2"
        |> Guid.tryGetRandomData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> tee (fun struct (_, mask) -> mask
                >> Assert.Seq.equalTo (Array.map byte
                    [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                       0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]))
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x25; 0x02; 0xf1; 0xd5; 0xc2; 0xa9; 0x07; 0xd3
               0x36; 0xd8; 0xd7; 0x67; 0x00; 0x94; 0xac; 0xe2 |])
#endif

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.TryGetCustomData_Version8Guid_GetExpectedDataAndMask() =
        Guid.parse "05db6c94-bba6-8702-88aa-548f4d6cd700"
        |> Guid.tryGetCustomData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> tee (fun struct (_, mask) -> mask
                >> Assert.Seq.equalTo (Array.map byte
                    [| 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0x0f; 0xff
                       0x3f; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff; 0xff |]))
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x05; 0xdb; 0x6c; 0x94; 0xbb; 0xa6; 0x07; 0x02
               0x08; 0xaa; 0x54; 0x8f; 0x4d; 0x6c; 0xd7; 0x00 |])
#endif

    [<TestMethod>]
    member _.ReplaceHashData_Version5Guid_GetInputHashData() =
        Guid.parse "00000000-0000-5000-8000-000000000000"
        |> Guid.replaceHashData (Array.map byte
            [| 0x76; 0x8a; 0x7b; 0x1b; 0xae; 0x51; 0x0c; 0x0a
               0x3c; 0x9d; 0xa8; 0x5a; 0x34; 0x3f; 0x2c; 0x24 |])
        |> Guid.tryGetHashData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x76; 0x8a; 0x7b; 0x1b; 0xae; 0x51; 0x0c; 0x0a
               0x3c; 0x9d; 0xa8; 0x5a; 0x34; 0x3f; 0x2c; 0x24 |])

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.ReplaceRandomData_Version7Guid_GetInputRandomData() =
        Guid.parse "017f22e2-79b0-7000-8000-000000000000"
        |> Guid.replaceRandomData (Array.map byte
            [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0c; 0xc3
               0x18; 0xc4; 0xdc; 0x0c; 0x0c; 0x07; 0x39; 0x8f |])
        |> Guid.tryGetRandomData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x00; 0x00; 0x00; 0x00; 0x00; 0x00; 0x0c; 0xc3
               0x18; 0xc4; 0xdc; 0x0c; 0x0c; 0x07; 0x39; 0x8f |])
#else
    [<TestMethod>]
    member _.ReplaceRandomData_Version4Guid_GetInputRandomData() =
        Guid.parse "00000000-0000-4000-8000-000000000000"
        |> Guid.replaceRandomData (Array.map byte
            [| 0x25; 0x02; 0xf1; 0xd5; 0xc2; 0xa9; 0x07; 0xd3
               0x36; 0xd8; 0xd7; 0x67; 0x00; 0x94; 0xac; 0xe2 |])
        |> Guid.tryGetRandomData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x25; 0x02; 0xf1; 0xd5; 0xc2; 0xa9; 0x07; 0xd3
               0x36; 0xd8; 0xd7; 0x67; 0x00; 0x94; 0xac; 0xe2 |])
#endif

#if !UUIDREV_DISABLE
    [<TestMethod>]
    member _.ReplaceCustomData_Version8Guid_GetInputCustomData() =
        Guid.parse "00000000-0000-8000-8000-000000000000"
        |> Guid.replaceCustomData (Array.map byte
            [| 0x05; 0xdb; 0x6c; 0x94; 0xbb; 0xa6; 0x07; 0x02
               0x08; 0xaa; 0x54; 0x8f; 0x4d; 0x6c; 0xd7; 0x00 |])
        |> Guid.tryGetCustomData
        |> tee (Assert.true' << ValueOption.isSome)
        |> ValueOption.get
        |> fun struct (data, _) -> data
        |> Assert.Seq.equalTo (Array.map byte
            [| 0x05; 0xdb; 0x6c; 0x94; 0xbb; 0xa6; 0x07; 0x02
               0x08; 0xaa; 0x54; 0x8f; 0x4d; 0x6c; 0xd7; 0x00 |])
#endif
