namespace global

open Microsoft.VisualStudio.TestTools.UnitTesting

// Indicates the parallelization level for unit tests run.
[<assembly: Parallelize(Scope = ExecutionScope.MethodLevel)>]
do ()
