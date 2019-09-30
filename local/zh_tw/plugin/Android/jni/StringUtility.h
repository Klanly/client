/************************************************************************/
/*String library                                                           */
/*Author: Gong Zhongliang   (2014/8/7)                                     */
/*String utilities */
/************************************************************************/

#pragma once
#include "./Defines.h"
#include <string>
#include <math.h>

	//convert string to lowercase
	std::string s2l(const std::string &s);

	inline std::string trim(std::string s)
	{
		while (s.find(' ') == 0) {
			s = s.substr(1);
		}

		while (s.length() && s.find_last_of(' ') == s.length() - 1) {
			s = s.substr(0, s.length() - 1);
		}

		return s;
	}

	//int to std::string
	inline std::string i2s(int val)
	{
		if (val == 0)
			return "0";

		std::string ret = "";
	
		while(val)
		{
			char ch[2] = {(char)('0' + (char)(val % 10)), (char)0};
			ret = std::string(ch) + ret;
			val /= 10;
		}

		return ret;
	}

	//char* to float
	inline float a2f(const char *str, size_t begin = 0, size_t len = 0)
	{
		long long val = 0;
		int sign = 1;
		size_t pos = begin;
	
		if (len == 0)
			len = strlen(str + begin);

		len = len < 64? len : 64; //too long

		//char *d = new char[len + 1];
		//memcpy(d, str + begin, len);
		//d[len] = 0;
		//string sd = d;
		//delete[] d;

		if (len == 3)
		{
			if ((str[begin] == 'n' || str[begin] == 'N')
				&& (str[begin + 1] == 'a' || str[begin + 1] == 'A')
				&& (str[begin + 2] == 'n' || str[begin + 2] == 'N'))
			{
				return 0.0f;
			}
		}

		if (str[pos] == '+') 
			++pos;
		else if (str[pos] == '-') {
			sign = -1;
			++pos;
		}

		while (pos < begin + len && str[pos] == '0') ++pos;

		bool bMajor = true;
		long long bit = 1;
		float exp = 0.0f;
		bool bexp = false;
		while(pos < begin + len)
		{
			if (str[pos] >= '0' && str[pos] <= '9')
			{
				val = val * 10 + (str[pos] - '0');
				if (bMajor == false)
					bit *= 10;
			}
			else if (str[pos] == '.')
			{
				if (bMajor == true) 
					bMajor = false;
				else //something wrong, more than one dot
					return 0.0f;
			}
			else if (str[pos] == 'e' || str[pos] == 'E')
			{
				exp = a2f(str, pos + 1, len - (pos - begin + 1));
				bexp = true;
				break;
			}
			else
			{
				return 0.0f;
			}
		
			++pos;
		}

		return float(val) / bit * sign * (bexp ? powf(10.0f, exp) : 1.0f);
	}

	//string to float
	inline float s2f(std::string &str, size_t begin = 0, size_t len = 0)
	{
		return a2f(str.c_str(), begin, len);
	}

	//char* to int
	inline int a2i(const char *str, size_t begin = 0, size_t len = 0)
	{
		int val = 0;
		int sign = 1;
		size_t pos = begin;
	
		if (len == 0)
			len = strlen(str + begin);

		len = len < 64? len : 64; //too long

		if (str[pos] == '+') 
			++pos;
		else if (str[pos] == '-') {
			sign = -1;
			++pos;
		}

		while (pos < begin + len && str[pos] == '0') ++pos;

		while(pos < begin + len)
		{
			if (str[pos] >= '0' && str[pos] <= '9')
			{
				val = val * 10 + (str[pos] - '0');
			}
			else
			{
				return 0;
			}
		
			++pos;
		}

		return val  * sign;
	}

	//string to int
	inline int s2i(std::string &str, size_t begin = 0, size_t len = 0)
	{
		return a2i(str.c_str(), begin, len);
	}

	//get file ext name
	std::string getFileExt(const std::string &s);
	
	//replace file ext name with another one
	std::string replaceFileExt(std::string file, std::string ext);
	
	//get the path from a full file path 
	std::string getFilePath(std::string file);
