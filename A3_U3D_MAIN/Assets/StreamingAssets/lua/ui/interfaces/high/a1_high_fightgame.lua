-- 鑳″瓩浠� QQ:75567044
require "model/PlayerModel"
require "model/AutoPlayModel"
require "model/VipModel"
require "model/MapModel"
require "proxy/GeneralProxy"

a1_high_fightgame = BaseLayer:new();
local this = a1_high_fightgame;
local currMaxExp = 0; 

a1_high_fightgame.init = function()
    this.alain();

    this.hp_bar = this.getComponentByPath("herohead/info/red", "Image");
    this.mp_bar = this.getComponentByPath("herohead/info/blue", "Image");
    this.txt_lv = this.getComponentByPath("herohead/info/lv", "Text");
    this.txt_zhuan = this.getComponentByPath("herohead/info/zhuan", "Text");
    this.txt_combpt = this.getComponentByPath("herohead/info/text_fighting", "Text");
    this.hpSlider = this.getComponentByPath("herohead/info/red/Slider", "Slider");
    this.mpSlider = this.getComponentByPath("herohead/info/blue/Slider", "Slider");
    this.sliderOnValueChanged(this.hpSlider, this.onHpLowerSliderChange);
    this.sliderOnValueChanged(this.mpSlider, this.onMpLowerSliderChange);
    this.val_hp = this.getComponentByPath("herohead/info/red/val_hp", "Text");
    this.val_mp = this.getComponentByPath("herohead/info/blue/val_mp", "Text");
    this.oldfingting = 999999999;


    this.goPk_ani = this.transform:FindChild("heroicon_head/ani").gameObject
    this.txtPk_ani = this.getComponentByPath("heroicon_head/ani/Text", "Text")


    this.txtMapName = this.transform:FindChild("a3_litemap/normal/btnMap/Text"):GetComponent("Text");
    this.txtline = this.transform:FindChild("a3_litemap/normal/btnMap/line"):GetComponent("Text");
    this.txtline.text = PlayerModel:getInstance().line + 1;
    this.txtPos = this.transform:FindChild("a3_litemap/normal/btnMap/txtPos"):GetComponent("Text");
    this.refreshMapname();


    this.exp_barImage = this.getComponentByPath("expbm_bar/exp_bar", "Image")
    this.exp_barImage.raycastTarget = true;
    this.exp_barText = this.getComponentByPath("expbm_bar/exp_bar/Text", "Text")
    this.transform:FindChild("expbm_bar/exp_bar/Text").gameObject:SetActive(false)

    this.getEventTriggerListener(this.exp_barImage.gameObject).onDown = this.onexpDown
    this.getEventTriggerListener(this.exp_barImage.gameObject).onUp = this.onexpUp
end

a1_high_fightgame.onShowed = function(prama)
    a1_high_fightgame.herohead_inst = this;
    this.refreshLv();
    this.refreshZhuan();
    this.refreshHp();
    this.refreshCombpt();
    this.onHpLowerChange();
    this.onMpLowerChange();

    a1_high_fightgame.heroicon_head_high_inst = this;

    a1_high_fightgame.a3_litemap_inst = this;

    a1_high_fightgame.instance = this;
    this.refreshExp();
end;

a1_high_fightgame.onHpLowerChange = function()
    log(AutoPlayModel:getInstance().nHpLower);
    this.hpSlider.value = AutoPlayModel:getInstance().nHpLower;
end;
a1_high_fightgame.onMpLowerChange = function()
    this.mpSlider.value = AutoPlayModel:getInstance().nMpLower;
end;
a1_high_fightgame.onHpLowerSliderChange = function(v)
    log("aaaaaaaaaaa");
end;
a1_high_fightgame.onMpLowerSliderChange = function(v)
    log("bbbbbbbbbb");
end;

a1_high_fightgame.refreshLv = function()
    this.txt_lv.text = PlayerModel:getInstance().lvl .. getCont("ji",{""});
end;

a1_high_fightgame.refreshZhuan = function()
    this.txt_zhuan.text = PlayerModel:getInstance().zhuan ..  getCont("zhuan",{""});
end;

a1_high_fightgame.refreshCombpt = function()
    this.txt_combpt.text = PlayerModel:getInstance().combpt;
    if PlayerModel:getInstance().combpt > this.oldfingting then
        if fightingup.instance ~= nil then
            fightingup.instance.runTxt(this.oldfingting, PlayerModel:getInstance().combpt);
        end
    end
    this.oldfingting = PlayerModel:getInstance().combpt;
end;

a1_high_fightgame.refreshHp = function()
    this.hp_bar.fillAmount = PlayerModel:getInstance().hp / PlayerModel:getInstance().max_hp;
    this.setVaule();
end;


a1_high_fightgame.showbtnIcon = function(v)
if (this.instance == nil) then
        return;
    end
	local open = v[0];
	this.transform:FindChild("a3_litemap/normal/btnMap").gameObject:SetActive(open);
end

a1_high_fightgame.refreshMp = function()
    this.mp_bar.fillAmount = PlayerModel:getInstance().mp / PlayerModel:getInstance().max_mp;
    this.setVaule();
end;

