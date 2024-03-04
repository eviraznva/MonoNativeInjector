using MonoNativeInjector.Common;

namespace MonoNativeInjector.Abstractions;

// Defines the base functionality for creating instances of types implementing the IMonoInjector interface.
internal abstract class MonoInjector
{
    /// <summary>
    /// When implemented in a derived class, creates and returns a new instance of a class implementing the IMonoInjector interface.
    /// </summary>
    /// <returns>An IMonoInjector instance created by the derived class.</returns>
    protected abstract IMonoInjector CreateInjector();
    
    /// <summary>
    /// Creates and returns an instance of a class implementing the IMonoInjector interface using the abstract CreateInjector method defined in derived classes.
    /// </summary>
    /// <returns>An instance of a class implementing the IMonoInjector interface.</returns>
    internal IMonoInjector CreateInstanceOfInjector() => CreateInjector();
}