require "proxy/PlayerInfoProxy"
require "model/PlayerModel"
require "Logic/Globle"
require "Logic/ProfessionAvatar"
require "model/a3_BagModel"

a3_role = BaseLayer:new();
local this = a3_role;

a3_role.init = function()
	a3_role.m_SelfObj = nil;
	a3_role.scene_Camera = nil;
	a3_role.scene_Obj = nil;

	a3_role.left_pt_num = 0;
	a3_role.rateTime = 0;
	a3_role.addTime = 0.5;
	a3_role.addType = 0;
	a3_role.seleltColorId = 0;
	a3_role.ty = 0;


	a3_role.isCanAdd = false;
	a3_role.isAdd = false;
	a3_role.isReduce = false
	a3_role.CanRun = true;

	a3_role.listPage = { };
	a3_role.listIcon = { };
	a3_role.listPos = { };
	a3_role.attr_text = { };
	a3_role.btn_pt_add = { };
	a3_role.btn_pt_reduce = { };

	a3_role.cur_att_pt = { };
	a3_role.true_att_pt = { };
	a3_role.base_att_pt = { };
    -- ==========================================


    this.addClick(this.transform:FindChild("btn_close").gameObject, this.onCloseClick);

    a3_role.roleAvatar = this.transform:FindChild("avatar/RawImage"):GetComponent("RawImage");
	a3_role.m_proAvatar = nil;

	a3_role.rolePanel_0 = this.getGameObject("playerInfo/contents/panel_attr");
	a3_role.rolePanel_1 = this.getGameObject("playerInfo/contents/panel_add");

	-- 初始化协议
	 PlayerInfoProxy:getInstacne();

	this.tabControler = this.getTabControler("playerInfo/panelTab", "playerInfo/contents", this.onSwitch);
	this.scrollControler = this.getScrollControler("playerInfo/contents/panel_attr/attr_scroll/scroll");
	this.itemListView = this.getTransform("playerInfo/contents/panel_attr/attr_scroll/scroll/contain");
	this.item_Parent = this.itemListView:GetComponent("GridLayoutGroup");
	local eve = this.getEventTriggerListener(this.itemListView.gameObject);
	eve.onDown = this.ondown;

	local head = this.getComponentByPath("playerInfo/contents/panel_attr/hero_ig/ig", "Image");
    head.sprite = this.loadSprite("h" .. tostring(PlayerModel:getInstance().profession))
    a3_role.isCanAddText=this.getComponentByPath("playerInfo/contents/panel_add/btn_add/Text","Text");

    this.addClick(this.rolePanel_1.transform:FindChild("btn_add").gameObject, this.onbtnAdd);

    this.addClick(this.rolePanel_1.transform:FindChild("btn_clear").gameObject, this.onbtnClear);
    this.addClick(this.rolePanel_1.transform:FindChild("tishi/can/yes").gameObject, this.on_yesClear);
    this.addClick(this.rolePanel_1.transform:FindChild("tishi/can/no").gameObject, this.on_noClear);
    this.addClick(this.rolePanel_1.transform:FindChild("tishi/no/yes").gameObject, this.onClealback)

    this:initPointAttr();
	this:initAttr();
end

a3_role.ondown = function(go)
	-- logError("ondown");
end

a3_role.onShowed = function()
	a3_role.instance = this;
    InterfaceMgr.changeState(InterfaceMgr.STATE_FUNCTIONBAR);
    --PlayerModel:getInstance();
    a3_role:refreshAttr();
    a3_role:onAttrChange(nil);

end

a3_role.refreshAttr = function()
	local proname = this.getComponentByPath("playerInfo/contents/panel_attr/name", "Text");
	proname.text = PlayerModel:getInstance().name;
	local txt_zhuan = this.getComponentByPath("playerInfo/contents/panel_attr/lv", "Text");
	txt_zhuan.text = "Lv" .. PlayerModel:getInstance().lvl .. "（" .. PlayerModel:getInstance().zhuan .. "转）";
	-- 等军团model
end
a3_role.onClosed = function()
	a3_role.instance = nil;
	InterfaceMgr.changeState(InterfaceMgr.STATE_NORMAL);
end

a3_role.onCloseClick = function(go)
	a3_role:showInfoTrue();
	InterfaceMgr.close(InterfaceMgr.A3_ROLE);

end

a3_role.showInfoTrue = function()
	local playerInfo = this.transform:FindChild("playerInfo").gameObject;
	playerInfo:SetActive(true);
end

