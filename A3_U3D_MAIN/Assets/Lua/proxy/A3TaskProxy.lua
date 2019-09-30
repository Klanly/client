require "proxy/BaseProxy"
require "model/A3TaskModel"
A3TaskProxy = BaseProxy:new ();
local this = A3TaskProxy;

function this:init ()
    this:addProxyListener (PKG_NAME.A3TASK, this.OP);


end;
--ȫ����Ϣ
this.SendGetTask = function()
    v = variant();
    v:setValue("mis_cmd", 1);
    this:sendRPC(PKG_NAME.A3TASK, v);
end;
--�ճ�ˢ��
this.SendReStar = function()
    v = variant();
    v:setValue("mis_cmd", 7);
    this:sendRPC(PKG_NAME.A3TASK, v);
end;
--�ճ�һ�����
this.SendFinAll = function()
    v = variant();
    v:setValue("mis_cmd", 6);
    this:sendRPC(PKG_NAME.A3TASK, v);
end;

--�ظ�
this.OP = function(v)
    log (v:dump ());
    if v:ContainsKey("res") then
		local i = v:getValue("res")._int;
		if i<0 then
           
        end;
    end;
	--�������
    if v:ContainsKey("mlmis") and v:getValue("mlmis").Count>0  then
		local mlmis = v:getValue("mlmis");
		maintask.id = mlmis:getValue("id")._int;
		maintask.cnt = mlmis:getValue("cnt")._int;
	end;
	--���֧��
	if v:ContainsKey("dmis") and v:getValue("dmis").Count>0 then
		local dmis = v:getValue("dmis");
		richangtask.dmiscount = dmis:getValue("dmis_count")._int;
		if richangtask.dmiscount==0 then
			--�ճ�����ȫ�����
			richangtask.id = nil;
		else 
			richangtask.id = dmis:getValue("id")._int;
			richangtask.cnt = dmis:getValue("cnt")._int;
			richangtask.star = dmis:getValue("star")._int;
		end
		
		
	end;
	--������������
	if v:ContainsKey("change_task") and v:getValue("change_task").Count>0 then
		
		local list = v:getValue("change_task")._arr;
		local mmi = list.Count-1;
		for i=0,mmi,1 do 
			local oo = list[i];
			
			if maintask.id and maintask.id==oo:getValue("id")._int then
				maintask.cnt = oo:getValue("cnt")._int;
			elseif richangtask.id and richangtask.id==oo:getValue("id")._int then
				richangtask.cnt = oo:getValue("cnt")._int;
				if oo:ContainsKey("star") then
				richangtask.star = oo:getValue("star")._int; end
			elseif zhixiantask.id and zhixiantask.id==oo:getValue("id")._int then
				
			end

		end  
		
	end;
	--�Ƴ�
	if v:getValue("res")._int==2 then 
		if maintask.id and maintask.id==v:getValue("id")._int then
			maintask.id = nil;
		elseif richangtask.id and richangtask.id==v:getValue("id")._int then
			richangtask.id = nil;
		elseif zhixiantask.id and zhixiantask.id==v:getValue("id")._int then
				
		end
	end
	
	a3_task:OnRefresh();
    
end;