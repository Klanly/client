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

public class IOSUserInfo
{

    public string uid;
    public string token;
    public string errMsg;
    public string username;
    public string time;
}

public class YanzhenInfo
{

    public string r;
    public string msg;

}

public class IOSReceipt
{

    //  账号对应未成功的订单

    //public string orderID = ""; //自己平台订单号  key
    //public string receipt = ""; //ios 验证收据    value

    public List<string> errRceiptLst = null;

}
