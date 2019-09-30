require "proxy/BagProxy"
require "model/a3_BagModel"
require "model/PlayerModel"
require "ui/interfaces/loadingui//IconImageMgr"
a3_bag = BaseLayer:new();
local this = a3_bag;
local itemListView;
local item_Parent;
local m_SelfObj;--角色的avatar
local scene_Obj; -- 场景
local scene_Camera; -- 摄像机
local cur_num = 0;

a3_bag.init = function()
    this.alain();
    this.imgidx = 0;
	itemListView = this.transform:FindChild("item_scroll/scroll_view/contain").gameObject;
	item_Parent = itemListView:GetComponent("GridLayoutGroup");

	this.addClick(this.transform:FindChild("btn_close").gameObject, this.onCloseClick);
	this.addClick(this.transform:FindChild("info/info").gameObject,this.onInfo);
	this.addClick(this.transform:FindChild("item_scroll/equip").gameObject,this.onEquipSell);
	this.addClick(this.transform:FindChild("piliang_fenjie/close").gameObject,this.onfenjieclose);
	this.addClick(this.transform:FindChild("item_scroll/bag").gameObject,this.onCangku);
	this.addClick(this.transform:FindChild("ig_bg1/pet").gameObject,this.OnOpenPet);
	this.addClick(this.transform:FindChild("piliang_fenjie/info_bg/go").gameObject,this.DoFenjie);

	--for i =0,itemListView.transform.childCount-1 do
		--if(i <= a3_BagModel.getInstance().curi)then
			--local lockig = itemListView.transform:GetChild(i):FindChild("lock").gameObject;
			--lockig:SetActive(true);
			--local tag = i+1;
			--this.addClick(lockig, this.onCloseClick);
		--end
	--end

	-- 初始化协议s
	--BagProxy:getInstance();
	--BagProxy:getInstance():sendLoadItems(0);
	--BagProxy:getInstance();
end;

--a3_bag.aa = function (data)
	--log("收到协议：" .. data:dump());
--end
a3_bag.onShowed = function(prama)
    a3_bag.instance=this;
	a3_bag.refreshMoney();
	a3_bag.refreshGold();
	a3_bag.refreshGift();
	this.initEquipIcon();
	this.onLoadItem();
	this.createAvatar_body();
	this.onOpenLockRec();
	this.createScene();
	this.transform:FindChild("ig_bg_bg").gameObject:SetActive(false);
	InterfaceMgr.changeState(InterfaceMgr.STATE_FUNCTIONBAR);
    --BagProxy:getInstance():sendLoadItems(1);
end;

a3_bag.onClosed = function()
	a3_bag.instance=nil;
	this.disposeAvatar();
	InterfaceMgr.changeState(InterfaceMgr.STATE_NORMAL); 
end;

a3_bag.onCloseClick = function(go)
    InterfaceMgr.close(InterfaceMgr.A3_BAG);
end;

a3_bag.onInfo = function(go)
    --跳转详细信息
	PlayerModel:getInstance().aa();
end;


a3_bag.onEquipSell = function(go)
    this.transform:FindChild("piliang_fenjie").gameObject:SetActive(true);
end;

a3_bag.onfenjieclose = function(go)
    this.transform:FindChild("piliang_fenjie").gameObject:SetActive(false);
end;

a3_bag.onCangku = function(go)
    --跳转仓库
	openUIByC("a3_warehouse");
    InterfaceMgr.close(InterfaceMgr.A3_BAG);
	--关闭背包
end;

a3_bag.refreshMoney = function()
	local text= this.transform:FindChild("item_scroll/money"):GetComponent("Text");
	text.text = PlayerModel:getInstance().money;
end

a3_bag.refreshGold = function()
	local text= this.transform:FindChild("item_scroll/stone"):GetComponent("Text");
	text.text = PlayerModel:getInstance().gold;
end

a3_bag.refreshGift = function()
	local text= this.transform:FindChild("item_scroll/bindstone"):GetComponent("Text");
	text.text = PlayerModel:getInstance().gift;
