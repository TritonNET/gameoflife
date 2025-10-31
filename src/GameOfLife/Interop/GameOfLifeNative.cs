using System.Runtime.InteropServices;

static class GameOfLifeNative
{
#if WINDOWS
    const string LIB = "gameoflife";
#elif ANDROID
    const string LIB = "gameoflife";
#elif IOS || MACCATALYST
    const string LIB = "__Internal";
#else
    const string LIB = "gameoflife";
#endif

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr gameoflife_create();
    
    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern void gameoflife_destroy(IntPtr h);
    
    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool gameoflife_init(IntPtr h, IntPtr nativeSurface, int width, int height);
    
    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern void gameoflife_resize(IntPtr h, int width, int height);
    
    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern void gameoflife_set_spheres(IntPtr h, float[] positionsXYZ, float[] radii, int count);
    
    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern void gameoflife_render(IntPtr h, float dtSeconds);
    
    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    public static extern void gameoflife_shutdown(IntPtr h);
}

public sealed class GameOfLifeRendererHost : IDisposable
{
    IntPtr _h;
    public GameOfLifeRendererHost() => _h = GameOfLifeNative.gameoflife_create();

    public bool Init(IntPtr surface, int w, int h) => GameOfLifeNative.gameoflife_init(_h, surface, w, h);
    
    public void Resize(int w, int h) => GameOfLifeNative.gameoflife_resize(_h, w, h);
    
    public void SetSpheres(float[] pos, float[] radii) => GameOfLifeNative.gameoflife_set_spheres(_h, pos, radii, radii.Length);
    
    public void Render(float dt) => GameOfLifeNative.gameoflife_render(_h, dt);
    
    public void Shutdown() => GameOfLifeNative.gameoflife_shutdown(_h);
    
    public void Dispose() { Shutdown(); if (_h != IntPtr.Zero) { GameOfLifeNative.gameoflife_destroy(_h); _h = IntPtr.Zero; } GC.SuppressFinalize(this); }
}
