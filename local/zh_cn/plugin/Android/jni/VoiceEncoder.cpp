#include "./Defines.h"
#include "VoiceEncoder.h"
#include "./lame/lame.h"

static size_t g_kFinalDataSize = 7200;

VoiceEncoder::VoiceEncoder(void)
    :m_pLame(nullptr)
    ,m_sampleRate(0)
    ,m_channels(0)
    ,m_bitPerSample(0)
    ,m_sampleCount(0)
{
}

VoiceEncoder::~VoiceEncoder()
{
    for(size_t i=0; i<m_dataVector.size(); ++i) {
        CC_SAFE_DELETE_ARRAY(m_dataVector[i].first);
    }
    m_dataVector.clear();
    lame_close(m_pLame);
}

void VoiceEncoder::initialize(int sampleRate, int channels, int bitPerSample)
{
    m_sampleRate = sampleRate;
    m_channels = channels;
    m_bitPerSample = bitPerSample;
    _initLame();
}

void VoiceEncoder::_initLame()
{
    m_pLame = lame_init();
    lame_set_in_samplerate(m_pLame, m_sampleRate);
    lame_set_num_channels(m_pLame, 1);   //We only need one channel.
    lame_set_brate(m_pLame, 8); //8 kHZ is enough for human voice.
    lame_set_mode(m_pLame, MONO);
    lame_set_quality(m_pLame, 5);
    lame_init_params(m_pLame);
}

void VoiceEncoder::pushAudioData(unsigned char *pData, size_t len)
{
    int bytesPerSample = m_bitPerSample >> 3;
    size_t sampleNum = (len / m_channels) / bytesPerSample;
    m_sampleCount += sampleNum;
    short *pInputVoice = new short[sampleNum];
    size_t destLen = sampleNum * bytesPerSample / m_sampleRate +
                    4 * 576 * bytesPerSample / m_sampleRate + 256;
    unsigned char *pDestBuffer = new unsigned char[destLen];
    memset(pInputVoice, 0, sampleNum << 1);
    memset(pDestBuffer, 0, destLen);

    for(size_t i=0; i<sampleNum; ++i) {
        const unsigned char *pCurrent = &pData[bytesPerSample * m_channels * i];
        pInputVoice[i] = pCurrent[1];
        pInputVoice[i] <<= 8;
        pInputVoice[i] |= pCurrent[0];
    }
    destLen = lame_encode_buffer(m_pLame, pInputVoice, nullptr, sampleNum, pDestBuffer, destLen);
    m_dataVector.push_back(std::pair<unsigned char*, size_t>(pDestBuffer, destLen));
}

unsigned char *VoiceEncoder::flushAudioData(size_t &fileLen)
{
    fileLen = 0;
    unsigned char *pFinalData = new unsigned char[g_kFinalDataSize];
    size_t len = lame_encode_flush(m_pLame, pFinalData, g_kFinalDataSize);
    if(0 < len) {
        m_dataVector.push_back(std::pair<unsigned char*, size_t>(pFinalData, len));
    } else {
        delete []pFinalData;
        pFinalData = nullptr;
    }

    for(size_t i=0; i<m_dataVector.size(); ++i)  {
        fileLen += m_dataVector[i].second;
    }
    if(0 == fileLen) {
        return nullptr;
    }
    unsigned char *pFileData = new unsigned char[fileLen];
    size_t pos = 0;
    for(size_t i=0; i<m_dataVector.size(); ++i) {
        memcpy(pFileData + pos, m_dataVector[i].first, m_dataVector[i].second);
        delete []m_dataVector[i].first;
        pos += m_dataVector[i].second;
    }
    m_dataVector.clear();
    return pFileData;
}

float VoiceEncoder::getAudioLength()
{
    return (float)m_sampleCount / (float)m_sampleRate;
}

void VoiceEncoder::reset()
{
    m_sampleCount = 0;
    for(size_t i=0; i<m_dataVector.size(); ++i) {
        CC_SAFE_DELETE_ARRAY(m_dataVector[i].first);
    }
    m_dataVector.clear();

    lame_close(m_pLame);
    m_pLame = nullptr;
    _initLame();
}