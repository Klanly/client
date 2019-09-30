A3TaskModel = Base:new( { });
local this = A3TaskModel;

maintask = {};
richangtask = {};
zhixiantask = {};

this.getChapterCount = function(cptid)
	if(maintask.id) then
		local si = 0;
		local xml = XmlMgr:GetSXML("task");
		local list = xml:GetNodeList("Task","Chapter_id=="..cptid);
		local mmi = list.Count-1;
		for i=0,mmi,1 do 
			local oo = list[i];
			if oo:getInt("id")<maintask.id then si = si+1 end
		end  
		return si,list.Count;
	end;
end;


