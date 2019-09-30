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

    /// ���������Ϣ�����Ŀ¼
    static std::string getVoiceDir();

    /// ����������ϢURL��ñ����ļ�·��
    /// �ڲ�ʵ����srcUrl��MD5�ַ�����Ϊ��Ƶ�ļ���
    /// @param srcUrl ������Ϣurl
    /// @return ��Ƶ�ļ�·��
    static std::string getVoicePath(const std::string &srcUrl);

    /// �����ݱ��浽�ļ�
    static bool saveFileData(const std::string &filePath, unsigned char *pFileData, size_t len);

    /// ɾ��Ŀ¼
    static void deleteDirectory(const char *curPath);

    /// ɾ���ļ�
    static void deleteFile(const char *filePath);
};