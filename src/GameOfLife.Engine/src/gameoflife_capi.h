#pragma once
#include <stdint.h>

#if defined(_WIN32)
#if defined(GAMEOFLIFE_BUILD)
#define GAMEOFLIFE_API __declspec(dllexport)
#else
#define GAMEOFLIFE_API __declspec(dllimport)
#endif
#else
#define GAMEOFLIFE_API __attribute__((visibility("default")))
#endif

#ifdef __cplusplus
extern "C" {
#endif

    typedef void* gameoflife_handle;

    GAMEOFLIFE_API gameoflife_handle gameoflife_create();
    GAMEOFLIFE_API void gameoflife_destroy(gameoflife_handle h);

    GAMEOFLIFE_API bool gameoflife_init(gameoflife_handle h, void* native_surface, int width, int height);
    GAMEOFLIFE_API void gameoflife_resize(gameoflife_handle h, int width, int height);
    GAMEOFLIFE_API void gameoflife_set_spheres(gameoflife_handle h, const float* positions_xyz, const float* radii, int count);
    GAMEOFLIFE_API void gameoflife_render(gameoflife_handle h, float dt_seconds);
    GAMEOFLIFE_API void gameoflife_shutdown(gameoflife_handle h);

#ifdef __cplusplus
}
#endif
