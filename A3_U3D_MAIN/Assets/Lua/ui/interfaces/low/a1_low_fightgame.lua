-- 胡孙仁 QQ:75567044

a1_low_fightgame = BaseLayer:new();
local this = a1_low_fightgame;

local godlight_active;-- 活动高亮
local godlight_fb;    -- 副本高亮
local godlight_enterElite;-- 首领高亮

local aninToggleButtons;
local toggleplus_isopen = false;

local godlight_shop;--商城高亮
local godlight_EnterLottery;--占卜高亮
local godlight_exchange;--兑换
local godlight_MonthCard;--月卡高亮
local godlight_huoyue;--活跃高亮
local godlight_huoyue1;--活跃高亮
local godlight_btnCseth;--神赐高亮s
local godlight_firstRechargeAward;--首冲高亮
local godlight_firstRechargeAward1;--首冲高亮
local godlight_awardCenter;--福利高亮
local godlight_awardCenter1;--福利高亮
local godlight_sevendays;--七日目标高亮
local godloght_openbtn;--打开按钮高亮
local godlight_houyue_bool = false;
local newact_open = false;
local newact_have = false;
local bossranking_btn;--boss排行按钮


local yueka_have = false;
local btn_out;

a1_low_fightgame.init = function()
    this.alain();

    this.canChangePk = true
    this.addClick(this.transform:FindChild('heroicon_head/info/vip').gameObject, this.onVip)
    this.goPk_list = { }
    for i = 0, 4 do
        this.goPk_list[i] = this.transform:FindChild("heroicon_head/info/pk/" .. i).gameObject
    end
    this.addClick(this.transform:FindChild("heroicon_head/info/pk").gameObject, this.onPk)


    this.instance = this;
    this.fbig = this.transform:FindChild("a3_litemap/normal/hidBtns/btnFB"):GetComponent("Image");
    this.activeig = this.transform:FindChild("a3_litemap/normal/hidBtns/btnActive"):GetComponent("Image");
    this.Eliteig = this.transform:FindChild("a3_litemap/normal/hidBtns/btn_enterElite"):GetComponent("Image");
    this.addClick(this.transform:FindChild("a3_litemap/normal/hidBtns/btnActive").gameObject, this.onActive);
    this.addClick(this.transform:FindChild("a3_litemap/normal/hidBtns/btnFB").gameObject, this.onBtnFBClick);
    this.addClick(this.transform:FindChild("a3_litemap/normal/hidBtns/btn_enterElite").gameObject, this.onenterElite);
    godlight_active = this.transform:FindChild("a3_litemap/normal/hidBtns/btnActive/fire").gameObject;
    godlight_fb = this.transform:FindChild("a3_litemap/normal/hidBtns/btnFB/fire").gameObject;
    godlight_enterElite = this.transform:FindChild("a3_litemap/normal/hidBtns/btn_enterElite/fire").gameObject;

    this.addClick(this.transform:FindChild("a3_litemap/normal/btnMap/btn_map").gameObject, this.onWorldMap);
    this.addClick(this.transform:FindChild("a3_litemap/normal/btnMap/btn_line").gameObject, this.onLine);

    this.CheckFB();
    this.CheckACTIVE();
    this.ChecElite();
	this.CheckSport();
	this.Checkhuoyue();

    this.addClick(this.transform:FindChild("a3_lm_btns/normal/togglePlus").gameObject, this.onTogglePlusClick);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnShop").gameObject, this.onBtnShopClick);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnMoneydraw").gameObject, this.onMoneyDraw);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnMonthCard").gameObject, this.onBtnMonthCardClick);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btn_enterLottery").gameObject, this.onBtnEnterLottery);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnAwardCenter").gameObject, this.onBtnAwardCenterClick);
    this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnAwardCenter").gameObject, this.onBtnAwardCenterClick);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnFirstRecharge").gameObject, this.onBtnFirstRechargeClick);
    this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnFirstRecharge").gameObject, this.onBtnFirstRechargeClick);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnCseth").gameObject, this.onBtnCsethClick);
	this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/ranking").gameObject, this.onranking);
    this.addClick (this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject, this.onhuoyue);
    this.addClick(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnhuoyue").gameObject, this.onhuoyue);
   -- this.addClick (this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject, this.onhuoyue);
    this.addClick (this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/sevendays").gameObject, this.onqitian);
	this.addClick (this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/newServer").gameObject, this.onnewServer);
	this.addClick(this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/sports").gameObject, this.onSports);
    this.addClick(this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtnnew/bossranking").gameObject, this.onBossRanking);
    aninToggleButtons = this.transform:FindChild("a3_lm_btns/normal/hidBtns"):GetComponent("Animator");

    godlight_shop=this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnShop/fire").gameObject;
    godlight_EnterLottery = this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btn_enterLottery/fire").gameObject;
    godlight_exchange = this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnMoneydraw/fire").gameObject;
    godlight_MonthCard = this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnMonthCard/fire").gameObject;
    godlight_huoyue = this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue/fire").gameObject;
    godlight_huoyue1 = this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnhuoyue/fire").gameObject;
	godlight_btnCseth = this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnCseth/fire").gameObject;
	godlight_firstRechargeAward=this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnFirstRecharge/fire").gameObject;
    godlight_firstRechargeAward1=this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnFirstRecharge/fire").gameObject;
	godlight_awardCenter=this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnAwardCenter/fire").gameObject;
    godlight_awardCenter1=this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnAwardCenter/fire").gameObject;

    bossranking_btn=this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtnnew/bossranking").gameObject;
    godlight_sevendays=this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/sevendays/fire").gameObject;
    godloght_openbtn=this.transform:FindChild("a3_lm_btns/normal/togglePlus/fire").gameObject;
    GeneralProxy:getInstance();

    godlight_btnCseth:SetActive (false);
    godlight_huoyue:SetActive(false);
     godlight_huoyue1:SetActive(false);
    this.CheckLock4Screamingbox();

	this.showActiveIcon(GeneralProxy:getInstance().active_open,GeneralProxy:getInstance().active_left_tm);
	godlight_exchange:SetActive(false);
	godlight_shop:SetActive(false);
	godlight_firstRechargeAward:SetActive(true);
    godlight_firstRechargeAward1:SetActive(true);
--
	--this.btn_out = this.transform:FindChild("a3_expbar/btn_out");

	ani = this.transform:FindChild("a3_expbar"):GetComponent("Animator");
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_down").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_up").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_0").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_1").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_2").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_4").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_5").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_7").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_8").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_9").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_10").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_11").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_12").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_13").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_14").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_15").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn16").gameObject, this.onBtn);
	this.addClick(this.transform:FindChild("a3_expbar/operator/btn_1_2").gameObject, this.onBtn);

