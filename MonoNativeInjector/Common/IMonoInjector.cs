namespace MonoNativeInjector.Common;

/// <summary>
/// Defines the contract for Mono injectors, including methods for injecting and ejecting assemblies, and disposing resources.
/// </summary>
public interface IMonoInjector : IDisposable
{
    /// <summary>
    /// Injects an assembly into a Mono process.
    /// </summary>
    /// <param name="assemblyPath">The path to the assembly to be injected.</param>
    /// <param name="namespaceName">The target namespace within the process.</param>
    /// <param name="className">The target class within the namespace.</param>
    /// <param name="methodName">The target method within the class to inject the assembly into.</param>
    /// <returns>A pointer indicating the result of the injection process.</returns>
    IntPtr Inject(string assemblyPath, string namespaceName, string className, string methodName);
    
    /// <summary>
    /// Ejects an injected assembly from a Mono process.
    /// </summary>
    /// <param name="assemblyPtr">A pointer to the injected assembly.</param>
    /// <param name="namespaceName">The namespace from which the assembly will be ejected.</param>
    /// <param name="className">The class from which the assembly will be ejected.</param>
    /// <param name="methodName">The method from which the assembly will be ejected.</param>
    void Eject(IntPtr assemblyPtr, string namespaceName, string className, string methodName);
}