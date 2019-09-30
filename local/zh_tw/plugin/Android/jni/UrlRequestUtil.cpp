#include "./Defines.h"
#include "UrlRequestUtil.h"
#include "StringUtility.h"
#include "ByteArray.h"
#include "android/curl/include/curl.h"
#include <vector>

size_t curlBufferCallback(void *pData, size_t blockSize, size_t count, void *pUser);
size_t curlBlockCallback(void *pData, size_t blockSize, size_t count, void *pUser);
size_t curlNullCallback(void *pData, size_t size, size_t count, void *pUser);

struct CurlDataBlock
{
    std::vector<int> blockSize;
    std::vector<unsigned char*> blockBuffers;
    CurlDataBlock() {}
    ~CurlDataBlock()
    {
        for(size_t i=0; i<blockBuffers.size(); ++i) {
            CC_SAFE_DELETE_ARRAY(blockBuffers[i]);
        }
    }
};

double UrlRequestUtil::getUrlDataLen(const char *url)
{
    if(nullptr == url) {
        return 0;
    }

    double dataLen = 0;
    CURL *pCurl = curl_easy_init();
    curl_easy_setopt(pCurl, CURLOPT_URL, url);
    curl_easy_setopt(pCurl, CURLOPT_HEADER, 1);
    curl_easy_setopt(pCurl, CURLOPT_NOBODY, 1);
    curl_easy_setopt(pCurl, CURLOPT_WRITEFUNCTION, curlNullCallback);
    if(CURLE_OK == curl_easy_perform(pCurl)) {
        curl_easy_getinfo(pCurl, CURLINFO_CONTENT_LENGTH_DOWNLOAD, &dataLen);
    }
    curl_easy_cleanup(pCurl);

    return dataLen;
}

static unsigned char *getFullBufferData(CURL *pCurl, size_t &dataLen)
{
    if(!pCurl) {
        dataLen = 0;
        return nullptr;
    }
    
    unsigned char *pBuffer = new unsigned char[dataLen];
    memset(pBuffer,0,dataLen);
    ByteArray byteArray(pBuffer,dataLen);
    byteArray.setPos(0);

    curl_easy_setopt(pCurl,CURLOPT_WRITEFUNCTION, curlBufferCallback);
    curl_easy_setopt(pCurl,CURLOPT_WRITEDATA, &byteArray);
    curl_easy_setopt(pCurl,CURLOPT_CONNECTTIMEOUT_MS, 0);
    CURLcode resCode = curl_easy_perform(pCurl);
    CURLINFO resInfo;
    curl_easy_getinfo(pCurl,CURLINFO_RESPONSE_CODE, &resInfo);
    curl_easy_cleanup(pCurl);
    if(CURLE_OK != resCode || 200 != resInfo) {
        CC_SAFE_DELETE_ARRAY(pBuffer);
        dataLen = 0;
    }

    return pBuffer;
}

static unsigned char *getBlockData(CURL *pCurl, size_t &dataLen)
{
    if(!pCurl) {
        dataLen = 0;
        return nullptr;
    }
#ifndef __arm64__
    CurlDataBlock dataBlock;
    curl_easy_setopt(pCurl, CURLOPT_WRITEFUNCTION, curlBlockCallback);
    curl_easy_setopt(pCurl, CURLOPT_WRITEDATA, &dataBlock);
    curl_easy_setopt(pCurl, CURLOPT_CONNECTTIMEOUT_MS, 0);
    CURLcode resCode = curl_easy_perform(pCurl);
    CURLINFO resInfo;
    curl_easy_getinfo(pCurl, CURLINFO_RESPONSE_CODE, &resInfo);
    curl_easy_cleanup(pCurl);
    if(CURLE_OK == resCode && 200 == resInfo) {
        for(size_t i=0; i<dataBlock.blockSize.size(); ++i) {
            dataLen += dataBlock.blockSize[i];
        }
        unsigned char *pBuffer = new unsigned char[dataLen];
        unsigned char *pPos = pBuffer;
        for(size_t i=0; i<dataBlock.blockBuffers.size(); ++i) {
            memcpy(pPos, dataBlock.blockBuffers[i], dataBlock.blockSize[i]);
            pPos += dataBlock.blockSize[i];
        }
        return pBuffer;
    }
#else
    CurlDataBlock *pDataBlock = new CurlDataBlock();
    curl_easy_setopt(pCurl, CURLOPT_WRITEFUNCTION, curlBlockCallback);
    curl_easy_setopt(pCurl, CURLOPT_WRITEDATA, pDataBlock);
    curl_easy_setopt(pCurl, CURLOPT_CONNECTTIMEOUT_MS, 0);
    CURLcode resCode = curl_easy_perform(pCurl);
    CURLINFO resInfo;
    curl_easy_getinfo(pCurl,CURLINFO_RESPONSE_CODE, &resInfo);
    curl_easy_cleanup(pCurl);
    if(CURLE_OK == resCode && 200 == resInfo) {
        for(size_t i=0; i<pDataBlock->blockSize.size(); ++i) {
            dataLen += pDataBlock->blockSize[i];
        }
        unsigned char *pBuffer = new unsigned char[dataLen];
        unsigned char *pPos = pBuffer;
        for(size_t i=0; i<pDataBlock->blockBuffers.size(); ++i) {
            memcpy(pPos, pDataBlock->blockBuffers[i], pDataBlock->blockSize[i]);
            pPos += pDataBlock->blockSize[i];
        }
        CC_SAFE_DELETE(pDataBlock);
        return pBuffer;
    }
    CC_SAFE_DELETE(pDataBlock);
#endif

    return nullptr;
}

