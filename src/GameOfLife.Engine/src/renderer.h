#pragma once

class GameOfLifeRenderer {
public:
    GameOfLifeRenderer();
    ~GameOfLifeRenderer();

    // nativeSurface:
    //  - Windows: HWND
    //  - Android: ANativeWindow*
    //  - iOS/Mac Catalyst: CAMetalLayer*
    bool Init(void* nativeSurface, int width, int height);
    void Resize(int width, int height);
    void SetSpheres(const float* positionsXYZ, const float* radii, int count);
    void Render(float dtSeconds);
    void Shutdown();

private:
    // internal state: bgfx handles, vertex/index buffers, instance data, camera params ...
    int _width = 0;
    int _height = 0;
    int _sphereCount = 0;
    // store positions and radii to rebuild instance transforms
    // minimal placeholders; implement your own containers
};
