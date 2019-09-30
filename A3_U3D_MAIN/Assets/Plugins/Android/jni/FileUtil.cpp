#include "./Defines.h"
#include "./FileUtil.h"
#include "./MD5.h"
#include <vector>
#include <unordered_map>

#include <stdlib.h>
#include <sys/stat.h>
#if CC_TARGET_PLATFORM == CC_PLATFORM_WIN32
#include <Windows.h>
#include <io.h>
#else
#include <sys/types.h>
#include <errno.h>
#include <dirent.h>
#endif

static std::string g_cacheDir = "";
static std::string g_voiceDir = "";

static bool _isAbsolutePath(const std::string &path)
{
    return (!path.empty() && path[0] == '/');
}

static bool _isDirectoryExist(const std::string &curDir)
{
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
    unsigned long fAttrib = GetFileAttributesA(curDir.c_str());
    if(fAttrib != INVALID_FILE_ATTRIBUTES &&
       (fAttrib & FILE_ATTRIBUTE_DIRECTORY))
    {
        return true;
    }
    return false;
#else
    struct stat st;
    if(stat(curDir.c_str(), &st) == 0)
    {
        return S_ISDIR(st.st_mode);
    }
    return false;
#endif
}

static bool _createDirectory(const std::string &path)
{
    if(_isDirectoryExist(path))
        return true;

    // Split the path
    size_t start = 0;
    size_t found = path.find_first_of("/\\", start);
    std::string subpath;
    std::vector<std::string> dirs;

    if(found != std::string::npos)
    {
        while(true)
        {
            subpath = path.substr(start, found - start + 1);
            if(!subpath.empty())
                dirs.push_back(subpath);
            start = found+1;
            found = path.find_first_of("/\\", start);
            if(found == std::string::npos)
            {
                if(start < path.length())
                {
                    dirs.push_back(path.substr(start));
                }
                break;
            }
        }
    }


#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
    if((GetFileAttributesA(path.c_str())) == INVALID_FILE_ATTRIBUTES)
    {
        subpath = "";
        for(int i = 0; i < dirs.size(); ++i)
        {
            subpath += dirs[i];
            if(!_isDirectoryExist(subpath))
            {
                BOOL ret = CreateDirectoryA(subpath.c_str(), NULL);
                if(!ret && ERROR_ALREADY_EXISTS != GetLastError())
                {
                    return false;
                }
            }
        }
    }
    return true;
#else
    DIR *dir = NULL;

    // Create path recursively
    subpath = "";
    for(int i = 0; i < dirs.size(); ++i)
    {
        subpath += dirs[i];
        dir = opendir(subpath.c_str());

        if(!dir)
        {
            // directory doesn't exist, should create a new one

            int ret = mkdir(subpath.c_str(), S_IRWXU | S_IRWXG | S_IRWXO);
            if(ret != 0 && (errno != EEXIST))
            {
                // current directory can not be created, sub directories can not be created too
                // should return
                return false;
            }
        }
        else
        {
            // directory exists, should close opened dir
            closedir(dir);
        }
    }
    return true;
#endif
}

void FileUtil::initialize(const char *cacheDir)
{
    g_cacheDir = cacheDir ? cacheDir : "";
    g_voiceDir = g_cacheDir + "/" + "VoiceData";
}

void FileUtil::destroy()
{
}

bool FileUtil::isFileExist(const char *pFilePath)
{
    if(!pFilePath) {
        return false;
    }

    bool isFound = false;
    FILE *fp = fopen(pFilePath, "r");
    if(fp)
    {
        isFound = true;
        fclose(fp);
    }

    return isFound;
}

std::string FileUtil::getVoiceDir()
{
    return g_voiceDir;
}

std::string FileUtil::getVoicePath(const std::string &srcUrl)
{
    if(srcUrl.empty()) {
        return std::string("");
    }
    //We use MD5 value of fileUrl as voice file name.
    std::string fileName = md5Hash((unsigned char*)srcUrl.c_str(), srcUrl.size()) + ".mp3";
    return (g_voiceDir + "/" + fileName);
}

bool FileUtil::saveFileData(const std::string &filePath, unsigned char *pFileData, size_t len)
{
    if(filePath.empty() || nullptr==pFileData|| 0==len) {
        return 0;
    }

    if(!FileUtil::isFileExist(filePath.c_str())) {
        std::string cpfilename = filePath;
        size_t pos = cpfilename.find_last_of("/");
        std::string strdirname = cpfilename.substr(0, pos);
        std::string strfilename = cpfilename.substr(pos + 1, cpfilename.size() - 1);

        if(!_isDirectoryExist(strdirname)) {
            if(!_createDirectory(strdirname)) {
            }
        }
    }

    FILE *pFile = fopen(filePath.c_str(), "wb");
    if(nullptr == pFile) {
        return 0;
    }
    size_t fileSize = fwrite(pFileData, 1, len, pFile);
    fclose(pFile);
    return fileSize;
}

void FileUtil::deleteDirectory(const char *curPath)
{
    if(!curPath) {
        return;
    }
    std::string path = curPath;
#if(CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
        std::string command = "cmd /c rd /s /q ";
    // Path may include space.
    command += "\"" + path + "\"";

    if (WinExec(command.c_str(), SW_HIDE) > 31)
        return true;
    else
        return false;
#elif (CC_TARGET_PLATFORM == CC_PLATFORM_IOS) || (CC_TARGET_PLATFORM == CC_PLATFORM_MAC)
    nftw(path.c_str(), unlink_cb, 64, FTW_DEPTH | FTW_PHYS);
#else
    std::string command = "rm -r ";
    // Path may include space.
    command += "\"" + path + "\"";
    system(command.c_str());
#endif
}

void FileUtil::deleteFile(const char *filePath)
{
    if(filePath) {
        remove(filePath);
    }
}