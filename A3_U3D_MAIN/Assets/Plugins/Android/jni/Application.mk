APP_PLATFORM := android-19
APP_ABI := armeabi armeabi-v7a

APP_STL := gnustl_static

APP_CPPFLAGS := -frtti -std=gnu++11 -fsigned-char -D__ANDROID__
APP_LDFLAGS := -latomic

ifeq ($(NDK_DEBUG),1)
  APP_OPTIM := debug
else
  APP_CPPFLAGS += -DNDEBUG
  APP_OPTIM := release
endif
