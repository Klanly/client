require "proxy/A3TaskProxy"
require "model/A3TaskModel"
require "model/PlayerModel"
require "proxy/BagProxy"
require "model/a3_BagModel"
a3_task = BaseLayer:new();
local this = a3_task;

a3_task.init = function ()
    this.addClick (this.transform:FindChild ("btn_close").gameObject, this.onCloseClick);
    this.tabControler = this.getTabControler ("tab", "content", this.onSwitch);
--主线任务
	 this.maintitle = this.getComponentByPath("content/zhuxian/0/title/Text", "Text");
	 this.maincontent = this.getComponentByPath("content/zhuxian/0/state/Text", "Text");
	 this.mainpgs = this.getComponentByPath("content/zhuxian/0/slider/text", "Text");
	 this.mainpgsslider = this.getComponentByPath("content/zhuxian/0/slider/slider", "Slider");
	 this.maintargettitle = this.getComponentByPath("content/zhuxian/1/title/Text", "Text");
	 this.maintarget = this.getComponentByPath("content/zhuxian/1/task/Text_state", "Text");
	 this.maincptrwctt = this.getTransform("content/zhuxian/0/reward/view/con");
	 this.maintaskrwctt = this.getTransform("content/zhuxian/1/reward/view/con");
	 this.addClick(this.transform:FindChild("content/zhuxian/1/btn_move").gameObject, this.onZhuxianMove);
--日常任务
	 this.richangpgs = this.getComponentByPath("content/richang/0/slider/text", "Text");
	 this.richangpgsslider = this.getComponentByPath("content/richang/0/slider/slider", "Slider");
	 this.richangtargettitle = this.getComponentByPath("content/richang/1/title/Text", "Text");
	 this.richangtarget = this.getComponentByPath("content/richang/1/task/Text_state", "Text");
	 this.addClick(this.transform:FindChild("content/richang/0/btn_onekey").gameObject, this.onRCOnekeyFinish);
	 this.addClick(this.transform:FindChild("content/richang/1/star/btn_oneKey").gameObject, this.onRCOnekeyStar);
	 this.richangexrwctt = this.getTransform("content/richang/0/reward/view/con");
	 this.richangtaskrwctt = this.getTransform("content/richang/1/reward/view/con");
	 this.richangstar = this.getTransform("content/richang/1/star/con_star");
-- 初始化协议
    A3TaskProxy:getInstance ();

	BagProxy:getInstance();
	BagProxy:getInstance():sendLoadItems(0);
	
--初始化界面
	this.getTransform("tab/zhuxian").gameObject:SetActive(false);
	this.getTransform("tab/richang").gameObject:SetActive(false);
	this.getTransform("tab/zhixian").gameObject:SetActive(false);
	this.getTransform("content").gameObject:SetActive(false);
end;

a3_task.onCloseClick = function (go)
    InterfaceMgr.close (InterfaceMgr.A3TASK);
end;

a3_task.onShowed = function (prama)
    a3_task.instance = this;
    A3TaskProxy:SendGetTask ();
end;

a3_task.onClosed = function ()
    a3_task.instance = nil;
end;

a3_task.onSwitch = function (tc)
	local index = tc:getSeletedIndex ();
    if index == 0 then
--主线任务
        this.showZhuxian ();
    elseif index == 1 then
--日常任务
        this.showRichang ();
    elseif index == 2 then
--支线任务
        this.showZhixian ();
    end;
end

a3_task.OnRefresh = function ()
    local index = this.tabControler:getSeletedIndex ();
    if index == 0 then
--主线任务
		a3_task.openFunc("zhuxian");	
        this.showZhuxian ();
    elseif index == 1 then
--日常任务
		a3_task.openFunc("richang");	
        this.showRichang ();
    elseif index == 2 then
--支线任务
		this.showZhixian ();    
    end;
	
