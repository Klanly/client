//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.IO;
//using System.Text;
//using System;
//using quicksdk;

//public class QS_EventHandle : QuickSDKListener
//{
//    static public QS_EventHandle _instance = null;

//    private string m_lancfg_platformName = "null";
//    private string m_lancfg_platformKey = "null";
//    private string m_lancfg_version = "null";
//    private string m_lancfg_hostUrl = "null";

//    private UserInfo m_login_UserInfo = null;

//    public GameObject messageBox;
//    public GameObject mExitDialogCanvas;

//    void showLog(string title, string message)
//    {
//        Debug.Log("title: " + title + ", message: " + message);
//    }

//    public static string AppContentPath()
//    {
//        string path = string.Empty;
//        switch (Application.platform)
//        {
//            case RuntimePlatform.Android:
//                path = "jar:file://" + Application.dataPath + "!/assets/";
//                break;
//            case RuntimePlatform.IPhonePlayer:
//                path = Application.dataPath + "/Raw/";
//                break;
//            default:
//                path = Application.dataPath + "/StreamingAssets/";
//                break;
//        }
//        return path;
//    }

//    void Load_lancfg_File(string path_name)
//    {
//        try
//        {
//            string file_base64_txt = File.ReadAllText(path_name);
//            byte[] file_bytes = Convert.FromBase64String(file_base64_txt);
//            string src_txt = Encoding.GetEncoding("utf-8").GetString(file_bytes);

//            file_base64_txt = null;
//            file_bytes = null;

//            var data = quicksdk.SimpleJSON.JSONNode.Parse(src_txt);
//            m_lancfg_platformName = data["platformName"];
//            m_lancfg_platformKey = data["signKey"];
//            m_lancfg_version = data["version"];
//            m_lancfg_hostUrl = data["hostUrl"];

//            //Debug.Log("sr=" + src_txt);
//            //Debug.Log("platformName=" + data["platformName"]);
//            //Debug.Log("platformKey=" + data["signKey"]);
//            //Debug.Log("version=" + data["version"]);
//            //Debug.Log("hostUrl=" + data["hostUrl"]);

//            //var configure_node = data["configure"];
//            //Debug.Log("appID=" + configure_node["appID"]);
//            //Debug.Log("appKey=" + configure_node["appKey"]);
//            //Debug.Log("redirectUrl=" + configure_node["redirectUrl"]);
//            //Debug.Log("loginUrl=" + configure_node["loginUrl"]);
//            //Debug.Log("payUrl=" + configure_node["payUrl"]);
//            //Debug.Log("serverUrl=" + configure_node["serverUrl"]);
//            //Debug.Log("returnUrl=" + configure_node["returnUrl"]);
//            //Debug.Log("scheme=" + configure_node["scheme"]);
//            //Debug.Log("gameName=" + configure_node["gameName"]);
//            //Debug.Log("reRunAppId=" + configure_node["reRunAppId"]);
//            //Debug.Log("reRunChannel=" + configure_node["reRunChannel"]);
//            //Debug.Log("WXAppkey=" + configure_node["WXAppkey"]);
//        }
//        catch (Exception e)
//        {
//            //路径与名称未找到文件则直接返回空
//            Debug.LogError("lan.cfg exception Message = " + e.Message);
//            Debug.LogError("lan.cfg exception StackTrace = " + e.StackTrace);
//            return;
//        }
//    }

//    void CheckTheSend_Data()
//    {
//        onInitSuccess();

//        //测试正确性的key
//        m_lancfg_platformKey = "SLzi=K7qpp7bCd-wUHgUGptcs*1M$o(le#5^092)";
//        string CheckBase64 = "eyJzaWduIjoiMTFlNjVkMTQ1Y2YzNDA1ZjE4NGFhYmQ2ZGZjMmRiZmUiLCJ1aWQiOiI5NTdlMTEzOWMxMDYwZjA3YTRkYjc2ODhjOTQ0ZWM0M0BfKCkiLCJ1c2VybmFtZSI6InhzajEiLCJuaWNrbmFtZSI6IiIsInRva2VuIjoiQDE3MUA5MUAxNjNAODJAMTExQDg4QDEwOEAxMDRAMTAzQDEwNEAxNTVAMTU5QDEwNkAxNDhAMTA1QDE1MUA4NkA5NkA4N0AxNzJAMTU5QDE0OUA5MEAxMTRAODNAMTEwQDEwN0AxMDhAMTU1QDEwMEAxMDRAMTA2QDEwNUAxNTZAOTdAOTZAMTA3QDEwMkAxNTdAMTAyQDEwNEAxNTJAMTA1QDE1N0AxNTFAMTA2QDEwNkAxMDdAMTA4QDE1MUAxMTBAMTA3QDEwNkAxNTBAMTU1QDEwOEAxMDBAMTE3QDE0OUA5M0A5NUA4NUA5OUA4OUAxNjRAMTYyQDE1N0AxNDlAODdAMTEyQDEwNEAxMDhAMTAwQDk5QDg3QDE1NkA4N0AxMDlAODZAOTlAODZAMTc3IiwiZW1haWwiOiIiLCJhdmF0YXIiOiIiLCJleHQiOiIwIn0 =";
//        Debug.Log("true_sign=" + CheckBase64);
//        byte[] bytes = Convert.FromBase64String(CheckBase64);
//        string src_txt = Encoding.GetEncoding("utf-8").GetString(bytes);
//        Debug.Log(src_txt);

