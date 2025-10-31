using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace GameOfLife;

public static partial class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.ConfigureMauiHandlers(handlers =>
        {
            // Platform-specific registrations happen in the partial below:
            RegisterPlatformHandlers(handlers);
        });

        return builder.Build();
    }

    // Declaration only; implemented per platform.
    static partial void RegisterPlatformHandlers(IMauiHandlersCollection handlers);
}
