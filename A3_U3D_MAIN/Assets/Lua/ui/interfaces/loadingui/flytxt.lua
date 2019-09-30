flytxt = BaseLayer:new();
local this = flytxt;

local  COMMON_TYPE = 1;

flytxt.init = function()
    this.m_num = {0,0,0,0,0};
    this.m_time = {0.2,0.2,0.2,0.2,0.4};
    this.m_txtmap = {};
    for i= 1,4 do
        local txt_list = {};
        this.m_txtmap[i] = txt_list;
    end
end

flytxt.fly = function(txt, tag, color)
    tag = tag or 0;
    color = color or nil;

    tag = tag + 1;
    if(this.m_num[tag] > 0 and this.m_time[tag] > 0) then
        this.m_txtmap[tag] = txt;
    end

    if(tag == 1) then
        this.fly0(txt);
    elseif(tag == 2) then
        this.fly1(txt);
    elseif(tag == 3) then
        this.fly2(txt);
    elseif(tag == 4) then
        this.fly3(txt);
    elseif(tag == 5) then
        this.fly4(txt, color);
    end
    this.m_num[tag] = this.m_num[tag] + 1;
end

flytxt.onEnd = function(go)
    GameObject.Destroy(go,3);
end

flytxt.fly0 = function(txt)
    local item = this.transform:FindChild("txt_1").gameObject;
    local txtclone = GameObject.Instantiate(item);
    txtclone:SetActive(true);
    txtclone.transform:SetParent(this.transform, false);

    local desc_txt = txtclone.transform:FindChild("txt"):GetComponent("Text");
    desc_txt.text = txt;
    --到时加DoTween
    this.onEnd(txtclone);
end

flytxt.fly1 = function(txt)
    local item = this.transform:FindChild("txt_1").gameObject;
    local txtclone = GameObject.Instantiate(item);
    txtclone:SetActive(true);
    txtclone.transform:SetParent(this.transform, false);

    local desc_txt = txtclone.transform:FindChild("txt"):GetComponent("Text");
    desc_txt.text = txt;
    --到时加DoTween
    this.onEnd(txtclone);
end

flytxt.fly2 = function(txt)
    local item = this.transform:FindChild("txt_1").gameObject;
    local txtclone = GameObject.Instantiate(item);
    txtclone:SetActive(true);
    txtclone.transform:SetParent(this.transform, false);

    local desc_txt = txtclone.transform:FindChild("txt"):GetComponent("Text");
    desc_txt.text = txt;
    desc_txt.fontSize = 30;
    desc_txt.color = Color.green;
    txtclone.transform:FindChild("bg").gameObject:SetActive(false);
    --到时加DoTween
    this.onEnd(txtclone);
end

flytxt.fly3 = function(txt)
    local item = this.transform:FindChild("txt_2").gameObject;
    local txtclone = GameObject.Instantiate(item);
    txtclone:SetActive(true);
    txtclone.transform:SetParent(this.transform, false);

    local desc_txt = txtclone.transform:FindChild("txt"):GetComponent("Text");
    desc_txt.text = txt;
    --到时加DoTween
    this.onEnd(txtclone);
end

flytxt.fly4 = function(txt, color)
    local item = this.transform:FindChild("txt_3").gameObject;
    local txtclone = GameObject.Instantiate(item);
    txtclone:SetActive(true);
    txtclone.transform:SetParent(this.transform, false);

    local desc_txt = txtclone.transform:FindChild("txt"):GetComponent("Text");
    desc_txt.text = txt;
    if(color ~= nil) then
        desc_txt.color = color;
    end
    --到时加DoTween
    this.onEnd(txtclone);
end


