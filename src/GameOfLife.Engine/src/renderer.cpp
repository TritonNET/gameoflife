#include "renderer.h"
#include "platform_bgfx.h"

#include <bgfx/bgfx.h>
#include <bgfx/platform.h>
#include <bx/math.h>

GameOfLifeRenderer::GameOfLifeRenderer() 
{
}

GameOfLifeRenderer::~GameOfLifeRenderer() 
{ 
    Shutdown(); 
}

bool GameOfLifeRenderer::Init(void* nativeSurface, int width, int height)
{
    _width = width; _height = height;

    platform_bgfx::SetupPlatform(nativeSurface);
    if (!platform_bgfx::InitRenderer(width, height))
        return false;

    // View 0 clear
    bgfx::setViewClear(0, BGFX_CLEAR_COLOR | BGFX_CLEAR_DEPTH, 0x303030ff, 1.0f, 0);
    bgfx::setViewRect(0, 0, 0, (uint16_t)width, (uint16_t)height);

    // TODO: create static buffers for axes/grid
    // TODO: create unit sphere mesh VBO/IBO
    // TODO: load shaders and create programs

    // Default camera
    // Store camera parameters if you prefer; here we’ll compute per-frame

    return true;
}

void GameOfLifeRenderer::Resize(int width, int height)
{
    _width = width; _height = height;
    platform_bgfx::Resize(width, height);
    bgfx::setViewRect(0, 0, 0, (uint16_t)width, (uint16_t)height);
}

void GameOfLifeRenderer::SetSpheres(const float* positionsXYZ, const float* radii, int count)
{
    // Store for instance transforms
    _sphereCount = count;
    // TODO: copy into internal arrays and build instance transforms during Render()
}

void GameOfLifeRenderer::Render(float dtSeconds)
{
    // Camera
    float view[16];
    float proj[16];
    const bx::Vec3 eye = { 80.0f, 60.0f, 80.0f };
    const bx::Vec3 at = { 0.0f,  0.0f,  0.0f };
    const bx::Vec3 up = { 0.0f,  1.0f,  0.0f };
    bx::mtxLookAt(view, eye, at, up);

    const float aspect = (_height > 0) ? (float)_width / (float)_height : 1.0f;
    bx::mtxProj(proj, 60.0f, aspect, 0.1f, 1000.0f, bgfx::getCaps()->homogeneousDepth);

    bgfx::setViewTransform(0, view, proj);

    // Clear view
    bgfx::touch(0);

    // TODO: submit axes/grid (static VBO as lines)
    // TODO: set up instance buffer for spheres and submit instanced draw using a program

    // Advance
    bgfx::frame();
}

void GameOfLifeRenderer::Shutdown()
{
    // TODO: destroy buffers/programs if created
    platform_bgfx::Shutdown();
}