unsigned char *UrlRequestUtil::getUrlData(const char *url, size_t &dataLen, const char *param, const char *method)
{
    unsigned char *pBuffer = nullptr;
    dataLen = 0;
    do {
        if(!url || !method) {
            break;
        }
        std::string curMethod = method;
        std::string fullUrl = url;
        if(param && strlen(param) > 0) {
            fullUrl += "?";
            fullUrl += param;
        }

        CURL *pCurl = curl_easy_init();
        if(!pCurl) {
            dataLen = 0;
            break;
        }
        
        if(s2l(curMethod) == "post") {
            curl_easy_setopt(pCurl, CURLOPT_POST, 1);
            curl_easy_setopt(pCurl, CURLOPT_URL, url);
            curl_easy_setopt(pCurl, CURLOPT_POSTFIELDS, param);
        }
        else if(s2l(curMethod) == "get") {
            curl_easy_setopt(pCurl, CURLOPT_URL, fullUrl.c_str());
        } else {
            break;
        }
        pBuffer = getBlockData(pCurl, dataLen);
    } while(false);

    return pBuffer;
}

unsigned char *UrlRequestUtil::downloadUrlData(const char *url, size_t &dataLen, const char *param, const char *method)
{
    unsigned char *pBuffer = nullptr;
    dataLen = 0;
    do {
        if(!url || !method) {
            break;
        }
        std::string curMethod = method;
        std::string fullUrl = url;
        if(param && strlen(param) > 0) {
            fullUrl += "?";
            fullUrl += param;
        }
        double len = getUrlDataLen(fullUrl.c_str());
        if(0.0 == len) {
            break;
        }
        dataLen = len > 0.0 ? (size_t)len : 0;

        CURL *pCurl = curl_easy_init();
        if(!pCurl) {
            dataLen = 0;
            break;
        }
        
        if(s2l(curMethod) == "post") {
            curl_easy_setopt(pCurl, CURLOPT_POST, 1);
            curl_easy_setopt(pCurl, CURLOPT_URL, url);
            curl_easy_setopt(pCurl, CURLOPT_POSTFIELDS, param);
        }
        else if(s2l(curMethod) == "get") {
            curl_easy_setopt(pCurl, CURLOPT_URL, fullUrl.c_str());
        } else {
            break;
        }

        if(dataLen > 0) {
            pBuffer = getFullBufferData(pCurl, dataLen);
        } else {
            pBuffer = getBlockData(pCurl, dataLen);
        }
    } while(false);

    return pBuffer;
}

unsigned char* UrlRequestUtil::uploadData(const char *url, const char *param, unsigned char *pData, size_t srcLen, size_t &retLen)
{
    if(nullptr==url || nullptr==param || nullptr==pData || 0==srcLen) {
        return nullptr;
    }

    curl_global_init(CURL_GLOBAL_ALL);
    struct curl_httppost *pHttpPost = nullptr;
    struct curl_httppost *pLastPost = nullptr;
    curl_formadd(&pHttpPost, &pLastPost,
                CURLFORM_PTRNAME, "param",
                CURLFORM_PTRCONTENTS, param,
                CURLFORM_CONTENTSLENGTH, strlen(param),
                CURLFORM_END);
    curl_formadd(&pHttpPost, &pLastPost,
                CURLFORM_PTRNAME, "data",
                CURLFORM_PTRCONTENTS, pData,
                CURLFORM_CONTENTSLENGTH, srcLen,
                CURLFORM_END);
    CURL *pCurl = curl_easy_init();
    curl_easy_setopt(pCurl, CURLOPT_URL, url);
    curl_easy_setopt(pCurl, CURLOPT_HTTPPOST, pHttpPost);
#if 0
    CURLcode resCode = curl_easy_perform(pCurl);
    curl_formfree(pHttpPost);
    CURLINFO resInfo;
    curl_easy_getinfo(pCurl, CURLINFO_RESPONSE_CODE, &resInfo);
    curl_easy_cleanup(pCurl);
    return (CURLE_OK == resCode && 200 == resInfo) ? true : false;
#else
    unsigned char *pRetBuffer = getBlockData(pCurl, retLen);
    curl_formfree(pHttpPost);
    return pRetBuffer;
#endif
}

size_t curlBufferCallback(void *pData, size_t blockSize, size_t count, void *pUser)
{
    size_t dataLen = blockSize * count;
    if(!pData || 0==dataLen || !pUser) {
        return 0;
    }
    if(((ByteArray*)pUser)->isEnd()) {
        return 0;
    }

    ((ByteArray*)pUser)->writeBytes(pData, dataLen);
    return dataLen;
}

size_t curlBlockCallback(void *pData, size_t blockSize, size_t count, void *pUser)
{
    size_t dataLen = blockSize * count;
    if(!pData || 0==dataLen || !pUser) {
        return dataLen;
    }
    
    unsigned char *pBuffer = new unsigned char[dataLen];
    memcpy(pBuffer, pData, dataLen);
    ((CurlDataBlock*)pUser)->blockSize.push_back(dataLen);
    ((CurlDataBlock*)pUser)->blockBuffers.push_back(pBuffer);
    return dataLen;
}

size_t curlNullCallback(void *pData, size_t size, size_t count, void *pUser)
{
    return size * count;
}