--
	this.addClick(this.transform:FindChild("a3_expbar/btn_out").gameObject, this.onBtnOut);
	this.transform:FindChild("a3_expbar/btn_out").gameObject:SetActive(false)
	 this.CheckLock();

end

a1_low_fightgame.onShowed = function(prama)
    local carr = PlayerModel:getInstance().profession;
    if carr ~= 2 then GameObject.Destroy(this.transform:FindChild('herohead/info/hero_ig_2').gameObject); end
    if carr ~= 3 then GameObject.Destroy(this.transform:FindChild('herohead/info/hero_ig_3').gameObject); end
    if carr ~= 5 then GameObject.Destroy(this.transform:FindChild('herohead/info/hero_ig_5').gameObject); end
	if(this.newact_open ~= nil)then
	this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/newServer").gameObject:SetActive(this.newact_open);
	end
	if(this.newact_have ~= nil) then
		this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/newServer/fire").gameObject:SetActive(this.newact_have);
	end

	if(this.yueka_have ~= nil) then
		this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnMonthCard/fire").gameObject:SetActive(this.yueka_have);
        godlight_MonthCard:SetActive(this.yueka_have);
	end
    this.addClick(this.transform:FindChild('herohead/info/role').gameObject, this.role);

    a1_low_fightgame.heroicon_head_low_inst = this;
    this.show_heroih_cs();
    this.open_btnlight();
end

a1_low_fightgame.role = function()
    openUIByC("a3_role");
end

a1_low_fightgame.onVip = function()
    openUIByC("a3_vip")
end

a1_low_fightgame.onVipChange = function()
    local cur_lv = VipModel:getInstance().level;
    for i = 0, 12 do
        if i == cur_lv then
            this.transform:FindChild('heroicon_head/info/vip/vip_' .. i).gameObject:SetActive(true);
        else
            this.transform:FindChild('heroicon_head/info/vip/vip_' .. i).gameObject:SetActive(false);
        end
    end
end

a1_low_fightgame.onPk = function()
    if PlayerModel:getInstance().zhuan < 1 then
        flytxts():fly(getCont("yizhuan",{""}), 1) 
    else
        if this.canChangePk == true then
            openUIByC("a3_pkmodel")
        else
            flytxts():fly(getCont("curscene",{""}))
        end
    end
end

a1_low_fightgame.refreskCanPk = function(args)
    this.canChangePk = args[0]
end

a1_low_fightgame.checkPkOpen = function()
    this.transform:FindChild("heroicon_head/info/pk").gameObject:SetActive(false)
    if (FunctionOpenMgr():check(FunctionOpenMgr().PK_MODEL) == true) then
        this.OpenPk()
    end