--检查功能开放（倒叙选择能用的打开）
		local opid = -1;
		local opuse = false;
		if zhixiantask.id then 
			opid=2;
			a3_task.openFunc("zhixian");
		else 
			if index == opid then opuse = true; end
			this.getTransform("tab/zhixian").gameObject:SetActive(false);
	    end
		if richangtask.id then
			opid=1;
			a3_task.openFunc("richang");
		else 
			this.getTransform("tab/richang").gameObject:SetActive(false);
			if index == opid then opuse = true; end
	    end
        if maintask.id then
			opid=0;
			a3_task.openFunc("zhuxian");
		else 
			this.getTransform("tab/zhuxian").gameObject:SetActive(false);
			if index == opid then opuse = true; end
	    end
		
		if opid == -1 then
			--所有页签均无效
		elseif opuse then
			--当前选择的页签无效,选择有效的打开
			this.tabControler:setSeletedIndex (opid);
		end
	   
end

--主线任务
a3_task.showZhuxian = function ()
	if(maintask.id) then
		--章节标题、章节提示、章节进度、任务标题、任务目标
		local xml = XmlMgr:GetSXML("task.Task", "id=="..maintask.id);
		cptid = xml:getInt("Chapter_id");
		this.maintitle.text = XmlMgr:GetSXML("task.Chapter", "id=="..cptid):getString("name");
		this.maincontent.text = XmlMgr:GetSXML("task.Chapter", "id=="..cptid):getString("description");
		local cc,cmax = A3TaskModel.getChapterCount(cptid);
		this.mainpgs.text = getCont("mianpgs", { tostring(cc), tostring(cmax)});
		this.mainpgsslider.Value = (cc)/(cmax);
		this.maintargettitle.text = xml:getString("name");
		if maintask.cnt==xml:getInt("target_param1") then
			this.maintarget.text = getCont("mianctt_fin", { xml:getString("target_desc")});
		else
			this.maintarget.text = getCont("mianctt", { xml:getString("target_desc"), tostring(maintask.cnt), xml:getString("target_param1")});
		end
		--章节奖励
		this.clearChild(this.maincptrwctt);
		local cpreward = XmlMgr:GetSXML("task.Cha_gift", "id=="..cptid);
		local list = cpreward:GetNodeList("RewardItem");
		local mmi = list.Count-1;
		for i=0,mmi,1 do 
			local oo = list[i];
			local go = this.newIcon(oo:getString("item_id"),this.getItemXml(oo:getString("item_id")).quality,oo:getInt("value"));	
			go.transform:SetParent( this.maincptrwctt);	
			go.transform.localScale = Vector3(0.6, 0.6, 1);
		end 
		--任务奖励
		this.clearChild(this.maintaskrwctt);
		local tskreward = XmlMgr:GetSXML("task.Task", "id=="..maintask.id);
		local tsklist = tskreward:GetNodeList("RewardValue");
		local tskmmi = tsklist.Count-1;
		for i=0,tskmmi,1 do 
			local oo = tsklist[i];
			local go = this.newIcon("value_"..oo:getString("type"),1,oo:getInt("value"));
			go.transform:SetParent( this.maintaskrwctt);
			go.transform.localScale = Vector3(0.7, 0.7, 1);
		end 
		local pf = PlayerModel:getInstance().profession;
		if pf then
			local tskreward = XmlMgr:GetSXML("task.Task", "id=="..maintask.id);
			tsklist = tskreward:GetNodeList("RewardEqp","carr=="..pf);
			if tsklist then
				tskmmi = tsklist.Count-1;
				for i=0,tskmmi,1 do 
					local oo = tsklist[i];
					local go = this.newIcon(oo:getString("id"),this.getItemXml(oo:getString("id")).quality,0);
					go.transform:SetParent( this.maintaskrwctt);
					go.transform.localScale = Vector3(scale, scale, 1);
				end 
			end
			
		end
		local tskreward = XmlMgr:GetSXML("task.Task", "id=="..maintask.id);
		local tsklist = tskreward:GetNodeList("RewardItem");
		if tsklist then
			local tskmmi = tsklist.Count-1;
			for i=0,tskmmi,1 do 
				local oo = tsklist[i];
				local go = this.newIcon(oo:getString("item_id"),this.getItemXml(oo:getString("item_id")).quality,oo:getInt("value"));
				go.transform:SetParent( this.maintaskrwctt);
				go.transform.localScale = Vector3(0.7, 0.7, 1);
			end 
		end
	end;
	
