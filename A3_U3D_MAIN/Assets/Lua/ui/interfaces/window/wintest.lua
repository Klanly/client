require "proxy/TestProxy"


wintest = BaseLayer:new();
local this = wintest;

wintest.init = function()
    this.alain();
    this.imgidx = 0;
    this.scrollControler = this.getScrollControler("item_scroll/scroll_view");
    this.itemListView = this.getTransform("item_scroll/scroll_view/contain");
    this.item_Parent = this.itemListView:GetComponent("GridLayoutGroup");
    this.txt = this.getComponentByPath("txt", "Text");


    addBehaviour(this.itemListView.gameObject, "Lui.LRichText");
    addBehaviour(this.transform:FindChild("close").gameObject, "Lui.LRichText");

    this.domoveX(this.transform:FindChild("close"), 90, 3,this.onOver);

    -- 增加按钮监听事件
    this.addClick(this.transform:FindChild("close").gameObject, this.onCloseClick);
    this.addClick(this.transform:FindChild("proxy").gameObject, this.onproxyClick);
    this.addClick(this.transform:FindChild("pic").gameObject, this.onAddPicClick);
    this.addClick(this.transform:FindChild("xml").gameObject, this.onXmlClick);
    this.addClick(this.transform:FindChild("cont").gameObject, this.oncontClick);
    this.addClick(this.transform:FindChild("open").gameObject, this.onOpenByCClick);

    this.bt =  this.getComponentByPath("open", "Button");
 this.bt.interactable=false;

  local  eve=  this.getEventTriggerListener( this.itemListView.gameObject);
  eve.onDown = this.ondown;

    -- 初始化协议
    TestProxy:getInstacne();

    -- 增加协议监听事件
    -- NetManager:addProxyListener(PKG_NAME.S2C_TEST, this.proxyHandle);
end;

wintest.onShowed = function(prama)
    -- logWarn( prama[1]);
    wintest.instance = this;
end;

wintest.ondown=function (go)
--logWarn("ondown!!!!!!!!!!!!!!");
end

wintest.onOver =function()
--log("dddddddddddddddddddddddddddd")
end

wintest.proxyHandle = function(v)
    this.txt.text = "收到协议：" .. v:dump();
end;

wintest.onClosed = function()
    wintest.instance = nil;
end;

wintest.onCloseClick = function(go)
InterfaceMgr.changeState(InterfaceMgr.STATE_HIDE_ALL);

   -- InterfaceMgr.close(InterfaceMgr.WIN_TEST);
end;

wintest.onproxyClick = function(go)

    TestProxy:getInstacne():sendTest();

    --    v = variant();
    --    v:setValue("op", 1);
    --    NetManager:sendRPC(PKG_NAME.C2S_TEST, v);
end;

wintest.onAddPicClick = function(go)

    local go = this.loadGo("icon");


    local img = this.loadSprite("101");
    go:GetComponent("Image").sprite = img;

    local con = this.itemListView:GetChild(this.imgidx);
    this.imgidx = this.imgidx + 1;
    if (con ~= nil) then
        go.transform:SetParent(con, false);
    end;
end;

wintest.onXmlClick = function(args)
    local xml = XmlMgr:GetSXML("item.item", "id==1501");
    this.txt.text = "查到xml:" .. xml:getString("item_name");
end

wintest.oncontClick = function(args)

    this.txt.text = getCont("comm_nolvmap", { "aaa", "bbb", "ccc" });

end

wintest.onOpenByCClick = function(args)
    openUIByC("a3_hudun");
    flytxt.fly("aaaaaaaaaaaa");
end;