end

a1_low_fightgame.OpenPk = function()
    if a1_low_fightgame.heroicon_head_low_inst ~= nil then
        this.transform:FindChild("heroicon_head/info/pk").gameObject:SetActive(true)
    end
end

a1_low_fightgame.refreshPkImages = function(isShowAni)
    isShowAni = isShowAni or false;
    if PlayerModel:getInstance().zhuan >= 1 then
        for i = 0, 4 do
            if i == PlayerModel:getInstance().now_pkState then
                this.goPk_list[i]:SetActive(true)
            else
                this.goPk_list[i]:SetActive(false)
            end
        end
    end

    if (a1_high_fightgame.heroicon_head_high_inst ~= nil) then
        a1_high_fightgame.heroicon_head_high_inst.refreshPkImages(isShowAni);
    end
end

a1_low_fightgame.show_heroih_cs = function()
    this.onVipChange()
    this.checkPkOpen()
    this.refreshPkImages()
end


a1_low_fightgame.onWorldMap = function(go)
    local data = MapModel:getInstance().curSvrConf;
    if (data:ContainsKey("maptype")) then
        if (data:getValue("maptype")._int > 0) then
            flytxts():fly(getCont("NoMap",{""}), 1)
            return;
        end
    end
    openUIByC("worldmap");
end

a1_low_fightgame.canget_yueka = function(v)
	if this.instance == nil then
	this.yueka_have = v[0];
		return;
    end
	local get = v[0];
	this.yueka_have = v[0];
	this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btnMonthCard/fire").gameObject:SetActive(get);
    godlight_MonthCard:SetActive(get);

end

a1_low_fightgame.onLine = function(go)
    local data = MapModel:getInstance().curSvrConf;
    if (data:ContainsKey("maptype") and data:getValue("maptype")._int > 0) then
        flytxts():fly(getCont("NoLine",{""}), 1)
        return;
    --[[elseif (data:getValue("id")._int == 10) then
        flytxts():fly(getCont("NoMain",{""}), 1)
        return;]]--
    elseif (canlines() == false) then
        flytxts():fly(getCont("NoWar",{""}), 1)
        return;
    end
    openUIByC("a3_mapChangeLine");
end

a1_low_fightgame.refresh_map_ByUIState = function(v)
    if (this.instance == nil) then
        return;
    end
    local curLevelId = 0;
    curLevelId = MapModel:getInstance().curLevelId;
    if (curLevelId > 0) then
        this.transform:FindChild("a3_litemap/normal/hidBtns").gameObject:SetActive(false);
    else
        this.transform:FindChild("a3_litemap/normal/hidBtns").gameObject:SetActive(true);
    end
end

a1_low_fightgame.showbtnIcon = function(v)
	if (this.instance == nil) then
        return;
    end
	local open = v[0];
	this.transform:FindChild("heroicon_head/info/vip").gameObject:SetActive(open);
	this.transform:FindChild("a3_litemap/normal/btnMap").gameObject:SetActive(open);

	if(open == true)then
		this.transform:FindChild("heroicon_head/info/pk").localScale = Vector3.one;
		else
		this.transform:FindChild("heroicon_head/info/pk").localScale =Vector3.zero;
	end
end
		
a1_low_fightgame.onActive = function(go)		
    openUIByC("a3_active", arr);		
end

a1_low_fightgame.onBtnFBClick = function(go)
    openUIByC("a3_counterpart");
end

a1_low_fightgame.onenterElite = function(go)
    openUIByC("A3_EliteMonster");
end

a1_low_fightgame.setactive_btn = function(v)
    if this.instance == nil then
        return;
    end
    local open = v[0];
    this.transform:FindChild("a3_litemap/normal/hidBtns/btnActive").gameObject:SetActive(open);		
    this.transform:FindChild("a3_litemap/normal/hidBtns/btnFB").gameObject:SetActive(open);
    this.transform:FindChild("a3_litemap/normal/hidBtns/btn_enterElite").gameObject:SetActive(open);

end

a1_low_fightgame.ChecElite = function()
    this.Eliteig.enabled = false;
    if (FunctionOpenMgr():check(FunctionOpenMgr().GLOBA_BOSS) == true) then
        this.openElite();
    end
end

a1_low_fightgame.openElite = function()
    if (this.instance == nil) then
        return;
    end
    this.Eliteig.enabled = true;
end

a1_low_fightgame.CheckFB = function()
    this.fbig.enabled = false;
    if (FunctionOpenMgr():check(FunctionOpenMgr().COUNTERPART) == true) then
        this.fbig.enabled = true;
    end
end

a1_low_fightgame.OpenFB = function()
    if (this.instance == nil) then
        return;
    end
    this.fbig.enabled = true;
