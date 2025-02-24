using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace OpenGLGameEngine;
internal class Program
{
    private static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(1440, 1080),
            Title = "MerfEngine a0.01",
            // This is needed to run on macos
            Flags = ContextFlags.ForwardCompatible,
        };

        using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            Console.WriteLine("Program Loaded");
            window.Run();
        }
        Console.WriteLine("Program Closed");
    }
}