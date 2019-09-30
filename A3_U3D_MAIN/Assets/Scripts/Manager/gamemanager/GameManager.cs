using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;
//using Junfine.Debuger;
using UnityEngine.UI;
using DG.Tweening;
using MuGame;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimpleFramework.Manager
{
    public class GameManager : LuaBehaviour
    {
        public LuaScriptMgr uluaMgr;
        private List<string> downloadFiles = new List<string>();


        public long curupdate_downProcess = -1;
        public long maxProcess = -1;
        public string curUpdateFile = "";
        public int stateProcess = 0;
        public bool realT = false;


        public static GameManager instance;
        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  //防止销毁自己

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {


            Cross.LoaderBehavior.DATA_PATH = Util.DataPath;
            debug.Log("!!!!!!!!!!!!!!!!!!!!Util.DataPath:" + Util.DataPath);

            if (!Application.isMobilePlatform)
            {
                if (Main.instance.debugMode == Main.ENUM_DEBUG_STATE.local_resource_debug)
                {
                    string[] files = File.ReadAllLines(Util.AppContentPath() + "files.txt");
                    foreach (string file in files)
                    {
                        if (string.IsNullOrEmpty(file))
                            continue;

                        string[] fs = file.Split('|');
                        buildPatch(fs);
                    }
                    curupdate_downProcess = 0;
                    OnResourceInited();
                    return;
                }
                else if (Main.instance.debugMode == Main.ENUM_DEBUG_STATE.update_resource_debug)
                {
                    Cross.LoaderBehavior.DATA_PATH = Application.dataPath + "/" + AppConst.AssetDirname + "/";
                }
            }



            StartCoroutine(OnExtractResource());    //启动释放协成 
        }







        public Action onBeginHelperHandle;
        public Action onEndHelperHandle;
        IEnumerator onHelper()
        {
            try
            {
                if (onBeginHelperHandle != null)
                    onBeginHelperHandle();
            }
            catch (Exception e)
            {
                Debug.LogError("helper error1::" + e.Message);
            }

        
            string url = getUrl() + "helper/";
            string listUrl = url + "files.txt";

            WWW www = new WWW(listUrl);
            Debug.LogWarning("onHelper---->>>" + listUrl);
            yield return www;

            string dataPath = Util.DataPath;  //数据目录

            if (www.error == null)
            {
                string[] files = www.text.Split('\n');
                string fileUrl = "";
                string localfile = "";
                for (int i = 0; i < files.Length; i++)
                {
                    bool canUpdate = false;
                    try
                    {
                        if (string.IsNullOrEmpty(files[i]))
                            continue;
                        string[] keyValue = files[i].Split('|');
                        //  initPathFile(keyValue);
                        string f = keyValue[1].Replace("\n", "").Replace("\r", "");
                        localfile = (dataPath + f).Trim();
                        string path = Path.GetDirectoryName(localfile);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        fileUrl = url + f ;

                        canUpdate = !File.Exists(localfile);
                        if (!canUpdate)
                        {
                            if (keyValue.Length > 2)
                            {
                                string remoteMd5 = keyValue[2].Trim();
                                string localMd5 = Util.md5file(localfile);
                                canUpdate = !remoteMd5.Equals(localMd5);
                            }
                            else
                            {
                                canUpdate = true;
                            }
                            if (canUpdate)
                                File.Delete(localfile);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("helper error2:" + e.Message);
                        continue;
                    }

                    if (canUpdate)
                    {
                        debug.Log("help:" + fileUrl + " " + localfile);
                        BeginDownload(fileUrl, localfile);

                        bool done = false;
                        while (!done)
                        {
                            if (loadingErrorInfo != "")
                            {
                                loadingErrorInfo = "";
                                done = true;
                            }
                            else
                            {
                                done = IsDownOK(localfile);
                            }

                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
            }


            try
            {
                if (onEndHelperHandle != null)
                    onEndHelperHandle();
            }
            catch (Exception e)
            {
                Debug.LogError("helper error3:" + e.Message);
                yield break;
            }
        }



        public void buildPatch(string[] fs)
        {
            //if (fs[0] == "3")
            //{
            //    ResManager.addPatch(fs[1], fs[2], fs[3]);
            //}
        }

        public Action onNeedupdateclientver;


        public Action onCheckingVerHandle;
        public Action onCheckingUpdateHandle;
        public Action onEndCheckingHandle;




        IEnumerator OnExtractResource()
        {
            //if (!Application.isMobilePlatform && Main.instance.debugMode == Main.ENUM_DEBUG_STATE.update_resource_debug)
            //{
            //    yield return StartCoroutine(loadDebugRs());
            //    yield break;
            //}


            // if (isExists && inited == Main.instance.client_ver)
            yield return StartCoroutine(onHelper());

            string dataPath = Util.DataPath;  //数据目录
            string resPath = Util.AppContentPath(); //游戏包资源目录

            if (onCheckingVerHandle != null)
                onCheckingVerHandle();

            //初始化更新文案
            string outtxtfile = dataPath + "OutAssets/outGameTxt.txt";

            string outStr = "";
            if (Application.isMobilePlatform)
            {
                WWW www = new WWW(outtxtfile);
                yield return www;

                if (www.error != null)
                {
                    outStr = www.text;
                }
                else
                {
                    www = new WWW(resPath + "OutAssets/outGameTxt.txt");
                    yield return www;

                    if (www.isDone)
                        outStr = www.text;
                }
            }
            else
            {
                if (File.Exists(outtxtfile))
                {
                    outStr = File.ReadAllText(outtxtfile);
                }
                else if (File.Exists(resPath + "OutAssets/outGameTxt.txt"))
                {
                    outStr = File.ReadAllText(resPath + "OutAssets/outGameTxt.txt");
                }
            }
            OutGameContMgr.initOutGame(outStr);

            yield return new WaitForEndOfFrame();


            //检测大版本
            if (Application.isMobilePlatform)
            {
                if (Main.instance.client_ver != AppConst.clientVer
                    && AppConst.clientVer != "ver1.1.0"
                    && AppConst.clientVer != "ver 1.1.0"//新版本兼容老版本
                    )
                {
                    debug.Log("版本号错误：" + Main.instance.client_ver + " " + AppConst.clientVer);

                    if (onEndCheckingHandle != null)
                        onEndCheckingHandle();

                    if (onNeedupdateclientver != null)
                        onNeedupdateclientver();


                    yield break;
                }
            }

            string clearinited = PlayerPrefs.GetString("clearinited");
            if (clearinited != Main.instance.client_var_clear)
            {
                if (Directory.Exists(dataPath))
                {
                    Directory.Delete(dataPath, true);
                    yield return new WaitForEndOfFrame();
                }
            }

            PlayerPrefs.SetString("clearinited", Main.instance.client_var_clear);


            bool isExists = Directory.Exists(Util.DataPath) && File.Exists(Util.DataPath + "files.txt");
            string inited = PlayerPrefs.GetString("inited");
            debug.Log("???????????CheckExtractResource::" + isExists + " " + inited + "  " + Application.isMobilePlatform + " " + (inited == Main.instance.client_ver));

            //检查是否要展开包

            if (isExists && inited == Main.instance.client_ver)//不需要展开，进入更新流程
            {
                yield return StartCoroutine(OnUpdateResource());
                yield break;
            }

            if (onInitingHandle != null)
            {
                onInitingHandle();
            }

            //开始展开包

            //Debug.Log("开始展开包");
            if (Directory.Exists(dataPath))
            {
                Directory.Delete(dataPath, true);
                yield return new WaitForEndOfFrame();
            }

            //Debug.Log("Directory.CreateDirectory");
            Directory.CreateDirectory(dataPath);


            string infile = resPath + "files.txt";
            string outfile = dataPath + "files.txt";
            if (File.Exists(outfile)) File.Delete(outfile);

            string message = "正在解包文件:>files.txt";
            //debug.Log("正在解包文件:in  files.txt=" + infile);
            //debug.Log("正在解包文件:out files.txt=" + outfile);

            if (Application.isMobilePlatform)
            {
#if UNITY_ANDROID
                //debug.Log("www 加载infile!!!");
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    //debug.Log("www 写入 outfile");
                    //debug.Log("www 写入 长度 " + www.bytes.Length);
                    File.WriteAllBytes(outfile, www.bytes);
                }

                //debug.Log("结束写入 outfile");
                www.Dispose();
                www = null;
                yield return 0;
#else
                //debug.Log("拷贝的测试 outfile");
                File.Copy(infile, outfile, true);
#endif
            }
            else
            {
                File.Copy(infile, outfile, true);
            };
            yield return new WaitForEndOfFrame();

            //释放所有文件到数据目录
            //Debug.Log("释放所有文件到数据目录");
            string[] files = File.ReadAllLines(outfile);
            //if( files != null )
            //{
            //    if (files.Length > 0)
            //    {
            //        Debug.Log("files[0]=" + files[0]);
            //    }
            //    else
            //    {
            //        Debug.Log("files的长度 = " + files.Length);
            //    }
            //}
            //else
            //{
            //    Debug.Log("files is null----------------------");
            //}

            maxProcess = files.Length;
            curupdate_downProcess = 0;
            stateProcess = 1;
            realT = true;
            int emptyidx = 0;
            foreach (var file in files)
            {
                string[] fs = file.Split('|');

                if (fs[0] == "2")
                {
                    infile = resPath + fs[1];  //
                    outfile = dataPath + fs[1];

                    //message = "正在解包文件:>" + fs[1];
                    ////    debug.Log("正在解包文件:>" + outfile);
                    //facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

                    string dir = Path.GetDirectoryName(outfile);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

#if UNITY_ANDROID
                            WWW www = new WWW(infile);
                            yield return www;
                  
                    if (www.isDone)
                            {
                                File.WriteAllBytes(outfile, www.bytes);
                            }
                            www.Dispose();
                            www = null;
                    //  yield return 0;
#else
                    if (File.Exists(outfile))
                    {
                        File.Delete(outfile);
                    }
                    File.Copy(infile, outfile, true);
#endif
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    emptyidx++;
                    if(emptyidx>20)
                    {
                        emptyidx = 0;
                        yield return new WaitForEndOfFrame();
                    }

                    //  debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!::::::" + fs[1]);
                }

                curupdate_downProcess++;
               


            }

            //Debug.Log("解包完成");

            message = "解包完成!!!";
            curupdate_downProcess = -1;
            stateProcess = 0;

            facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
            yield return new WaitForSeconds(0.1f);

            message = string.Empty;

            if (onInitOverHandle != null)
            {
                onInitOverHandle();
            }



            PlayerPrefs.SetString("inited", Main.instance.client_ver);


            //释放完成，开始启动更新资源
            // load();
            yield return StartCoroutine(onHelper());

            yield return StartCoroutine(OnUpdateResource());
        }

        public Action onInitOverHandle;



        /// <summary>
        /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
        /// </summary>
        IEnumerator OnUpdateResource()
        {


            WWW www = new WWW(getUrl() + "verMd5.txt");
            yield return www;
            bool needUpdate = true;
            if (www.error == null)
            {
                string localfile = (Util.DataPath + "files.txt").Trim();

                string md5str = Util.md5file(localfile);
                needUpdate = www.text.Replace("\n", "").Replace("\r", "") != md5str;
                debug.Log("md5::" + www.text + " " + md5str + " " + needUpdate);
            }
            www.Dispose();
            www = null;


            if (MuGame.Globle.QSMY_game_ver == "android_zh_20180207"
                || MuGame.Globle.QSMY_game_ver == "android_zh_20180523"
                || MuGame.Globle.QSMY_game_ver == "ios_zh_20180523"
                )
            {//兼容线上版本
                needUpdate = false;
            }


            if (!needUpdate)
            {
                //string fileUrl = getUrl() + "files.txt?v=" + random;



                //www = new WWW(fileUrl);
                //yield return www;

                //if (www.isDone)
                //{
                //    debug.Log("!!!!!!!!!!!!!!!!:aaa:::" + fileUrl);

                //    string filesText = www.text;
                //    string[] f = filesText.Split('\n');
                //    foreach (string file in f)
                //    {
                //        if (string.IsNullOrEmpty(file))
                //            continue;
                //        string[] fs = file.Split('|');
                //        buildPatch(fs);
                //    }

                //}

                if (onEndCheckingHandle != null)
                    onEndCheckingHandle();

                curupdate_downProcess = 0;
                OnResourceInited();

                yield break;
            }






            if (onCheckingUpdateHandle != null)
                onCheckingUpdateHandle();

            //if (!AppConst.UpdateMode)
            //{
            //    OnResourceInited();
            //    yield break;
            //}
            string dataPath = Util.DataPath;  //数据目录

            string message = string.Empty;
            //     string random = DateTime.Now.ToString("yyyymmddhhmmss");
            string url = getUrl();
            string listUrl = url + "files.txt" ;
            Debug.LogWarning("LoadUpdate---->>>" + listUrl);

            www = new WWW(listUrl);
            yield return www;
            bool waiting = true;
            int waitingsec = 0;
            while (waiting)
            {
                debug.Log("::" + www.isDone);
                if (www.isDone)
                {
                    waiting = false;
                    if (www.error != null)
                    {

                        Debug.LogWarning("www.error---->>>" + www.error.ToString());
                        onLoadingError(OutGameContMgr.getOutGameCont("error", "1"));
                        yield break;
                    }
                }

                else if (waitingsec >= 3)
                {
                    waiting = false;
                    onLoadingError("Loading Timeout");
                    yield break;
                }

                waitingsec++;
                yield return new WaitForSeconds(1f);
            }

            //  yield return www;





            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            string filesText = www.text;
            string[] files = filesText.Split('\n');
            string localFilesPath = Util.DataPath + "files.txt";
            Dictionary<string, string> dlocalFiles = new Dictionary<string, string>();
            if (File.Exists(Util.DataPath + "files.txt"))
            {
                string[] localFiles = File.ReadAllLines(Util.DataPath + "files.txt");
                foreach (string file in localFiles)
                {
                    string[] fileinfo = file.Split('|');
                    if (fileinfo[0] == "1")
                    {
                        dlocalFiles[fileinfo[1]] = fileinfo[2];
                    }
                }

                yield return new WaitForEndOfFrame();
            }



            Dictionary<string, List<string>> filesNeedUpdate = new Dictionary<string, List<string>>();
            Dictionary<string, long> filesSize = new Dictionary<string, long>();
            long updatesize = 0;

            for (int i = 0; i < files.Length; i++)
            {
                if (string.IsNullOrEmpty(files[i])) continue;
                string[] keyValue = files[i].Split('|');

                if (keyValue[0] != "3")
                {
                    string f = keyValue[1];
                    string localfile = (dataPath + f).Trim();
                    string path = Path.GetDirectoryName(localfile);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    bool isinstream = keyValue[0] == "1";
                    string fileUrl = url + f ;
                    bool canUpdate = !File.Exists(localfile);

                    if (!canUpdate)//比对包外
                    {
                        string remoteMd5 = keyValue[2].Trim();
                        string localMd5 = Util.md5file(localfile);
                        canUpdate = !remoteMd5.Equals(localMd5);


                        if (canUpdate)
                            File.Delete(localfile);
                    }
                    else if (isinstream)//不存在且是保内文件
                    {
                        if (dlocalFiles.ContainsKey(f))
                        {
                            if (dlocalFiles[f] == keyValue[2])
                                canUpdate = false;
                        }

                    }

                    if (canUpdate)
                    {
                        long size = long.Parse(keyValue[3]);
                        updatesize += size;
                        filesNeedUpdate[fileUrl] = new List<string>() { localfile, f };
                        filesSize[fileUrl] = size;


                    }
                }
                else
                {

                    buildPatch(keyValue);
                }
            }



            Debug.Log("------------------------------------------------------------------------");

            foreach (string k in filesNeedUpdate.Keys)
            {
                Debug.Log(k);
            }


            Debug.Log("------需要更新：" + updatesize + "  " + filesNeedUpdate.Keys.Count);

            //  debug.Log("需要更新：" + updatesize + "  " + filesNeedUpdate.Keys.Count);

            int filenum = filesNeedUpdate.Keys.Count;


            maxProcess = updatesize;

            if (filesNeedUpdate.Count > 0)
            {
                if (onComfirmUpdate != null)
                {
                    onComfirmUpdate(updatesize, filenum, onUpdateClick);
                }



                while (waitupdating)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                realT = true;

                stateProcess = 2;
                curupdate_downProcess = 0;
                foreach (string key in filesNeedUpdate.Keys)
                {
                    string curlocalfile = filesNeedUpdate[key][0];
                    debug.Log(key);
                    message = "downloading>>" + key;
                    facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
                    curUpdateFile = filesNeedUpdate[key][1];
                    //这里都是资源文件，用线程下载
                    BeginDownload(key, curlocalfile);
                    bool done = false;
                    while (!done)
                    {

                        if (loadingErrorInfo != "")
                        {
                            onLoadingError(loadingErrorInfo);
                            loadingErrorInfo = "";
                        }
                        else
                        {
                            done = IsDownOK(curlocalfile);
                        }

                        yield return new WaitForEndOfFrame();
                    }
                    if (filesSize.ContainsKey(key))
                        curupdate_downProcess += filesSize[key];

                    curUpdateFile = "";
                    debug.Log("::::::::::" + realT + "  " + curupdate_downProcess + "::" + maxProcess);
                }

            }



            yield return new WaitForEndOfFrame();

            if (onEndCheckingHandle != null)
                onEndCheckingHandle();

            File.WriteAllBytes(dataPath + "files.txt", www.bytes);
            curupdate_downProcess = maxProcess;
            message = "更新完成!!";
            facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);
            //    stateProcess = 0;
            OnResourceInited();

            //Debug.Log("更新完毕");
        }

        public Action<long, int, Action> onComfirmUpdate;

        public void onLoadingError(string error)
        {
            if (onEndCheckingHandle != null)
                onEndCheckingHandle();

            if (!Application.isMobilePlatform || Main.instance.debugMode != Main.ENUM_DEBUG_STATE.none_debug)
            {
                curupdate_downProcess = 0;
                OnResourceInited();
            }
            else
                OnUpdateFailed(error);
        }

        public Action<Action> onComFirmWifihandle;



        bool waitupdating = true;
        public void onUpdateClick()
        {
            //if (!Util.IsWifi)
            //{
            //    if (onComFirmWifihandle != null)
            //    {
            //        onComFirmWifihandle(onWifiClick);
            //    }
            //    else
            //        waitupdating = false;

            //    return;
            //}
            waitupdating = false;
        }

        public void onWifiClick()
        {
            waitupdating = false;
        }

        public string getUrl()
        {

            if (!Application.isMobilePlatform && Main.instance.debugMode == Main.ENUM_DEBUG_STATE.update_resource_debug)
            {
                return "http://10.1.8.60/muAsset/test2/StreamingAssets/";
            }


            string url = MuGame.Globle.WebUrl + MuGame.Globle.QSMY_game_ver + "/";

#if UNITY_IPHONE
            url += "ios/";
#else
            url += "android/";
#endif
            MuGame.login.m_QSMY_Update_url = url;
            return url;
        }


        public Action<string> onUpdateFailhanlde;

        void OnUpdateFailed(string file)
        {
            //  string message = "更新失败!>" + file;
            if (onUpdateFailhanlde != null)
                onUpdateFailhanlde(file);

        }

        /// <summary>
        /// 是否下载完成
        /// </summary>
        bool IsDownOK(string file)
        {
            return downloadFiles.Contains(file);
        }

        /// <summary>
        /// 线程下载
        /// </summary>
        void BeginDownload(string url, string file)
        {     //线程下载
            object[] param = new object[2] { url, file };

            ThreadEvent ev = new ThreadEvent();
            ev.Key = NotiConst.UPDATE_DOWNLOAD;
            ev.evParams.AddRange(param);
            ThreadManager.AddEvent(ev, OnThreadCompleted);   //线程下载
        }

        /// <summary>
        /// 线程完成
        /// </summary>
        /// <param name="data"></param>
        void OnThreadCompleted(NotiData data)
        {
            switch (data.evName)
            {
                case NotiConst.UPDATE_EXTRACT:  //解压一个完成
                                                //
                    break;
                case NotiConst.UPDATE_DOWNLOAD: //下载一个完成
                    downloadFiles.Add(data.evParam.ToString());
                    break;
                case NotiConst.UPDATE_ERROR: //错误
                    Debug.LogError("更新错误：：" + data.evParam);
                    loadingErrorInfo = OutGameContMgr.getOutGameCont("error", "2");

                    break;
            }
        }

        public string loadingErrorInfo = "";

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited()
        {
            m_b_loadAsset = true;
            ResManager.Initialize();
        }

        public Action<Action> onAllDone;

        void resourceLoadedOverhandle()
        {
            //     debug.Log("resourceLoadedOverhandle");
            //#if !ASYNC_MODE
            //            //  debug.Log("!ASYNC_MODE");

            //#endif

            Debug.Log("开始加载lua文件-------------------------------------------------------------------------------");

            LuaManager.Start();
            //LuaManager.DoFile("Logic/Network");      //加载游戏
            UnityEngine.Debug.Log("run lua file Logic/GameManager");
            LuaManager.DoFile("Logic/GameManager");   //加载网络

            Debug.Log("结束加载lua文件------------------------------------------------------------------------------");
            initialize = true;



            //object[] panels = CallMethod("LuaScriptPanel");
            ////---------------------Lua面板---------------------------
            //foreach (object o in panels)
            //{
            //    string name = o.ToString().Trim();
            //    if (string.IsNullOrEmpty(name)) continue;
            //    name += "Panel";    //添加

            //    LuaManager.DoFile("View/" + name);
            //    Debug.LogWarning("LoadLua---->>>>" + name + ".lua");
            //}
            ////------------------------------------------------------------



            CallMethod("OnInitOK");   //初始化完成

            Main.instance.begin();

            if (onResourceLoadedOverhandle != null)
            {
                onResourceLoadedOverhandle();

            }



            // InterfaceMgr.instacne.open("wintest");          
        }

        public Action onResourceLoadedOverhandle;
        public Action onInitingHandle;
        bool m_b_loadAsset = false;
        void Update()
        {
            if (LuaManager != null && initialize)
            {
                LuaManager.Update();
            }

            if (m_b_loadAsset)
            {
                if(ResManager.m_nLoading_ab_Task > 0)
                {
                    UpdateScrollbar.SetShow_Txt("开始异步加载初始资源 " + ResManager.m_nLoading_ab_Task);
                }

                if (ResManager.b_load_finish && ResManager.m_nLoading_ab_Task == 0)
                {
                    UpdateScrollbar.SetShow_Txt("加载完毕");

                    //要移到第一次进入游戏中
                    //MuGame.AnyPlotformSDK.LoadAB_Res();

                    if (onAllDone != null)
                        onAllDone(resourceLoadedOverhandle);

                    m_b_loadAsset = false;
                }
            }
        }

        void LateUpdate()
        {
            if (LuaManager != null && initialize)
            {
                LuaManager.LateUpate();
            }
        }

        void FixedUpdate()
        {
            if (LuaManager != null && initialize)
            {
                LuaManager.FixedUpdate();
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy()
        {

            if (LuaManager != null)
            {
                LuaManager.Destroy();
                LuaManager = null;
            }
            debug.Log("~GameManager was destroyed");
        }








    }
}

