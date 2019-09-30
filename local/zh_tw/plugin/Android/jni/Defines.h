#pragma once
#include <stddef.h>
#include <string>
#include <stdint.h>

#define CC_PLATFORM_UNKNOWN            0
#define CC_PLATFORM_IOS                1
#define CC_PLATFORM_ANDROID            2
#define CC_PLATFORM_WIN32              3

#define CC_TARGET_PLATFORM             CC_PLATFORM_UNKNOWN

#if defined WIN32
    #undef CC_TARGET_PLATFORM
    #define CC_TARGET_PLATFORM CC_PLATFORM_WIN32
#endif

#if defined __APPLE__
    #undef CC_TARGET_PLATFORM
    #define CC_TARGET_PLATFORM CC_PLATFORM_IOS
#endif

#if defined __ANDROID__
    #undef CC_TARGET_PLATFORM
    #define CC_TARGET_PLATFORM CC_PLATFORM_ANDROID
#endif

#define CC_SAFE_DELETE(p)           do { delete (p); (p) = nullptr; } while(0)
#define CC_SAFE_DELETE_ARRAY(p)     do { if(p) { delete[] (p); (p) = nullptr; } } while(0)

void consoleLog(const char* format, ...);