#define MuFlag
#define DEBUG
using UnityEngine;
using System;
using System.Collections;
using Cross;

#if MuFlag
using MuGame;
using GameFramework;
#else
using CGBFramework;
using MGQJGame;

#endif

public class Main : MonoBehaviour
{
    public enum ENUM_DEBUG_STATE
    {
        none_debug = 0,
        update_resource_debug = 1,
        local_resource_debug = 2
    }

    //#if MuFlag
    //    static gameST game;

    //#else
    //    static MGQJGameClient m_gameClient;
    //#endif
    static osImpl m_os;

    // public bool m_bforceOnlyU3DAssets = true;
    public ENUM_DEBUG_STATE debugMode = ENUM_DEBUG_STATE.update_resource_debug;

    public bool TryUseRsourcePrefab = false;

    public int m_nTestMonsterID = 0;

    public string m_QSMY_ver = "20170606";
    public string client_ver = "1";            //提供整包更新的大版本号
    public string client_var_clear = "0";      //是否清楚整包替换时的缓存资源版本号

    public ENUM_QSMY_PLATFORM CurPlatform = ENUM_QSMY_PLATFORM.QSPF_None;
    public ENUM_SDK_PLATFORM CurSdkType = ENUM_SDK_PLATFORM.QISJ_QUICK;
    public ENUM_SDK_CHILD CurSdkChild = ENUM_SDK_CHILD.none;

    public static Main instance;
    public bool m_bUpdateGameOver = false;

    private bool m_bCGPlayOver = false;

    void Awake()
    {
        instance = this;

    }

    void Start()
    {
#if UNITY_IPHONE
        //gameObject.AddComponent<CR_EventHandle>();
        //gameObject.AddComponent<JX_EventHandle>();
        //gameObject.AddComponent<YM_EventHandle>();
        //gameObject.AddComponent<LY_EventHandle>();
        //gameObject.AddComponent<DY_EventHandle>();
        //gameObject.AddComponent<YMI_EventHandle>();
        //gameObject.AddComponent<ML_EventHandle
        gameObject.AddComponent<ZX_EventHandle>();
#endif

        //PlayerPrefs.SetInt("debugShow", 1);
        //debug.show_debug = true;


        debug.initLog();

        U3DAPI.Init_DEFAULT();

        //修改原手机已安装后覆盖会出现资源问题
        SimpleFramework.AppConst.AppName = "mu_" + client_ver;

        if (!Application.isMobilePlatform)
        {
            SimpleFramework.AppConst.UpdateMode = debugMode == ENUM_DEBUG_STATE.update_resource_debug;
        }



        //Baselayer.setDesignContentScale();

        AndroidPlotformSDK.ANDROID_PLOTFORM_SDK_CALL = AndroidJavaMethodCall;
        AndroidPlotformSDK.ANDROID_PLOTFORM_SDK_INFO_CALL = AndroidJavaMethodInfoCall;
        AndroidPlotformSDK.ANDROID_HIDE_STATUSBAR = AndroidHideStatusBar;

        IOSPlatformSDK.IOS_PLOTFORM_SDK_CALL = IOS_PLOTFORM_SDK_CALL;

        Globle.QSMY_Platform_Index = CurPlatform;
        Globle.QSMY_SDK_Index = CurSdkType;
        Globle.QSMY_SDK_CHILD = CurSdkChild;

        GameSdkMgr.init();
        AnyPlotformSDK.InitSDK();


        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //生成更新资源的下载目录
        AssetManagerImpl.UPDATE_DOWN_PATH = Application.persistentDataPath + "/OutAssets/v/" + m_QSMY_ver + "/";
        AssetManagerImpl.preparePath(AssetManagerImpl.UPDATE_DOWN_PATH);
        debug.Log(AssetManagerImpl.UPDATE_DOWN_PATH);

        DoAfterMgr.init();
        if (GameObject.Find("Sequence") == null)
        {
            //挂上UI Root根节点
            GameObject ui_root_prefab = Resources.Load("qsmy_uiRoot") as GameObject;
            if (ui_root_prefab != null)
            {
                GameObject ui_root = GameObject.Instantiate(ui_root_prefab) as GameObject;
            }

            GameObject eventsystem_prefab = Resources.Load("qsmy_EventSystem") as GameObject;
            if (eventsystem_prefab != null)
            {
                GameObject eventsystem = GameObject.Instantiate(eventsystem_prefab) as GameObject;
            }
        }

        Globle.DebugMode = debugMode == ENUM_DEBUG_STATE.none_debug ? 0 : 2;
        if (CurPlatform != 0)
            Globle.DebugMode = 0;


        if (!Application.isMobilePlatform)
        {
            Globle.VER = m_QSMY_ver;
            Globle.WebUrl = SimpleFramework.AppConst.WebUrl;
            Globle.QSMY_game_ver = m_QSMY_ver;

            Globle.m_nTestMonsterID = m_nTestMonsterID;
            InitGameMangager();
        }
        else
        {
            Debug.Log("waitting sdk call back");
        }

    }


