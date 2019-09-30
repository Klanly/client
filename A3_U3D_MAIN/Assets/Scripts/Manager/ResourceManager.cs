using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MuGame;


namespace SimpleFramework.Manager
{
    public enum ABF_LOAD
    {
        ABFL_NONE = 0,
        ABFL_LOADING = 1,
        ABFL_LOADED = 100,
        ABFL_FAILED = -1,
    }

    public class OneABFile
    {
        public OneABFile(string asset_name)
        {
            m_strOneAssetName = asset_name;
            m_nCacheLifeCount = 1;
        }

        public string m_strOneAssetName = null;
        public Object m_cacheObj = null;
        public int m_nCacheLifeCount = 0;
        public ABF_LOAD m_load_Step = ABF_LOAD.ABFL_NONE;
        public AssetBundle m_file_AB = null;

        //public Dictionary<string, Object> m_mapLoadedResAB = new Dictionary<string, Object>();
        public List<System.Action<Object, System.Object>> m_listAB_CB = new List<System.Action<Object, System.Object>>();
        public List<System.Object> m_listAB_CDATA = new List<System.Object>();
    }

    public class ResourceManager : View
    {
        public static ResourceManager _inst;

        private int m_listCallBack_AB_Index = 0;
        private List<OneABFile> m_listCallBack_ABFiles = new List<OneABFile>();
        private Dictionary<string, OneABFile> m_mapResAB_bundles = new Dictionary<string, OneABFile>();
        //  private Dictionary<string, Dictionary<string, string>> m_mapPatchInfo = new Dictionary<string, Dictionary<string, string>>();
        private OneABFile m_UnloadOne = null;

        public bool b_load_finish = false;

        void Awake()
        {
            GAMEAPI.LoadAsset_Async = LoadAsset_Async;
            GAMEAPI.Res_Async_Loaded = Res_Async_Loaded;
            GAMEAPI.Unload_Asset = Unload_Asset;
            GAMEAPI.LoadAsset_Obj = LoadAsset_Obj;
            GAMEAPI.LoadOneAsset_Async = LoadOneAsset_Async;
            GAMEAPI.LoadNow_GameObject_OneAsset = LoadNow_OneAsset<GameObject>;
            GAMEAPI.LoadNow_Sprite_OneAsset = LoadNow_OneAsset<Sprite>;
            GAMEAPI.ClearAllOneAsset = ClearAllOneAsset;
            GAMEAPI.KeepOneAsset = KeepOneAsset;
            GAMEAPI.KillOneAsset = KillOneAsset;

            _inst = this;
        }

        bool Res_Async_Loaded()
        {
            return m_nLoading_ab_Task == 0;
        }

        void ClearAllOneAsset()
        {
            if (m_mapResAB_bundles.Count <= 0) return;
            string[] map_keys = new string[m_mapResAB_bundles.Count];

            m_mapResAB_bundles.Keys.CopyTo(map_keys, 0);
            for (int i = 0; i < map_keys.Length; ++i)
            {
                OneABFile one_abf = m_mapResAB_bundles[map_keys[i]];
                if (one_abf.m_strOneAssetName != null && one_abf.m_load_Step == ABF_LOAD.ABFL_LOADED)
                {
                    --one_abf.m_nCacheLifeCount;

                    if (one_abf.m_nCacheLifeCount < 0)
                    {
                        one_abf.m_cacheObj = null;
                        one_abf.m_load_Step = ABF_LOAD.ABFL_NONE;

                        //Debug.Log("清理了加载资源:" + one_abf.m_strOneAssetName);
                    }
                }
            }

            map_keys = null;
        }

