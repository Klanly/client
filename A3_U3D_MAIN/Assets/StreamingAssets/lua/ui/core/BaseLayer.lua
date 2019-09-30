BaseLayer = Base:new( { });
local this = BaseLayer;
local super;
local SCREEN_WIDTH = 0;
local SCREEN_HEIGHT;
local cemaraRectTran;
function BaseLayer:new(o)
    o = o or { }
    setmetatable(o, self)
    self.__index = self
    super = self;

    if SCREEN_WIDTH == 0 then
        local aaa = find("canvas_main");

        cemaraRectTran = find("Canvas_overlay"):GetComponent("RectTransform");

        local overlay = find("canvas_main"):GetComponent("RectTransform");
        SCREEN_WIDTH = overlay.rect.width;
        SCREEN_HEIGHT = overlay.rect.height;
        -- destroy(overlay.gameObject);
        -- logWarn("BaseLayer.alain:"..SCREEN_WIDTH.." "..SCREEN_HEIGHT.." "..Screen.width.." "..Screen.height);
    end;

    o.Awake = function(obj)
        self.gameObject = obj;
        self.transform = obj.transform;
        self.luaBehaviour = obj:GetComponent('LuaUI');

        o.init();
      --  InterfaceMgr.m_pool[obj.name].ctrl = o;
    end

    return o
end;

BaseLayer.Start = function()

end;

BaseLayer.init = function()
    --logWarn("-----------------------------BaseLayer.init--------")
end;

BaseLayer.onShowed = function(prama)
    --logWarn("BaseLayer::::onShowed:::");
end;

BaseLayer.onClosed = function()
end;

BaseLayer.addClick = function(go, handle)
    this.luaBehaviour:AddClick(go, handle);
end;

BaseLayer.getGameObject = function(path)
    return this.transform:FindChild(path).gameObject;
end;

BaseLayer.getTransform = function(path)
    return this.transform:FindChild(path);
end;

BaseLayer.getScrollControler = function(path)
    return PanelManager:newScrollControler(this.transform:FindChild(path));
end;

BaseLayer.getTabControler = function(tabpath,mainpath,onswitch)
    return PanelManager:newTabControler(this.transform:FindChild(tabpath), this.transform:FindChild(mainpath), onswitch);
end;

BaseLayer.loadGo = function(name)
    return PanelManager:resLoad(name);
end;

BaseLayer.loadSprite = function(name)
    return PanelManager:resPicLoad(name);
end;


BaseLayer.getComponentByPath = function(path, component)
    return this.transform:FindChild(path):GetComponent(component);
end;

BaseLayer.getComponent = function(componentName)
    return this.transform:GetComponent(componentName);
end;

BaseLayer.alain = function()

    this.gameObject:GetComponent("RectTransform").sizeDelta = Vector2.New(SCREEN_WIDTH, SCREEN_HEIGHT);
end;




BaseLayer.update = function(func)
    this.__update = FrameTimer.New(func, 1, -1);
    this.__update:Start()
end;

BaseLayer.stopUpdate = function()
    if (this.__update == nil) then return end;
    this.__update:stop();
end;


BaseLayer.addBg = function()

    if (BaseLayer.showBG == false) then
        return
    end;
    local goBg = PanelManager:newGameobject("ig_bg_bg");
    local bg = PanelManager:newImage(goBg);
    local cv = cemaraRectTran;
    local vec = Vector2.New(cv.rect.width * 2, cv.rect.height * 2);


    local rect = goBg:GetComponent("RectTransform");


    rect.sizeDelta = vec;

    bg.color = Color.New(0, 0, 0, 0.5);
    goBg.transform:SetParent(this.transform, false);
    goBg.transform:SetSiblingIndex(0);
end;

BaseLayer.domoveX = function(trans, value, duration, completehadle)
    return PanelManager:domoveX(trans, value, duration, completehadle);
end;