//        UserInfo us_in = new UserInfo();
//        us_in.uid = "957e1139c1060f07a4db7688c944ec43@_()";
//        us_in.userName = "xsj1";
//        us_in.token = "@171@91@163@82@111@88@108@104@103@104@155@159@106@148@105@151@86@96@87@172@159@149@90@114@83@110@107@108@155@100@104@106@105@156@97@96@107@102@157@102@104@152@105@157@151@106@106@107@108@151@110@107@106@150@155@108@100@117@149@93@95@85@99@89@164@162@157@149@87@112@104@108@100@99@87@156@87@109@86@99@86@177";
//        onLoginSuccess(us_in);

//        getOrderID();
//    }


//    void Start()
//    {
//        //初始化2个基础的变量
//        QW_pay_orderInfo.goodsID = ""; //产品ID，用来识别购买的产品
//        QW_pay_orderInfo.goodsName = ""; //产品名称
//        QW_pay_orderInfo.goodsDesc = "元宝"; //商品描述（仅IOS需要）
//        QW_pay_orderInfo.quantifier = "个"; //商品量词，“元宝”为“个”，“月卡”为“张”（仅IOS需要）
//        QW_pay_orderInfo.cpOrderID = "";//产品订单号（游戏方的订单号）
//        QW_pay_orderInfo.callbackUrl = "";
//        QW_pay_orderInfo.extrasParams = "";//透传参数
//        QW_pay_orderInfo.price = 0f;//商品单价（仅IOS需要）
//        QW_pay_orderInfo.amount = 0f;//支付总额
//        QW_pay_orderInfo.count = 0;//购买数量

//        QW_com_gameRoleInfo.serverName = "";
//        QW_com_gameRoleInfo.serverID = "";
//        QW_com_gameRoleInfo.gameRoleName = "";
//        QW_com_gameRoleInfo.gameRoleID = "";
//        QW_com_gameRoleInfo.gameRoleBalance = "";
//        QW_com_gameRoleInfo.vipLevel = "";
//        QW_com_gameRoleInfo.gameRoleLevel = "";
//        QW_com_gameRoleInfo.partyName = "";
//        QW_com_gameRoleInfo.roleCreateTime = "";
//        QW_com_gameRoleInfo.gameRoleGender = "";
//        QW_com_gameRoleInfo.gameRolePower = "";
//        QW_com_gameRoleInfo.partyId = "";
//        QW_com_gameRoleInfo.professionId = "";
//        QW_com_gameRoleInfo.profession = "";
//        QW_com_gameRoleInfo.partyRoleId = "";
//        QW_com_gameRoleInfo.partyRoleName = "";
//        QW_com_gameRoleInfo.friendlist = "";


//        _instance = this;

//        //读取lan.cfg的数据，并解析
//        string lancfg_file = AppContentPath() + "lan.cfg";
//        Load_lancfg_File(lancfg_file);

//        //初始化
//        QuickSDK.getInstance().setListener(this);

//        //////测试加解密的数据
//        //CheckTheSend_Data();
//        //return;
//    }

//    public void onLogin()
//    {
//        QuickSDK.getInstance().login();
//    }

//    public void onLogout()
//    {
//        QuickSDK.getInstance().logout();
//    }

//    public void onWeiboShare()
//    {
//        Hashtable ht = new Hashtable();
//        ht.Add("A", "1");
//        ht.Add("B", "2");
//        ht.Add("C", "3");
//        ht.Add("D", "4");

//        QuickSDK.getInstance().weiboShare(ht);
//    }
//    public void onWxShare()
//    {
//        Hashtable ht = new Hashtable();
//        ht.Add("A", "1");
//        ht.Add("B", "2");
//        ht.Add("C", "3");
//        ht.Add("D", "4");