end
a3_bag.onOpenLockRec = function() -- 包裹格子
	for i =50,itemListView.transform.childCount-1 do
		local lockig = itemListView.transform:GetChild(i):FindChild("lock").gameObject;
		if(i >= a3_BagModel:getInstance().curi)then
			lockig:SetActive(true);
		else
			lockig:SetActive(false);
		end
	end
end

a3_bag.OnOpenPet = function(go)
	this.hh();
    --跳转宠物
	--关闭背包
end;

a3_bag.createAvatar_body = function()
	--if(true)then
		--m_SelfObj =  this.loadGo("mage_avatar");
		
	--end

	
end

a3_bag.createScene = function()
	local obj_prefab;
	obj_prefab = this.loadGo("show_scene");
	scene_Obj = GameObject.Instantiate(obj_prefab, Vector3(-77.38, -0.49, 15.1), Quaternion.New(0, 180, 0, 0));
	GameObject.Destroy(obj_prefab);
	obj_prefab = this.loadGo("scene_ui_camera");
	scene_Camera = GameObject.Instantiate(obj_prefab);
	GameObject.Destroy(obj_prefab);
	--for i =0,scene_Obj.transform.transform.childCount-1 do
		--if(scene_Obj.transform:GetChild(i).gameObject.name == "scene_ta")then
			--scene_Obj.transform:GetChild(i)gameObject.layer = LayerMask.NameToLayer("selfrole");
		--else
			--scene_Obj.transform:GetChild(i)gameObject.layer = LayerMask.NameToLayer("fx");
		--end
	--end
end

a3_bag.disposeAvatar = function()
	if (m_SelfObj ~= null) then
	GameObject.Destroy(m_SelfObj);
	end
    if (scene_Obj ~= null)then
	 GameObject.Destroy(scene_Obj);
	 end
    if (scene_Camera ~= null) then
	GameObject.Destroy(scene_Camera);
	end
end


--a3_bag.onOpenLock = function(go)
	--this.transform:FindChild("panel_open").gameObject:SetActive(false);
	--if(open_choose_tag == 1)then
		--BagProxy:getInstance():sendOpenLock(2,cur_num, true);
	--else
		--BagProxy:getInstance():sendOpenLock(2,cur_num, false);
		--end
--end

local i = 0;
a3_bag .hh =function()
	local iconPrefab = this.loadGo("iconimage");
	local icon = iconPrefab.transform:FindChild("icon"):GetComponent("Image");
	icon.sprite = this.loadSprite("101");
	--local iconborder = iconPrefab.transform:FindChild("iconborder"):GetComponent("Image");
	--iconborder.sprite = this.loadSprite("");

	local con = this.transform:FindChild("item_scroll/scroll_view/contain"):GetChild(i);
	   if (con ~= nil) then
        icon.transform:SetParent(con, false);
		i=i+1;
    end;
end
a3_bag.DoFenjie = function(go)
	this.transform:FindChild("piliang_fenjie").gameObject:SetActive(false);
end;



a3_bag.initEquipIcon = function()
	for i= 1,10 do
		local go = this.transform:FindChild("ig_bg1/txt"..i);
		if(go.transform.childCount> 0) then
			GameObject.Destroy(go.transform:GetChild(0).gameObject);
		end
	end
	--equips = {};

end

a3_bag.refreshScrollRect =function()
	local num = itemListView.transform.childCount;
	if(num<=0)then
		return;
	end
	local height = item_Parent.cellSize.y;
	local row Math.Ceiling(num /5);
	local rect = itemListView:GetComponent("RectTransform");
	local vec = Vector2:new(0, row * height);
	rect.sizeDelta = vec;
end
a3_bag.onLoadItem = function()
	local items = a3_BagModel:getInstance():getItems();
	local i = 0;
	for i,v in pairs(items)do
		this.CreateItemIcon(v,i);
	end
	--local len = a3_BagModel:getInstance().getItems().Count;
	--or j=0,len-1 do
		--i= i+1;
		--this.CreateItemIcon(items.Values[j],i);
	--end
	--this.refreshScrollRect();
end

a3_bag.CreateItemIcon = function(data,i)
	local icon = IconImageMgr.createA3ItemIcon2(data,true,data.num);

end
