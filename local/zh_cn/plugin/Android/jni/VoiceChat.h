#pragma once
#include "./Defines.h"
#include <stddef.h>

class VoiceEncoder;
class VoiceChat
{
private:
    VoiceChat();

public:
    ~VoiceChat();
    static VoiceChat *getInstance();
    static void destroyInstance();

    void initialize(const char *cacheDir, int sampleRate, int channelNum, int bitsPerSample);

    void onRecord(unsigned char *pAudioData, size_t len);

    void uploadVoice(const char *url, const char *postData);

    void downloadVoice(const char *url, const char *postData);

    void deleteVoice(const char *url);

    void deleteAllVoice();

protected:
    void _onUploadVoice(const char *voiceUrl, int seconds);
    void _onDownloadVoice(const char *voicePath);

public:
    VoiceEncoder *m_pVoiceEncoder;
    bool m_isInitialized;
};