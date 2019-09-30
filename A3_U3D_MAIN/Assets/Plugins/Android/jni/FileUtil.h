#pragma once
#include "./Defines.h"
#include <string>

class FileUtil
{
private:
    FileUtil();
    ~FileUtil();

public:
    static void initialize(const char *cacheDir);

    static void destroy();

    static bool isFileExist(const char *pFilePath);

    /// 获得语音消息保存的目录
    static std::string getVoiceDir();

    /// 根据语音消息URL获得本地文件路径
    /// 内部实现以srcUrl的MD5字符串作为音频文件名
    /// @param srcUrl 语音消息url
    /// @return 音频文件路径
    static std::string getVoicePath(const std::string &srcUrl);

    /// 将数据保存到文件
    static bool saveFileData(const std::string &filePath, unsigned char *pFileData, size_t len);

    /// 删除目录
    static void deleteDirectory(const char *curPath);

    /// 删除文件
    static void deleteFile(const char *filePath);
};