a1_high_fightgame.setVaule = function()
    -- if(PlayerModel:getInstance().hp < PlayerModel:getInstance().max_hp or PlayerModel:getInstance().mp < PlayerModel:getInstance().max_mp)then
    -- this.transform:FindChild("info/red/val_hp").gameObject:SetActive(true);
    -- this.transform:FindChild("info/blue/val_mp").gameObject:SetActive(true);
    this.val_hp.text = PlayerModel:getInstance().hp .. "/" .. PlayerModel:getInstance().max_hp;
    this.val_mp.text = PlayerModel:getInstance().mp .. "/" .. PlayerModel:getInstance().max_mp;
    -- else
    -- this.transform:FindChild("info/red/val_hp").gameObject:SetActive(false);
    -- this.transform:FindChild("info/blue/val_mp").gameObject:SetActive(false);
    -- end
end

a1_high_fightgame.refreshPkImages = function(isShowAni)
    isShowAni = isShowAni or false;
    this.goPk_ani:SetActive(false)
    if PlayerModel:getInstance().zhuan >= 1 then
        if isShowAni == true then
            local str_pkmode =getCont("Weizhi",{""})
 ;
            local pk_state = PlayerModel:getInstance().now_pkState;
            if pk_state == 0 then str_pkmode = getCont("peace",{""}) end
            if pk_state == 1 then str_pkmode = getCont("allman",{""}) end
            if pk_state == 2 then str_pkmode = getCont("teamboy",{""}) end

            this.txtPk_ani.text =getCont("ThreeModel",{str_pkmode})  
            this.goPk_ani:SetActive(true)
            local timer = Timer.New(this.close_heroih_ani, 2)
            timer:Start()
        end
    end
end

a1_high_fightgame.close_heroih_ani = function()
    this.goPk_ani:SetActive(false)
end

a1_high_fightgame.refreshMapname = function()
    if this.a3_litemap_inst == nil then
        return;
    end

    local data = MapModel:getInstance().curSvrConf;
    if (data == nil) then
        return
    end

    if (data:ContainsKey("pk")) then
        if (data:getValue("pk")._int == 0) then
            if (data:ContainsKey("map_name")) then
                this.txtMapName.text = "<color=#66FF02FF>" .. data:getValue("map_name")._str .. "</color>";
            else
                this.txtMapName.text = "<color=#66FF02FF>" .. "--" .. "</color>";
            end
        elseif (data:getValue("pk")._int == 1) then
            if (data:ContainsKey("map_name")) then
                this.txtMapName.text = "<color=#FFFF02FF>" .. data:getValue("map_name")._str .. "</color>";
            else
                this.txtMapName.text = "<color=#FFFF02FF>" .. "--" .. "</color>";
            end

        elseif (data:getValue("pk")._int == 2) then
            if (data:ContainsKey("map_name")) then
                this.txtMapName.text = "<color=#F70C0CFF>" .. data:getValue("map_name")._str .. "</color>";
            else
                this.txtMapName.text = "<color=#F70C0CFF>" .. "--" .. "</color>";
            end
        end
    else
        if (data:ContainsKey("map_name")) then
            this.txtMapName.text = "<color=#66FF02FF>" .. data:getValue("map_name")._str .. "</color>";
        else
            this.txtMapName.text = "<color=#66FF02FF>" .. "--" .. "</color>";
        end
    end

end

a1_high_fightgame.change__Line = function(v)
    if (v == nil) then
        return;
    end
    local txt = v[0];
    this.txtline.text = txt;
end

a1_high_fightgame.setTextMapPos = function(v)
    if (v == nil) then
        return;
    end
    local txt = v[0];
    this.txtPos.text = txt;
end


a1_high_fightgame.onexpDown = function(go)
    this.transform:FindChild("expbm_bar/exp_bar/Text").gameObject:SetActive(true)
end

a1_high_fightgame.onexpUp = function(go)
    this.transform:FindChild("expbm_bar/exp_bar/Text").gameObject:SetActive(false)
end

a1_high_fightgame.refreshExp = function()

    local sxml = XmlMgr:GetSXML("carrlvl.carr", "carr==" .. tostring(PlayerModel:getInstance().profession))
    local s_exp = sxml:GetNode("zhuanshen", "zhuan=="..PlayerModel:getInstance().zhuan)
    local xml = s_exp:GetNode("carr", "lvl=="..PlayerModel:getInstance().lvl)
    local cost_exp = xml:getInt("exp")
    
    local nextXml = s_exp:GetNode("carr", "lvl=="..PlayerModel:getInstance().lvl+1)
    
    if nextXml == nil  and currMaxExp == 0 then
      
      local nexts_exp = sxml:GetNode("zhuanshen", "zhuan==".. PlayerModel:getInstance().zhuan + 1 )  -- 取下一转职等级配置
     
      if nexts_exp ~= nil then 
     
      local maxLvl = s_exp:getInt("exp_pool_level");  -- 根据当前转职等级配置的字段判断
     
      for i = 1 , maxLvl - 1 , 1 do 
      
      local currLvl = nexts_exp:GetNode("carr", "lvl==".. i);
      
      currMaxExp = currMaxExp + currLvl:getInt("exp");
      
      end
         
      cost_exp=0;
      
     end
     
    elseif nextXml ~= nil then
    
      currMaxExp = 0;
      
    end   
    
    if currMaxExp ~= 0 then
    	
    	cost_exp = 0;
    	
    end    
    
     -- zmh
    
    this.exp_barImage.fillAmount = tonumber(PlayerModel:getInstance().exp) / (tonumber(cost_exp) + tonumber(currMaxExp))
    
    this.exp_barText.text =  PlayerModel:getInstance().exp.."/"..(cost_exp+currMaxExp)
    
end


 



