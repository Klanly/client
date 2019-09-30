#pragma once
#include "./Defines.h"
#include <string>

class UrlRequestUtil
{
private:
    UrlRequestUtil(void);
    ~UrlRequestUtil(void);

public:
    static double getUrlDataLen(const char *url);
    /// @param url      Request url
    /// @param param    Post field.
    /// @param method   Request method : "post", "get"...
    static unsigned char *getUrlData(const char *url, size_t &dataLen, const char *param = nullptr, const char *method = "get");
    /// @param url      Request url
    /// @param param    Post field.
    /// @param method   Request method : "post", "get"...
    /// @note This method will check data length before request it.
    static unsigned char *downloadUrlData(const char *url, size_t &dataLen, const char *param = nullptr, const char *method = "get");
    /// @param url      Request url
    /// @param param    Post field
    /// @param pData    Data buffer to upload
    /// @param len      Data size
    static unsigned char * uploadData(const char *url, const char *param, unsigned char *pData, size_t srcLen, size_t &retLen);
};