end
--日常任务
a3_task.showRichang = function ()
	if(richangtask.id) then
		--任务进度、任务标题、任务目标
		local xmlrch = XmlMgr:GetSXML("task.limit_num");
		local xmlrc = XmlMgr:GetSXML("task.Task", "id=="..richangtask.id);
		this.richangpgs.text = getCont("mianpgs", { tostring(richangtask.cnt), xmlrch:getString("dailytask")});
		this.richangpgsslider.Value = (richangtask.cnt)/xmlrch:getInt("dailytask");
		this.richangtargettitle.text = xmlrc:getString("name");
		this.richangtarget.text  = xmlrc:getString("target_desc");
		--额外奖励
		this.clearChild(this.richangexrwctt);
		local cpreward = XmlMgr:GetSXML("task.Task", "id=="..richangtask.id);
		local exid = cpreward:getString("extra_award");
		local cpreward = XmlMgr:GetSXML("task.extra", "id=="..exid);
		local list = cpreward:GetNodeList("RewardItem");
		local mmi = list.Count-1;
		for i=0,mmi,1 do 
			local oo = list[i];
			local go = this.newIcon(oo:getString("item_id"),this.getItemXml(oo:getString("item_id")).quality,oo:getInt("value"));	
			go.transform:SetParent( this.richangexrwctt);	
			go.transform.localScale = Vector3(0.8, 0.8, 1);
		end 
		--任务奖励
		this.clearChild(this.richangtaskrwctt);
		local tskreward = XmlMgr:GetSXML("task.Task", "id=="..richangtask.id);
		local tsklist = tskreward:GetNodeList("RewardValue");
		local tskmmi = tsklist.Count-1;
		for i=0,tskmmi,1 do 
			local oo = tsklist[i];
			local go = this.newIcon("value_"..oo:getString("type"),1,oo:getInt("value"));
			go.transform:SetParent( this.richangtaskrwctt);
			go.transform.localScale = Vector3(0.8, 0.8, 1);
		end 
		local pf = PlayerModel:getInstance().profession;
		if pf then
			local tskreward = XmlMgr:GetSXML("task.Task", "id=="..richangtask.id);
			tsklist = tskreward:GetNodeList("RewardEqp","carr=="..pf);
			if tsklist then
				tskmmi = tsklist.Count-1;
				for i=0,tskmmi,1 do 
					local oo = tsklist[i];
					local go = this.newIcon(oo:getString("id"),this.getItemXml(oo:getString("item_id")).quality,0);
					go.transform:SetParent( this.richangtaskrwctt);
					go.transform.localScale = Vector3(scale, scale, 1);
				end 
			end
			
		end
		--奖励星数
		for i=0, this.richangstar.transform.childCount-1, 1 do
			local go = this.richangstar.transform:GetChild(i).gameObject;
			if i<richangtask.star then 
				go.transform:FindChild("Image_on"):SetAsLastSibling();
			else go.transform:FindChild("Image_on"):SetAsFirstSibling();
			end
		end
	end;
end
--支线任务
a3_task.showZhixian = function ()
	if(zhixiantask.id) then
		
	end;
end

a3_task.openFunc = function(tabname)
	this.getTransform("content").gameObject:SetActive(true);
	this.getTransform("tab/"..tabname).gameObject:SetActive(true);
end

--主线前往
a3_task.onZhuxianMove = function(go)
	--this.invoke("TTT",a3_task.TTT,1,0.02);
end;
local i = 0;
a3_task.TTT = function(go)
    log(tostring(i)); 	
	i=i+1;		
end;

--日常一键完成
a3_task.onRCOnekeyFinish = function(go)
   A3TaskProxy:SendFinAll ();
   --this.invokeCancel("TTT");
   i=0;
end;

--日常一键满星
a3_task.onRCOnekeyStar = function(go)
     A3TaskProxy:SendReStar ();
end;