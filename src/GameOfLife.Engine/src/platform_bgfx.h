#pragma once
#include <stdint.h>

namespace platform_bgfx {

	// Fill bgfx::PlatformData based on OS native surface
	bool SetupPlatform(void* nativeSurface);

	// Create/reset bgfx with the current size
	bool InitRenderer(int width, int height);

	// Notify resize to bgfx
	void Resize(int width, int height);

	// Shutdown
	void Shutdown();

} // namespace