    public void begin()
    {
        ////给TestBird测试用
        //int plat_uid = 70000000 + UnityEngine.Random.Range(1, 9999999);
        //m_debug_SvrList_Platuid = plat_uid.ToString();




        //初始化代码
        Globle.game_CrossMono = new gameST();

        //debug.Log("Screen.width::" + Screen.width + " " + Screen.height);

        instance = this;
        Screen.SetResolution(Screen.width, Screen.height, false);

        m_os = new osImpl();
        m_os.init(this.gameObject, Screen.width, Screen.height);

        //  if (m_bforceOnlyU3DAssets)
        {
            os.asset.async = false;
        }

        Globle.Lan = "zh_cn";
#if zh_sg
  Globle.Lan = "zh_sg";
#endif

#if zh_bs
  Globle.Lan = "zh_bs";
  Globle.DebugMode = 2;
  Globle.QSMY_Platform_Index = ENUM_QSMY_PLATFORM.QSPF_None;   
#endif

#if debug
  Globle.DebugMode=2;
        Globle.QSMY_Platform_Index = ENUM_QSMY_PLATFORM.QSPF_None;   
#endif




        if (GameObject.Find("Sequence") != null)
        {

            CG_PlayOver();
        }
        else
        {
            //以后游戏开头的CG只播放一次
            if (true)//if (false == PlayeLocalInfo.checkKey("cg_" + m_QSMY_ver))
            {
                PlayeLocalInfo.saveInt("cg_" + m_QSMY_ver, 1);
                StartCoroutine(PlayVideoCoroutine("qsmy_cg.mp4"));
            }
            else
            {
                m_bCGPlayOver = true;
            }
        }

    }

    IEnumerator PlayVideoCoroutine(string videoPath)
    {
        //Handheld.PlayFullScreenMovie(videoPath, Color.black, FullScreenMovieControlMode.CancelOnInput);
        yield return new WaitForEndOfFrame();
        debug.Log("片头CG播放完毕");

        m_bCGPlayOver = true;
    }

    void InitGameMangager()
    {
        string name = "GameManager";
        GameObject manager = GameObject.Find(name);
        if (manager == null)
        {
            manager = new GameObject(name);
            manager.name = name;

            AppFacade.Instance.StartUp();   //启动游戏
        }
    }


    private void platform_Call(string cmd)
    {
        AnyPlotformSDK.Call_Cmd(cmd);
    }

    private void CG_PlayOver()
    {
        Globle.A3_DEMO = true;

        Application.targetFrameRate = 60;

        //进行硬件的适配
        CheckDeviceInfo();
        bool has_ie_shader = true; //全屏特效
        if (Application.platform == RuntimePlatform.Android)
        {
            if (SystemInfo.graphicsDeviceVersion.IndexOf("OpenGL ES 3.0") < 0)
            {
                has_ie_shader = false;
            }
        }

        if (has_ie_shader)
        {
            //开启各种全屏的效果
            FastBloom.FAST_BLOOMSHADER = Resources.Load<Shader>("ieshader/MobileBloom");

            DepthOfField34.DOF_BLUR_SHADER = Resources.Load<Shader>("ieshader/SeparableWeightedBlurDof34");
            DepthOfField34.DOF_SHADER = Resources.Load<Shader>("ieshader/DepthOfField34");
            DepthOfField34.BOKEN_SHADER = Resources.Load<Shader>("ieshader/Bokeh34");
        }

        CheckUpdateVer();
    }

