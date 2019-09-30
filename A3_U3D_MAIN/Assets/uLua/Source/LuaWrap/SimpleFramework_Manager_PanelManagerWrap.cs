using System;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;
using Object = UnityEngine.Object;

public class SimpleFramework_Manager_PanelManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ui_unshow", ui_unshow),
			new LuaMethod("open", open),
			new LuaMethod("CreateUI_Layer", CreateUI_Layer),
			new LuaMethod("newGameobject", newGameobject),
			new LuaMethod("onSound", onSound),
			new LuaMethod("newImage", newImage),
			new LuaMethod("GAME_CAMERA", GAME_CAMERA),
			new LuaMethod("newScrollControler", newScrollControler),
			new LuaMethod("newcellSize", newcellSize),
			new LuaMethod("new_Split", new_Split),
			new LuaMethod("newTabControler", newTabControler),
			new LuaMethod("resLoad", resLoad),
			new LuaMethod("resPicLoad", resPicLoad),
			new LuaMethod("xmlMgr", xmlMgr),
			new LuaMethod("getKeyWord", getKeyWord),
			new LuaMethod("domoveX", domoveX),
			new LuaMethod("domoveY", domoveY),
			new LuaMethod("doScaleX", doScaleX),
			new LuaMethod("doScaleY", doScaleY),
			new LuaMethod("doScale", doScale),
			new LuaMethod("doRotate", doRotate),
			new LuaMethod("killTween", killTween),
			new LuaMethod("getCont", getCont),
			new LuaMethod("getError", getError),
			new LuaMethod("openByC", openByC),
			new LuaMethod("closeByC", closeByC),
			new LuaMethod("changeStateByC", changeStateByC),
			new LuaMethod("addBehaviour", addBehaviour),
			new LuaMethod("getEventTrigger", getEventTrigger),
			new LuaMethod("sliderOnValueChanged", sliderOnValueChanged),
			new LuaMethod("doByC", doByC),
			new LuaMethod("tween", tween),
			new LuaMethod("setGameJoy", setGameJoy),
			new LuaMethod("setGameSkill", setGameSkill),
			new LuaMethod("functionOpenMgr", functionOpenMgr),
			new LuaMethod("flytxts", flytxts),
			new LuaMethod("canlines", canlines),
			new LuaMethod("New", _CreateSimpleFramework_Manager_PanelManager),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SimpleFramework.Manager.PanelManager", typeof(SimpleFramework.Manager.PanelManager), regs, fields, typeof(View));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSimpleFramework_Manager_PanelManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "SimpleFramework.Manager.PanelManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(SimpleFramework.Manager.PanelManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ui_unshow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		obj.ui_unshow();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int open(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.open(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateUI_Layer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
		obj.CreateUI_Layer(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int newGameobject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = obj.newGameobject(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onSound(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.onSound(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int newImage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		Image o = obj.newImage(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GAME_CAMERA(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.GAME_CAMERA(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int newScrollControler(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		GameFramework.ScrollControler o = obj.newScrollControler(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int newcellSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		obj.newcellSize(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int new_Split(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string[] o = obj.new_Split(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int newTabControler(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 3, typeof(Transform));
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
		GameFramework.TabControl o = obj.newTabControler(arg0,arg1,arg2);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int resLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = obj.resLoad(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int resPicLoad(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Sprite o = obj.resPicLoad(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int xmlMgr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		MuGame.XMLMgr o = obj.xmlMgr();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getKeyWord(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		MuGame.KeyWord o = obj.getKeyWord();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int domoveX(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		DG.Tweening.Tween o = obj.domoveX(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int domoveY(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		DG.Tweening.Tween o = obj.domoveY(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int doScaleX(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		DG.Tweening.Tween o = obj.doScaleX(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int doScaleY(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		DG.Tweening.Tween o = obj.doScaleY(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int doScale(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		DG.Tweening.Tween o = obj.doScale(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int doRotate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		DG.Tweening.Tween o = obj.doRotate(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int killTween(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		obj.killTween(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getCont(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 3);
		string o = obj.getCont(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getError(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string o = obj.getError(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int openByC(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.openByC(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int closeByC(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.closeByC(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int changeStateByC(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.changeStateByC(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int addBehaviour(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		GameObject o = obj.addBehaviour(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getEventTrigger(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		SimpleFramework.EventTriggerListenerLua o = obj.getEventTrigger(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int sliderOnValueChanged(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Slider arg0 = (Slider)LuaScriptMgr.GetUnityObject(L, 2, typeof(Slider));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		obj.sliderOnValueChanged(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int doByC(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
		obj.doByC(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int tween(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		LuaTable arg2 = LuaScriptMgr.GetLuaTable(L, 4);
		obj.tween(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setGameJoy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.setGameJoy(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setGameSkill(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.setGameSkill(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int functionOpenMgr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		MuGame.FunctionOpenMgr o = obj.functionOpenMgr();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int flytxts(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		MuGame.flytxt o = obj.flytxts();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int canlines(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		SimpleFramework.Manager.PanelManager obj = (SimpleFramework.Manager.PanelManager)LuaScriptMgr.GetUnityObjectSelf(L, 1, "SimpleFramework.Manager.PanelManager");
		bool o = obj.canlines();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

