using System.Runtime.InteropServices;

namespace MonoNativeInjector.Misc;

/// <summary>
/// Provides logging functionalities for the application, allowing the logging of information, warnings, and debug messages.
/// </summary>
internal static unsafe class Logger
{
    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    internal static void LogInfo(string message)
    {
        if (Main.Logger is null) return; // Check if the logger delegate is set, exit if not
        
        // Convert the message to a native ANSI string pointer, appending a null terminator
        var messagePtr = Marshal.StringToHGlobalAnsi(string.Join("", message.Append('\0')));
        
        Main.Logger(messagePtr, 0); // Log the message as information (log level 0)
        
        Marshal.FreeHGlobal(messagePtr); // Free the allocated memory for the message
    }
    
    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    internal static void LogWarning(string message)
    {
        if (Main.Logger is null) return; // Check if the logger delegate is set, exit if not
        
        // Convert the message to a native ANSI string pointer, appending a null terminator
        var messagePtr = Marshal.StringToHGlobalAnsi(string.Join("", message.Append('\0')));
        
        Main.Logger(messagePtr, 1); // Log the message as a warning (log level 1)
        
        Marshal.FreeHGlobal(messagePtr); // Free the allocated memory for the message
    }
    
    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    internal static void LogDebug(string message)
    {
        if (Main.Logger is null) return; // Check if the logger delegate is set, exit if not
        
        // Convert the message to a native ANSI string pointer, appending a null terminator
        var messagePtr = Marshal.StringToHGlobalAnsi(string.Join("", message.Append('\0')));
        
        Main.Logger(messagePtr, 2); // Log the message as debug information (log level 2)
        
        Marshal.FreeHGlobal(messagePtr); // Free the allocated memory for the message
    }
}