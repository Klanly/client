#include <jni.h>
#include <android/log.h>

#define LOG_TAG "--TestFunc--"
#define  LOGD(...)  __android_log_print(ANDROID_LOG_DEBUG,LOG_TAG,__VA_ARGS__)
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)

void Java_com_quwei_voicechat_VoiceDock_initializeTest(JNIEnv *env, jobject thiz)
{
    LOGE("%s:1", __FUNCTION__);
}