//        QuickSDK.getInstance().wxShare(ht, false);
//    }
//    public void onWxShareFriendCircle()
//    {
//        Hashtable ht = new Hashtable();
//        ht.Add("A", "1");
//        ht.Add("B", "2");
//        ht.Add("C", "3");
//        ht.Add("D", "4");


//        QuickSDK.getInstance().wxShare(ht, true);
//    }
//    public void onPay()
//    {
//        OrderInfo orderInfo = new OrderInfo();
//        GameRoleInfo gameRoleInfo = new GameRoleInfo();

//        orderInfo.goodsID = "1";
//        orderInfo.goodsName = "勾玉";
//        orderInfo.goodsDesc = "10个勾玉";
//        orderInfo.quantifier = "个";
//        orderInfo.extrasParams = "extparma";
//        orderInfo.count = 10;
//        orderInfo.amount = 1;
//        orderInfo.price = 0.1f;
//        orderInfo.callbackUrl = "";
//        orderInfo.cpOrderID = "cporderidzzw";

//        gameRoleInfo.gameRoleBalance = "0";
//        gameRoleInfo.gameRoleID = "000001";
//        gameRoleInfo.gameRoleLevel = "1";
//        gameRoleInfo.gameRoleName = "钱多多";
//        gameRoleInfo.partyName = "同济会";
//        gameRoleInfo.serverID = "1";
//        gameRoleInfo.serverName = "火星服务器";
//        gameRoleInfo.vipLevel = "1";
//        gameRoleInfo.roleCreateTime = "roleCreateTime";
//        QuickSDK.getInstance().pay(orderInfo, gameRoleInfo);
//    }

//    public void onCreatRole()
//    {
//        //注：GameRoleInfo的字段，如果游戏有的参数必须传，没有则不用传
//        GameRoleInfo gameRoleInfo = new GameRoleInfo();

//        gameRoleInfo.gameRoleBalance = "0";
//        gameRoleInfo.gameRoleID = "000001";
//        gameRoleInfo.gameRoleLevel = "1";
//        gameRoleInfo.gameRoleName = "钱多多";
//        gameRoleInfo.partyName = "同济会";
//        gameRoleInfo.serverID = "1";
//        gameRoleInfo.serverName = "火星服务器";
//        gameRoleInfo.vipLevel = "1";
//        gameRoleInfo.roleCreateTime = "roleCreateTime";//UC与1881渠道必传，值为10位数时间戳

//        gameRoleInfo.gameRoleGender = "男";//360渠道参数
//        gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
//        gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串

//        gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
//        gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
//        gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
//        gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
//        gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190


//        QuickSDK.getInstance().createRole(gameRoleInfo);//创建角色
//    }

//    public void onEnterGame()
//    {
//        //注：GameRoleInfo的字段，如果游戏有的参数必须传，没有则不用传
//        GameRoleInfo gameRoleInfo = new GameRoleInfo();

//        gameRoleInfo.gameRoleBalance = "0";
//        gameRoleInfo.gameRoleID = "000001";
//        gameRoleInfo.gameRoleLevel = "1";
//        gameRoleInfo.gameRoleName = "钱多多";
//        gameRoleInfo.partyName = "同济会";
//        gameRoleInfo.serverID = "1";
//        gameRoleInfo.serverName = "火星服务器";
//        gameRoleInfo.vipLevel = "1";
//        gameRoleInfo.roleCreateTime = "roleCreateTime";//UC与1881渠道必传，值为10位数时间戳

//        gameRoleInfo.gameRoleGender = "男";//360渠道参数
//        gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
//        gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串

//        gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
//        gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
//        gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
//        gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
//        gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190


//        QuickSDK.getInstance().enterGame(gameRoleInfo);//开始游戏
//        //Application.LoadLevel("scene4");
//    }

//    public void onUpdateRoleInfo()
//    {
//        //注：GameRoleInfo的字段，如果游戏有的参数必须传，没有则不用传
//        GameRoleInfo gameRoleInfo = new GameRoleInfo();

//        gameRoleInfo.gameRoleBalance = "0";
//        gameRoleInfo.gameRoleID = "000001";
//        gameRoleInfo.gameRoleLevel = "1";
//        gameRoleInfo.gameRoleName = "钱多多";
//        gameRoleInfo.partyName = "同济会";
//        gameRoleInfo.serverID = "1";
//        gameRoleInfo.serverName = "火星服务器";
//        gameRoleInfo.vipLevel = "1";
//        gameRoleInfo.roleCreateTime = "roleCreateTime";//UC与1881渠道必传，值为10位数时间戳