    void CheckUpdateVer()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            MsgBoxMgr.getInstance().showConfirm("无法连接网络", closeApp);
        }
        else
        {
            //  login.m_QSMY_Update_url = m_QSMY_Update_url + m_QSMY_ver.ToString() + "/OutAssets/";
            showLoginUI();
        }
    }

    void closeApp()
    {
        AnyPlotformSDK.Call_Cmd("close");
    }

    void showLoginUI()
    {
        //进入选服界面，这里gameST还没有初始化（<----请注意）
        debug.Log("进入了新的选服务器界面");
        //这是久的发送请求服务器列表的接口，改改就可以用了
        // LGPlatInfo.inst.test();
        InterfaceMgr.getInstance().ui_now_open(InterfaceMgr.LOGIN);

        //else if (PlotMain._inst.m_bUntestPlot)
        //{
        if (Globle.DebugMode > 0)
        {
            GameObject goCanvas_main = GameObject.Find("loadingLayer");
            GameObject loginroot = Resources.Load("login_loacal") as GameObject;
            //   GameObject go = GameObject.Find("login_loacal");
            if (goCanvas_main && loginroot)
            {
                GameObject go = GameObject.Instantiate(loginroot) as GameObject;
                go.transform.SetParent(goCanvas_main.transform, false);
                go.SetActive(true);

                if (go.GetComponent<login_loacal>() == null)
                    go.AddComponent<login_loacal>();
            }
        }
    }

    private void CheckDeviceInfo()
    {
        try
        {
            //设备的数据，需要在这里进行硬件的适配
            string systemInfo = null;
            systemInfo = "\tTitle:当前系统基础信息：\n设备模型：" + SystemInfo.deviceModel + "\n设备名称：" + SystemInfo.deviceName + "\n设备类型：" + SystemInfo.deviceType +
                "\n设备唯一标识符：" + SystemInfo.deviceUniqueIdentifier + "\n显卡标识符：" + SystemInfo.graphicsDeviceID +
                "\n显卡设备名称：" + SystemInfo.graphicsDeviceName + "\n显卡厂商：" + SystemInfo.graphicsDeviceVendor +
                "\n显卡厂商ID:" + SystemInfo.graphicsDeviceVendorID + "\n显卡支持版本:" + SystemInfo.graphicsDeviceVersion +
                "\n显存（M）：" + SystemInfo.graphicsMemorySize + "\n显卡像素填充率(百万像素/秒)，-1未知填充率：" + SystemInfo.graphicsPixelFillrate +
                "\n显卡支持Shader层级：" + SystemInfo.graphicsShaderLevel + "\n支持最大图片尺寸：" + SystemInfo.maxTextureSize +
                "\nnpotSupport：" + SystemInfo.npotSupport + "\n操作系统：" + SystemInfo.operatingSystem +
                "\nCPU处理核数：" + SystemInfo.processorCount + "\nCPU类型：" + SystemInfo.processorType;
            debug.Log(systemInfo);

            systemInfo = "\nsupportedRenderTargetCount：" + SystemInfo.supportedRenderTargetCount + "\nsupports3DTextures：" + SystemInfo.supports3DTextures +
             "\nsupportsAccelerometer：" + SystemInfo.supportsAccelerometer + "\nsupportsComputeShaders：" + SystemInfo.supportsComputeShaders +
             "\nsupportsGyroscope：" + SystemInfo.supportsGyroscope + "\nsupportsImageEffects：" + SystemInfo.supportsImageEffects +
             "\nsupportsInstancing：" + SystemInfo.supportsInstancing + "\nsupportsLocationService：" + SystemInfo.supportsLocationService +
             "\nsupportsRenderTextures：" + SystemInfo.supportsRenderTextures + "\nsupportsRenderToCubemap：" + SystemInfo.supportsRenderToCubemap +
             "\nsupportsShadows：" + SystemInfo.supportsShadows + "\nsupportsSparseTextures：" + SystemInfo.supportsSparseTextures +
             "\nsupportsStencil：" + SystemInfo.supportsStencil + "\nsupportsVertexPrograms：" + SystemInfo.supportsVertexPrograms +
             "\nsupportsVibration：" + SystemInfo.supportsVibration + "\n内存大小：" + SystemInfo.systemMemorySize;

            debug.Log(systemInfo);
        }
        catch (Exception e)
        {

        }
    }

    public void initParam(uint uid, string tkn, serverData sd)
    {
        Globle.game_CrossMono = new gameST();
        if (Globle.DebugMode == 2)
            Globle.setDebugServerD(sd.sid, "http://" + sd.ip + "/do.php");
        Globle.game_CrossMono.init(
                 "http://10.1.8.60/do.php",	/*server_config_url,*/
                sd.ip, /*server_ip, */
                sd.sid, /*server_id, */
                sd.port,/*port, */
                900000020,/*uid, */
                sd.clnt,/*clnt, */
                "123",/*token, */
                "main" /*mainConfig */
            );
    }

    void LateUpdate()
    {
        DoAfterMgr.instacne.onAfterRender();
        if (TickMgr.instance != null)
            TickMgr.instance.updateAfterRender();
    }

    void Update()
    {
        AnyPlotformSDK.FrameMove();

        if (true == m_bCGPlayOver)
        {
            m_bCGPlayOver = false;
            CG_PlayOver();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                //这里是安卓按回退键
                //os.sys.exit();
            }
        }


        if (m_bUpdateGameOver == false) return;

        float fdt = Time.deltaTime;

        SceneCamera.FrameMove();

        MonsterMgr._inst.FrameMove(fdt);
        OtherPlayerMgr._inst.FrameMove(fdt);
        FollowBullet_Mgr.FrameMove(fdt);
        //SceneFXMgr.FrameMove(fdt);
        InterfaceMgr.getInstance().FrameMove(fdt);

        SelfRole.FrameMove(fdt);
        if (TickMgr.instance != null) TickMgr.instance.update(fdt);
    }

    //string infoString;
    //// 接收AndroidJava传递的信息
    //void AndroidJavaReceiveString(string jsonString)
    //{
    //    // 项目自行处理jsonInfo字符串
    //    infoString = jsonString;
    //    Debug.LogError("--------->" + infoString);
    //    AndroidSDKManager.SetReceiveString(jsonString, m_os);
    //}


    // U3D默认值，不做修改 start
    private string androidJavaClass = "com.unity3d.player.UnityPlayer";
    private string androidJavaObject = "currentActivity";
    // U3D默认值，不做修改 end

    //private string infoString = "提示信息...";
    void AndroidJavaMethodCall(string androidJavaMethod)
    {
#if UNITY_ANDROID
        debug.Log("AndroidJavaMethodCall::" + androidJavaMethod);
        AndroidJavaClass jc = new AndroidJavaClass(androidJavaClass);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(androidJavaObject);
        jo.Call(androidJavaMethod);
#endif
    }

    void AndroidHideStatusBar()
    {
#if UNITY_ANDROID      
        AndroidJavaClass jc = new AndroidJavaClass(androidJavaClass);
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>(androidJavaObject);
        activity.Call("runOnUiThread", new AndroidJavaRunnable(setSystemUiVisibilityInThread));
#endif
    }

    void setSystemUiVisibilityInThread()
    {
#if UNITY_ANDROID
        int SYSTEM_UI_FLAG_IMMERSIVE_STICKY = 4096;
        int SYSTEM_UI_FLAG_HIDE_NAVIGATION = 2;
        int SYSTEM_UI_FLAG_FULLSCREEN = 4;
        using (var unityPlayer = new AndroidJavaClass(androidJavaClass))
        {
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>(androidJavaObject))
            {
                using (var window = activity.Call<AndroidJavaObject>("getWindow"))
                {
                    using (var view = window.Call<AndroidJavaObject>("getDecorView"))
                    {
                        view.Call("setSystemUiVisibility",
                             SYSTEM_UI_FLAG_FULLSCREEN | SYSTEM_UI_FLAG_HIDE_NAVIGATION | SYSTEM_UI_FLAG_IMMERSIVE_STICKY);
                    }
                }
            }
        }
