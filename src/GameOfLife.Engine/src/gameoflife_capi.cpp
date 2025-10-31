#include "gameoflife_capi.h"
#include "renderer.h"

extern "C" {

    gameoflife_handle gameoflife_create() 
    {
        return new GameOfLifeRenderer();
    }

    void gameoflife_destroy(gameoflife_handle h) 
    {
        if (!h) return;
        delete static_cast<GameOfLifeRenderer*>(h);
    }

    bool gameoflife_init(gameoflife_handle h, void* native_surface, int width, int height) 
    {
        if (!h) 
            return false;

        return static_cast<GameOfLifeRenderer*>(h)->Init(native_surface, width, height);
    }

    void gameoflife_resize(gameoflife_handle h, int width, int height) 
    {
        if (!h) 
            return;

        static_cast<GameOfLifeRenderer*>(h)->Resize(width, height);
    }

    void gameoflife_set_spheres(gameoflife_handle h, const float* positions_xyz, const float* radii, int count) 
    {
        if (!h) 
            return;

        static_cast<GameOfLifeRenderer*>(h)->SetSpheres(positions_xyz, radii, count);
    }

    void gameoflife_render(gameoflife_handle h, float dt_seconds) 
    {
        if (!h) 
            return;

        static_cast<GameOfLifeRenderer*>(h)->Render(dt_seconds);
    }

    void gameoflife_shutdown(gameoflife_handle h) 
    {
        if (!h) 
            return;

        static_cast<GameOfLifeRenderer*>(h)->Shutdown();
    }

} // extern "C"
