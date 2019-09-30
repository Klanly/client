using System;
using System.Collections.Generic;

public static class LuaBinder
{
	public static List<string> wrapList = new List<string>();
	public static void Bind(IntPtr L, string type = null)
	{
		if (type == null || wrapList.Contains(type)) return;
		wrapList.Add(type); type += "Wrap";
		switch (type) {
			case "ComponentWrap": ComponentWrap.Register(L); break;
			case "GameObjectWrap": GameObjectWrap.Register(L); break;
			case "ObjectWrap": ObjectWrap.Register(L); break;
			case "RectTransformWrap": RectTransformWrap.Register(L); break;
			case "SimpleFramework_LuaHelperWrap": SimpleFramework_LuaHelperWrap.Register(L); break;
			case "SimpleFramework_Manager_PanelManagerWrap": SimpleFramework_Manager_PanelManagerWrap.Register(L); break;
			case "SimpleFramework_UtilWrap": SimpleFramework_UtilWrap.Register(L); break;
			case "System_ObjectWrap": System_ObjectWrap.Register(L); break;
			case "TimeWrap": TimeWrap.Register(L); break;
			case "TransformWrap": TransformWrap.Register(L); break;
		}
	}
}
