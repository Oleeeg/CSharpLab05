using System;
namespace CSharpLab05
{
    internal static class StationManager
    {
        public static event Action StopThreads;
        public static bool Stop = true;
        
        internal static void CloseApp()
        {
            Stop = false;
            StopThreads?.Invoke();
            Environment.Exit(0);
        }

    }
}