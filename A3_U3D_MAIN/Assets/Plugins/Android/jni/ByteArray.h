#pragma once
#include "./Defines.h"
#include <string>

class ByteArray
{
public:
    enum
    {
        kLittleEndian = 0,
        kBigEndian = 1,
    };

	ByteArray(unsigned char *pBuf, size_t size, size_t offset = 0);
	~ByteArray();

    unsigned char *getBufferPtr(void) {
        return m_pBuf;
    }

	inline bool isEnd(void) {
		return m_pos >= m_size;
	}

    inline void setEndian(int endian) {
        m_endianFlag = endian == kLittleEndian ? kLittleEndian : kBigEndian;
    }

	inline void setPos(size_t p) {
		m_pos = p;
	}

	inline size_t getPos(void) {
		return m_pos;
	}

	unsigned char readUchar(void);
	char readChar(void);
	int readInt(void);
	unsigned int readUint(void);
	short readShort(void);
	unsigned short readUshort(void);
	float readFloat(void);
	double readDouble(void);
	std::string readString(size_t len);
	void readBytes(void *pDst, size_t size);

    void writeChar(char val);
    void writeUchar(unsigned char val);
    void writeInt(int val);
    void writeUint(unsigned int val);
    void writeShort(short val);
    void writeUshort(unsigned short val);
    void writeFloat(float val);
    void writeDouble(double val);
    void writeString(std::string val);
    void writeBytes(void *pSrc, size_t size);

protected:
	unsigned char *m_pBuf;
	size_t m_size;
	size_t m_pos;
    int m_endianFlag;
};