end

this.CheckACTIVE = function()
    this.activeig.enabled = false;
    if (FunctionOpenMgr():check(FunctionOpenMgr().ACTIVITES) == true) then
        this.activeig.enabled = true;
    end
end

a1_low_fightgame.OpenActive = function()
    if this.instance == nil then
        return;
    end
    this.activeig.enabled = true;
end

this.CheckSport= function()
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/sports").gameObject:SetActive(false);
	if(FunctionOpenMgr():check(FunctionOpenMgr().SPORT) == true)then
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/sports").gameObject:SetActive(true);
	end
end

this.Checkhuoyue= function()
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject:SetActive(false);
	if(FunctionOpenMgr():check(FunctionOpenMgr().HUOYUE) == true)then
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject:SetActive(true);
	end
end

-------------------------------9:副本----------------------------------
-------------------------------10:活动---------------------------------
a1_low_fightgame.Light_Active = function()
    if (this.instance ~= nil) then
        if (FunctionOpenMgr():check(FunctionOpenMgr().ACTIVITES) == true) then
            if (tonumber(os.date("%H")) == 14) then
                godlight_active:SetActive(true);
            else
                if (FunctionOpenMgr():check(FunctionOpenMgr().PVP_DUNGEON) == true) then
                    if (tonumber(os.date("%H")) == 12 or tonumber(os.date("%H")) == 19 or tonumber(os.date("%H")) == 14) then
                        godlight_active:SetActive(true);
                    end
                end
                if (FunctionOpenMgr():check(FunctionOpenMgr().FOR_CHEST) == true) then
                    if (tonumber(os.date("%H")) == 11 or tonumber(os.date("%H")) == 17) then
                        if (tonumber(os.date("%M")) >= 30 and tonumber(os.date("%M")) <= 55) then
                            godlight_active:SetActive(true);
                        end
                    end
                end
            end
        end
    end
end
UpdateBeat:Add(a1_low_fightgame.Light_Active, self);
-------------------------------11:首领---------------------------------

a1_low_fightgame.Open_Light_enterElite = function()
    if (this.instance ~= nil) then
        if (FunctionOpenMgr():check(FunctionOpenMgr().GLOBA_BOSS) == true) then
            godlight_enterElite:SetActive(true);
        end
    end
end

local can_entElite = true;

a1_low_fightgame.jingyingguai_Light_enterElite = function()
    if (this.instance ~= nil) then
        if (FunctionOpenMgr():check(FunctionOpenMgr().GLOBA_BOSS) == true) then
            if (tonumber(os.date("%H")) == 12 or tonumber(os.date("%H")) == 18) then
                if (can_entElite) then
                    godlight_enterElite:SetActive(true);
                else
                    godlight_enterElite:SetActive(false);
                end
            else
                godlight_enterElite:SetActive(false);
            end
        end
    end
end

a1_low_fightgame.shijieboss_Light_enterElite = function(v)
    can_entElite = v[0];
end



local showDrawAvaiable = false;

a1_low_fightgame.onTogglePlusClick = function(go)
	if this.instance == nil then
		return;
    end
	if(toggleplus_isopen == true)then
		toggleplus_isopen = false;
	elseif(toggleplus_isopen == false)then
		toggleplus_isopen = true;
	end
	this.transform:FindChild("a3_lm_btns/normal/togglePlus/Background/Checkmark").gameObject:SetActive(toggleplus_isopen);
	aninToggleButtons:SetBool("onoff", toggleplus_isopen);
	if(toggleplus_isopen)then
		godloght_openbtn:SetActive(false);
	else
		this.open_btnlight();
	end
end

a1_low_fightgame.onBossRanking = function(go)
    openUIByC("a3_bossranking");
end

a1_low_fightgame.onBtnShopClick = function(go)
	openUIByC("shop_a3");
end

a1_low_fightgame.onMoneyDraw = function(go)
	openUIByC("a3_exchange");
end

a1_low_fightgame.onBtnMonthCardClick = function(go)

	--flytxts():fly(getCont("qidai",{""}), 1)
	openUIByC("a3_sign");
end

a1_low_fightgame.onBtnEnterLottery = function (go)

    openUIByC("a3_lottery");
end

a1_low_fightgame.onBtnAwardCenterClick = function(go)
	openUIByC("a3_awardCenter");
end

a1_low_fightgame.onBtnFirstRechargeClick = function(go)
	openUIByC("a3_firstRechargeAward");
	godlight_firstRechargeAward:SetActive(false);
    godlight_firstRechargeAward1:SetActive(false);
end

