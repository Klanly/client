using UnityEngine;
using System.Collections;
using SimpleFramework;
using SimpleFramework.Manager;

public class StartUpCommand : ControllerCommand {

    public override void Execute(IMessage message) {
        if (!Util.CheckEnvironment()) return;

        GameObject gameMgr = GameObject.Find("GlobalGenerator");
        if (gameMgr != null)
        {
            AppView appView = gameMgr.AddComponent<AppView>();
        }
        //-----------------关联命令-----------------------
        // AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

        //-----------------初始化管理器-----------------------
        AppFacade.Instance.AddManager(ManagerName.Lua, new LuaScriptMgr());
        AppFacade.Instance.AddManager<InterfaceLuaMgr>(ManagerName.Interfacemgr);
        AppFacade.Instance.AddManager<PanelManager>(ManagerName.Panel);
        AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
        AppFacade.Instance.AddManager<NetworkManager>(ManagerName.Network);
        AppFacade.Instance.AddManager<ResourceManager>(ManagerName.Resource);
        AppFacade.Instance.AddManager<ThreadManager>(ManagerName.Thread);
        AppFacade.Instance.AddManager<GameManager>(ManagerName.Game);

        //-----------------初始化更新界面-----------------------
        GameObject obj_update = GameObject.Find("canvas_main/updateui");
        obj_update.SetActive(true);
        UpdateScrollbar updateUi = obj_update.AddComponent<UpdateScrollbar>();
        GameManager mgr = AppFacade.Instance.GetManager<GameManager>(ManagerName.Game);
        updateUi.init(mgr);

        mgr.Init();

        Debug.Log("GameMUFramework StartUp-------->>>>>");
    }
}