a3_role.initPointAttr = function()
	for i = 1, 5 do
		local btn = this.rolePanel_1.transform:FindChild("btn_" .. i).gameObject;
		if PlayerModel:getInstance().profession == 3 and i == 3 then
			btn:SetActive(false);
		elseif (PlayerModel:getInstance().profession ~= 3 and i == 2) then
            btn:SetActive(false);
		else
            local btn_reduce_trans = btn.transform:FindChild("btn_reduce");
			this.btn_reduce = btn_reduce_trans:GetComponent("Button");
			local btn_reduce_go = btn_reduce_trans.gameObject;

			local btn_add_trans = btn.transform:FindChild("btn_add");
			this.btn_add = btn_add_trans:GetComponent("Button");
			local btn_add_go = btn_add_trans.gameObject;

            local ty = i;

			local eve1 = this.getEventTriggerListener(btn_add_go);
			eve1.onDown = this.onClickAdd;
			eve1.onExit = this.onClickAddExit;

			local eve2 = this.getEventTriggerListener(btn_reduce_go);
			eve2.onDown = this.onClickReduce;
			eve2.onExit = this.onClickReduceExit;

			this.btn_pt_add[ty] = this.btn_add;
			this.btn_pt_reduce[ty] = this.btn_reduce;
		end
	end
	local s_xml = XmlMgr:GetSXML("creat_character.character", "job_type==" .. PlayerModel:getInstance().profession);
	local x = s_xml:GetNodeList("character");
	local mim = x.Count - 1;
	for i = 1, mim do
		if x[i]:getInt("att_type") == 1 then
			this.base_att_pt[1] = x[i]:getInt("att_value");
		elseif x[i]:getInt("att_type") == 2 then
			this.base_att_pt[3] = x[i]:getInt("att_value");
		elseif x[i]:getInt("att_type") == 3 then
			this.base_att_pt[4] = x[i]:getInt("att_value");
		elseif x[i]:getInt("att_type") == 4 then
			this.base_att_pt[2] = x[i]:getInt("att_value");
		elseif x[i]:getInt("att_type") == 34 then
			this.base_att_pt[5] = x[i]:getInt("att_value");
		end
	end
end

a3_role.initAttr = function()
	local item = this.transform:FindChild("playerInfo/contents/panel_attr/attr_scroll/scroll/item").gameObject;
	local parent = this.transform:FindChild("playerInfo/contents/panel_attr/attr_scroll/scroll/contain").gameObject;
	local xml = XmlMgr:GetSXML("carrlvl");
	local str = xml:GetNode("att_show"):getString("att_type");
	local att = { };
	for n in string.gmatch(str, "(%d*),") do
		table.insert(att, n);
		local s = 0;
		att[s] = n;
		s = s + 1;
	end
	local index = 0;
	for m = 1, table.maxn(att) do
		index = index + 1;
		local itemclone = GameObject.Instantiate(item);
		itemclone:SetActive(true);
		local names = itemclone.transform:FindChild("name"):GetComponent("Text");
		local values = itemclone.transform:FindChild("value"):GetComponent("Text");

		local i = tonumber(att[m]);
		names.text = Globle.getAttrNameById(i) .. "：";
		if i == 5 then
			values.text = PlayerModel:getInstance().attr_list[38] .. "-" .. PlayerModel:getInstance().attr_list[5];
		elseif (i == 17 or i == 19 or i == 20 or i == 24 or i == 25 or i == 29 or i == 30 or i == 31 or i == 32
			or i == 33 or i == 35 or i == 36 or i == 37 or i == 39 or i == 40 or i == 17 or i == 41) then
			values.text = "+" .. tostring((PlayerModel:getInstance().attr_list[i]) / 10) .. "%";
		else
			values.text = "+" .. tostring(PlayerModel:getInstance().attr_list[i]);
		end
		if index % 2 ~= 0 then
			itemclone.transform:FindChild("ig_bg").gameObject:SetActive(false);
		end

		itemclone.transform:SetParent(parent.transform);

		this.attr_text[i] = values;
		itemclone.transform.localScale = Vector3.New(1, 1, 1);
	end

	local height = parent.transform:GetComponent("GridLayoutGroup").cellSize.y;
	local rect = parent:GetComponent("RectTransform");
    rect.sizeDelta = Vector2.New(0, table.maxn(att) * height);
end

a3_role.refreshAttPoint = function()



end

a3_role.onSwitch = function(tc)
	local index = tc:getSeletedIndex();
	if index == 0 then
		this.rolePanel_0:SetActive(true);
		this.rolePanel_1:SetActive(false);
	elseif index == 1 then
		this.rolePanel_0:SetActive(false);
		this.rolePanel_1:SetActive(true);
	end
end

a3_role.refreshAttPointAuto=function()-- ============推荐属性加点
    if PlayerModel:getInstance().profession == 2 then
        local addtype = { 4, 3, 1, 5 };
        local tem = { 3, 2, 2, 2 };
        this:addPointAuto(this.left_pt_num, addtype, tem);
    end
    if PlayerModel:getInstance().profession == 3 then
        local addtype = { 4, 2, 5, 1 };
        local tem = { 4, 2, 2, 2 };
        this:addPointAuto(this.left_pt_num, addtype, tem);
    end
    if PlayerModel:getInstance().profession == 5 then
        local addtype = { 3, 4, 1, 5 };
        local tem = { 3, 3, 2, 2 };
        this:addPointAuto(this.left_pt_num, addtype, tem);
    end
end

