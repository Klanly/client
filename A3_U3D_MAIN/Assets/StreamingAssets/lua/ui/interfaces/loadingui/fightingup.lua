fightingup = BaseLayer:new();
local this = fightingup;

fightingup.init = function()
   this.instance = this;
   this.speech = 0;
   this.curtxt = 0;
   this.totxt = 0;
   this.info = this.transform:FindChild("info").gameObject;
   this.txt = this.getComponentByPath("info/txt", "Text");
   this.isInUpdata = false; --是否在update中
   this.isUpdata = true;
   this.isEnd = false;
   this.time = 2;
end

fightingup.onShowed = function(prama)
	this.info:SetActive(false);
end
fightingup.onClose = function(prama)
end

fightingup.runTxt = function (oldtxt, newtxt)
	
	this.transform:SetAsFirstSibling();
	this.info:SetActive(true);
	this.curtxt = oldtxt;
	this.totxt = newtxt;
	speech = math.ceil((newtxt - oldtxt) / 50);
	
	if this.isInUpdata == false then
		UpdateBeat:Add(fightingup.onUpdate, this);
	end
	this.isUpdata = true;
	this.isInUpdata = true;
end

fightingup.onUpdate = function()
	if this.isUpdata == true then
		if this.curtxt < this.totxt then
			this.curtxt = this.curtxt + speech;
 			this.txt.text = tostring(this.curtxt);
		else
			this.isUpdata = false;
			this.txt.text = tostring(this.totxt);
			this.time = 2;
			this.isEnd = true;
		end
	end
	if this.isEnd == true then 
		this.time = this.time - Time.deltaTime;
		if this.time <= 0 then
			this.isEnd = false;
			this.onEnd();
		end
	end
end

fightingup.onEnd = function ()
	this.info:SetActive(false);
	this.isInUpdata = false;
	UpdateBeat:Remove(this.onUpdate, this);
end