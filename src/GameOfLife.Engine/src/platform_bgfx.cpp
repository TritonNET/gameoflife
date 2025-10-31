#include "platform_bgfx.h"
#include <bgfx/bgfx.h>
#include <bgfx/platform.h>

#if defined(_WIN32)
#include <windows.h>
#elif defined(__ANDROID__)
#include <android/native_window.h>
#elif defined(__APPLE__)
#include <TargetConditionals.h>
#include <QuartzCore/CAMetalLayer.h>
#endif

namespace platform_bgfx {

    bool SetupPlatform(void* nativeSurface)
    {
        bgfx::PlatformData pd{};
#if defined(_WIN32)
        pd.nwh = nativeSurface; // HWND
#elif defined(__ANDROID__)
        pd.nwh = nativeSurface; // ANativeWindow*
#elif defined(__APPLE__)
        // pass CAMetalLayer* for Metal
        pd.nwh = nativeSurface;
#else
        pd.nwh = nativeSurface;
#endif
        bgfx::setPlatformData(pd);
        return true;
    }

    bool InitRenderer(int width, int height)
    {
        bgfx::Init init{};
        init.type = bgfx::RendererType::Count; // auto
        init.resolution.width = (uint32_t)width;
        init.resolution.height = (uint32_t)height;
        init.resolution.reset = BGFX_RESET_VSYNC;
        return bgfx::init(init);
    }

    void Resize(int width, int height)
    {
        bgfx::reset((uint32_t)width, (uint32_t)height, BGFX_RESET_VSYNC);
    }

    void Shutdown()
    {
        bgfx::shutdown();
    }

} // namespace