function a3_role:addPointAuto(left_num, addtype, tem)
    local adds = { 0, 0, 0, 0 };
    local sum = 0;


    for i = 1, table.maxn(tem) do
        sum = sum + tem[i];
    end
    local a = math.floor(left_num / sum);
    local b = left_num % sum;
    for i = 1, 4 do
        adds[i] = tem[i] * a;
    end
    if b > 0 then
        local over_num = b;
        for i = 1, 4 do
            if over_num >= tem[i] then
                adds[i] = adds[i] + tem[i];
                over_num = over_num - tem[i];
            else
                adds[i] = adds[i] + over_num;
                over_num = 0;

            end
            if over_num <= 0 then
                break;
            end
        end
    end
    for i = 1, 4 do
        this.cur_att_pt[addtype[i]] = adds[i];
    end
    for key, value in pairs(this.cur_att_pt) do
        this.rolePanel_1.transform:FindChild("btn_" .. key .. "/value"):GetComponent("Text").text = tostring(this.cur_att_pt[key]);
    end
    this.left_pt_num = 0;
    this.rolePanel_1.transform:FindChild("num"):GetComponent("Text").text = tostring(this.left_pt_num);
    this:checkLeftPtNum();
end

a3_role.checkLeftPtNum=function()
    if this.left_pt_num <= 0 then
        this.left_pt_num = 0;
        for key, btn in pairs(this.btn_pt_add) do
            btn.interactable = false;
            this.isAdd = false;
        end
    else
        for key, btn in pairs(this.btn_pt_add) do
            btn.interactable = true;
        end
    end	
--===========
    if this.left_pt_num < PlayerModel:getInstance().pt_att then
        for key, value in pairs(this.cur_att_pt) do
            if this.cur_att_pt[key] > 0 then
                this.btn_pt_reduce[key].interactable = true;
            else
                this.btn_pt_reduce[key].interactable = false;
            end
        end
        this.isCanAdd=true;
        this.isCanAddText.text=tostring("确定");
    else
        this.isReduce=false;
        for key,value in pairs(this.cur_att_pt) do
            this.btn_pt_reduce[key].interactable=false;
        end
		this.isCanAdd = false;
        this.isCanAddText.text = tostring("推荐加点");
    end
--==========
    for k, value in pairs(this.cur_att_pt) do
        if k == this.addType then
            if this.cur_att_pt[this.addType] <= 0 then
                this.isReduce = false;
            end
        end
    end

end


a3_role.onbtnClear=function(go)
    this.rolePanel_1.transform:FindChild("tishi").gameObject:SetActive(true);
    local xml = XmlMgr:GetSXML("carrlvl");
    local str = xml:GetNode("points_reset"):getString("cost");
    local txt = this.rolePanel_1.transform:FindChild("tishi/text"):GetComponent("Text");
    if PlayerModel:getInstance().up_lvl == 0 and PlayerModel:getInstance().lvl <= 80 then
        txt.text="1转后开启次功能！";
        this.rolePanel_1.transform:FindChild("tishi/no").gameObject:SetActive(true);
        this.rolePanel_1.transform:FindChild("tishi/can").gameObject:SetActive(false);
    else
        txt.text="需要花费" .. str .. "个钻石，是否继续？";
        this.rolePanel_1.transform:FindChild("tishi/no").gameObject:SetActive(false);
        this.rolePanel_1.transform:FindChild("tishi/can").gameObject:SetActive(true);
    end
end

a3_role.on_yesClear=function(go)
    PlayerInfoProxy:getInstacne():sendAddPoint(1,nil);
    this.rolePanel_1.transform:FindChild("tishi").gameObject:SetActive(false);
end
a3_role.on_noClear=function(go)
    this.rolePanel_1.transform:FindChild("tishi").gameObject:SetActive(false);
end
a3_role.onClealback=function(go)
    this.rolePanel_1.transform:FindChild("tishi").gameObject:SetActive(false);
end

a3_role.onbtnAdd=function(go)
    if this.isCanAdd then
        local add_pt={}
        for key,value in pairs(this.cur_att_pt)do
            if this.cur_att_pt[key]>0 then
                add_pt[key]=value;
            end
            logError(value.."===key==");
        end
        if table.maxn(add_pt)>0 then
            PlayerInfoProxy:getInstacne():sendAddPoint(0,add_pt);
        end
        logError(table.getn(this.cur_att_pt).."长度");
    else
        this:refreshAttPointAuto();
    end

end



a3_role.onAttrChange=function(e)
    for i,value in pairs(this.attr_text)  do
        if i==5 then
            this.attr_text[i].text =PlayerModel:getInstance().attr_list[38] .. "-" .. PlayerModel:getInstance().attr_list[5];
        elseif(i == 17 or i == 19 or i == 20 or i == 24 or i == 25 or i == 29 or i == 30 or i == 31 or i == 32
        or i == 33 or i == 35 or i == 36 or i == 37 or i == 39 or i == 40 or i == 17 or i == 41)then
            this.attr_text[i].text = "+" .. tostring((PlayerModel:getInstance().attr_list[i]) / 10) .. "%";
		else
			this.attr_text[i].text = "+" .. tostring(PlayerModel:getInstance().attr_list[i]);
        end
    end
    local fight=this.getComponentByPath("fighting/value","Text");
    fight.text=tostring(PlayerModel:getInstance().combpt);
--=======装备
end