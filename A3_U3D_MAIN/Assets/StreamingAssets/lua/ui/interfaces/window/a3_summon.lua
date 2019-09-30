a3_summon = BaseLayer:new ();
local this = a3_summon;

a3_summon.init = function ()
    this.addClick (this.transform:FindChild ("btn_close").gameObject, this.onCloseClick);
    this.tabControler = this.getTabControler ("tabs", "contents", this.onSwitch);
	
	--祭坛
	this.jitan_trans = this.getTransform("sub_jitan");
	this.sub_jitan_summonlist = this.getTransform("sub_jitan/Panel/summons/scroll/content");
	this.sub_jitan_0 = this.getTransform("sub_jitan/Panel/summons/scroll/0");
	this.addClick (this.transform:FindChild ("btn_sub_jitan").gameObject, this.sub_jitan_open);
	this.addClick (this.transform:FindChild ("sub_jitan/Panel/btn_close").gameObject, this.sub_jitan_close);
	this.sub_jitan_tabControler = this.getTabControler ("sub_jitan/Panel/tabs", "sub_jitan/Panel/contents", this.sub_jitan_switch);
end;

a3_summon.onCloseClick = function (go)
    InterfaceMgr.close (InterfaceMgr.A3SUMMON);
end;

a3_summon.onSwitch = function (tc)
	
end

--祭坛1：打开
a3_summon.sub_jitan_open = function(go)
    this.jitan_trans.gameObject:SetActive(true);
	this.sub_jitan_tabControler:setSelectedIndex(0,true);
end
--祭坛2：关闭
a3_summon.sub_jitan_close = function(go)
     this.jitan_trans.gameObject:SetActive(false);
end
--祭坛3：切换页签
a3_summon.sub_jitan_switch = function(tc)
	this.clearChild(this.sub_jitan_summonlist);
	--this.SF_Cancel("jitan_content");
    local index = tc:getSeletedIndex ();
	local xml = XmlMgr:GetSXML("callbeast");
	local list = nil;
	if index~=4 then 
		list = xml:GetNodeList("callbeast","type=="..1);
	else list = xml:GetNodeList("callbeast","type=="..2);
	end;
	--local uuli = this.GetCList();
	if index == 0 then
--可用
	
	elseif index == 1 then
--普通非变异
		for i=0,list.Count-1,1 do 
			local oo = list[i];
			if oo:getInt("quality")==1 then
			--uuli:ADD(oo);
			end
		end  
    elseif index == 2 then
--精英非变异
		for i=0,list.Count-1,1 do 
			local oo = list[i];
			if oo:getInt("quality")==2 then
			--uuli:ADD(oo);
			end
		end  
    elseif index == 3 then
--领主非变异
		for i=0,list.Count-1,1 do 
			local oo = list[i];
			if oo:getInt("quality")==3 then
			--uuli:ADD(oo);
			end
		end  
	elseif index == 4 then
--变异		
		for i=0,list.Count-1,1 do 
			local oo = list[i];
			--uuli:ADD(oo);
		end  
    end;
	--this.SF_Create("jitan_content",this.sub_jitan_0.gameObject,uuli,a3_summon.sub_jitan_setPrefab);
end
--祭坛4：xml设置预制件
a3_summon.sub_jitan_setPrefab = function(go, xmldata)
	go.transform:SetParent(this.sub_jitan_summonlist);
	go:SetActive(true);
	go.transform.localScale = Vector3(1, 1, 1);
    log(xmldata:getString("name"));
end