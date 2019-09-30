using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleFramework
{
    class OutGameContMgr
    {


      private static  Dictionary<string, string> dOutGameText;
        static public void initOutGame(string path)
        {
            dOutGameText = new Dictionary<string, string>();
            string[] files = path.Split(new char[] { '\n' });

            for (int i = 0; i < files.Length; i++)
            {
                string[] keyValue = files[i].Split('|');

                if (keyValue.Length != 2)
                    continue;

                dOutGameText[keyValue[0]] = keyValue[1];
            }
        }

        static private Dictionary<string, string> dOutGameText2;
        static void initOutGame2()
        {
            dOutGameText2 = new Dictionary<string, string>();
            dOutGameText2["helper"] ="登陆中，请稍后....." /*"核心初始化中"*/;
            dOutGameText2["update"] = "检测到更新包<n> 文件个数：<0>个 <n> 容量：<1>  <n> 点击确认开始更新";
            dOutGameText2["wifi"] = "发现当前网络不是wifi,是否继续更新？<n> 点击确认开始更新";
            dOutGameText2["error"] = "无法更新，请检查网络是否连接或稍后再试,错误码：<0>";
            dOutGameText2["initing"] = "登陆中，请稍后....." /* "请勿关闭客户端，初始化中..."*/;
            dOutGameText2["init1"] = "初始化中 <0>%.";
            dOutGameText2["init2"] = "初始化中 <0>%..";
            dOutGameText2["init3"] = "初始化中 <0>%...";
            dOutGameText2["loading1"] = "更新中 <0>%.";
            dOutGameText2["loading2"] = "更新中 <0>%..";
            dOutGameText2["loading3"] = "更新中 <0>%...";
            dOutGameText2["needupdateclientver"] = "游戏客户端已经更新，请到应用市场下载新的客户端";
            dOutGameText2["checkver"] = "登陆中，请稍后....." /* "检测版本中"*/;
            dOutGameText2["checkupdate"] = "登陆中，请稍后....." /* "检测更新中"*/;
            dOutGameText2["loadlua"] = "读取lua文件<0>";
            dOutGameText2["debug1"] = "请求登入服务器";
            dOutGameText2["debug2"] = "正在获取角色列表";
            dOutGameText2["debug3"] = "服务器维护中...";
            dOutGameText2["debug4"] = "正在构建场景信息";
            dOutGameText2["debug5"] = "正在加载界面资源";
        }
        static string getOutGameCont2(string id)
        {
            if (dOutGameText2.ContainsKey(id))
            {
                return dOutGameText2[id];
            }
            return id;
        }


        static public string getOutGameCont(string id, params string[] vals)
        {
            //if (dOutGameText == null)
            //    initOutGame();
            string str; int idx; string[] str1; string[] arr2;
            if (dOutGameText == null || !dOutGameText.ContainsKey(id))
            {

                if (dOutGameText2 == null)
                    initOutGame2();

                str = getOutGameCont2(id);
                idx = 0;
                foreach (string p in vals)
                {
                    string[] stringSeparators = new string[] { "<" + idx + ">" };
                    string[] arr = str.Split(stringSeparators, StringSplitOptions.None);
                    str = string.Join(p, arr);
                    idx++;
                }

                str1 = new string[] { "<n>" };
                arr2 = str.Split(str1, StringSplitOptions.None);
                str = string.Join("\n", arr2);
                return str;


            }


            str = dOutGameText[id];

            idx = 0;
            foreach (string p in vals)
            {
                string[] stringSeparators = new string[] { "<" + idx + ">" };
                string[] arr = str.Split(stringSeparators, StringSplitOptions.None);
                str = string.Join(p, arr);
                idx++;
            }

            str1 = new string[] { "<n>" };
            arr2 = str.Split(str1, StringSplitOptions.None);
            str = string.Join("\n", arr2);
            return str;
        }

    }
}
