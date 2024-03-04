using System.Diagnostics;
using MonoNativeInjector.Common;
using MonoNativeInjector.Abstractions;
using MonoNativeInjector.MonoInjectors;

namespace MonoNativeInjector.MonoInjectorCreators;

/// <summary>
/// A factory class for creating instances of <see cref="MonoInjector32"/>, tailored for 32-bit Mono processes.
/// </summary>
internal sealed class MonoInjector32Creator(Process _gameProcess) : MonoInjector
{
    /// <summary>
    /// Creates an instance of <see cref="MonoInjector32"/>, providing the necessary implementation of <see cref="IMonoInjector"/> for 32-bit processes.
    /// </summary>
    /// <returns>An instance of <see cref="MonoInjector32"/> specialized for injecting into 32-bit Mono processes.</returns>
    protected override IMonoInjector CreateInjector() => new MonoInjector32(_gameProcess);
}