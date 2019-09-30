#include "MD5.h"
#include <vector>
#include <map>


	/**
	* Auxiliary function f as defined in RFC
	*/
	inline int f(int x, int y, int z)
	{
		return (x & y) | ((~x) & z);
	}

	/**
	* Auxiliary function g as defined in RFC
	*/
	inline int g(int x, int y, int z)
	{
		return (x & z) | (y & (~z));
	}

	/**
	* Auxiliary function h as defined in RFC
	*/
	inline int h(int x, int y, int z)
	{
		return x ^ y ^ z;
	}

	/**
	* Auxiliary function i as defined in RFC
	*/
	inline int i(int x, int y, int z)
	{
		return y ^ (x | (~z));
	}

	inline int rol(int x, int n)
	{
		unsigned int ux = *(unsigned int*)&x;
		return (x << n) | (ux >> (32 - n));
	}

	/**
	* ff transformation function
	*/
	inline int ff(int a, int b, int c, int d, int x, int s, int t)
	{
		int transform = f(b, c, d);
		int tmp = a + transform + x + t;
		return rol(tmp, s) + b;
	}

	/**
	* gg transformation function
	*/
	inline int gg(int a, int b, int c, int d, int x, int s, int t)
	{
		int transform = g(b, c, d);
		int tmp = a + transform + x + t;
		return rol(tmp, s) + b;
	}

	/**
	* hh transformation function
	*/
	inline int hh(int a, int b, int c, int d, int x, int s, int t)
	{
		int transform = h(b, c, d);
		int tmp = a + transform + x + t;
		return rol(tmp, s) + b;
	}

	/**
	* ii transformation function
	*/
	inline int ii(int a, int b, int c, int d, int x, int s, int t)
	{
		int transform = i(b, c, d);
		int tmp = a + transform + x + t;
		return rol(tmp, s) + b;
	}

	int createBlocks(std::map<int, int> &blocks, unsigned char *pBuf, size_t size)
	{
		int len = size * 8;
		int mask = 0xFF; // ignore hi byte of characters > 0xFF
		for (int i = 0; i < len; i += 8)
		{
			int idx = int(i >> 5);
			blocks[idx] |= (pBuf[i / 8] & mask) << (i % 32);
		}

		// append padding and length
		blocks[int(len >> 5)] |= 0x80 << (len % 32);
		
		int tmp = len + 64;
		unsigned int utmp = *(unsigned int*)&tmp;
		blocks[int(((utmp >> 9) << 4) + 14)] = len;
        //Return loop times.
        return ((tmp>>9) + 1);
	}

	const std::string hexChars = "0123456789abcdef";

    static bool isBigEndian(void)
    {
        union testEndian
        {
            int val;
            char raw[4];
        };
        testEndian data;
        data.raw[0] = 0x01;
        data.raw[1] = 0x02;
        data.raw[2] = 0x03;
        data.raw[3] = 0x04;

        return data.val==0x01020304 ? true : false;
    }

    static inline void toHex(char *str, int n, bool bigEndian = false)
    {
        if (bigEndian) {
            for (int i = 0; i < 4; ++i) {
                str[i * 2 + 0] = hexChars[(n >> ((3 - i) * 8 + 4)) & 0xF];
                str[i * 2 + 1] = hexChars[(n >> ((3 - i) * 8)) & 0xF];
            }
        }
        else  {
            for (int i = 0; i < 4; ++i) {
                str[i * 2 + 0] = hexChars[(n >> (i * 8 + 4)) & 0xF];
                str[i * 2 + 1] = hexChars[(n >> (i * 8)) & 0xF];
            }
        }
    }

	std::string md5Hash(unsigned char *pBuf, size_t size)
	{
		// initialize the md buffers
		int a = 1732584193;
		int b = -271733879;
		int c = -1732584194;
		int d = 271733878;

		// variables to store previous values
		int aa, bb, cc, dd;

		// Create the blocks from the string and
		// get the looping times.
		std::map<int, int> x;
        int loopTimes = createBlocks(x, pBuf, size);

		// loop over all of the blocks
		for (int i = 0; i < loopTimes*16; i += 16) {
			// save previous values
			aa = a;
			bb = b;
			cc = c;
			dd = d;

			// Round 1
			a = ff(a, b, c, d, x[int(i + 0)], 7, -680876936); 	// 1
			d = ff(d, a, b, c, x[int(i + 1)], 12, -389564586);	// 2
			c = ff(c, d, a, b, x[int(i + 2)], 17, 606105819); 	// 3
			b = ff(b, c, d, a, x[int(i + 3)], 22, -1044525330);	// 4
			a = ff(a, b, c, d, x[int(i + 4)], 7, -176418897); 	// 5
			d = ff(d, a, b, c, x[int(i + 5)], 12, 1200080426); 	// 6
			c = ff(c, d, a, b, x[int(i + 6)], 17, -1473231341);	// 7
			b = ff(b, c, d, a, x[int(i + 7)], 22, -45705983); 	// 8
			a = ff(a, b, c, d, x[int(i + 8)], 7, 1770035416); 	// 9
			d = ff(d, a, b, c, x[int(i + 9)], 12, -1958414417);	// 10
			c = ff(c, d, a, b, x[int(i + 10)], 17, -42063); 		// 11
			b = ff(b, c, d, a, x[int(i + 11)], 22, -1990404162);	// 12
			a = ff(a, b, c, d, x[int(i + 12)], 7, 1804603682); 	// 13
			d = ff(d, a, b, c, x[int(i + 13)], 12, -40341101); 	// 14
			c = ff(c, d, a, b, x[int(i + 14)], 17, -1502002290);	// 15
			b = ff(b, c, d, a, x[int(i + 15)], 22, 1236535329); // 16

			// Round 2
			a = gg(a, b, c, d, x[int(i + 1)], 5, -165796510); 	// 17
			d = gg(d, a, b, c, x[int(i + 6)], 9, -1069501632);	// 18
			c = gg(c, d, a, b, x[int(i + 11)], 14, 643717713); 	// 19
			b = gg(b, c, d, a, x[int(i + 0)], 20, -373897302); 	// 20
			a = gg(a, b, c, d, x[int(i + 5)], 5, -701558691); 	// 21
			d = gg(d, a, b, c, x[int(i + 10)], 9, 38016083); 	// 22
			c = gg(c, d, a, b, x[int(i + 15)], 14, -660478335); 	// 23
			b = gg(b, c, d, a, x[int(i + 4)], 20, -405537848); 	// 24
			a = gg(a, b, c, d, x[int(i + 9)], 5, 568446438); 	// 25
			d = gg(d, a, b, c, x[int(i + 14)], 9, -1019803690);	// 26
			c = gg(c, d, a, b, x[int(i + 3)], 14, -187363961); 	// 27
			b = gg(b, c, d, a, x[int(i + 8)], 20, 1163531501); 	// 28
			a = gg(a, b, c, d, x[int(i + 13)], 5, -1444681467);	// 29
			d = gg(d, a, b, c, x[int(i + 2)], 9, -51403784); 	// 30
			c = gg(c, d, a, b, x[int(i + 7)], 14, 1735328473); 	// 31
			b = gg(b, c, d, a, x[int(i + 12)], 20, -1926607734);	// 32

			// Round 3
			a = hh(a, b, c, d, x[int(i + 5)], 4, -378558); 	    // 33
			d = hh(d, a, b, c, x[int(i + 8)], 11, -2022574463);	// 34
			c = hh(c, d, a, b, x[int(i + 11)], 16, 1839030562); // 35
			b = hh(b, c, d, a, x[int(i + 14)], 23, -35309556); 	// 36
			a = hh(a, b, c, d, x[int(i + 1)], 4, -1530992060);	// 37
			d = hh(d, a, b, c, x[int(i + 4)], 11, 1272893353); 	// 38
			c = hh(c, d, a, b, x[int(i + 7)], 16, -155497632); 	// 39
			b = hh(b, c, d, a, x[int(i + 10)], 23, -1094730640);	// 40
			a = hh(a, b, c, d, x[int(i + 13)], 4, 681279174); 	// 41
			d = hh(d, a, b, c, x[int(i + 0)], 11, -358537222); 	// 42
			c = hh(c, d, a, b, x[int(i + 3)], 16, -722521979); 	// 43
			b = hh(b, c, d, a, x[int(i + 6)], 23, 76029189); 	// 44
			a = hh(a, b, c, d, x[int(i + 9)], 4, -640364487); 	// 45
			d = hh(d, a, b, c, x[int(i + 12)], 11, -421815835); 	// 46
			c = hh(c, d, a, b, x[int(i + 15)], 16, 530742520); 	// 47
			b = hh(b, c, d, a, x[int(i + 2)], 23, -995338651); 	// 48

			// Round 4
			a = ii(a, b, c, d, x[int(i + 0)], 6, -198630844); 	// 49
			d = ii(d, a, b, c, x[int(i + 7)], 10, 1126891415); 	// 50
			c = ii(c, d, a, b, x[int(i + 14)], 15, -1416354905);	// 51
			b = ii(b, c, d, a, x[int(i + 5)], 21, -57434055); 	// 52
			a = ii(a, b, c, d, x[int(i + 12)], 6, 1700485571); 	// 53
			d = ii(d, a, b, c, x[int(i + 3)], 10, -1894986606);	// 54
			c = ii(c, d, a, b, x[int(i + 10)], 15, -1051523); 	// 55
			b = ii(b, c, d, a, x[int(i + 1)], 21, -2054922799);	// 56
			a = ii(a, b, c, d, x[int(i + 8)], 6, 1873313359); 	// 57
			d = ii(d, a, b, c, x[int(i + 15)], 10, -30611744); 	// 58
			c = ii(c, d, a, b, x[int(i + 6)], 15, -1560198380);	// 59
			b = ii(b, c, d, a, x[int(i + 13)], 21, 1309151649); // 60
			a = ii(a, b, c, d, x[int(i + 4)], 6, -145523070); 	// 61
			d = ii(d, a, b, c, x[int(i + 11)], 10, -1120210379);	// 62
			c = ii(c, d, a, b, x[int(i + 2)], 15, 718787259); 	// 63
			b = ii(b, c, d, a, x[int(i + 9)], 21, -343485551); 	// 64

			a += aa;
			b += bb;
			c += cc;
			d += dd;
		}

		// Finish up by containing the buffers with their hex output.
        char str[33] = {'\0'};
        bool bigEndian = isBigEndian();
        toHex(str, a, bigEndian);
        toHex(str+8, b, bigEndian);
        toHex(str+16, c, bigEndian);
        toHex(str+24, d, bigEndian);
        std::string res = str;
        return res;
	}