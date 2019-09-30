#include "Defines.h"
#include "./JniLanguageDock.h"
#include "./android/JniHelper.h"
#include "./VoiceChat.h"

#define JNI_CLASS_NAME "com/quwei/voicechat/VoiceDock"

void JniLanguageDock::onUploadVoice(const char *voiceUrl, int seconds)
{
    JniMethodInfo methodInfo;
    if(JniHelper::getStaticMethodInfo(methodInfo,
        JNI_CLASS_NAME, "onUploadVoice",
        "(Ljava/lang/String;I)V"))
    {
        jstring str0 = methodInfo.env->NewStringUTF(voiceUrl ? voiceUrl : "");
        methodInfo.env->CallStaticVoidMethod(methodInfo.classID, methodInfo.methodID, str0, (jint)seconds);
        methodInfo.env->DeleteLocalRef(str0);
        methodInfo.env->DeleteLocalRef(methodInfo.classID);
    }
}

void JniLanguageDock::onDownloadVoice(const char *voicePath)
{
    JniMethodInfo methodInfo;
    if(JniHelper::getStaticMethodInfo(methodInfo,
        JNI_CLASS_NAME, "onDownloadVoice",
        "(Ljava/lang/String;)V"))
    {
        jstring str0 = methodInfo.env->NewStringUTF(voicePath ? voicePath : "");
        methodInfo.env->CallStaticVoidMethod(methodInfo.classID, methodInfo.methodID, str0);
        methodInfo.env->DeleteLocalRef(str0);
        methodInfo.env->DeleteLocalRef(methodInfo.classID);
    }
}

static unsigned char *g_pAudioBuffer = nullptr;
static size_t g_bufferSize = 512;

static unsigned char * getAudioBuffer(size_t len)
{
    if(0 == len) {
        return nullptr;
    }

    bool isNeedToNew = false;
    if(g_bufferSize < len) {
        g_bufferSize = (len / g_bufferSize + 1) * g_bufferSize;
        isNeedToNew = true;
    }
    if(!g_pAudioBuffer) {
        isNeedToNew = true;
    }
    if(isNeedToNew) {
        CC_SAFE_DELETE_ARRAY(g_pAudioBuffer);
        g_pAudioBuffer = new unsigned char[g_bufferSize];
        memset(g_pAudioBuffer, 0, g_bufferSize);
    }
    return g_pAudioBuffer;
}

//#ifdef __cpluscplus
extern "C"
{
//#endif

jint JNI_OnLoad(JavaVM *vm, void *reserved)
{
    JniHelper::setJavaVM(vm);
    return JNI_VERSION_1_4;
}

void Java_com_quwei_voicechat_VoiceDock_initializeJni(JNIEnv*  env, jobject thiz, jobject context)
{
    JniHelper::setClassLoaderFrom(context);
}

void Java_com_quwei_voicechat_VoiceDock_initVoiceChat(JNIEnv *env, jobject thiz, jstring val0, jint val1, jint val2, jint val3)
{
    if(!env) {
        return;
    }

    const char *cacheDir = nullptr;
    if(nullptr != val0) {
        cacheDir = env->GetStringUTFChars(val0, NULL);
    }
    VoiceChat::getInstance()->initialize(cacheDir, (int)val1, (int)val2, (int)val3);
    if(nullptr != val0) {
        env->ReleaseStringUTFChars(val0, cacheDir);
    }
}

void Java_com_quwei_voicechat_VoiceDock_destroyVoiceChat(JNIEnv *env, jobject thiz)
{
    VoiceChat::destroyInstance();
    CC_SAFE_DELETE_ARRAY(g_pAudioBuffer);
}

void Java_com_quwei_voicechat_VoiceDock_onRecord(JNIEnv *env, jobject thiz, jbyteArray buffer, jint len)
{
    if(!env || 0>=len) {
        return;
    }
    unsigned char *pAudioBuffer = getAudioBuffer((size_t)len);
    env->GetByteArrayRegion(/*env, */buffer, (jsize)0, (jsize)len, (jbyte*)pAudioBuffer);
    VoiceChat::getInstance()->onRecord(pAudioBuffer, (size_t)len);
}

void Java_com_quwei_voicechat_VoiceDock_uploadVoice(JNIEnv *env, jobject thiz, jstring val0, jstring val1)
{
    if(!env) {
        return;
    }
    const char *url = nullptr;
    const char *postData = nullptr;
    if(nullptr != val0) {
        url = env->GetStringUTFChars(val0, NULL);
    }
    if(nullptr != val1) {
        postData = env->GetStringUTFChars(val1, NULL);
    }
    VoiceChat::getInstance()->uploadVoice(url, postData);
    if(nullptr != val0) {
        env->ReleaseStringUTFChars(val0, url);
    }
    if(nullptr != val1) {
        env->ReleaseStringUTFChars(val1, postData);
    }
}

void Java_com_quwei_voicechat_VoiceDock_downloadVoice(JNIEnv *env, jobject thiz, jstring val0, jstring val1)
{
    if(!env) {
        return;
    }
    const char *url = nullptr;
    const char *postData = nullptr;
    if(nullptr != val0) {
        url = env->GetStringUTFChars(val0, NULL);
    }
    if(nullptr != val1) {
        postData = env->GetStringUTFChars(val1, NULL);
    }
    VoiceChat::getInstance()->downloadVoice(url, postData);
    if(nullptr != val0) {
        env->ReleaseStringUTFChars(val0, url);
    }
    if(nullptr != val1) {
        env->ReleaseStringUTFChars(val1, postData);
    }
}

void Java_com_quwei_voicechat_VoiceDock_deleteVoice(JNIEnv *env, jobject thiz, jstring val0)
{
    if(!env) {
        return;
    }
    const char *url = nullptr;
    if(nullptr != val0) {
        url = env->GetStringUTFChars(val0, NULL);
    }
    VoiceChat::getInstance()->deleteVoice(url);
    if(nullptr != val0) {
        env->ReleaseStringUTFChars(val0, url);
    }
}

void Java_com_quwei_voicechat_VoiceDock_deleteAllVoice(JNIEnv *env, jobject thiz)
{
    VoiceChat::getInstance()->deleteAllVoice();
}

//#ifdef __cpluscplus
}
//#endif