//        gameRoleInfo.gameRoleGender = "男";//360渠道参数
//        gameRoleInfo.gameRolePower = "38";//360渠道参数，设置角色战力，必须为整型字符串
//        gameRoleInfo.partyId = "1100";//360渠道参数，设置帮派id，必须为整型字符串

//        gameRoleInfo.professionId = "11";//360渠道参数，设置角色职业id，必须为整型字符串
//        gameRoleInfo.profession = "法师";//360渠道参数，设置角色职业名称
//        gameRoleInfo.partyRoleId = "1";//360渠道参数，设置角色在帮派中的id
//        gameRoleInfo.partyRoleName = "帮主"; //360渠道参数，设置角色在帮派中的名称
//        gameRoleInfo.friendlist = "无";//360渠道参数，设置好友关系列表，格式请参考：http://open.quicksdk.net/help/detail/aid/190

//        QuickSDK.getInstance().updateRole(gameRoleInfo);
//    }

//    //public void onNext()
//    //{
//    //    //Application.LoadLevel("scene3");
//    //}

//    public void onExit()
//    {
//        if (QuickSDK.getInstance().isChannelHasExitDialog())
//        {
//            QuickSDK.getInstance().exit();
//        }
//        else
//        {
//            //游戏调用自身的退出对话框，点击确定后，调用QuickSDK的exit()方法
//            mExitDialogCanvas.SetActive(true);
//        }
//    }

//    public void onExitCancel()
//    {
//        mExitDialogCanvas.SetActive(false);
//    }
//    public void onExitConfirm()
//    {
//        mExitDialogCanvas.SetActive(false);
//        QuickSDK.getInstance().exit();
//    }

//    public void onShowToolbar()
//    {
//        QuickSDK.getInstance().showToolBar(ToolbarPlace.QUICK_SDK_TOOLBAR_BOT_LEFT);
//    }

//    public void onHideToolbar()
//    {
//        QuickSDK.getInstance().hideToolBar();
//    }

//    public void onEnterUserCenter()
//    {
//        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_USER_CENTER);
//    }

//    public void onEnterBBS()
//    {
//        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_BBS);
//    }
//    public void onEnterCustomer()
//    {
//        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_ENTER_CUSTOMER_CENTER);
//    }
//    public void onUserId()
//    {
//        string uid = QuickSDK.getInstance().userId();
//        showLog("userId", uid);
//    }
//    public void onChannelType()
//    {
//        int type = QuickSDK.getInstance().channelType();
//        showLog("channelType", "" + type);
//    }
//    public void onFuctionSupport(int type)
//    {
//        bool supported = QuickSDK.getInstance().isFunctionSupported((FuncType)type);
//        showLog("fuctionSupport", supported ? "yes" : "no");
//    }
//    public void onGetConfigValue(string key)
//    {
//        string value = QuickSDK.getInstance().getConfigValue(key);
//        showLog("onGetConfigValue", key + ": " + value);
//    }

//    public void onOk()
//    {
//        messageBox.SetActive(false);
//    }

//    public void onPauseGame()
//    {
//        Time.timeScale = 0;
//        QuickSDK.getInstance().callFunction(FuncType.QUICK_SDK_FUNC_TYPE_PAUSED_GAME);
//    }

//    public void onResumeGame()
//    {
//        Time.timeScale = 1;
//    }

//    ///////////////////////////////////////////////////////////////////////后台通讯的加解密/////////////////////////////////////////////////////////////
//    public static string encryption(string key, List<string> list_key, List<string> list_value)
//    {
//        string result = null;

//        try
//        {
//            List<string> arrayList = new List<string>();
//            for (int i = 0; i < list_key.Count; ++i)
//            {
//                arrayList.Add(list_key[i]);
//                //Debug.Log("encryption arrayList add = " + list_key[i]);
//            }
//            arrayList.Sort(Compare);

//            List<string> value = new List<string>();
//            for (int i = 0; i < arrayList.Count; ++i)
//            {
//                for (int j = 0; j < list_key.Count; ++j)
//                {
//                    if (arrayList[i] == list_key[j])
//                    {
//                        value.Add(list_value[j]);
//                        //Debug.Log("encryption value add = " + list_value[j]);
//                        break;
//                    }
//                }
//            }
//            string sign = getSign(key, value);
//            arrayList.Clear();
//            value.Clear();

//            string json = "{";
//            json += "\"sign\":" + "\"" + sign + "\"";
//            for (int i = 0; i < list_key.Count; ++i)
//            {
//                json += ",\"" + list_key[i] + "\":" + "\"" + list_value[i] + "\"";
//            }
//            json += "}";

