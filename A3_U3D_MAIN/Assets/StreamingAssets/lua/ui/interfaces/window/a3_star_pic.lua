require"proxy/a3_star_picproxy";

a3_star_pic = BaseLayer:new ();
local this = a3_star_pic;
local listView;
local view_Parent;
local Tran={};
local pos={};
local star_pos={};
local activepow_v=0;
local can_open=false;
local xx;
local yy;
local buy_time;
local can_buy;
local  vip;
local first_open=false;
local pic_xml={};
this.events={};
local needmoney;
local score;
local score_now;
a3_star_pic.init = function ()
    this.alain();
    listView = this.transform:FindChild ("gezi_scroll/scroll_view/contain").gameObject;
    view_Parent = this.getComponentByPath("gezi_scroll/scroll_view/contain","GridLayoutGroup");
    this.transform:FindChild("full_panel").gameObject:SetActive(false);
    this.addClick (this.transform:FindChild ("btn/help_btn").gameObject, this.onHelpClick);
    this.addClick (this.transform:FindChild ("help/btn").gameObject, this.onCloseHelpClick);
    this.addClick (this.transform:FindChild ("btn/close_btn").gameObject, this.onCloseClick);
    this.addClick (this.transform:FindChild ("btn/active_btn").gameObject,this.onActiveClick);--活动力及提示购买按钮；
    this.addClick(this.transform:FindChild("active_buy/close").gameObject,this.onclosebuy);
    this.addClick(this.transform:FindChild("active_buy/buy_btn").gameObject,this.onBuy_active);--购买活动力

    this.addClick(this.transform:FindChild("bg_right/events/event_15").gameObject,this.onEvent_15);
    this.addClick(this.transform:FindChild("bg_right/events/event_30").gameObject,this.onEvent_30);
    this.addClick(this.transform:FindChild("bg_right/events/event_45").gameObject,this.onEvent_45);
    this.addClick(this.transform:FindChild("sjjs_panel/yes_btn").gameObject,this.onEventClick);
    this.addClick(this.transform:FindChild("sjjs_panel/no_btn").gameObject,this.onEvent_no);
    a3_star_picproxy:getInstance ()

    this.transform:FindChild("tishi").gameObject:SetActive(false);
end
a3_star_pic.onShowed = function (prama)
    a3_star_pic.instance=this;

    first_open=false;
    a3_star_picproxy:getInstance():sendstar_pos(2);
   this.GAME_CAMERA(false);
end;
a3_star_pic.onEvent_15=function()
    this.transform:FindChild("sjjs_panel").gameObject:SetActive(true);
    score=XmlMgr:GetSXML("star_map.score_events","id=="..1):getInt("scores");

end;

a3_star_pic.onEvent_30=function()
    this.transform:FindChild("sjjs_panel").gameObject:SetActive(true);
    score=XmlMgr:GetSXML("star_map.score_events","id=="..2):getInt("scores");

end;
a3_star_pic.onEvent_45=function()
    this.transform:FindChild("sjjs_panel").gameObject:SetActive(true);
    score=XmlMgr:GetSXML("star_map.score_events","id=="..3):getInt("scores");
end;





a3_star_pic.onEventClick=function()
    if(score>score_now)then
        flytxt.fly(getCont("star_noscore", {""}));
        this.transform:FindChild("sjjs_panel").gameObject:SetActive(false);
        return;

    end;

    if(score==15)then
        a3_star_picproxy:getInstance():sendstar_open(1);
    elseif(score==30)then
        a3_star_picproxy:getInstance():sendstar_open(2);
    elseif(score==45)then
        a3_star_picproxy:getInstance():sendstar_open(3);
    end;

    this.transform:FindChild("sjjs_panel").gameObject:SetActive(false);
end;
a3_star_pic.onEvent_no=function()
    this.transform:FindChild("sjjs_panel").gameObject:SetActive(false);
end;

