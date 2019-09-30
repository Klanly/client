using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
using Cross;
using System.Runtime.InteropServices;// 加入运行时动态加载服务

using MuGame;
using GameFramework;



    class YV_Evenhandle : MonoBehaviour
    {
        //unity  call  ios  functhion

        [DllImportAttribute("__Internal")]
        public static extern void LoginView();

        [DllImportAttribute("__Internal")]
        public static extern void GameLoginOut();

        [DllImportAttribute("__Internal")]
        public static extern void GameExit();

        [DllImportAttribute("__Internal")]
        public static extern void AppStoreBuy(string price, string roleid, string cp_trade_no, string serverid, string waresId,
        string waresName, string cpPrivateInfo);


    //end

    static public YV_Evenhandle _instance = null;

        private string m_lancfg_platformName = "null";
        private string m_lancfg_platformKey = "null";
        private string m_lancfg_version = "null";
        private string m_lancfg_hostUrl = "null";

        public static string loginPath = Application.persistentDataPath + "/" + "sightseer.text";
        public static string receiptPath = Application.persistentDataPath + "/" + "iosreceipt.text";

        public List<string> errReceiptLst = new List<string>();  // 运行当中缓存错误订单
        public List<string> errLocalReceiptLst = null;  // 本机错误订单记录
        public Dictionary<string, int> errReceiptCount = new Dictionary<string, int>(); // 错误订单验证次数
        bool isrepeating = false;
        public float checkTime = 3f;
        public float currTime = 0f;



    void showLog(string title, string message)
    {
        Debug.Log("title: " + title + ", message: " + message);
    }

    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/StreamingAssets/";
                break;
        }
        return path;
    }

    void Load_lancfg_File(string path_name)
    {
        try
        {
            string file_base64_txt = File.ReadAllText(path_name);
            byte[] file_bytes = Convert.FromBase64String(file_base64_txt);
            string src_txt = Encoding.GetEncoding("utf-8").GetString(file_bytes);

            file_base64_txt = null;
            file_bytes = null;

            var data = quicksdk.SimpleJSON.JSONNode.Parse(src_txt);
            m_lancfg_platformName = data["platformName"];
            m_lancfg_platformKey = data["signKey"];
            m_lancfg_version = data["version"];
            m_lancfg_hostUrl = data["hostUrl"];
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            Debug.LogError("lan.cfg exception Message = " + e.Message);
            Debug.LogError("lan.cfg exception StackTrace = " + e.StackTrace);
            return;
        }
    }

    void Start()
    {
        Debug.LogError(" PlayerPrefs.GetInt(sightseershow)  ================================= " + PlayerPrefs.GetInt("sightseershow"));

        _instance = this;

        //读取lan.cfg的数据，并解析
        string lancfg_file = AppContentPath() + "lan.cfg";
        Load_lancfg_File(lancfg_file);

        onInitSuccess();
    }


    ///////////////////////////////////////////////////////////////////////后台通讯的加解密/////////////////////////////////////////////////////////////
    public static string encryption(string key, List<string> list_key, List<string> list_value)
    {
        string result = null;

        try
        {
            List<string> arrayList = new List<string>();
            for (int i = 0; i < list_key.Count; ++i)
            {
                arrayList.Add(list_key[i]);
                //Debug.Log("encryption arrayList add = " + list_key[i]);
            }
            arrayList.Sort(Compare);

            List<string> value = new List<string>();
            for (int i = 0; i < arrayList.Count; ++i)
            {
                for (int j = 0; j < list_key.Count; ++j)
                {
                    if (arrayList[i] == list_key[j])
                    {
                        value.Add(list_value[j]);
                        //Debug.Log("encryption value add = " + list_value[j]);
                        break;
                    }
                }
            }
            string sign = getSign(key, value);
            arrayList.Clear();
            value.Clear();

            string json = "{";
            json += "\"sign\":" + "\"" + sign + "\"";
            for (int i = 0; i < list_key.Count; ++i)
            {
                json += ",\"" + list_key[i] + "\":" + "\"" + list_value[i] + "\"";
            }
            json += "}";

            //Debug.Log(json);
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(json);
            result = Convert.ToBase64String(bytes);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            Debug.LogError("encryption exception Message = " + e.Message);
            Debug.LogError("encryption exception StackTrace = " + e.StackTrace);
        }

        return result;
    }

    public static string decryption(string key, string json)
    {
        string result = null;

        //暂时不存在验证的意义，使用相信去处理
        byte[] bytes = Convert.FromBase64String(json);
        result = Encoding.GetEncoding("utf-8").GetString(bytes);

        //以下是java 的解包的代码
        //byte[] decode = Base64.decode(json, 0);
        //String data = new String(decode);
        //JSONObject jObject = new JSONObject(data);
        //String sign = jObject.getString("sign");
        //jObject.remove("sign");

        //Iterator keys = jObject.keys();
        //ArrayList<String> arrayList = new ArrayList();
        //while (keys.hasNext())
        //{
        //    String name = (String)keys.next();
        //    arrayList.add(name);
        //}
        //Collections.sort(arrayList, new MyComparator(null));

        //ArrayList<String> value = new ArrayList();
        //for (String key2 : arrayList)
        //{
        //    value.add(jObject.getString(key2));
        //}
        //String sign2 = getSign(key, value, charset);
        //if (sign2.equals(sign))
        //{
        //    KLog.d(KLog.Tag.KSECURITY, "decryption dataObject: " + data.toString());
        //    result = data.toString();
        //}

        return result;
    }

    static int Compare(string lhs, string rhs)
    {
        return lhs.CompareTo(rhs);
    }

    public static string getMD5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    private static string getSign(string key, List<string> list)
    {
        StringBuilder builder = new StringBuilder();
        string temp = null;

        for (int i = 0; i < list.Count; ++i)
        {
            builder.Append(list[i] + ".");
        }

        temp = builder.ToString();
        if ("".Equals(temp))
        {
            temp = temp.Substring(0, temp.Length);
        }
        else
        {
            temp = temp.Substring(0, temp.Length - 1);
        }
        temp = temp + key;

        //Debug.Log("encryption list: " + temp);
        return getMD5(temp);
    }

    //************************************************************通知游戏的接口*************************************************************************************************************************
    private void SDKSendToGame(string cmd, string result, string data)
    {
        string json = "{";
        json += "\"cmd\":" + "\"" + cmd + "\"";
        json += ",\"result\":" + "\"" + result + "\"";
        json += ",\"data\":" + "\"" + data + "\"";
        json += "}";

        //AndroidJavaReceiveString
        //Debug.Log("SDKSendToGame" + json);
        Main.IOS_SDK_Send(json);
    }

    //************************************************************同公司服务器的通讯接口*************************************************************************************************************************
    //callback
    private const int HTTP_RETRY_MAX_TIME = 3;

    IEnumerator HTTP_post(string reqParams, string url, Action<bool, WWW> call_back) //请求登入趣味的后台
    {
        WWWForm www_form = new WWWForm();
        www_form.AddField("data", reqParams);
        www_form.AddField("pid", m_lancfg_platformName);
        www_form.AddField("v", m_lancfg_version);

        WWW www = new WWW(m_lancfg_hostUrl + url, www_form);

        Debug.LogError("url    :  " + (m_lancfg_hostUrl + url));
        Debug.Log("data:" + reqParams + "  pid:" + m_lancfg_platformName + "  v:" + m_lancfg_version);

        float timer = 0;
        bool time_out = false;
        while (!www.isDone)
        {
            if (timer > 30f) { time_out = true; break; }
            timer += Time.deltaTime;
            //Debug.Log("timer=" + timer);
            yield return null;
        }

        call_back(time_out, www);

        www.Dispose();
        www = null;
    }

    public enum CheckHttpRes
    {
        CHR_failed_and_retry = 0,
        CHR_success = 1,
        CHR_failed_donot_try = -1,
    }

    private CheckHttpRes CheckHttpReq(bool timeout, WWW www, string func_name, ref int retry_time)
    {
        bool failed = false;
        if (timeout)
        {
            failed = true; Debug.LogError(func_name + " ---- time_out");
        }

        if (www.error != null)
        {
            failed = true; Debug.LogError(func_name + " error---->>>" + www.error.ToString());
        }

        if (failed)
        {
            if (retry_time >= HTTP_RETRY_MAX_TIME)
            {
                Debug.LogError(func_name + " retry time over over");
                return CheckHttpRes.CHR_failed_donot_try;
            }

            ++retry_time;
            return CheckHttpRes.CHR_failed_and_retry;
        }

        retry_time = 0;
        return CheckHttpRes.CHR_success;
    }

    private string QW_init_params;
    private int QW_init_retry_count = 0;
    private void on_QW_Init(bool timeout, WWW www)
    {
        //1 = 初始化成功 2 = 初始化失败 3 = QuickSDK初始化失败
        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_Init", ref QW_init_retry_count);
        if (res == CheckHttpRes.CHR_success)
        {
            Debug.Log("QUICK_SDK初始化成功 on_QW_Init");

            try
            {
                var init_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
                //Debug.Log(decryption(m_lancfg_platformKey, init_req["data"]));

                SDKSendToGame("init", "1", decryption(m_lancfg_platformKey, init_req["data"]));
                init_req = null;
                QW_init_params = null;
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                Debug.LogError(www.text);
                Debug.LogError("on_QW_Init exception Message = " + e.Message);
                Debug.LogError("on_QW_Init exception StackTrace = " + e.StackTrace);

                SDKSendToGame("init", "2", "");
            }
        }
        else if (res == CheckHttpRes.CHR_failed_and_retry)
        {
            StartCoroutine(HTTP_post(QW_init_params, "/initinfos.php", on_QW_Init));
        }
        else
        {
            SDKSendToGame("init", "2", "");
        }
    }

    private string QW_login_params;
    private int QW_login_retry_count = 1;
    private void on_QW_Login(bool timeout, WWW www)
    {
        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_Login", ref QW_login_retry_count);
        if (res == CheckHttpRes.CHR_success)
        {
            try
            {
                //Debug.Log("告诉服务器登入 成功了");
                //Debug.Log(www.text);

                var login_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
                var togame_data = quicksdk.SimpleJSON.JSONNode.Parse(decryption(m_lancfg_platformKey, login_req["data"]));
                //Debug.Log(decryption(m_lancfg_platformKey, login_req["data"]));

                togame_data["pid"] = m_lancfg_platformName;
                togame_data["content"] = login_req["notice"]["content"]; //公告内容
                togame_data["titles"] = login_req["notice"]["titles"]; //公告标题
                if (togame_data["content"] == null) togame_data["content"] = "";
                if (togame_data["titles"] == null) togame_data["titles"] = "";

                string send_data = togame_data.ToString();
                //Debug.Log("send_data=" + send_data);

                login_req = null;
                togame_data = null;
                QW_login_params = null;


                SDKSendToGame("login", "11", send_data);

            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                Debug.LogError(www.text);
                Debug.LogError("on_QW_Login exception Message = " + e.Message);
                Debug.LogError("on_QW_Login exception StackTrace = " + e.StackTrace);

                SDKSendToGame("login", "12", "");
            }
        }
        else if (res == CheckHttpRes.CHR_failed_and_retry)
        {
            StartCoroutine(HTTP_post(QW_login_params, "/accountverify.php", on_QW_Login));
        }
        else
        {
            SDKSendToGame("login", "12", "");
        }
    }
    //************************************************************以下是需要实现的回调接口*************************************************************************************************************************
    //callback
    //callback

    public void onInitSuccess()
    {
        showLog("onInitSuccess", "");

        //QuickSDK.getInstance().login(); //如果游戏需要启动时登录，需要在初始化成功之后调用

        string str_channel_id = "9";
        Debug.Log("str_channel_id=" + str_channel_id);

        List<string> m_keys = new List<string>();
        List<string> m_values = new List<string>();
        m_keys.Add("channelid"); m_values.Add(str_channel_id);

        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
        m_keys.Clear(); m_keys = null;
        m_values.Clear(); m_values = null;

        QW_init_params = sign;
        StartCoroutine(HTTP_post(QW_init_params, "/initinfos.php", on_QW_Init));
    }


    public void getOrderID() //向公司后台请求订单号
    {
        List<string> m_keys = new List<string>();
        List<string> m_values = new List<string>();
        m_keys.Add("uid"); m_values.Add(Globle.YR_srvlists__platuid);
        m_keys.Add("server"); m_values.Add(Globle.curServerD.sid.ToString());
        m_keys.Add("custom"); m_values.Add("");
        m_keys.Add("pcustom"); m_values.Add(Globle.curServerD.sids.ToString() + "|" + m_buy_date["productId"]._str);
        m_keys.Add("amount"); m_values.Add(m_buy_date["productPrice"]._str);

        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
        m_keys.Clear(); m_keys = null;
        m_values.Clear(); m_values = null;

        debug.Log("uid=" + Globle.YR_srvlists__platuid + " server=" + Globle.curServerD.sid.ToString());

        QW_pay_params = sign;
        StartCoroutine(HTTP_post(QW_pay_params, "/ordercreate.php", on_QW_Pay));
    }

    private string QW_pay_params = "";
    private int QW_pay_retry_count = 1;


    private void on_QW_Pay(bool timeout, WWW www)
    {
        //支付
        //public final static int PAYMENT_STATUS_SUCC = 31;       // 充值成功
        //public final static int PAYMENT_STATUS_ERROR = 32;      // 充值失败
        //public final static int PAYMENT_STATUS_CLOSE = 33;      // 关闭充值
        //public final static int PAYMENT_STATUS_SUBMIT = 34;     // 订单取消

        CheckHttpRes res = CheckHttpReq(timeout, www, "on_QW_Pay", ref QW_pay_retry_count);
        if (res == CheckHttpRes.CHR_success)
        {
            try
            {
                Debug.Log("向公司后台请求订单号 成功返回");

                Debug.Log(www.text);

                var pay_req = quicksdk.SimpleJSON.JSONNode.Parse(www.text);
                var togame_data = quicksdk.SimpleJSON.JSONNode.Parse(decryption(m_lancfg_platformKey, pay_req["data"]));

                string cpOrderID = togame_data["order"];

                Debug.Log("-- SDKPay -- 获取到的订单ID: " + cpOrderID);


                StartBuyToApp(cpOrderID);

                pay_req = null;
                togame_data = null;

            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                Debug.LogError(www.text);
                Debug.LogError("on_QW_Pay exception Message = " + e.Message);
                Debug.LogError("on_QW_Pay exception StackTrace = " + e.StackTrace);

                //SDKSendToGame("pay", "34", "");
            }

        }
        else if (res == CheckHttpRes.CHR_failed_and_retry)
        {
            StartCoroutine(HTTP_post(QW_pay_params, "/ordercreate.php", on_QW_Pay));
        }
        else
        {
            //SDKSendToGame("pay", "34", "");
        }
    }

    public void sdk_loginOut()
    {
        GameLoginOut();
    }
    public void game_close()
    {
        GameExit();
    }


    //发送给ios的登入
    public void sdk_login()
    {
        LoginView();
    }

    ////这里写ios 登入成功的回调
    public void onLoginSuccess(IOSUserInfo userInfo)
    {
        showLog("onLoginSuccess", "uid: " + userInfo.uid + " ,userToken: " + userInfo.token + " ,username: " + userInfo.username + ", msg: " + userInfo.errMsg);

        //m_login_UserInfo = userInfo;

        List<string> m_keys = new List<string>();
        List<string> m_values = new List<string>();
        m_keys.Add("uid"); m_values.Add(userInfo.uid);
        m_keys.Add("username"); m_values.Add(userInfo.username);
        m_keys.Add("token"); m_values.Add(userInfo.token);
        m_keys.Add("nickname"); m_values.Add("");
        m_keys.Add("ext"); m_values.Add("9");
        m_keys.Add("email"); m_values.Add("");
        m_keys.Add("avatar"); m_values.Add("");

        string sign = encryption(m_lancfg_platformKey, m_keys, m_values);
        //Debug.Log("my__sign=" + sign);
        m_keys.Clear(); m_keys = null;
        m_values.Clear(); m_values = null;

        QW_login_params = sign;
        StartCoroutine(HTTP_post(QW_login_params, "/accountverify.php", on_QW_Login));
    }

    // ------------------------------------------------------------------- ios 登录  

    public void IOSErrorHandle(string errMessage)
    {

        Debug.LogError(errMessage);

    } // 登录不成功操作

    // login success handle

    public void IOSLoginSuccessHandle(string successMessage)
    {
        IOSUserInfo userInfo = GetIOSUserInfo(successMessage);

        onLoginSuccess(userInfo);

    } // ios 登录成功操作


    int i_relogin = 100;
    bool b_relogin = false;
    public void IOSLoginFileHandle(string fileMessage)
    {
        b_relogin = true;
        i_relogin = 0;
    } // ios 登录失败操作

    void Update()
    {
        if (b_relogin)
        {
            if (i_relogin > 100)
            {
                b_relogin = false;
                LoginView();
            }
            else
                i_relogin++;
        }
    }


    public IOSUserInfo GetIOSUserInfo(string successMessage)
    {

        IOSUserInfo userInfo = new IOSUserInfo();

        string[] messageLst = successMessage.Split(',');

        userInfo.username = " ";
        for (int i = 0; i < messageLst.Length; i++)
        {
            messageLst[i] = messageLst[i].Replace("[", "");
            messageLst[i] = messageLst[i].Replace("]", "");

            string[] values = messageLst[i].Split(':');

            if (values[0] == "uid")
            {
                userInfo.uid = values[1];
            }
            if (values[0] == "username")
            {
                userInfo.username = values[1];
            }
            if (values[0] == "token")
            {
                userInfo.token = values[1];
            }
        }

        userInfo.errMsg = "";

        return userInfo;

    }


    // ----------------------------------------------------------------- unity 通知ios 发动支付请求

    Variant m_buy_date;
    public void StartBuy(Variant v)
    {
        m_buy_date = v;
        getOrderID();
        //AppStoreBuy();
    }

    public void StartBuyToApp(string cpOrderID)
    {
        AppStoreBuy(m_buy_date["productPrice"]._str,
            m_buy_date["roleId"]._str,
            cpOrderID,
            m_buy_date["serverId"]._str,
            m_buy_date["productShopId"]._str,
            m_buy_date["productName"]._str,
            Globle.curServerD.sids.ToString() + "|" + m_buy_date["productId"]._str + "|" + cpOrderID + "|" + m_buy_date["gpuid"]._str
            );

    }

    // ----------------------------------------------------------------- ios 内部支付回调

    public void IOSAppBuyErrorHandle()
    {
        Debug.LogError("购买失败");

    }  // 支付失败操作

    public void IOSAppBuySuccessHandle()
    {

        Debug.LogError("购买成功");
    }

}

