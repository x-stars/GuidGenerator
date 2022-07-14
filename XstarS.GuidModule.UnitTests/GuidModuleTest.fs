namespace XNetEx

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type GuidModuleTest() =

    [<TestMethod>]
    member this.TestMethodPassing() =
        Assert.IsTrue(true)
