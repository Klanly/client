#pragma once
#include "./Defines.h"
#include <vector>

struct lame_global_struct;
typedef struct lame_global_struct lame_global_flags;

class VoiceEncoder
{
public:
    VoiceEncoder(void);
    ~VoiceEncoder(void);

public:
    void initialize(int sampleRate, int channels, int bitPerSample);
    void pushAudioData(unsigned char *pData, size_t len);
    unsigned char *flushAudioData(size_t &fileLen);
    /// �����Ƶʱ�䳤�ȣ���λΪ��
    float getAudioLength();
    void reset();

protected:
    void _initLame();

protected:
    std::vector<std::pair<unsigned char*, size_t> > m_dataVector;
    lame_global_flags *m_pLame;
    int m_sampleRate;
    int m_channels;
    int m_bitPerSample;
    int m_sampleCount;
};
