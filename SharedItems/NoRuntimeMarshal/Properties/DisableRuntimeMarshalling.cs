// Copyright (c) 2024 XstarS
// This file is released under the MIT License.
// https://opensource.org/licenses/MIT

#if NET7_0_OR_GREATER
// Disables the built-in runtime managed/unmanaged marshalling subsystem
// for P/Invokes, Delegate types, and unmanaged function pointer invocations.
[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]
#endif
