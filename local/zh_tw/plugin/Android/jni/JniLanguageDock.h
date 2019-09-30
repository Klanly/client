#pragma once
#include "./Defines.h"

class JniLanguageDock
{
private:
    JniLanguageDock();
    ~JniLanguageDock();

public:
    static void onUploadVoice(const char *voiceUrl, int seconds);
    
    static void onDownloadVoice(const char *voicePath);
};