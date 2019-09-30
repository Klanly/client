#include "./Defines.h"
#include "VoiceChat.h"
#include "./FileUtil.h"
#include "./VoiceEncoder.h"
#include "./UrlRequestUtil.h"
#include "./picojson.h"
#include "JniLanguageDock.h"

static VoiceChat *g_pVoiceChat = nullptr;

VoiceChat::VoiceChat()
:m_pVoiceEncoder(nullptr)
,m_isInitialized(false)
{
}

VoiceChat::~VoiceChat()
{
    CC_SAFE_DELETE(m_pVoiceEncoder);
}

VoiceChat *VoiceChat::getInstance()
{
    if(!g_pVoiceChat) {
        g_pVoiceChat = new VoiceChat();
    }
    return g_pVoiceChat;
}

void VoiceChat::destroyInstance()
{
    CC_SAFE_DELETE(g_pVoiceChat);
}

void VoiceChat::initialize(const char *cacheDir, int sampleRate, int channelNum, int bitsPerSample)
{
    FileUtil::initialize(cacheDir);
    CC_SAFE_DELETE(m_pVoiceEncoder);
    m_pVoiceEncoder = new VoiceEncoder();
    m_pVoiceEncoder->initialize(sampleRate, channelNum, bitsPerSample);
    m_isInitialized = true;
}

void VoiceChat::onRecord(unsigned char *pAudioData, size_t len)
{
    if(m_pVoiceEncoder) {
        m_pVoiceEncoder->pushAudioData(pAudioData, len);
    }
}

void VoiceChat::uploadVoice(const char *url, const char *postData)
{
	//TODO: test
	consoleLog("%s:test----------", __FUNCTION__);
    if(!m_pVoiceEncoder) {
        _onUploadVoice("", 0);
        return;
    }

    std::string voiceUrl = "";
    int seconds = 0;
    do {
        if(!url || !postData) {
            break;
        }
        size_t len = 0;
        unsigned char *pVoiceData = m_pVoiceEncoder->flushAudioData(len);
        if(nullptr == pVoiceData || 0 == len) {
            consoleLog("%s:Failed to upload. Voice data is empty!", __FUNCTION__);
            CC_SAFE_DELETE_ARRAY(pVoiceData);
            break;
        }
        char helperBuffer[32] ={0};
		//Max voice second is 31.
		seconds = (int)m_pVoiceEncoder->getAudioLength();
        if(31 < seconds) {
        	seconds = 0;
        	break;
        }
		
        sprintf(helperBuffer, "&ext=mp3&sec=%d", (int)m_pVoiceEncoder->getAudioLength());
        std::string finalPost = std::string(postData) + helperBuffer;
        size_t retLen = 0;
        unsigned char *pBuffer = UrlRequestUtil::uploadData(url, finalPost.c_str(), pVoiceData, len, retLen);
        if(nullptr == pBuffer || 0 == retLen) {
            CC_SAFE_DELETE_ARRAY(pBuffer);
            consoleLog("%s:Failed to upload. Unable to connect voice server!", __FUNCTION__);
            break;
        }
        //Parse require result.
        picojson::value jsonNode;
        const char *iterPos = (const char*)pBuffer;
        const char *iterEnd = iterPos + retLen;
        std::string jsonError = picojson::parse(jsonNode, iterPos, iterEnd);
        if(!jsonError.empty() || !jsonNode.is<picojson::object>()) {
            consoleLog("%s:Failed to upload. Response message is invalid!", __FUNCTION__);
            CC_SAFE_DELETE_ARRAY(pBuffer);
            break;
        }
        const picojson::value &resCode = jsonNode.get("r");
        const picojson::value &resUrl = jsonNode.get("url");
        if(!resCode.is<double>() || resCode.get<double>()!=1 || !resUrl.is<std::string>()) {
            consoleLog("%s:Failed to upload. Server denied!", __FUNCTION__);
            CC_SAFE_DELETE_ARRAY(pBuffer);
            break;
        }
        voiceUrl = resUrl.get<std::string>();
        CC_SAFE_DELETE_ARRAY(pBuffer);
        std::string filePath = FileUtil::getVoicePath(voiceUrl);
        retLen = FileUtil::saveFileData(filePath.c_str(), pVoiceData, len);
        if(0 == retLen) {
            consoleLog("%s:Failed to upload. Unable to save the voice file!", __FUNCTION__);
            break;
        }
        seconds = (int)m_pVoiceEncoder->getAudioLength();
		
    } while(false);

    _onUploadVoice(voiceUrl.c_str(), seconds);
    m_pVoiceEncoder->reset();
}

void VoiceChat::downloadVoice(const char *url, const char *postData)
{
    if(!m_isInitialized) {
        _onDownloadVoice(nullptr);
        return;
    }
    std::string voicePath = FileUtil::getVoicePath(url);
    if(voicePath.empty())  {
        _onDownloadVoice(nullptr);
        return;
    }

    bool isComplete = false;
    do 
    {
        if(FileUtil::isFileExist(voicePath.c_str())) {
            isComplete = true;
            break;
        }
        size_t dataLen = 0;
        unsigned char *pVoiceData = UrlRequestUtil::getUrlData(url, dataLen, postData, "post");
        if(nullptr == pVoiceData) {
            consoleLog("%s:Failed to download. Voice data is empty!", __FUNCTION__);
            break;
        }
        if(FileUtil::saveFileData(voicePath, pVoiceData, dataLen)) {
            isComplete = true;
            break;
        }
        consoleLog("%s:Failed to download. Unable to save the voice data!", __FUNCTION__);
    } while (false);

    _onDownloadVoice(isComplete ? voicePath.c_str() : nullptr);
}

void VoiceChat::deleteVoice(const char *url)
{
    if(m_isInitialized && url) {
        std::string filePath = FileUtil::getVoicePath(url);
        if(!filePath.empty()) {
            FileUtil::deleteFile(filePath.c_str());
        }
    }
}

void VoiceChat::deleteAllVoice()
{
    if(m_isInitialized) {
        FileUtil::deleteDirectory(FileUtil::getVoiceDir().c_str());
    }
}

void VoiceChat::_onUploadVoice(const char *voiceUrl, int seconds)
{
    JniLanguageDock::onUploadVoice(voiceUrl, seconds);
}

void VoiceChat::_onDownloadVoice(const char *voicePath)
{
    JniLanguageDock::onDownloadVoice(voicePath);
}