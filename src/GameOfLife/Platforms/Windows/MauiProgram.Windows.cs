#if WINDOWS
using GameOfLife.Views;
using GameOfLife.WinUI;
using Microsoft.Maui.Hosting;

namespace GameOfLife;

public static partial class MauiProgram
{
    static partial void RegisterPlatformHandlers(IMauiHandlersCollection handlers)
    {
        handlers.AddHandler(typeof(GameOfLifeView), typeof(GameOfLifeViewHandler));
    }
}
#endif
