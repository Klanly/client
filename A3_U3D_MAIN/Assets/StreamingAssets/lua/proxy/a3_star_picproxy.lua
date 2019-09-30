require "proxy/BaseProxy"
a3_star_picproxy = BaseProxy:new ();
local this = a3_star_picproxy;
local lvl={0};
function this:init ()
--    a3_star_picproxy.getInstacne ()=this;
 
    this:addProxyListener (PKG_NAME.S2C_STAR_POS, this.star_pos);
   
end;

function this:sendstar_pos(vl)
    v = variant ();
    v:setValue ("op", vl);
    this:sendRPC (PKG_NAME.C2S_STAR_POS, v);
  
end;

function this:sendstar_open(vl)
    v = variant ();
    v:setValue ("op", 5);
    v:setValue("id",vl)
    this:sendRPC (PKG_NAME.C2S_STAR_POS, v);
    a3_star_picproxy:getInstance():sendstar_pos(2);
end;

function this:sendopen_pic(x,y)
    v = variant ();
    v:setValue ("op", 1);
    v:setValue("x",x);
    v:setValue("y",y);
    this:sendRPC (PKG_NAME.C2S_STAR_POS, v);

    a3_star_picproxy:sendstar_pos(2);
end;
function this.star_pos(data)
    --log(data:dump());
    local res =data:getValue("res")._int;
    if(res < 0)then
   
    end   
   
   
    if(res==1)then
        if(data:ContainsKey("vip"))then
            a3_star_pic.vip=data:getValue("vip")._int;
            a3_star_pic.instance.buy(data);
        end;
        if(data:ContainsKey("lvl"))then  
           
            if(lvl[0]~=data:getValue("lvl")._int)then
                lvl[0]= data:getValue("lvl")._int;
                a3_star_pic.instance.star_lvl(data)
            end;
        end;
        if(data:ContainsKey("map"))then
            a3_star_pic.instance.child_pos(data);
        end;
        if(data:ContainsKey("active_step"))then
            a3_star_pic.instance.star_active(data);
        end; 
        if(data:ContainsKey("att_msg"))then
            a3_star_pic.instance.Attribute_add(data);
        end;
        if(data:ContainsKey("foresee"))then
            if(data:getValue("foresee")._arr~=nil)then
                a3_star_pic.instance.foresee(data);

            end;
        end;
    elseif(res==2)then--行动力改变
        if(data:ContainsKey("active_step"))then
            a3_star_pic.instance.star_active(data);
        end;   
    elseif(res==3)then--行解锁
        if(data:ContainsKey("map"))then
            a3_star_pic.instance.child_pos(data);
        end;
    elseif(res==4)then--列解锁
       -- logError(data:dump());
      --  a3_star_pic.instance. star_unlock(data)
    elseif(res==5)then--四周解锁
    elseif(res==6)then--解锁全部
    elseif(res==7)then--预报位置
        if(data:ContainsKey("foresee"))then
            if(data:getValue("foresee")._arr~=nil)then
                a3_star_pic.instance.foresee(data);

            end;
        end;
    elseif(res==8)then--属性变化
        if(data:ContainsKey("att_msg"))then
            a3_star_pic.instance.Attribute_add(data);
        end;
    elseif(res==9)then--属性变化  
        a3_star_pic.instance.buy(data);
    end;

end;
