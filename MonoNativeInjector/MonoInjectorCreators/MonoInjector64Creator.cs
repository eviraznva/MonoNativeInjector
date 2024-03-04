using System.Diagnostics;
using MonoNativeInjector.Common;
using MonoNativeInjector.Abstractions;
using MonoNativeInjector.MonoInjectors;

namespace MonoNativeInjector.MonoInjectorCreators;

/// <summary>
/// A factory class for creating instances of <see cref="MonoInjector64"/>, specifically designed for 64-bit Mono processes.
/// </summary>
internal sealed class MonoInjector64Creator(Process _gameProcess) : MonoInjector
{
    /// <summary>
    /// Creates an instance of <see cref="MonoInjector64"/>, providing a concrete implementation of <see cref="IMonoInjector"/> optimized for 64-bit processes.
    /// </summary>
    /// <returns>An instance of <see cref="MonoInjector64"/> that is specialized for injecting assemblies into 64-bit Mono processes.</returns>
    protected override IMonoInjector CreateInjector() => new MonoInjector64(_gameProcess);
}