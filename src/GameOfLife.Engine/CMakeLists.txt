cmake_minimum_required(VERSION 3.21)
project(GameOfLife3D LANGUAGES C CXX)

# Options
option(GAMEOFLIFE_BUILD_SHARED "Build shared library" ON)  # ON for Win/Android, OFF for iOS/mac
option(GAMEOFLIFE_WITH_BGFX_FETCH "Fetch bgfx toolchain" ON)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# ---- bgfx via bgfx.cmake ----
# Turn off examples/tools to avoid extra targets and warnings
set(BGFX_BUILD_EXAMPLES OFF CACHE BOOL "" FORCE)
set(BGFX_BUILD_TOOLS    OFF CACHE BOOL "" FORCE)

if (GAMEOFLIFE_WITH_BGFX_FETCH)
  include(FetchContent)
  FetchContent_Declare(
    bgfx.cmake
    GIT_REPOSITORY https://github.com/bkaradzic/bgfx.cmake.git
    GIT_TAG        master
  )
  FetchContent_MakeAvailable(bgfx.cmake)  # provides targets: bgfx bx bimg
endif()

# ---- Sources ----
set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)

set(GAMEOFLIFE_SOURCES
  ${SRC_DIR}/renderer.cpp
  ${SRC_DIR}/gameoflife_capi.cpp
  ${SRC_DIR}/platform_bgfx.cpp
)
set(GAMEOFLIFE_HEADERS
  ${SRC_DIR}/renderer.h
  ${SRC_DIR}/gameoflife_capi.h
  ${SRC_DIR}/platform_bgfx.h
)

# ---- Library ----
if (GAMEOFLIFE_BUILD_SHARED)
  add_library(gameoflife SHARED ${GAMEOFLIFE_SOURCES} ${GAMEOFLIFE_HEADERS})
  target_compile_definitions(gameoflife PRIVATE GAMEOFLIFE_BUILD=1)
else()
  add_library(gameoflife STATIC ${GAMEOFLIFE_SOURCES} ${GAMEOFLIFE_HEADERS})
endif()

target_include_directories(gameoflife PUBLIC ${SRC_DIR})

# Link bgfx stack
target_link_libraries(gameoflife PRIVATE bgfx bx bimg)

# Per-platform defs and libs
if (WIN32)
  target_compile_definitions(gameoflife PRIVATE BGFX_CONFIG_RENDERER_DIRECT3D11=1)
  target_link_libraries(gameoflife PRIVATE d3d11 dxgi)
elseif(APPLE)
  find_library(FOUNDATION Foundation)
  find_library(QUARTZCORE QuartzCore)
  find_library(METAL Metal)
  find_library(METALKIT MetalKit)
  target_link_libraries(gameoflife PRIVATE ${FOUNDATION} ${QUARTZCORE} ${METAL} ${METALKIT})
elseif(ANDROID)
  find_library(ANDROID_LIB android)
  find_library(LOG_LIB log)
  target_link_libraries(gameoflife PRIVATE ${ANDROID_LIB} ${LOG_LIB})
endif()

# Export macro for consumers
target_compile_definitions(gameoflife PUBLIC GAMEOFLIFE_EXPORTS=1)

# Install
install(TARGETS gameoflife
  RUNTIME DESTINATION bin
  LIBRARY DESTINATION lib
  ARCHIVE DESTINATION lib
)