//            //Debug.Log(json);
//            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(json);
//            result = Convert.ToBase64String(bytes);
//        }
//        catch (Exception e)
//        {
//            //路径与名称未找到文件则直接返回空
//            Debug.LogError("encryption exception Message = " + e.Message);
//            Debug.LogError("encryption exception StackTrace = " + e.StackTrace);
//        }

//        return result;
//    }


//    public static string decryption(string key, string json)
//    {
//        string result = null;

//        //暂时不存在验证的意义，使用相信去处理
//        byte[] bytes = Convert.FromBase64String(json);
//        result = Encoding.GetEncoding("utf-8").GetString(bytes);

//        //以下是java 的解包的代码
//        //byte[] decode = Base64.decode(json, 0);
//        //String data = new String(decode);
//        //JSONObject jObject = new JSONObject(data);
//        //String sign = jObject.getString("sign");
//        //jObject.remove("sign");

//        //Iterator keys = jObject.keys();
//        //ArrayList<String> arrayList = new ArrayList();
//        //while (keys.hasNext())
//        //{
//        //    String name = (String)keys.next();
//        //    arrayList.add(name);
//        //}
//        //Collections.sort(arrayList, new MyComparator(null));

//        //ArrayList<String> value = new ArrayList();
//        //for (String key2 : arrayList)
//        //{
//        //    value.add(jObject.getString(key2));
//        //}
//        //String sign2 = getSign(key, value, charset);
//        //if (sign2.equals(sign))
//        //{
//        //    KLog.d(KLog.Tag.KSECURITY, "decryption dataObject: " + data.toString());
//        //    result = data.toString();
//        //}

//        return result;
//    }

//    static int Compare(string lhs, string rhs)
//    {
//        return lhs.CompareTo(rhs);
//    }

//    public static string getMD5(string source)
//    {
//        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
//        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
//        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
//        md5.Clear();

//        string destString = "";
//        for (int i = 0; i < md5Data.Length; i++)
//        {
//            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
//        }
//        destString = destString.PadLeft(32, '0');
//        return destString;
//    }

//    private static string getSign(string key, List<string> list)
//    {
//        StringBuilder builder = new StringBuilder();
//        string temp = null;

//        for (int i = 0; i < list.Count; ++i)
//        {
//            builder.Append(list[i] + ".");
//        }

//        temp = builder.ToString();
//        if ("".Equals(temp))
//        {
//            temp = temp.Substring(0, temp.Length);
//        }
//        else
//        {
//            temp = temp.Substring(0, temp.Length - 1);
//        }
//        temp = temp + key;

//        //Debug.Log("encryption list: " + temp);
//        return getMD5(temp);
//    }

//    //************************************************************通知游戏的接口*************************************************************************************************************************
//    private void SDKSendToGame(string cmd, string result, string data)
//    {
//        string json = "{";
//        json += "\"cmd\":" + "\"" + cmd + "\"";
//        json += ",\"result\":" + "\"" + result + "\"";
//        json += ",\"data\":" + "\"" + data + "\"";
//        json += "}";

//        //AndroidJavaReceiveString
//        //Debug.Log("SDKSendToGame" + json);
//        Main.IOS_SDK_Send(json);
//    }

//    //************************************************************同公司服务器的通讯接口*************************************************************************************************************************
//    //callback
//    public GameRoleInfo QW_com_gameRoleInfo = new GameRoleInfo();
//    private const int HTTP_RETRY_MAX_TIME = 3;

//    IEnumerator HTTP_post(string reqParams, string url, Action<bool, WWW> call_back) //请求登入趣味的后台
//    {
//        WWWForm www_form = new WWWForm();
//        www_form.AddField("data", reqParams);
//        www_form.AddField("pid", m_lancfg_platformName);
//        www_form.AddField("v", m_lancfg_version);

//        WWW www = new WWW(m_lancfg_hostUrl + url, www_form);

//        float timer = 0;
//        bool time_out = false;
//        while (!www.isDone)
//        {
//            if (timer > 30f) { time_out = true; break; }
//            timer += Time.deltaTime;
//            //Debug.Log("timer=" + timer);
//            yield return null;
//        }

//        call_back(time_out, www);

//        www.Dispose();
//        www = null;
//    }

//    public enum CheckHttpRes
//    {
//        CHR_failed_and_retry = 0,
//        CHR_success = 1,
//        CHR_failed_donot_try = -1,
//    }

