#include "ByteArray.h"

ByteArray::ByteArray(unsigned char *pBuf, size_t size, size_t offset)
{
	m_pBuf = pBuf;
	m_size = size;
	m_pos = offset;
    m_endianFlag = kLittleEndian;
}

ByteArray::~ByteArray()
{

}

unsigned char ByteArray::readUchar(void)
{
	return m_pBuf[m_pos++];
}

char ByteArray::readChar(void)
{
	char r = *(char*)(m_pBuf + m_pos);
	m_pos++;
	return r;
}

int ByteArray::readInt(void)
{
    int ret = 0;
    if(kLittleEndian == m_endianFlag) {
        ret = *(int*)(m_pBuf + m_pos);
    } else {
        char pBuffer[4];
        pBuffer[0] = m_pBuf[m_pos + 3];
        pBuffer[1] = m_pBuf[m_pos + 2];
        pBuffer[2] = m_pBuf[m_pos + 1];
        pBuffer[3] = m_pBuf[m_pos + 0];
        ret = *(int*)pBuffer;
    }

	m_pos += 4;
	return ret;
}

unsigned int ByteArray::readUint(void)
{
    unsigned int ret = 0;
    if(kLittleEndian == m_endianFlag) {
        ret = *(unsigned int*)(m_pBuf + m_pos);
    } else {
        unsigned char pBuffer[4];
        pBuffer[0] = m_pBuf[m_pos + 3];
        pBuffer[1] = m_pBuf[m_pos + 2];
        pBuffer[2] = m_pBuf[m_pos + 1];
        pBuffer[3] = m_pBuf[m_pos + 0];
        ret = *(unsigned int*)pBuffer;
    }

    m_pos += 4;
    return ret;
}

short ByteArray::readShort(void)
{
    short ret = 0;
    if(kLittleEndian == m_endianFlag) {
        ret = *(short*)(m_pBuf + m_pos);
    } else {
        unsigned char pBuffer[2];
        pBuffer[0] = m_pBuf[m_pos + 1];
        pBuffer[1] = m_pBuf[m_pos + 0];
        ret = *(short*)pBuffer;
    }
    
	m_pos += 2;
	return ret;
}

unsigned short ByteArray::readUshort(void)
{
    unsigned short ret = 0;
    if(kLittleEndian == m_endianFlag) {
        ret = *(unsigned short*)(m_pBuf + m_pos);
    } else {
        unsigned char pBuffer[2];
        pBuffer[0] = m_pBuf[m_pos + 1];
        pBuffer[1] = m_pBuf[m_pos + 0];
        ret = *(unsigned short*)pBuffer;
    }

	m_pos += 2;
	return ret;
}

float ByteArray::readFloat(void)
{
    float ret = 0.0f;
    if(kLittleEndian == m_endianFlag) {
        ret = *(float*)(m_pBuf + m_pos);
    } else {
        unsigned char pBuffer[4];
        pBuffer[0] = m_pBuf[m_pos + 3];
        pBuffer[1] = m_pBuf[m_pos + 2];
        pBuffer[2] = m_pBuf[m_pos + 1];
        pBuffer[3] = m_pBuf[m_pos + 0];
        ret = *(float*)pBuffer;
    }

	m_pos += 4;
	return ret;
}

double ByteArray::readDouble(void)
{
    double ret = 0.0;
    if(kLittleEndian == m_endianFlag) {
        ret = *(double*)(m_pBuf + m_pos);
    } else {
        unsigned char pBuffer[8];
        for(int i=0; i<8; ++i) {
            pBuffer[i] = m_pBuf[m_pos + 8 - 1 - i];
        }
        ret = *(double*)pBuffer;
    }

	m_pos += 8;
	return ret;
}

std::string ByteArray::readString(size_t len)
{
	std::string s;
	for (size_t i = m_pos; i < m_pos + len; ++i)
		s += m_pBuf[i];
	m_pos += len;
	return s;
}

void ByteArray::readBytes(void *pDst, size_t size)
{
	memcpy(pDst, m_pBuf + m_pos, size);
	m_pos += size;
}

//////////////////////////////////////////////////////////////////////////
///TODO:
///Little endian / big endian for write.
//////////////////////////////////////////////////////////////////////////

void ByteArray::writeChar(char val)
{
    *(char*)(m_pBuf + m_pos) = val;
    ++m_pos;
}

void ByteArray::writeUchar(unsigned char val)
{
    *(unsigned char*)(m_pBuf + m_pos) = val;
    ++m_pos;
}

void ByteArray::writeInt(int val)
{
    *(int*)(m_pBuf + m_pos) = val;
    m_pos += 4;
}

void ByteArray::writeUint(unsigned int val)
{
    *(unsigned int*)(m_pBuf + m_pos) = val;
    m_pos += 4;
}

void ByteArray::writeShort(short val)
{
    *(short*)(m_pBuf + m_pos) = val;
    m_pos += 2;
}

void ByteArray::writeUshort(unsigned short val)
{
    *(unsigned short*)(m_pBuf + m_pos) = val;
    m_pos += 2;
}

void ByteArray::writeFloat(float val)
{
    *(float*)(m_pBuf + m_pos) = val;
    m_pos += 4;
}

void ByteArray::writeDouble(double val)
{
    *(double*)(m_pBuf + m_pos) = val;
    m_pos += 8;
}

void ByteArray::writeString(std::string val)
{
    memcpy(m_pBuf+m_pos, val.c_str(), val.size());
    m_pos += val.size();
}

void ByteArray::writeBytes(void *pSrc, size_t size)
{
    memcpy(m_pBuf+m_pos, pSrc, size);
    m_pos += size;
}