a1_low_fightgame.openorclosefr = function(v)
   if(this.instance==nil)then
        return;
   end;
   local isopen=v[0];
   this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/btnFirstRecharge").gameObject:SetActive(isopen);
   this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnnew/cont/btnFirstRecharge").gameObject:SetActive(isopen);
end


a1_low_fightgame.onBtnCsethClick = function(go)
	openUIByC("a3_active_godlight");
end

a1_low_fightgame.onranking = function(go)
	 openUIByC ("a3_ranking");
    --InterfaceMgr.open(InterfaceMgr.A3_STAR_PIC);
end

a1_low_fightgame.onhuoyue = function(go)
     openUIByC("a3_activeOnline");
--openUIByC("a3_activeDegree");
-- openUIByC("a3_star_pic");
end

a1_low_fightgame.onqitian = function(go)
openUIByC("a3_sevenday");
end

a1_low_fightgame.onnewServer = function(go)
openUIByC("a3_newActive");
end




a1_low_fightgame.onSports = function(go)
openUIByC("a3_sports");
end

a1_low_fightgame.onshow_newact = function(v)
	if this.instance == nil then
	 this.newact_open =  v[0];
		return;
    end
	local open = v[0];
	this.newact_open =  v[0];
	this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/newServer").gameObject:SetActive(open);
end

a1_low_fightgame.canget_newact = function(v)
	if this.instance == nil then
	this.newact_have = v[0];
		return;
    end
	local have = v[0];
	this.newact_have = v[0];
	this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/newServer/fire").gameObject:SetActive(have);
end

a1_low_fightgame.hideSevendays=function()
	if this.instance == nil then
		return;
    end
    this.transform:FindChild ("a3_lm_btns/normal/hidBtns/canclosebtn/cont/sevendays").gameObject:SetActive(false);
end

a1_low_fightgame.refresh_btn_ByUIState = function(v)
	if(v == nil)then
		return;
	end
	local curLevelId = 0;
	--if(v:ContainsKey("curLevelId"))then
		--curLevelId = v:getValue("curLevelId")._int;
	--end
	curLevelId = MapModel:getInstance().curLevelId;
	if(curLevelId > 0)then
		this.transform:FindChild("a3_lm_btns/normal/togglePlus").gameObject:SetActive(false);
		this.transform:FindChild("a3_lm_btns/normal/hidBtns").gameObject:SetActive(false);
	
	else
		this.transform:FindChild("a3_lm_btns/normal/togglePlus").gameObject:SetActive(true);
		this.transform:FindChild("a3_lm_btns/normal/hidBtns").gameObject:SetActive(true);
		toggleplus_isopen = true;
		this.onTogglePlusClick(nil);
	end
end
-------------------------------------1:商城--------------------------------
--[[a1_low_fightgame.openLight_shop=function()
     this.open_btnlights();
     godlight_shop:SetActive(true);
end

a1_low_fightgame.closeLight_shop=function()
     godlight_shop:SetActive(false);
end]]--
-------------------------------------2:占卜--------------------------------
local timesDraw = 0;
local time = 0;
local timer;
local ten_time=0;
local ten_timer;
a1_low_fightgame.ShowFreeDrawAvaible = function(v)
	if(v == nil)then
		return;
	end
	local data = v[0];
	if(data:ContainsKey("left_times"))then
		timesDraw = data:getValue("left_times")._int;
	end
	if(data:ContainsKey("left_tm"))then
		this.time = data:getValue("left_tm")._float;
	end
	--[[if(timesDraw >0 and this.time <= 0)then
		this.open_btnlights();
		godlight_EnterLottery:SetActive(true);
		showDrawAvaiable = true;
	else
		godlight_EnterLottery:SetActive(false);
		this.timer = Timer.New(this.openlignt,this.time);
	    this.timer:Start();
		showDrawAvaiable = false;
	end]]--
	if(timesDraw>0)then
         if(this.time <= 0)then
         	showDrawAvaiable = true;
         	godlight_EnterLottery:SetActive(true);
         else
         	this.timer = Timer.New(this.openlignt,this.time);
         	this.timer:Start();
         	godlight_EnterLottery:SetActive(false);
         	showDrawAvaiable = false;
         end
    else
    	godlight_EnterLottery:SetActive(false);
    	showDrawAvaiable = false;
    end



	if(data:ContainsKey("ten_free_time"))then
		this.ten_time= data:getValue("ten_free_time")._int;
	end
	if(this.ten_time<=0)then
		this.open_btnlights();
		godlight_EnterLottery:SetActive(true);
		showDrawAvaiable = true;
	else
		if(showDrawAvaiable==true)then
			return;
		else
	        this.ten_timer = Timer.New(this.openlignts,this.ten_time);
	        this.ten_timer:Start();
		    showDrawAvaiable = false;
		end
	end
