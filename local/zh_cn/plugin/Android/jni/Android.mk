LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

#LOCAL_MODULE := voice_static_static
LOCAL_MODULE := VoiceChat

LOCAL_MODULE_FILENAME := libVoiceChat

LOCAL_CFLAGS := -std=c++11 -D__ANDROID__
LOCAL_CFLAGS += -DHAVE_CONFIG_H
LOCAL_CFLAGS += -DUNIX
LOCAL_CFLAGS += -w -frtti -fexceptions
LOCAL_CFLAGS +=  -DUSE_FILE32API
LOCAL_CPPFLAGS := -Wno-deprecated-declarations -Wno-extern-c-compat
LOCAL_EXPORT_CFLAGS   := -DUSE_FILE32API
LOCAL_EXPORT_CPPFLAGS := -Wno-deprecated-declarations -Wno-extern-c-compat

LOCAL_SRC_FILES := \
android/JniHelper.cpp \
lame/lame.c \
lame/set_get.c \
lame/bitstream.c \
lame/encoder.c \
lame/fft.c \
lame/gain_analysis.c \
lame/id3tag.c \
lame/mpglib_interface.c \
lame/newmdct.c \
lame/presets.c \
lame/psymodel.c \
lame/quantize.c \
lame/quantize_pvt.c \
lame/reservoir.c \
lame/tables.c \
lame/takehiro.c \
lame/util.c \
lame/vbrquantize.c \
lame/VbrTag.c \
lame/version.c \
ByteArray.cpp \
Defines.cpp \
FileUtil.cpp \
JniLanguageDock.cpp \
Md5.cpp \
StringUtility.cpp \
UrlRequestUtil.cpp \
VoiceChat.cpp \
VoiceEncoder.cpp

LOCAL_STATIC_LIBRARIES := cocos_curl_static

LOCAL_LDLIBS := -llog \
                -lz \
                -landroid

ifeq ($(TARGET_ARCH_ABI),armeabi-v7a)
LOCAL_ARM_NEON  := true
endif

#include $(BUILD_STATIC_LIBRARY)
include $(BUILD_SHARED_LIBRARY)

$(call import-module, curl/prebuilt)