        T LoadNow_OneAsset<T>(string abname, string assetname) where T : UnityEngine.Object
        {
            abname = abname.ToLower();
            abname = getPatchAb(abname, assetname);
            if (m_mapResAB_bundles.ContainsKey(abname) == false)
            {
                m_mapResAB_bundles.Add(abname, new OneABFile(assetname));
            }

            OneABFile one_abf = m_mapResAB_bundles[abname];
            if (one_abf.m_load_Step == ABF_LOAD.ABFL_NONE)
            {
                one_abf.m_load_Step = ABF_LOAD.ABFL_LOADING;

                string localfile = Util.DataPath + abname;
                if (File.Exists(localfile))
                {
                    //Debug.Log("读取本地文件 " + localfile);
                }
                else
                {
                    //如果文件不存在就从主包内读取文件
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        localfile = Application.dataPath + "/Raw/" + abname;
                    }
                    else if (Application.platform == RuntimePlatform.Android)
                    {
                        localfile = Application.dataPath + "!assets/" + abname;
                    }
                    else
                    {
                        localfile = Application.dataPath + "/" + AppConst.AssetDirname + "/" + abname;
                    }

                    //Debug.Log("读取安装包内文件 " + localfile);
                }

                one_abf.m_file_AB = AssetBundle.LoadFromFile(localfile);

                if (one_abf.m_file_AB == null)
                {
                    Debug.LogError("load now fail: " + localfile);
                    one_abf.m_load_Step = ABF_LOAD.ABFL_FAILED;
                }
                else
                {
                    if (one_abf.m_strOneAssetName != null)
                    {
                        one_abf.m_cacheObj = one_abf.m_file_AB.LoadAsset<T>(one_abf.m_strOneAssetName);
                        //debug.Log("文件解析成功0000000 " + localfile);

                        one_abf.m_file_AB.Unload(false);
                        one_abf.m_file_AB = null;

                        one_abf.m_load_Step = ABF_LOAD.ABFL_LOADED;
                        //Debug.Log("Load Now 文件加载成功 " + localfile);
                    }
                    else
                    {
                        Debug.LogError("m_strOneAssetName null fail: " + localfile);
                        one_abf.m_load_Step = ABF_LOAD.ABFL_FAILED;
                    }
                }
            }

            one_abf.m_nCacheLifeCount = 1;
            return one_abf.m_cacheObj as T;
        }

        void KeepOneAsset(string abname)
        {
            abname = abname.ToLower();
            abname = getPatchAb(abname, "");

            if (m_mapResAB_bundles.ContainsKey(abname) == true)
            {
                OneABFile one_abf = m_mapResAB_bundles[abname];
                one_abf.m_nCacheLifeCount = int.MaxValue;
            }
        }

        void KillOneAsset(string abname)
        {
            abname = abname.ToLower();
            abname = getPatchAb(abname, "");

            if (m_mapResAB_bundles.ContainsKey(abname) == true)
            {
                OneABFile one_abf = m_mapResAB_bundles[abname];
                one_abf.m_nCacheLifeCount = 0;
            }
        }

        void LoadOneAsset_Async(string abname, string assetname, System.Action<Object, System.Object> call_back, System.Object back_data)
        {
            abname = abname.ToLower();
            abname = getPatchAb(abname, assetname);
            if (m_mapResAB_bundles.ContainsKey(abname) == false)
            {
                m_mapResAB_bundles.Add(abname, new OneABFile(assetname));
            }

            OneABFile one_abf = m_mapResAB_bundles[abname];
            one_abf.m_nCacheLifeCount = 1;
            switch (one_abf.m_load_Step)
            {
                case ABF_LOAD.ABFL_NONE:
                    //添加到表中
                    one_abf.m_listAB_CB.Add(call_back);
                    one_abf.m_listAB_CDATA.Add(back_data);

                    m_listCallBack_ABFiles.Add(one_abf);
                    Start_LoadAB(abname, one_abf);
                    break;
                case ABF_LOAD.ABFL_LOADING:
                    //添加到表中
                    one_abf.m_listAB_CB.Add(call_back);
                    one_abf.m_listAB_CDATA.Add(back_data);

                    m_listCallBack_ABFiles.Add(one_abf);
                    break;
                case ABF_LOAD.ABFL_LOADED:
                    call_back(one_abf.m_cacheObj, back_data);
                    break;
                case ABF_LOAD.ABFL_FAILED:
                    call_back(null, back_data);
                    break;
            }
        }