end

a1_low_fightgame.openlignt = function()
	godlight_EnterLottery:SetActive(true);
	this.open_btnlights();
	this.showDrawAvaiable =true;
	this.time = 0;
	this.timer:Stop();
end
a1_low_fightgame.openlignts = function()
	godlight_EnterLottery:SetActive(true);
	this.open_btnlights();
	this.showDrawAvaiable =true;
	this.ten_time = 0;
	this.ten_timer:Stop();
end

-------------------------------------3:兑换---------------------------------

--[[a1_low_fightgame.Light_exchange=function(v)
	if(v == nil)then
		return;
	end
	local data = v[0];
	local num=0;
	if(data:ContainsKey("yinpiao_count"))then
       num=data:getValue("yinpiao_count")._int;
    end
	if(num==10)then
		godlight_exchange:SetActive(false);
	else
		this.open_btnlights();
		godlight_exchange:SetActive(true);
	end
end]]--
-------------------------------------4:月卡---------------------------------
a1_low_fightgame.refreshSign = function(v)
	--if(v == nil)then
		--return;
	--end
	--local data = v[0];
	--local len = data:getValue("qd_days").Count;
	--log ("123"..data:dump());
	--local time = os.time()
	--if(len > 0 )then
		--if(data:getValue("qd_days")._arr[len -1]._int == tonumber(os.date("%d",time)))then
			--godlight_MonthCard:SetActive(false);
		--else
			--this.open_btnlights();
			--godlight_MonthCard:SetActive(true);
		--end
	--else
		--this.open_btnlights();
		--godlight_MonthCard:SetActive(true);
	--end
end

a1_low_fightgame.singorrepair = function(v)
	--if(v == nil)then
		--return;
	--end
	--local data = v[0];
	--local time = os.time()
	--if(data:getValue("daysign")._int ==tonumber(os.date("%d",time)))then
		--godlight_MonthCard:SetActive(false);
	--end
end
-------------------------------------5:活跃----------------------------------
a1_low_fightgame.open_light_huoyue = function ()
	if(this.instance == nil)then
		return;
	end
    if(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject.activeSelf==false)then
        return;
    end
	 this.open_btnlights();
     godlight_huoyue:SetActive(true);
     godlight_huoyue1:SetActive(true);
end

a1_low_fightgame.close_light_huoyue=function()
	if(this.instance == nil)then
		return;
	end
    if(this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject.activeSelf==false)then
        return;
    end
    godlight_huoyue:SetActive(false);
    godlight_huoyue1:SetActive(false);

end
-------------------------------------6:神赐之光------------------------------
a1_low_fightgame.Light_btnCseth = function()
     if(this.instance~=nil)then 
		 if(tonumber(os.date("%H"))==12 or tonumber(os.date("%H"))==19)then
		 	this.open_btnlights();
			godlight_btnCseth:SetActive(true);
		 else
		 	godlight_btnCseth:SetActive(false);
		 end
     end
			 --log("12312");	 
end
UpdateBeat:Add(this.Light_btnCseth,self);
-------------------------------------7:首冲----------------------------------
--打开界面关掉
-------------------------------------8:福利----------------------------------
a1_low_fightgame.open_Light_awardCenter = function(v)
     this.open_btnlights();
     godlight_awardCenter:SetActive(true);
     godlight_awardCenter1:SetActive(true);

end
a1_low_fightgame.close_Light_awardCenter=function(v)
     godlight_awardCenter:SetActive(false);
     godlight_awardCenter1:SetActive(true);
end  
-------------------------------------9:七日目标----------------------------------
a1_low_fightgame.open_Light_sevenday=function()
    this.open_btnlights();
    godlight_sevendays:SetActive(true);
end
a1_low_fightgame.close_Light_sevenday=function()		
    godlight_sevendays:SetActive(false);
end
------------------------------------10:打开按钮----------------------------------
a1_low_fightgame.open_btnlight=function()	
   if(godlight_shop.activeSelf==false and godlight_EnterLottery.activeSelf==false and godlight_exchange.activeSelf==false and 
   	godlight_MonthCard.activeSelf==false and godlight_huoyue.activeSelf==false and godlight_btnCseth.activeSelf==false and 
   	godlight_firstRechargeAward.activeSelf==false and godlight_awardCenter.activeSelf==false and godlight_sevendays.activeSelf==false
   	)then
    	godloght_openbtn:SetActive(false);
   else
    	godloght_openbtn:SetActive(true);
   end 

end
a1_low_fightgame.open_btnlights=function()
	if(this.transform:FindChild("a3_lm_btns/normal/togglePlus/Background/Checkmark").gameObject.activeSelf)then
	    godloght_openbtn:SetActive(false);
	else
	    godloght_openbtn:SetActive(true);
	end

