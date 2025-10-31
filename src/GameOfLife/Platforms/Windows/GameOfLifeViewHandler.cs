// Platforms/Windows/GameOfLifeViewHandler.cs
#if WINDOWS
using System;
using GameOfLife.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinGrid = Microsoft.UI.Xaml.Controls.Grid;

namespace GameOfLife.WinUI;

internal class GameOfLifeViewHandler : ViewHandler<GameOfLifeView, WinGrid>
{
    public static readonly IPropertyMapper<GameOfLifeView, GameOfLifeViewHandler> Mapper
        = new PropertyMapper<GameOfLifeView, GameOfLifeViewHandler>(ViewMapper);

    public GameOfLifeViewHandler() : base(Mapper) { }

    private WinGrid? _grid;
    private GameOfLifeRendererHost? _host;
    private DateTime _last;

    protected override WinGrid CreatePlatformView()
    {
        _grid = new WinGrid();
        _grid.Loaded += OnLoaded;
        _grid.Unloaded += OnUnloaded;
        _grid.SizeChanged += (s, e) =>
        {
            if (_host != null && _grid != null)
                _host.Resize((int)_grid.ActualWidth, (int)_grid.ActualHeight);
        };
        return _grid;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (MauiContext is null || _grid is null) return;

        var app = MauiContext.Services.GetRequiredService<IApplication>();
        var mauiWindow = app.Windows.Count > 0 ? app.Windows[0] : null;
        if (mauiWindow?.Handler?.PlatformView is not MauiWinUIWindow winuiWindow) return;

        var hwnd = winuiWindow.WindowHandle;

        _host = new GameOfLifeRendererHost();
        _host.Init(hwnd, (int)_grid.ActualWidth, (int)_grid.ActualHeight);
        _host.SetSpheres(
            new float[] { 10, 10, 20, 50, 50, 50, -20, -20, -20 },
            new float[] { 5, 5, 5 });

        Microsoft.UI.Xaml.Media.CompositionTarget.Rendering += OnRendering;
        _last = DateTime.UtcNow;
    }

    private void OnRendering(object? s, object? a)
    {
        if (_host == null) return;
        var now = DateTime.UtcNow;
        var dt = (float)(now - _last).TotalSeconds;
        _last = now;
        _host.Render(dt);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        Microsoft.UI.Xaml.Media.CompositionTarget.Rendering -= OnRendering;
        _host?.Shutdown();
        _host?.Dispose();
        _host = null;
    }

    protected override void DisconnectHandler(WinGrid platformView)
    {
        Microsoft.UI.Xaml.Media.CompositionTarget.Rendering -= OnRendering;
        _host?.Shutdown();
        _host?.Dispose();
        _host = null;
        base.DisconnectHandler(platformView);
    }
}
#endif