        void Update()
        {
            if (m_listCallBack_ABFiles.Count > 0)
            {
                if (m_listCallBack_AB_Index > m_listCallBack_ABFiles.Count) m_listCallBack_AB_Index = 0;

                OneABFile one_abf = m_listCallBack_ABFiles[m_listCallBack_AB_Index];
                if (one_abf.m_load_Step != ABF_LOAD.ABFL_LOADING)
                {
                    Object cb_obj = null;
                    if (one_abf.m_load_Step == ABF_LOAD.ABFL_LOADED)
                    {
                        cb_obj = one_abf.m_cacheObj;
                    }

                    for (int i = 0; i < one_abf.m_listAB_CB.Count; ++i)
                    {
                        try
                        {
                            one_abf.m_listAB_CB[i](cb_obj, one_abf.m_listAB_CDATA[i]);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError(ex.ToString());
                        }
                    }

                    one_abf.m_listAB_CB.Clear();
                    one_abf.m_listAB_CDATA.Clear();

                    m_listCallBack_ABFiles.Remove(one_abf);
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            UpdateScrollbar.SetShow_Txt("开始异步加载初始资源");

            LoadAsset_Async("ab_font.assetbundle", "");

            LoadAsset_Async("ab_ui.assetbundle", "");
            //LoadAsset_Async("ab_layer.assetbundle", "");

            //LoadAsset_Async("ab_fight.assetbundle", "");

            //加载3个职业的基础模型动作
            LoadOneAsset_Async("profession_warrior_inst.assetbundle", "profession_warrior_inst", Profession_LoadedOK, null);
            LoadOneAsset_Async("profession_mage_inst.assetbundle", "profession_mage_inst", Profession_LoadedOK, null);
            LoadOneAsset_Async("profession_assa_inst.assetbundle", "profession_assa_inst", Profession_LoadedOK, null);
        }

        public void LoadGameFightNeedRes()
        {
            LoadAsset_Async("ab_fight.assetbundle", "");

            //前期需要的UI
            LoadOneAsset_Async("uilayer_maploading.assetbundle", "uilayer_maploading", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_wait_loading.assetbundle", "uilayer_wait_loading", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a1_gamejoy.assetbundle", "uilayer_a1_gamejoy", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_A3_BeStronger.assetbundle", "uilayer_A3_BeStronger", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_flytxt.assetbundle", "uilayer_flytxt", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_liteminimap.assetbundle", "uilayer_a3_liteminimap", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_expbar.assetbundle", "uilayer_a3_expbar", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_equipup.assetbundle", "uilayer_a3_equipup", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_lowblood.assetbundle", "uilayer_a3_lowblood", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_trrigerdialog.assetbundle", "uilayer_a3_trrigerdialog", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_buff.assetbundle", "uilayer_a3_buff", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_teamapplypanel.assetbundle", "uilayer_a3_teamapplypanel", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_lvup.assetbundle", "uilayer_a3_lvup", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_mapname.assetbundle", "uilayer_a3_mapname", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_funcopen.assetbundle", "uilayer_a3_funcopen", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_skillopen.assetbundle", "uilayer_a3_skillopen", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_runeopen.assetbundle", "uilayer_a3_runeopen", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_tragethead.assetbundle", "uilayer_tragethead", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_quickop.assetbundle", "uilayer_a3_quickop", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_taskopt.assetbundle", "uilayer_a3_taskopt", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_chatroom.assetbundle", "uilayer_a3_chatroom", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_attchange.assetbundle", "uilayer_a3_attchange", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_pk_notify.assetbundle", "uilayer_pk_notify", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a3_herohead.assetbundle", "uilayer_a3_herohead", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_broadcasting.assetbundle", "uilayer_broadcasting", GameFight_LoadedOK, null);

            //lua__ui
            LoadOneAsset_Async("uilayer_flytxt.assetbundle", "uilayer_flytxt", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_fightingup.assetbundle", "uilayer_fightingup", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a1_low_fightgame.assetbundle", "uilayer_a1_low_fightgame", GameFight_LoadedOK, null);
            LoadOneAsset_Async("uilayer_a1_high_fightgame.assetbundle", "uilayer_a1_high_fightgame", GameFight_LoadedOK, null);
        }

        private void GameFight_LoadedOK(UnityEngine.Object model_obj, System.Object data)
        {

        }

        private void Profession_LoadedOK(UnityEngine.Object model_obj, System.Object data)
        {

        }

        public void addPatch(string patchAb, string ab, string an)
        {
            //if (!m_mapPatchInfo.ContainsKey(ab))
            //    m_mapPatchInfo[ab] = new Dictionary<string, string>();

            //m_mapPatchInfo[ab][ab] = patchAb;
            //m_mapPatchInfo[ab][an] = patchAb;
            //debug.Log("!!!!!addPatch!!!!!" + patchAb + "  " + an);
        }

        public string getPatchAb(string ab, string an)
        {
            return "mother_package/" + ab;
            //if (!m_mapPatchInfo.ContainsKey(ab))
            //    return "mother_package/" + ab;
            //Dictionary<string, string> d = m_mapPatchInfo[ab];

            //if (!d.ContainsKey(an))
            //    return "mother_package/" + ab;
            //string patch = m_mapPatchInfo[ab][an];
            //debug.Log("!!!!!getPatchAb!!!!!" + patch + "  " + an);
            //return patch;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        public GameObject LoadAsset(string abname, string assetname)
        {
            if (!Application.isMobilePlatform && Main.instance.TryUseRsourcePrefab)
            {
                GameObject go = Resources.Load(assetname) as GameObject;
                if (go != null)
                    return go;
            }

            return LoadAsset_FromAB<GameObject>(abname, assetname, U3DAPI.DEF_GAMEOBJ);
        }

        public Object LoadAsset_Obj(string abname, string assetname)
        {
            return LoadAsset_FromAB<Object>(abname, assetname, U3DAPI.DEF_GAMEOBJ);
        }

        private T LoadAsset_FromAB<T>(string abname, string assetname, T def_obj) where T : UnityEngine.Object
        {
            abname = abname.ToLower();
            abname = getPatchAb(abname, assetname);
            OneABFile bundle = LoadAssetBundle(abname);
            //if (bundle == null || assetname == null || bundle.m_mapLoadedResAB.ContainsKey(assetname) == false)
            //{
            //    return def_obj;
            //}

            //return bundle.m_mapLoadedResAB[assetname];

            if (bundle == null || assetname == null || bundle.m_file_AB == null)
            {
                return def_obj;
            }

            return bundle.m_file_AB.LoadAsset<T>(assetname);
        }



        public Sprite LoadAsset_Sprite(string abname, string assetname)
        {
            return LoadAsset_FromAB<Sprite>(abname, assetname, U3DAPI.DEF_SPRITE);
        }

        public Sprite LoadSpriteAsset(string abname, string assetname)
        {
            return LoadAsset_FromAB<Sprite>(abname, assetname, U3DAPI.DEF_SPRITE);
        }

        //public void LoadAsset(string abname, string assetname, LuaFunction func) {
        //    abname = abname.ToLower();
        //    StartCoroutine(OnLoadAsset(abname, assetname, func));
        //}

        //IEnumerator OnLoadAsset(string abname, string assetName, LuaFunction func) {
        //    yield return new WaitForEndOfFrame();
        //    GameObject go = LoadAsset(abname, assetName);
        //    if (func != null) func.Call(go);
        //}

        public int m_nLoading_ab_Task = 0;

        protected static string _getStreamingFilePath(string filename)
        {
            string path = "";
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = "file:///" + Application.streamingAssetsPath + filename;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = "file://" + Application.dataPath + "/Raw/" + filename;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                path = "jar:file://" + Application.dataPath + "!/assets/" + filename;  //加入jar就是读取apk里面的数据

                //下面是读取指定目录：需要打开Unity --> PlayerSetting --> PublishSetting --> Split Application Binary
                //string obb_path = Application.dataPath.Substring(0, Application.temporaryCachePath.LastIndexOf("/"));
                //obb_path = obb_path.Substring(0, obb_path.Length - 1);
                //obb_path = obb_path.Substring(0, obb_path.LastIndexOf("/"));
                //path = "file://" + obb_path + "/OutAssets/" + filename;
            }

            return path;
        }

        public void LoadAsset_Async(string abname, string assetname)
        {
            abname = abname.ToLower();
            abname = getPatchAb(abname, assetname);

            if (m_mapResAB_bundles.ContainsKey(abname) == false)
            {
                m_mapResAB_bundles.Add(abname, new OneABFile(null));
            }

            //Debug.Log("LoadAsset_Async   " + abname);
            OneABFile one_abf = m_mapResAB_bundles[abname];
            Start_LoadAB(abname, one_abf);
        }

        private void Start_LoadAB(string abname, OneABFile one_abf)
        {
            if (one_abf.m_load_Step != ABF_LOAD.ABFL_NONE) return;
            one_abf.m_load_Step = ABF_LOAD.ABFL_LOADING;

            //debug.Log("async :: ab ==== ........................................................................ " + abname);

            //这里要支持多开 -- 协程，这样加载会快很多(因为file的loading和解析可以并行，但要保证好资源的不重复)
            ++m_nLoading_ab_Task;
            StartCoroutine(OnLoadAsset_Async(abname, one_abf));
        }

        IEnumerator OnLoadAsset_Async(string abname, OneABFile one_abf)
        {
            yield return new WaitForEndOfFrame();

            //获取所有的关联，然后加载
            //abname = abname.ToLower();
            //if (!abname.EndsWith(AppConst.ExtName))
            //{
            //    abname += AppConst.ExtName;
            //}
            //LoadDependencies(abname);

            //string[] dependencies = manifest.GetAllDependencies(name);
            //for (int i = 0; i < dependencies.Length; ++i)
            //    dependencies[i] = RemapVariantName(dependencies[i]);

            //string[] need_loading_files = new string[dependencies.Length + 1];
            //for (int i = 0; i < dependencies.Length; ++i)
            //{
            //    need_loading_files[i] = dependencies[i];
            //    //LoadAssetBundle(dependencies[i]);
            //}
            //need_loading_files[dependencies.Length] = abname;

            //for (int i = 0; i < need_loading_files.Length; ++i)
            {
                //debug.Log("need_loading_files i="+i+"   " + need_loading_files[i]);
                string localfile = Util.DataPath + abname;// need_loading_files[0];
                //string url_file = null;
                if (File.Exists(localfile))
                {
                    //Debug.Log("读取本地文件 " + localfile);
                }
                else
                {
                    //如果文件不存在就从主包内读取文件
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        localfile = Application.dataPath + "/Raw/" + abname;
                    }
                    else if (Application.platform == RuntimePlatform.Android)
                    {
                        localfile = Application.dataPath + "!assets/" + abname;
                    }
                    else
                    {
                        localfile = Application.dataPath + "/" + AppConst.AssetDirname + "/" + abname;
                    }

                    //Debug.Log("读取安装包内文件 " + localfile);
                }


                //本地调试时候的特殊处理
                if (!Application.isMobilePlatform && Main.instance != null && Main.instance.debugMode == Main.ENUM_DEBUG_STATE.update_resource_debug)
                {
                    if (localfile.Contains("ab_ui.assetbundle") /*|| localfile.Contains("ab_layer.assetbundle")*/)
                    {
                        //url_file = "file:///" + localfile.Replace(Util.DataPath, Application.dataPath + "/" + AppConst.AssetDirname + "/");
                        localfile = localfile.Replace(Util.DataPath, Application.dataPath + "/" + AppConst.AssetDirname + "/");
                    }
                }

                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(localfile);
                yield return abcr;

                if (abcr == null || abcr.isDone == false || abcr.assetBundle == null) //读取不到文件时: abcr.assetBundle是null
                {
                    //Debug.LogError("abcr fail: " + localfile);
                    one_abf.m_load_Step = ABF_LOAD.ABFL_FAILED;
                }
                else
                {
                    one_abf.m_file_AB = abcr.assetBundle;

                    //if (abcr.assetBundle.isStreamedSceneAssetBundle == false)
                    //{
                    //    //AssetBundleRequest abr = one_abf.m_file_AB.LoadAllAssetsAsync();
                    //    //yield return abr;

                    //    //Object[] loadones = abr.allAssets;
                    //    //for (int i = 0; i < loadones.Length; ++i)
                    //    //{
                    //    //    if (one_abf.m_mapLoadedResAB.ContainsKey(loadones[i].name) == false)
                    //    //    {
                    //    //        one_abf.m_mapLoadedResAB.Add(loadones[i].name, loadones[i]);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        Debug.LogError("包里有同名资源 " + abname + "  asset =" + loadones[i].name);
                    //    //    }
                    //    //}
                    //    //one_abf.m_file_AB.Unload(false);
                    //}


                    if (one_abf.m_strOneAssetName != null)
                    {
                        AssetBundleRequest abr = one_abf.m_file_AB.LoadAssetAsync(one_abf.m_strOneAssetName);
                        yield return abr;

                        //debug.Log("文件解析成功0000000 " + localfile);

                        one_abf.m_cacheObj = abr.asset;
                        one_abf.m_file_AB.Unload(false);
                        one_abf.m_file_AB = null;
                    }
                    //else
                    //{
                    //    if( one_abf.m_file_AB.isStreamedSceneAssetBundle == false )
                    //    {
                    //        AssetBundleRequest abr = one_abf.m_file_AB.LoadAllAssetsAsync();
                    //        yield return abr;
                    //    }
                    //}


                    one_abf.m_load_Step = ABF_LOAD.ABFL_LOADED;

                    //Debug.Log("文件加载成功 " + localfile);

                    b_load_finish = true;
                }
            }

            //UnityEngine.Debug.Log("文件加载成功 " + abname);
            --m_nLoading_ab_Task;
        }

        public void Unload_Asset(string abname)
        {
            abname = getPatchAb(abname, abname);
            //debug.Log("记录释放资源：" + abname);
            if (!m_mapResAB_bundles.ContainsKey(abname))
            {
                //Debug.LogError("释放不存在的资源：" + abname);
            }
            else
            {
                OneABFile one_abf = null;
                m_mapResAB_bundles.TryGetValue(abname, out one_abf);

                //如果loadscene完马上unload会出错，所以要避免进入同一地图时的释放
                if (m_UnloadOne != one_abf)
                {
                    if (m_UnloadOne != null)
                    {
                        if (m_UnloadOne.m_load_Step == ABF_LOAD.ABFL_LOADED)
                        {
                            //debug.Log("释放资源：" + m_UnloadOne.m_file_AB.name);
                            m_UnloadOne.m_file_AB.Unload(false); //最好是用true，但是现在全部清除rendertarget会出错
                            m_UnloadOne.m_load_Step = ABF_LOAD.ABFL_NONE;
                            m_UnloadOne.m_file_AB = null;
                        }
                    }

                    m_UnloadOne = one_abf;
                }
            }
        }

        OneABFile LoadAssetBundle(string abname)
        {
            OneABFile one_fab = null;
            if (!m_mapResAB_bundles.ContainsKey(abname))
            {
                Debug.LogError("找不到ab = " + abname);
            }
            else
            {
                m_mapResAB_bundles.TryGetValue(abname, out one_fab);
            }

            return one_fab;
        }

        //void LoadDependencies(string name)
        //{
        //    if (m_all_ab_manifest == null)
        //    {
        //        Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
        //        return;
        //    }
        //    // Get dependecies from the AssetBundleManifest object..
        //    string[] dependencies = m_all_ab_manifest.GetAllDependencies(name);
        //    if (dependencies.Length == 0) return;

        //    for (int i = 0; i < dependencies.Length; i++)
        //        dependencies[i] = RemapVariantName(dependencies[i]);

        //    // Record and load all dependencies.
        //    for (int i = 0; i < dependencies.Length; i++)
        //    {
        //        LoadAssetBundle(dependencies[i]);
        //    }
        //}

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        //string RemapVariantName(string assetBundleName)
        //{
        //    string[] bundlesWithVariant = m_all_ab_manifest.GetAllAssetBundlesWithVariant();

        //    // If the asset bundle doesn't have variant, simply return.
        //    if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
        //        return assetBundleName;

        //    string[] split = assetBundleName.Split('.');

        //    int bestFit = int.MaxValue;
        //    int bestFitIndex = -1;
        //    // Loop all the assetBundles with variant to find the best fit variant assetBundle.
        //    for (int i = 0; i < bundlesWithVariant.Length; i++)
        //    {
        //        string[] curSplit = bundlesWithVariant[i].Split('.');
        //        if (curSplit[0] != split[0])
        //            continue;

        //        int found = System.Array.IndexOf(m_Variants, curSplit[1]);
        //        if (found != -1 && found < bestFit)
        //        {
        //            bestFit = found;
        //            bestFitIndex = i;
        //        }
        //    }
        //    if (bestFitIndex != -1)
        //        return bundlesWithVariant[bestFitIndex];
        //    else
        //        return assetBundleName;
        //}

        void OnDestroy()
        {
            //if (m_all_ab_manifest != null) m_all_ab_manifest = null;
            //debug.Log("~ResourceManager was destroy!");
        }
    }

}