end
------------------------------------10:boss排行----------------------------------
a1_low_fightgame.bossrkOp=function()        
    if(bossranking_btn~=nil)then
    bossranking_btn:SetActive(true);
    end
end

a1_low_fightgame.bossrkCl=function() 
    if(bossranking_btn~=nil)then       
    bossranking_btn:SetActive(false);
    end
end




local active_leftTm = 0;
local runtime = false;
local timerrun ;
a1_low_fightgame.showActiveIcon = function(open , time)
	if(open)then
		this.active_leftTm = time;
		godlight_btnCseth:SetActive(true);
		this.timerrun = Timer.New(this.openlignt2,this.this.active_leftTm);
		this.timerrun:Start();
	else
		godlight_btnCseth:SetActive(false);
	end
end
a1_low_fightgame.openlignt2=function()
	godlight_btnCseth:SetActive(false);
	this.active_leftTm =0;
	this.timerrun:Stop();
end

a1_low_fightgame.setToggle = function(v)
	toggleplus_isopen = v[0];
	this.onTogglePlusClick(nil);
end

a1_low_fightgame.CheckLock4Screamingbox = function()
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btn_enterLottery").gameObject:SetActive(false);
	if(FunctionOpenMgr():check(FunctionOpenMgr().SCREAMINGBOX) == true)then
		this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btn_enterLottery").gameObject:SetActive(true);
	end
end

a1_low_fightgame.OpenMH= function(v)
	if this.instance == nil then
		return;
    end
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/btn_enterLottery").gameObject:SetActive(true);
end

a1_low_fightgame.OpenSport = function(v)
	if this.instance == nil then
		return;
    end
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtnup/cont/sports").gameObject:SetActive(true);
end

a1_low_fightgame.Openhuoyue = function(v)
	if this.instance == nil then
		return;
    end
	this.transform:FindChild("a3_lm_btns/normal/hidBtns/canclosebtn/cont/huoyue").gameObject:SetActive(true);
end

a1_low_fightgame.bag_Count = function(args) 
   if (this.instance == nil) then
        return;
    end
	local count = args[0] - args[1];
	if(count == 1 or count == 2 or count == 3 or count == 4 or count == 5)then 
	this.transform:FindChild("a3_expbar/operator/btn_1/count").gameObject:SetActive(true);
	this.transform:FindChild("a3_expbar/operator/btn_1/man").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1/count/count"):GetComponent("Text").text = count 
	this.transform:FindChild("a3_expbar/operator/btn_1_2/count").gameObject:SetActive(true);
	this.transform:FindChild("a3_expbar/operator/btn_1_2/man").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1_2/count/count"):GetComponent("Text").text = count .."";
	elseif(count == 0) then
	this.transform:FindChild("a3_expbar/operator/btn_1/count").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1/man").gameObject:SetActive(true);
	this.transform:FindChild("a3_expbar/operator/btn_1_2/count").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1_2/man").gameObject:SetActive(true);
	else
	this.transform:FindChild("a3_expbar/operator/btn_1/count").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1/man").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1_2/count").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_1_2/man").gameObject:SetActive(false);
	end
end





a1_low_fightgame.onBtn = function(go)
	if(go.name == "btn_0")then
	openUIByC("a3_chatroom");
	elseif (go.name == "btn_1")then
	openUIByC("a3_bag");
	elseif(go.name == "btn_1_2")then
	openUIByC("a3_bag");
	elseif(go.name == "btn_2")then
	openUIByC("a3_task");
	elseif(go.name == "btn_3")then
	openUIByC("a3_role");
	elseif(go.name == "btn_4")then
	openUIByC("a3_shejiao");
	elseif(go.name == "btn_5")then
	openUIByC("a3_equip");
	elseif(go.name == "btn_6")then
	openUIByC("a3_auction");
	elseif(go.name == "btn_7")then
	openUIByC("a3_achievement");
	elseif(go.name == "btn_8")then
	openUIByC("a3_summon_new");
	elseif(go.name == "btn_9")then
	openUIByC("skill_a3");
	elseif(go.name == "btn_10")then
	openUIByC("a3_wing_skin");
	elseif(go.name == "btn_11")then
	openUIByC("a3_mail");
	elseif(go.name == "btn_12")then
	openUIByC("a3_systemSetting");
	elseif(go.name == "btn_13")then
	openUIByC("a3_new_pet");
	elseif(go.name == "btn_14")then
	openUIByC("a3_hudun");
	elseif(go.name == "btn16")then
	openUIByC("a3_auction");
	elseif(go.name == "btn_17")then
	openByLua("a3_star_pic");
	elseif(go.name == "btn_18")then
	openByLua("a3_runestone");
	elseif(go.name == "btn_up")then
	this.On_Btn_Up();
	elseif(go.name == "btn_down")then
	this.On_Btn_Down();
	elseif(go.name == "mail")then
	openUIByC("a3_mail");
	end