BaseLayer.domoveY = function(trans, value, duration, completehadle)
    return PanelManager:domoveY(trans, value, delta, completehadle);

end;
BaseLayer.soundplay=function(path)
    return PanelManager:onSound(path);
end;
BaseLayer.GAME_CAMERA=function(tf)
    return PanelManager:GAME_CAMERA(tf);
end;
BaseLayer.ui_unshow=function()
    return PanelManager:ui_unshow();
end;
BaseLayer.Grild_cellSize = function(path,x,y)

    return PanelManager:newcellSize(this.transform:FindChild(path),x,y);
-- this.gameObject:GetComponent("GridLayoutGroup").cellSize = Vector2.New(SCREEN_WIDTH, SCREEN_HEIGHT);
end;
BaseLayer.Split=function(str)
    
    return PanelManager:new_Split(str);


end;

BaseLayer.doScaleX = function(trans, value, duration, completehadle)
    return PanelManager:doScaleX(trans, value, delta, completehadle);

end;

BaseLayer.doScaleY = function(trans, value, duration, completehadle)
    return PanelManager:doScaleY(trans, value, delta, completehadle);

end;

BaseLayer.doScale = function(trans, vec, duration, completehadle)
    return PanelManager:doScale(trans, vec, delta, completehadle);
end;

BaseLayer.doRotate = function(trans, vec, duration, completehadle)
    return PanelManager:doRotate(trans, vec, delta, completehadle);
end;

BaseLayer.doKillTween = function(trans)
    PanelManager:killTween(trans);
end;

BaseLayer.doScale = function(trans, vec, duration, completehadle)
    return PanelManager:doScale(trans, vec, delta, completehadle);
end;

BaseLayer.getEventTriggerListener = function(gameobj)
    return PanelManager:getEventTrigger(gameobj);
end;

BaseLayer.sliderOnValueChanged = function(slider, handle)
    PanelManager:sliderOnValueChanged(slider, handle);
end;

BaseLayer.setGameJoy = function(b)
	return PanelManager:setGameJoy(b);
end

BaseLayer.setGameSkill = function(b)
	return  PanelManager:setGameSkill(b);
end
--获得一个物品的表对象
BaseLayer.getItemXml = function(itemid)
	local item={};
	local xmlitm = XmlMgr:GetSXML("item.item", "id=="..itemid);
	item.id = xmlitm:getInt("id");
	item.name = xmlitm:getString("item_name");
	item.icon_file = xmlitm:getString("icon_file");
	item.desc = xmlitm:getString("desc");
	item.quality = xmlitm:getInt("quality");
	item.value = xmlitm:getInt("value");
	item.item_type = xmlitm:getInt("item_type");
	item.maxnum = xmlitm:getInt("maxnum");
	item.use_type = xmlitm:getInt("use_type");
	item.main_effect = xmlitm:getInt("main_effect");
	item.use_lv = xmlitm:getInt("use_lv");
	item.use_limit = xmlitm:getInt("use_limit");
	return item;
end;

--创建图标：图标名，外框名，数量
BaseLayer.newIcon = function(iconName,borderName,num)
	local icon = this.loadGo("iconimage");
	local img = this.loadSprite(iconName);
	icon.transform:FindChild("icon"):GetComponent("Image").sprite = img;
	local border = this.loadSprite("iconborder_"..borderName);
	icon.transform:FindChild("iconborder"):GetComponent("Image").sprite = border;
	if num<=0 then
		icon.transform:FindChild("num").gameObject:SetActive(false);
	else icon.transform:FindChild("num"):GetComponent("Text").text = tostring(num);
	end
	icon.name = tostring(iconName);
    return icon;
end;

--清除物体下所有子物体
BaseLayer.clearChild = function(transform)
	if transform.transform.childCount>0 then
		for i=0, transform.transform.childCount-1, 1 do
			local go = transform.transform:GetChild(i).gameObject;
			destroy(go);
		end
	end;
end;