#endif
    }

    void AndroidJavaMethodInfoCall(string androidJavaMethod, string infoJsonString)
    {
#if UNITY_ANDROID
        debug.Log("AndroidJavaMethodInfoCall::" + androidJavaMethod + " " + infoJsonString);
        AndroidJavaClass jc = new AndroidJavaClass(androidJavaClass);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(androidJavaObject);
        jo.Call(androidJavaMethod, infoJsonString);
#endif
    }

    void IOS_PLOTFORM_SDK_CALL(string cmd, string json_params)
    {
        if (cmd == "login")
        {
            //ML_EventHandle._instance.sdk_login();
            //YMI_EventHandle._instance.sdk_login();
            //DY_EventHandle._instance.sdk_login();
            //LY_EventHandle._instance.sdk_login();
            //YM_EventHandle._instance.sdk_login();
            //CR_EventHandle._instance.sdk_login();
            //JX_EventHandle._instance.sdk_login();
            ZX_EventHandle._instance.sdk_login();
        }
        else if (cmd == "pay")
        {
            string jsonString = json_params.Replace(" ", "");
            Variant v = JsonManager.StringToVariant(jsonString);
            //ML_EventHandle._instance.StartBuy(v);
            //YMI_EventHandle._instance.StartBuy(v);
            //DY_EventHandle._instance.StartBuy(v);
            //LY_EventHandle._instance.StartBuy(v);
            //YM_EventHandle._instance.StartBuy(v);
            //CR_EventHandle._instance.StartBuy(v);
            //JX_EventHandle._instance.StartBuy(v);
            ZX_EventHandle._instance.StartBuy(v);
        }
        else if (cmd == "loginout")
        {
            //ML_EventHandle._instance.sdk_loginOut();
            //YMI_EventHandle._instance.sdk_loginOut();
            //DY_EventHandle._instance.sdk_loginOut();
            //LY_EventHandle._instance.sdk_loginOut();
            //YM_EventHandle._instance.sdk_loginOut();
            //CR_EventHandle._instance.sdk_loginOut();
            //JX_EventHandle._instance.sdk_loginOut();
            ZX_EventHandle._instance.sdk_loginOut();
        }
        else if (cmd == "close")
        {
            // ML_EventHandle._instance.game_close();
            //YMI_EventHandle._instance.game_close();
            //DY_EventHandle._instance.game_close();
            //LY_EventHandle._instance.game_close();
            //YM_EventHandle._instance.game_close();
            //CR_EventHandle._instance.game_close();
            //JX_EventHandle._instance.game_close();
            ZX_EventHandle._instance.game_close();
        }
        else if (cmd == "createRole")
        {
            string jsonString = json_params.Replace(" ", "");
            Variant v = JsonManager.StringToVariant(jsonString);
            //LY_EventHandle._instance.sdk_createRole(v);
            //YM_EventHandle._instance.sdk_createRole(v);
            //JX_EventHandle._instance.sdk_createRole(v);
        }
        else if (cmd == "enterGame")
        {
            string jsonString = json_params.Replace(" ", "");
            Variant v = JsonManager.StringToVariant(jsonString);
            //LY_EventHandle._instance.sdk_enterGame(v);
            //YM_EventHandle._instance.sdk_enterGame(v);
            //CR_EventHandle._instance.sdk_enterGame(v);
            //JX_EventHandle._instance.sdk_enterGame(v);
        }
        else if (cmd == "exitPage")
        {
            string jsonString = json_params.Replace(" ", "");
            Variant v = JsonManager.StringToVariant(jsonString);
            //LY_EventHandle._instance.sdk_exitPage(v);
            //JX_EventHandle._instance.sdk_exitPage(v);
        }
        else if (cmd == "roleUpgrade")
        {
            string jsonString = json_params.Replace(" ", "");
            Variant v = JsonManager.StringToVariant(jsonString);
            //LY_EventHandle._instance.sdk_levelUp(v);
            //YM_EventHandle._instance.sdk_levelUp(v);
            //JX_EventHandle._instance.sdk_levelUp(v);
        }
        else
        {
            Debug.LogError("IOS_PLOTFORM_SDK_CALL error cmd = " + cmd + "   json=" + json_params);
        }
    }

    static public void IOS_SDK_Send(string jsonString)
    {
        if (Main.instance == null)
        {
            Debug.LogError("IOS_SDK_Send Main.instance == null");
        }
        else
        {
            Main.instance.AndroidJavaReceiveString(jsonString);
        }
    }

    // 接收AndroidJava传递的信息
    public void AndroidJavaReceiveString(string jsonString)
    {
        debug.Log("AndroidJavaReceiveString::" + jsonString);
        jsonString = jsonString.Replace(" ", "");
        Variant v = JsonManager.StringToVariant(jsonString);

        if (v.ContainsKey("result"))
        {
            if (v["result"]._int == 1)
            {
                if (debugMode == ENUM_DEBUG_STATE.none_debug)
                {
                    if (v.ContainsKey("data"))
                    {

                        Variant dta = v["data"];
                        if (dta.ContainsKey("versions"))
                        {
                            Globle.VER = dta["versions"];
                            Globle.QSMY_game_ver = Globle.VER.ToString();
                        }
                        if (dta.ContainsKey("resourceurl"))
                            SimpleFramework.AppConst.WebUrl = Globle.WebUrl = dta["resourceurl"];

                        if (dta.ContainsKey("clientversions"))
                        {
                            SimpleFramework.AppConst.clientVer = Globle.CLIENT_VER = dta["clientversions"];
                        }
                        if (dta.ContainsKey("clienturl"))
                        {
                            Globle.CLIENT_URL = dta["clienturl"];
                        } 
                    }
                }
                InitGameMangager();
                return;

            }

        }

        AnyPlotformSDK.Cmd_CallBack(v);

        //// 项目自行处理jsonInfo字符串
        //infoString = jsonString;
    }

    // 接收IOS SDK传递的信息
    void IOSSDKMessage(string jsonString)
    {
        debug.Log("IOS SDK Message::" + jsonString);
        Variant v = JsonManager.StringToVariant(jsonString);
        AnyPlotformSDK.Cmd_CallBack(v);
    }

    void OnApplicationFocus(bool isFocus)
    {
        debug.Log("--------OnApplicationFocus---" + isFocus);

        Globle.OnApplicationFocus(isFocus);
    }



}