#include "Defines.h"
#include "StringUtility.h"
	
	std::string s2l(const std::string &s)
	{
		std::string ret = "";

		size_t len = s.length();
		for (size_t i = 0; i < len; ++i)
		{
			char c = s[i];
			if (c >= 65 && c <= 90)
				c += 32;
			ret += c;
		}

		return ret;
	}

	std::string getFileExt(const std::string &s)
	{
		size_t p = s.find_last_of('.');
		if (p == s.npos)
			return "";

		return s.substr(p + 1);
	}

	std::string replaceFileExt(std::string file, std::string ext)
	{
		int pos = file.find_last_of('.');
		if (pos == std::string::npos)
			return file + "." + ext;
		return file.substr(0, pos) + "." + ext;
	}
	
	std::string getFilePath(std::string file)
	{
		int pos0 = file.find_last_of('\\');
		int pos1 = file.find_last_of('/');
		int pos = -1;

		if (pos0 != file.npos && pos1 != file.npos)
			pos = pos0 > pos1 ? pos0 : pos1;
		else if (pos0 != file.npos)
			pos = pos0;
		else if (pos1 != file.npos)
			pos = pos1;

		if (pos <= 0)
			return "";

		return file.substr(0, pos);
	}