a3_star_pic.pic_bg=function(i)
    local xml=XmlMgr:GetSXML("star_map.tile","id=="..i);
    local order=xml:getString("order");
    
    pic_xml=this.Split(order);
   
   
end;

a3_star_pic.destory=function()
--    local con_x = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.x;
--    local con_y = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y;
--    local x = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.x;
--    local y = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.y;
--    local s=(con_x/x)*(con_y/y);
    local s=listView.transform.childCount;
    for i= 0,s-1 do       
        if(listView.transform.childCount> 0) then
            GameObject.DestroyImmediate(listView.transform:GetChild(0).gameObject);
          
        end
    end

end;


 a3_star_pic.onHelpClick = function (go)
    this.transform:FindChild ("help").gameObject:SetActive (true);
end;
a3_star_pic.onCloseHelpClick = function (go)
    this.transform:FindChild ("help").gameObject:SetActive (false);
end;
a3_star_pic.onCloseClick = function (go)
    a3_star_pic.instance=nil;
    InterfaceMgr.close (InterfaceMgr.A3_STAR_PIC)
  
end;

a3_star_pic.onClosed = function ()
   this.GAME_CAMERA(true);
end
a3_star_pic.onActiveClick = function (go)--活动力及提示购买按钮
    this.transform:FindChild("active_buy").gameObject:SetActive(true);
    local text= this.transform:FindChild("active_buy/can_buy/text"):GetComponent("Text");
    local buy_btn=this.transform:FindChild("active_buy/buy_btn/sum"):GetComponent("Text");
    local xml=XmlMgr:GetSXML("vip.viplevel","vip_level=="..vip);
    local list = xml:GetNode("vt","type=="..20);
    can_buy=list:getInt("value");
    local  xml=XmlMgr:GetSXML("star_map.act");
    local default_price=xml:getInt("default_price");
    local price_rise=xml:getInt("price_rise");
    text.text=getCont("star_active_buy",{tostring(buy_time),tostring(can_buy)});
    buy_btn.text=tostring(default_price+price_rise*buy_time);
    needmoney=default_price+price_rise*buy_time;
end;

a3_star_pic.onclosebuy=function()
    this.transform:FindChild("active_buy").gameObject:SetActive(false);
end;
a3_star_pic.onBuy_active=function()
   
   
    if(buy_time>=can_buy)then
        flytxt.fly(getCont("star_active_y", {""}));
    else
        if(PlayerModel:getInstance().gold<needmoney)then
            flytxt.fly(getCont("star_nocoin", {""}));
        else
            a3_star_picproxy:getInstance():sendstar_pos(4);
        end;
    end;

end;


a3_star_pic.clear_att_info=function()
    local s= this.transform:FindChild("bg_right/shuxing/info").transform.childCount;   
    for i= 0,s-1,1 do   
        local go=this.transform:FindChild("bg_right/shuxing/info");
    
        if(go.transform.childCount> 0) then
            GameObject.DestroyImmediate(go.transform:GetChild(0).gameObject);
         
        end
    end
end;

a3_star_pic.Attribute_add = function (data)--属性相关的函数；
    a3_star_pic.clear_att_info();

    local info = data:getValue("att_msg")._arr;
    local att_prefab=this.transform:FindChild("bg_right/shuxing/0");
    local len=info.Count;
    for i=0,len-1,1 do
        local go = GameObject.Instantiate (att_prefab);
        go.gameObject:SetActive(true);
        go.transform:SetParent (this.transform:FindChild("bg_right/shuxing/info"));
        go.transform.localScale = Vector3(1, 1, 1);
 
        local text=go.transform:GetComponent("Text");
        text.text= XmlMgr:GetSXML("star_map.att", "att_type=="..info[i]:getValue("att_type")._int):getString("att_name")..": +"..info[i]:getValue("att_value")._int;
       
      
    end;   
