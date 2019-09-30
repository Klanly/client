#include "./Defines.h"
#include <stdarg.h>
#include <stdio.h>

#if CC_TARGET_PLATFORM == CC_PLATFORM_WIN32
#include <Windows.h>
#endif

#if CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID
#include <android/log.h>
#endif

void consoleLog(const char* format, ...)
{
    char buffer[1024];
    int result = 0;
    va_list argptr;
    va_start(argptr, format);
    result = vsnprintf(buffer, 1024, format, argptr);
    va_end(argptr);

#if CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID
    __android_log_print(ANDROID_LOG_DEBUG, "cocos2d-x debug info", "%s", buffer);

#elif CC_TARGET_PLATFORM ==  CC_PLATFORM_WIN32
    WCHAR wszBuf[4096] ={0};
    MultiByteToWideChar(CP_UTF8, 0, buffer, -1, wszBuf, sizeof(wszBuf));
    OutputDebugStringW(wszBuf);
    WideCharToMultiByte(CP_ACP, 0, wszBuf, -1, buffer, sizeof(buffer), nullptr, FALSE);
    printf("%s", buffer);
    fflush(stdout);
#else
    // Linux, Mac, iOS, etc
    fprintf(stdout, "%s", buf);
    fflush(stdout);
#endif
}