//    private CheckHttpRes CheckHttpReq(bool timeout, WWW www, string func_name, ref int retry_time)
//    {
//        bool failed = false;
//        if (timeout)
//        {
//            failed = true; Debug.LogError(func_name + " ---- time_out");
//        }

//        if (www.error != null)
//        {
//            failed = true; Debug.LogError(func_name + " error---->>>" + www.error.ToString());
//        }

//        if (failed)
//        {
//            if (retry_time >= HTTP_RETRY_MAX_TIME)
//            {
//                Debug.LogError(func_name + " retry time over over");
//                return CheckHttpRes.CHR_failed_donot_try;
//            }

//            ++retry_time;
//            return CheckHttpRes.CHR_failed_and_retry;
//        }

//        retry_time = 0;
//        return CheckHttpRes.CHR_success;
//    }

//    private string QW_init_params;
//    private int QW_init_retry_count = 0;
//    private void on_QW_Init(bool timeout, WWW www)
//    {
//        //1 = 初始化成功 2 = 初始化失败 3 = QuickSDK初始化失败
//        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_Init", ref QW_init_retry_count);
//        if (res == CheckHttpRes.CHR_success)
//        {
//            Debug.Log("QUICK_SDK初始化成功 on_QW_Init");

//            try
//            {
//                var init_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
//                //Debug.Log(decryption(m_lancfg_platformKey, init_req["data"]));

//                SDKSendToGame("init", "1", decryption(m_lancfg_platformKey, init_req["data"]));
//                init_req = null;
//                QW_init_params = null;
//            }
//            catch (Exception e)
//            {
//                //路径与名称未找到文件则直接返回空
//                Debug.LogError(www.text);
//                Debug.LogError("on_QW_Init exception Message = " + e.Message);
//                Debug.LogError("on_QW_Init exception StackTrace = " + e.StackTrace);

//                SDKSendToGame("init", "2", "");
//            }
//        }
//        else if (res == CheckHttpRes.CHR_failed_and_retry)
//        {
//            StartCoroutine(HTTP_post(QW_init_params, "/initinfos.php", on_QW_Init));
//        }
//        else
//        {
//            SDKSendToGame("init", "2", "");
//        }
//    }

//    private string QW_login_params;
//    private int QW_login_retry_count = 1;
//    private void on_QW_Login(bool timeout, WWW www)
//    {
//        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_Login", ref QW_login_retry_count);
//        if (res == CheckHttpRes.CHR_success)
//        {
//            try
//            {
//                //Debug.Log("告诉服务器登入 成功了");
//                //Debug.Log(www.text);

//                var login_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
//                var togame_data = quicksdk.SimpleJSON.JSONNode.Parse(decryption(m_lancfg_platformKey, login_req["data"]));
//                //Debug.Log(decryption(m_lancfg_platformKey, login_req["data"]));

//                togame_data["pid"] = m_lancfg_platformName;
//                togame_data["content"] = login_req["notice"]["content"]; //公告内容
//                togame_data["titles"] = login_req["notice"]["titles"]; //公告标题
//                if (togame_data["content"] == null) togame_data["content"] = "";
//                if (togame_data["titles"] == null) togame_data["titles"] = "";

//                string send_data = togame_data.ToString();
//                //Debug.Log("send_data=" + send_data);

//                login_req = null;
//                togame_data = null;
//                QW_login_params = null;

//                SDKSendToGame("login", "11", send_data);
//            }
//            catch (Exception e)
//            {
//                //路径与名称未找到文件则直接返回空
//                Debug.LogError(www.text);
//                Debug.LogError("on_QW_Login exception Message = " + e.Message);
//                Debug.LogError("on_QW_Login exception StackTrace = " + e.StackTrace);

//                SDKSendToGame("login", "12", "");
//            }
//        }
//        else if (res == CheckHttpRes.CHR_failed_and_retry)
//        {
//            StartCoroutine(HTTP_post(QW_login_params, "/accountverify.php", on_QW_Login));
//        }
//        else
//        {
//            SDKSendToGame("login", "12", "");
//        }
//    }

//    public OrderInfo QW_pay_orderInfo = new OrderInfo();
//    private string QW_pay_params = "";
//    private int QW_pay_retry_count = 1;
//    private void on_QW_Pay(bool timeout, WWW www)
//    {
//        //支付
//        //public final static int PAYMENT_STATUS_SUCC = 31;       // 充值成功
//        //public final static int PAYMENT_STATUS_ERROR = 32;      // 充值失败
//        //public final static int PAYMENT_STATUS_CLOSE = 33;      // 关闭充值
//        //public final static int PAYMENT_STATUS_SUBMIT = 34;     // 订单取消