end;
a3_star_pic.star_lvl=function(data)
   
    local lvl=this.transform:FindChild("bg_right/starpic_dj/Text"):GetComponent("Text");
    if(data:getValue("lvl")._int>500)then
        lvl.text=500;
        this.transform:FindChild("full_panel").gameObject:SetActive(true);
        return;
    end;
    lvl.text=data:getValue("lvl")._int;
    vip=data:getValue("vip")._int;
  
    buy_time=data:getValue("buy_time")._int;

    if(data:ContainsKey("socre"))then
       score_now=data:getValue("socre")._int;      
    end;


--   this.events={};
    local info = data:getValue("map")._arr;
    for i= 0, info.Count-1 do       
        -- event[i]=info[i]:getValue("events")._int
        table.insert(this.events,i,info[i]:getValue("events")._int);
       -- logError("sss"..this.events[i]);
    end;

    local p=(data:getValue("lvl")._int)%10+1;
    a3_star_pic.pic_bg(p);
    --  goo=nil;
    local star_lvl=data:getValue("lvl")._int;
   

    local  xml=XmlMgr:GetSXML("star_map.info","level=="..star_lvl);
      local  xx=xml:getInt("sum_x");
      local  yy=xml:getInt("sum_y");
    a3_star_pic.instance.UnLock_Size(xx,yy);
-- a3_star_pic.instance.UnLock_Size(xx,yy);
   
end;
a3_star_pic.buy=function(data)
    buy_time=data:getValue("buy_time")._int;
    if(data:ContainsKey("vip"))then
        vip=data:getValue("vip")._int;
    end;

    local text= this.transform:FindChild("active_buy/can_buy/text"):GetComponent("Text");
    local buy_btn=this.transform:FindChild("active_buy/buy_btn/sum"):GetComponent("Text");
    local xml=XmlMgr:GetSXML("vip.viplevel","vip_level=="..vip);
    local list = xml:GetNode("vt","type=="..20);
    can_buy=list:getInt("value");
    local  xml=XmlMgr:GetSXML("star_map.act");
    local default_price=xml:getInt("default_price");
    local price_rise=xml:getInt("price_rise");
    text.text=getCont("star_active_buy",{tostring(buy_time),tostring(can_buy)});
    buy_btn.text=tostring(default_price+price_rise*buy_time);

end;

a3_star_pic.star_active=function(data)
    local active_v=this.transform:FindChild("btn/active_btn/Text"):GetComponent("Text");
    active_v.text=getCont("star_active", {tostring(data:getValue("active_step")._int)});
    activepow_v=data:getValue("active_step")._int;
--    logError(active_v);
end;