end
a1_low_fightgame.On_Btn_Up = function()
	if(btnDwonClicked == false)then
		return;
	end
	btnDwonClicked = false;
	ani:SetBool("onoff", true);
	this.transform:FindChild("a3_expbar/operator/btn_down").gameObject:SetActive(true);
	this.transform:FindChild("a3_expbar/operator/btn_up").gameObject:SetActive(false);
	BaseLayer.setGameJoy(true);
	BaseLayer.setGameSkill(true);
	toggleplus_isopen = false;
	this.onTogglePlusClick(nil);
	this.transform:FindChild("a3_expbar/btn_out").gameObject:SetActive(true)
end

a1_low_fightgame.On_Btn_Down = function()
	if(btnDwonClicked == true)then
		return;
	end
	btnDwonClicked = true;
	ani:SetBool("onoff", false);
	this.transform:FindChild("a3_expbar/operator/btn_down").gameObject:SetActive(false);
	this.transform:FindChild("a3_expbar/operator/btn_up").gameObject:SetActive(true);
	BaseLayer.setGameJoy(false);
	BaseLayer.setGameSkill(false);
	toggleplus_isopen = true;
	this.onTogglePlusClick(nil);
	this.transform:FindChild("a3_expbar/btn_out").gameObject:SetActive(false)
end

a1_low_fightgame.onBtnOut = function(go)
	this.transform:FindChild("a3_expbar/btn_out").gameObject:SetActive(false)
	this.On_Btn_Down();
end

a1_low_fightgame.setRollWord = function()
	




end



a1_low_fightgame.CheckLock = function()	
	this.transform:FindChild("a3_expbar/operator/btn_10/local").gameObject:SetActive(true);
    --this.transform:FindChild("a3_expbar/operator/btn_10").gameObject:GetComponent("Button").interactable = false;
	--this.transform:FindChild("a3_expbar/operator/btn_5").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_5/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_6").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_6/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_8").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_8/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_7").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_7/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_9").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_9/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_13").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_13/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_14").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_14/local").gameObject:SetActive(true);
	--this.transform:FindChild("a3_expbar/operator/btn_17").gameObject:GetComponent("Button").interactable = false;
	this.transform:FindChild("a3_expbar/operator/btn_17/local").gameObject:SetActive(true);


	if(FunctionOpenMgr():check(FunctionOpenMgr().PET_SWING) == true)then
		this.OpenSWING_PET();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().EQP) == true)then
		this.OpenEQP();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().STAR_PIC) == true)then
		this.OpenSTAR_PIC();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().HUDUN) == true)then
		this.OpenHUDUN();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().AUCTION_GUILD) == true)then
		this.OpenAuction();
	end

	if(FunctionOpenMgr():check(FunctionOpenMgr().SUMMON_MONSTER) == true)then
		this.OpenSummon();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().ACHIEVEMENT) == true)then
		this.OpenAchievement();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().SKILL) == true)then
		this.OpenSkill();
	end
	if(FunctionOpenMgr():check(FunctionOpenMgr().PET) == true)then
		this.OpenPET();
	end
end

a1_low_fightgame.OpenSWING_PET =  function()
      if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_10").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_10/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenEQP = function()
      if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_5").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_5/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenSTAR_PIC=function()
      if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_17").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_17/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenHUDUN=function()
      if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_14").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_14/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenAuction = function()
   if a1_low_fightgame.heroicon_head_low_inst ~= nil then   
	--this.transform:FindChild("a3_expbar/operator/btn_6").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_6/local").gameObject:SetActive(false);
	end
end

a1_low_fightgame.OpenSummon = function()
   if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_8").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_8/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenAchievement= function()
     if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_7").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_7/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenSkill= function()
       if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_9").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_9/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenPET= function()
      if (this.instance == nil) then
        return;
    end
	--this.transform:FindChild("a3_expbar/operator/btn_13").gameObject:GetComponent("Button").interactable = true;
	this.transform:FindChild("a3_expbar/operator/btn_13/local").gameObject:SetActive(false);
end

a1_low_fightgame.OpenPet  = function()
      if (this.instance == nil) then
        return;
    end
   this.transform:FindChild("a3_expbar/operator/btn_13/nofeed").gameObject:SetActive(true);
end

a1_low_fightgame.feedOpenPet= function()
      if (this.instance == nil) then
        return;
    end
   this.transform:FindChild("a3_expbar/operator/btn_13/nofeed").gameObject:SetActive(false);

end