//        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_Pay", ref QW_pay_retry_count);
//        if (res == CheckHttpRes.CHR_success)
//        {
//            try
//            {
//                Debug.Log("向公司后台请求订单号 成功返回");
//                Debug.Log(www.text);

//                var pay_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
//                var togame_data = quicksdk.SimpleJSON.JSONNode.Parse(decryption(m_lancfg_platformKey, pay_req["data"]));

//                QW_pay_orderInfo.cpOrderID = togame_data["order"];
//                Debug.Log("--SDKPay-- 获取到的订单ID: " + QW_pay_orderInfo.cpOrderID);

//                pay_req = null;
//                togame_data = null;

//                quicksdk.QuickSDK.getInstance().pay(QW_pay_orderInfo, QW_com_gameRoleInfo);

//                //PayResult payResult = new PayResult();
//                //onPaySuccess(payResult);
//                //SDKSendToGame("login", "11", send_data);
//            }
//            catch (Exception e)
//            {
//                //路径与名称未找到文件则直接返回空
//                Debug.LogError(www.text);
//                Debug.LogError("on_QW_Pay exception Message = " + e.Message);
//                Debug.LogError("on_QW_Pay exception StackTrace = " + e.StackTrace);

//                SDKSendToGame("pay", "34", "");
//            }

//        }
//        else if (res == CheckHttpRes.CHR_failed_and_retry)
//        {
//            StartCoroutine(HTTP_post(QW_pay_params, "/ordercreate.php", on_QW_Pay));
//        }
//        else
//        {
//            SDKSendToGame("pay", "34", "");
//        }
//    }

//    private string QW_cbpay_params = "";
//    private int QW_cbpay_retry_count = 1;
//    private void on_QW_CBPay(bool timeout, WWW www)
//    {
//        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_CBPay", ref QW_cbpay_retry_count);
//        if (res == CheckHttpRes.CHR_success)
//        {
//            Debug.Log("验证充值的订单");
//            Debug.Log(www.text);

//            var cbpay_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
//            int res_code = 0;
//            int.TryParse(cbpay_req["code"], out res_code);
//            if (res_code == 1)
//            {
//                SDKSendToGame("pay", "31", "");
//            }
//            else
//            {
//                SDKSendToGame("pay", "32", "");
//            }
//            cbpay_req = null;
//        }
//        else if (res == CheckHttpRes.CHR_failed_and_retry)
//        {
//            StartCoroutine(HTTP_post(QW_cbpay_params, "/ordercallback.php", on_QW_CBPay));
//        }
//        else
//        {
//            SDKSendToGame("pay", "32", "");
//        }
//    }

//    public void getOrderID() //向公司后台请求订单号
//    {
//        List<string> m_keys = new List<string>();
//        List<string> m_values = new List<string>();
//        m_keys.Add("uid"); m_values.Add(m_login_UserInfo.uid);
//        m_keys.Add("server"); m_values.Add(QW_com_gameRoleInfo.serverID);
//        m_keys.Add("custom"); m_values.Add("");
//        m_keys.Add("pcustom"); m_values.Add("");
//        m_keys.Add("productPackage"); m_values.Add(Application.bundleIdentifier);

//        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
//        m_keys.Clear(); m_keys = null;
//        m_values.Clear(); m_values = null;

//        QW_pay_params = sign;
//        StartCoroutine(HTTP_post(QW_pay_params, "/ordercreate.php", on_QW_Pay));
//    }

//    //************************************************************以下是需要实现的回调接口*************************************************************************************************************************
//    //callback
//    public override void onInitSuccess()
//    {
//        showLog("onInitSuccess", "");

//        //QuickSDK.getInstance().login(); //如果游戏需要启动时登录，需要在初始化成功之后调用

//        string str_channel_id = QuickSDK.getInstance().channelType().ToString();
//        Debug.Log("str_channel_id=" + str_channel_id);

//        List<string> m_keys = new List<string>();
//        List<string> m_values = new List<string>();
//        m_keys.Add("channelid"); m_values.Add(str_channel_id);

//        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
//        m_keys.Clear(); m_keys = null;
//        m_values.Clear(); m_values = null;

//        QW_init_params = sign;
//        StartCoroutine(HTTP_post(QW_init_params, "/initinfos.php", on_QW_Init));
//    }

//    public override void onInitFailed(ErrorMsg errMsg)
//    {
//        showLog("onInitFailed", "msg: " + errMsg.errMsg);
//        SDKSendToGame("init", "3", "");
//    }