a3_star_pic.UnLock_Size = function (x, y)--解锁图 确定覆盖图的格子的大小
    local con_x = this.getComponentByPath ("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.x;
    local con_y =  this.getComponentByPath ("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y;
   
   -- local view_P= this.transform:FindChild ("gezi_scroll/scroll_view/contain").gameObject;--.cellSize;
    this.Grild_cellSize("gezi_scroll/scroll_view/contain",con_x/x, con_y/y);

    local v= this.getComponentByPath ("gezi_scroll/scroll_view/gezi","RectTransform");
    v.sizeDelta= Vector3(con_x/x, con_y/y,0);

    local v1=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/hui","RectTransform");
    v1.sizeDelta= Vector3 (con_x/x, con_y/y,0);
   
    local v2=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/green","RectTransform");
    v2.sizeDelta= Vector3 (con_x/x, con_y/y,0);
    local v3=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/null","RectTransform");
    v3.sizeDelta= Vector3 (con_x/x, con_y/y,0);

    local v4=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/animator","RectTransform");
    v4.sizeDelta= Vector3 (con_x/x, con_y/y,0);

    local v5=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/item","RectTransform");
    v5.sizeDelta= Vector3 (con_x/x, con_y/y,0);
    local v41=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/animator/image","RectTransform");
    v41.sizeDelta= Vector3 (con_x/x, con_y/y,0);
--    local v51=this.getComponentByPath ("gezi_scroll/scroll_view/gezi/animator2/image","RectTransform");
--    v51.sizeDelta= Vector3 (con_x/x, con_y/y,0);

    a3_star_pic.Can_UnLock();

end;

a3_star_pic.Can_UnLock = function()
    a3_star_pic.destory();

    local con_x = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.x;
    local con_y = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y;
    local x = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.x;
    local y = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.y;
    local s=(con_x/x)*(con_y/y);

    local num =s; --view_Parent.transform.childCount;
    local obj_prefab;
    obj_prefab = this.transform:FindChild ("gezi_scroll/scroll_view/gezi").gameObject;
 
    for i=0, num-1, 1 do      
        local go = GameObject.Instantiate (obj_prefab);
        go.transform:SetParent (this.transform:FindChild("gezi_scroll/scroll_view/contain"));
        go.transform.localScale = Vector3(1, 1, 1);
        local icon= go.transform:FindChild("green"):GetComponent("Image");
       
        icon.sprite = this.loadSprite("icon_tile_"..tostring(pic_xml[i]));
        go:SetActive(true);      
        this.addClick(go,this.onPrefabClick);      
    end
   
end;
a3_star_pic.onPrefabClick=function(go)
    if(activepow_v<=0 and go.transform:FindChild("hui").gameObject.activeSelf==false and go.transform:FindChild("green").gameObject.activeSelf==true)then
        flytxt.fly(getCont("star_active_v", {""}));
        return;
    end;
   
    if(can_open==true)then
        if( go.transform:FindChild("hui").gameObject.activeSelf==true or go.transform:FindChild("null").gameObject.activeSelf==true)then      
        return;
        end;
    end; 
  
    this.soundplay("star_pic_unlock");
    first_open=true;
    --local con_x = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.x;
    local con_y = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y;
     -- local x = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.x;
        local y = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.y;
        for i=0, listView.transform.childCount-1, 1 do
        if(go.transform.position==listView.transform:GetChild(i).transform.position)then
            local xx;
            local yy;
            xx=math.floor(i/(con_y/y));           
            if(xx<0)then xx=0;end;
            yy=i-(xx*(con_y/y));          
            if(yy<0)then yy=0;end; 
           
            a3_star_picproxy:getInstance():sendopen_pic(xx,yy);
            if( go.transform:FindChild("animator"))then
                go.transform:FindChild("animator").gameObject:SetActive(true);
                GameObject.Destroy(go.transform:FindChild("animator").gameObject,1);
              --  first_open=false;
            end;
        end;

   

    end;

  
end;

a3_star_pic.foresee=function(data)

    local con_x = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.x;
    local con_y = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y;
    local x = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.x;
    local y = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.y;
      local s=(con_x/x)*(con_y/y);


    local info=data:getValue("foresee")._arr;
    if(info==nil)then
        return;
    end;
    for i=0,s-1 do
        if(listView.transform:GetChild(i).transform:FindChild("item"))then
            listView.transform:GetChild(i).transform:FindChild("item").gameObject:SetActive(false); 
        end;
    end;

    local len = info.Count;
   
    --local default_price=xml:getInt("default_price");
    for i= 0, len-1 do
        local xx=  info[i]:getValue("x")._int;
        local yy=info[i]:getValue("y")._int;
        local event=info[i]:getValue("event_id")._int;
        local  xml=XmlMgr:GetSXML("star_map.events","id=="..event);
        local item_id=xml:getInt("p1");
        local  step=info[i]:getValue("step")._int;
       

        local  j=xx*(con_y/y)+yy
        if(listView.transform:GetChild(j).transform:FindChild("item"))then
        listView.transform:GetChild(j).transform:FindChild("item").gameObject:SetActive(true); 
        local icon = listView.transform:GetChild(j).transform:FindChild("item"):GetComponent("Image");
        icon.sprite = this.loadSprite("icon_item_"..item_id);
        local num=listView.transform:GetChild(j).transform:FindChild("item/Text"):GetComponent("Text");
        num.text=step;
        end;
    end;


end
a3_star_pic.child_pos=function(data)
    can_open=false;   
    this.ui_unshow();
    if(data:ContainsKey("socre"))then
        score_now=data:getValue("socre")._int; 
        local text=this.transform:FindChild("bg_right/jifen/Text"):GetComponent("Text");
        text.text= score_now;    
    end;
    local info = data:getValue("map")._arr;
    local len = info.Count;
     
        for i= 0, len-1 do
        listView.transform:GetChild(i).transform:FindChild("hui").gameObject:SetActive(true);
        listView.transform:GetChild(i).transform:FindChild("green").gameObject:SetActive(true);
        listView.transform:GetChild(i).transform:FindChild("null").gameObject:SetActive(false);
        listView.transform:GetChild(i).transform:FindChild("click_hint").gameObject:SetActive(false);
        listView.transform:GetChild(i).transform:FindChild("event").gameObject:SetActive(false);
       
    end;
   

    for i= 0, len-1 do
       
        if(info[i]:getValue("events")._int==0)then
            can_open=true;
            --show_secend=true;

           
            listView.transform:GetChild(i).transform:FindChild("hui").gameObject:SetActive(false);
            listView.transform:GetChild(i).transform:FindChild("green").gameObject:SetActive(false);
            listView.transform:GetChild(i).transform:FindChild("click_hint").gameObject:SetActive(false);
            listView.transform:GetChild(i).transform:FindChild("null").gameObject:SetActive(true);
            if( listView.transform:GetChild(i).transform:FindChild("animator")and first_open==true)then
                listView.transform:GetChild(i).transform:FindChild("animator").gameObject:SetActive(true);
                GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("animator").gameObject,1);
            end;
            if(listView.transform:GetChild(i).transform:FindChild("att_show")and listView.transform:GetChild(i).transform:FindChild("events_show")and first_open==true)then
                listView.transform:GetChild(i).transform:FindChild("att_show").gameObject:SetActive(true);
                listView.transform:GetChild(i).transform:FindChild("events_show").gameObject:SetActive(true);

              
                local text=listView.transform:GetChild(i).transform:FindChild("att_show").transform:GetComponent("Text");
                local xml= XmlMgr:GetSXML("star_map.att", "att_type=="..info[i]:getValue("att")._int)
                text.text=xml:getString("att_name")..": +"..xml:getString("att_value");
          
                local pic= XmlMgr:GetSXML("star_map.events", "type=="..this.events[i]):getString("icon")
                local icon = listView.transform:GetChild(i).transform:FindChild("events_show"):GetComponent("Image");
                icon.sprite = this.loadSprite(pic);
  
                GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("events_show").gameObject,1);
                GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("att_show").gameObject,1);
            end;

            if(listView.transform:GetChild(i).transform:FindChild("att_show")and listView.transform:GetChild(i).transform:FindChild("events_show")and first_open==false)then
                GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("events_show").gameObject);
                GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("att_show").gameObject);
            end;


            if( listView.transform:GetChild(i).transform:FindChild("animator")and first_open==false)then            
                GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("animator").gameObject);
             
            end;

            if(listView.transform:GetChild(i).transform:FindChild("item"))then
            GameObject.Destroy(listView.transform:GetChild(i).transform:FindChild("item").gameObject);
            end;


            local ss_x=(info[i]:getValue("x")._int);
            local ss_y=(info[i]:getValue("y")._int);
            for j= 0, len-1 do
                local gg_x=info[j]:getValue("x")._int;
                local gg_y=info[j]:getValue("y")._int;
                if((ss_x==gg_x and math.abs(ss_y-gg_y)==1)or (ss_y==gg_y and math.abs(ss_x-gg_x)==1))then
                    if(listView.transform:GetChild(j).transform:FindChild("null").gameObject.activeSelf==false)then                      
                        listView.transform:GetChild(j).transform:FindChild("hui").gameObject:SetActive(false);
                        listView.transform:GetChild(j).transform:FindChild("green").gameObject:SetActive(true);
                        listView.transform:GetChild(j).transform:FindChild("click_hint").gameObject:SetActive(true);
                    end;
                end;
            end;
        --listView.transform:GetChild(i).transform:FindChild("null").gameObject:SetActive(true);
        else           
            if(listView.transform:GetChild(i).transform:FindChild("hui").gameObject.activeSelf==true)then
                listView.transform:GetChild(i).transform:FindChild("hui").gameObject:SetActive(true);
                listView.transform:GetChild(i).transform:FindChild("green").gameObject:SetActive(true);
                listView.transform:GetChild(i).transform:FindChild("null").gameObject:SetActive(false);
            --  listView.transform:GetChild(i).transform:FindChild("click_hint").gameObject:SetActive(false);

            --   local pic= XmlMgr:GetSXML("star_map.events", "type=="..this.events[i]):getString("icon")
               
            end;
          
        end;

    end;
   
    if(can_open==false)then       
        for i= 0, len-1 do
            listView.transform:GetChild(i).transform:FindChild("hui").gameObject:SetActive(false);
            listView.transform:GetChild(i).transform:FindChild("green").gameObject:SetActive(true);
            listView.transform:GetChild(i).transform:FindChild("null").gameObject:SetActive(false);
        end;
        this.transform:FindChild("tishi").gameObject:SetActive(true);
    else

        if( this.transform:FindChild("tishi").gameObject.activeSelf==true)then
            this.transform:FindChild("tishi").gameObject:SetActive(false);
        end;

    end;
    for i= 0, len-1 do
        if(this.events[i]<7 and this.transform:FindChild("tishi").gameObject.activeSelf==false and info[i]:getValue("events")._int~=0)then             
        local icon = listView.transform:GetChild(i).transform:FindChild("event"):GetComponent("Image");
        icon.sprite = this.loadSprite("icon_events_"..this.events[i]);
        listView.transform:GetChild(i).transform:FindChild("event").gameObject:SetActive(true);
        end;

    end;

