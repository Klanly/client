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
    //#if MuFlag
    //    static gameST game;

    //#else
    //    static MGQJGameClient m_gameClient;
    //#endif
    static osImpl m_os;

    public bool m_bforceOnlyU3DAssets = true;
    public bool debugMode = true;

    public int m_QSMY_ver = 1511131004;
    public string m_QSMY_Update_url = "?";

    public string m_debug_SvrList_Platform = "lan";
    public string m_debug_SvrList_Platuid = "11111452";

    public ENUM_QSMY_PLATFORM CurPlatform = ENUM_QSMY_PLATFORM.QSPF_None;

    public static Main instance;

    private bool m_bCGPlayOver = false;
    
    void Start()
    {
        ////给TestBird测试用
        //int plat_uid = 70000000 + UnityEngine.Random.Range(1, 9999999);
        //m_debug_SvrList_Platuid = plat_uid.ToString();

        AnyPlotformSDK.InitSDK();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //生成更新资源的下载目录
        AssetManagerImpl.UPDATE_DOWN_PATH = Application.persistentDataPath + "/OutAssets/v/" + m_QSMY_ver.ToString() + "/";
        AssetManagerImpl.preparePath(AssetManagerImpl.UPDATE_DOWN_PATH);
        Debug.Log(AssetManagerImpl.UPDATE_DOWN_PATH);

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


        //初始化代码
        Globle.game_CrossMono = new gameST();

        Debug.Log("Screen.width::" + Screen.width + " " + Screen.height);

        instance = this;
        Screen.SetResolution(
             Screen.width, Screen.height,
            //GameConstant.SCREEN_DEF_WIDTH,
            //GameConstant.SCREEN_DEF_HEIGHT,
            true
        );
        m_os = new osImpl();
        m_os.init(
            this.gameObject, Screen.width, Screen.height
            //GameConstant.SCREEN_DEF_WIDTH,
            //GameConstant.SCREEN_DEF_HEIGHT
        );

        if (m_bforceOnlyU3DAssets)
        {
            os.asset.async = false;
        }


        Globle.DebugMode = debugMode ? 2 : 1;
        if (CurPlatform != 0)
            Globle.DebugMode = 0;
        Globle.QSMY_Platform_Index = CurPlatform;




        Globle.QSMY_game_ver = m_QSMY_ver.ToString();
        Globle.YR_srvlists__platform = m_debug_SvrList_Platform;
        Globle.YR_srvlists__platuid = m_debug_SvrList_Platuid;

        AndroidPlotformSDK.ANDROID_PLOTFORM_SDK_CALL = AndroidJavaMethodCall;
        AndroidPlotformSDK.ANDROID_PLOTFORM_SDK_INFO_CALL = AndroidJavaMethodInfoCall;

        if (GameObject.Find("Sequence") != null)
        {

            CG_PlayOver();
        }
        else
        {
            //以后游戏开头的CG只播放一次
            if(true)//if (false == PlayeLocalInfo.checkKey("cg_" + m_QSMY_ver))
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
        Handheld.PlayFullScreenMovie(videoPath, Color.black, FullScreenMovieControlMode.CancelOnInput);
        yield return new WaitForEndOfFrame();
        Debug.Log("片头CG播放完毕");

        m_bCGPlayOver = true;
    }


    private void platform_Call(string cmd)
    {
        AnyPlotformSDK.Call_Cmd(cmd);
    }

    private void CG_PlayOver()
    {
        Application.targetFrameRate = 30;

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

        if (PlotMain._inst.m_bUntestPlot)
        {
            CheckUpdateVer();
        }
    }

    void CheckUpdateVer()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            MsgBoxMgr.getInstance().showConfirm("无法连接网络", CheckUpdateVer, CheckUpdateVer);
        }
        else
        {
			login.m_QSMY_Update_url = m_QSMY_Update_url + m_QSMY_ver.ToString() + "/OutAssets/";
            showLoginUI();
        }
    }

    void showLoginUI()
    {
        //进入选服界面，这里gameST还没有初始化（<----请注意）
        Debug.Log("进入了新的选服务器界面");

        //这是久的发送请求服务器列表的接口，改改就可以用了
        // LGPlatInfo.inst.test();
        InterfaceMgr.getInstance().open(InterfaceMgr.LOGIN);

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
            Debug.Log(systemInfo);

            systemInfo = "\nsupportedRenderTargetCount：" + SystemInfo.supportedRenderTargetCount + "\nsupports3DTextures：" + SystemInfo.supports3DTextures +
             "\nsupportsAccelerometer：" + SystemInfo.supportsAccelerometer + "\nsupportsComputeShaders：" + SystemInfo.supportsComputeShaders +
             "\nsupportsGyroscope：" + SystemInfo.supportsGyroscope + "\nsupportsImageEffects：" + SystemInfo.supportsImageEffects +
             "\nsupportsInstancing：" + SystemInfo.supportsInstancing + "\nsupportsLocationService：" + SystemInfo.supportsLocationService +
             "\nsupportsRenderTextures：" + SystemInfo.supportsRenderTextures + "\nsupportsRenderToCubemap：" + SystemInfo.supportsRenderToCubemap +
             "\nsupportsShadows：" + SystemInfo.supportsShadows + "\nsupportsSparseTextures：" + SystemInfo.supportsSparseTextures +
             "\nsupportsStencil：" + SystemInfo.supportsStencil + "\nsupportsVertexPrograms：" + SystemInfo.supportsVertexPrograms +
             "\nsupportsVibration：" + SystemInfo.supportsVibration + "\n内存大小：" + SystemInfo.systemMemorySize;

            Debug.Log(systemInfo);
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
                 "http://10.1.8.45/do.php",	/*server_config_url,*/
                sd.ip, /*server_ip, */
                sd.sid, /*server_id, */
                sd.port,/*port, */
                uid,/*uid, */
                sd.clnt,/*clnt, */
                tkn,/*token, */
                "main" /*mainConfig */
            );
    }

    void LateUpdate()
    {
        DoAfterMgr.instacne.onAfterRender();
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
        Debug.Log("AndroidJavaMethodCall::" + androidJavaMethod);
        AndroidJavaClass jc = new AndroidJavaClass(androidJavaClass);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(androidJavaObject);
        jo.Call(androidJavaMethod);
#endif
    }

    void AndroidJavaMethodInfoCall(string androidJavaMethod, string infoJsonString)
    {
#if UNITY_ANDROID
        Debug.Log("AndroidJavaMethodInfoCall::" + androidJavaMethod + " " + infoJsonString);
        AndroidJavaClass jc = new AndroidJavaClass(androidJavaClass);
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>(androidJavaObject);
        jo.Call(androidJavaMethod, infoJsonString);
#endif
    }

    // 接收AndroidJava传递的信息
    void AndroidJavaReceiveString(string jsonString)
    {
        Debug.Log("AndroidJavaReceiveString::" + jsonString );
        Variant v = JsonManager.StringToVariant(jsonString);
        AnyPlotformSDK.Cmd_CallBack(v);

        //// 项目自行处理jsonInfo字符串
        //infoString = jsonString;
    }

    // 接收IOS SDK传递的信息
    void IOSSDKMessage(string jsonString)
    {
        Debug.Log("IOS SDK Message::" + jsonString);
        Variant v = JsonManager.StringToVariant(jsonString);
        AnyPlotformSDK.Cmd_CallBack(v);
    }

    void OnApplicationFocus(bool isFocus)
    {
        Debug.Log("--------OnApplicationFocus---" + isFocus);

        Globle.OnApplicationFocus(isFocus);
    }



}