//    public override void onLoginSuccess(UserInfo userInfo)
//    {
//        showLog("onLoginSuccess", "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);

//        m_login_UserInfo = userInfo;

//        List<string> m_keys = new List<string>();
//        List<string> m_values = new List<string>();
//        m_keys.Add("uid"); m_values.Add(userInfo.uid);
//        m_keys.Add("username"); m_values.Add(userInfo.userName);
//        m_keys.Add("nickname"); m_values.Add("");
//        m_keys.Add("token"); m_values.Add(userInfo.token);
//        m_keys.Add("email"); m_values.Add("");
//        m_keys.Add("avatar"); m_values.Add("");
//        m_keys.Add("ext"); m_values.Add("0");

//        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
//        //Debug.Log("my__sign=" + sign);
//        m_keys.Clear(); m_keys = null;
//        m_values.Clear(); m_values = null;

//        QW_login_params = sign;
//        StartCoroutine(HTTP_post(QW_login_params, "/accountverify.php", on_QW_Login));
//    }

//    public override void onSwitchAccountSuccess(UserInfo userInfo)
//    {
//        //切换账号成功，清除原来的角色信息，使用获取到新的用户信息，回到进入游戏的界面，不需要再次调登录
//        showLog("onLoginSuccess", "uid: " + userInfo.uid + " ,username: " + userInfo.userName + " ,userToken: " + userInfo.token + ", msg: " + userInfo.errMsg);
//        //Application.LoadLevel("scene2");
//    }

//    public override void onLoginFailed(ErrorMsg errMsg)
//    {
//        showLog("onLoginFailed", "msg: " + errMsg.errMsg);
//        SDKSendToGame("login", "13", "");
//    }

//    public override void onLogoutSuccess()
//    {
//        showLog("onLogoutSuccess", "");
//        //注销成功后回到登陆界面
//        //Application.LoadLevel("scene1");
//    }

//    public override void onPaySuccess(PayResult payResult)
//    {
//        showLog("onPaySuccess", "orderId: " + payResult.orderId + ", cpOrderId: " + payResult.cpOrderId + " ,extraParam" + payResult.extraParam);

//        List<string> m_keys = new List<string>();
//        List<string> m_values = new List<string>();
//        //String uid = Extend.getInstance().getChannelType() + "_" + LanPay.getInstance().getPayInfo("serverId");
//        m_keys.Add("uid"); m_values.Add(m_login_UserInfo.uid);
//        m_keys.Add("server"); m_values.Add(QW_com_gameRoleInfo.serverID);
//        m_keys.Add("custom"); m_values.Add("");
//        m_keys.Add("status"); m_values.Add("");
//        m_keys.Add("order"); m_values.Add(payResult.cpOrderId);
//        m_keys.Add("porder"); m_values.Add("");
//        m_keys.Add("amount"); m_values.Add("");
//        m_keys.Add("count"); m_values.Add("");
//        m_keys.Add("product"); m_values.Add("");
//        m_keys.Add("description"); m_values.Add("");

//        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
//        m_keys.Clear(); m_keys = null;
//        m_values.Clear(); m_values = null;

//        QW_cbpay_params = sign;
//        StartCoroutine(HTTP_post(QW_cbpay_params, "/ordercallback.php", on_QW_CBPay));
//    }

//    public override void onPayCancel(PayResult payResult)
//    {
//        showLog("onPayCancel", "orderId: " + payResult.orderId + ", cpOrderId: " + payResult.cpOrderId + " ,extraParam" + payResult.extraParam);
//        SDKSendToGame("pay", "33", "");
//    }

//    public override void onPayFailed(PayResult payResult)
//    {
//        showLog("onPayFailed", "orderId: " + payResult.orderId + ", cpOrderId: " + payResult.cpOrderId + " ,extraParam" + payResult.extraParam);
//        SDKSendToGame("pay", "32", "");
//    }

//    public override void onExitSuccess()
//    {
//        showLog("onExitSuccess", "");
//        //退出成功的回调里面调用  QuickSDK.getInstance ().exitGame ();  即可实现退出游戏，杀进程。为避免与渠道发生冲突，请不要使用  Application.Quit ();
//        QuickSDK.getInstance().exitGame();
//    }

//    public override void onShareSuccess()
//    {
//        showLog("onShareSuccess", "msg: ");
//    }
//    public override void onShareCancel()
//    {
//        showLog("onShareCancel", "msg: ");
//    }

//    public override void onShareFiled(ErrorMsg errMsg)
//    {
//        showLog("onShareFiled", "msg: " + errMsg.errMsg);
//    }

//}