end

a3_star_pic.star_unlock=function(data)
--    local con_y = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y; 
--    local y = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.y;
    local res=data:getValue("way")._int;
    
    if(res==1)then
      local  x=data:getValue("x")._int;
      local  y=data:getValue("y")._int;
      --  a3_star_pic.set_starboom(x,y)
       



    elseif(res==2)then
    elseif(res==3)then
    elseif(res==4)then

    end;
end;

a3_star_pic.set_starboom=function(x,y)
-- this.wait()
   
    local con_x = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.x; 
    local con_y = this.getComponentByPath("gezi_scroll/scroll_view/contain","RectTransform").sizeDelta.y; 
    local xx = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.x;
    local yy = this.getComponentByPath("gezi_scroll/scroll_view/gezi/hui","RectTransform").sizeDelta.y;
    local k=x*(con_y/yy)+y;
    local s=(con_x/xx)*(con_y/yy);

    for i=0,(con_x/xx)-1 do
        local  j=k+i*(con_y/yy)
        if(j<s)then
            if( listView.transform:GetChild(j).transform:FindChild("animator"))then
           
     
                listView.transform:GetChild(j).transform:FindChild("animator").gameObject:SetActive(true);
                GameObject.Destroy(listView.transform:GetChild(j).transform:FindChild("animator").gameObject,2*i);   

                      
            end;
       end;
    end;


    if(x-1>=0)then

        a3_star_pic.set_starboom(x-1,y)
    end;
    this.wait()
    flytxt.fly(getCont("star_active_v", {""}));
end;

