
-- 输出日志--
function log(str)
    Util.Log(str);
end

-- 错误日志--
function logError(str)
    Util.LogError(str);
end

-- 警告日志--
function logWarn(str)
    Util.LogWarning(str);
end

-- 查找对象--
function find(str)
    return GameObject.Find(str);
end

function destroy(obj)
    GameObject.Destroy(obj);
end

function newObject(prefab)
    return GameObject.Instantiate(prefab);
end

-- 创建面板--
function createPanel(name)
    PanelManager:CreatePanel(name);
end

function variant()
    return NetManager:newVariant();
end



function newGameObject(name)
    PanelManager:newGameObject(name);
end

function resLoad(pkgname, name)
    return PanelManager:resLoad(pkgname, name);
end


function child(str)
    return transform:FindChild(str);
end

function subGet(childNode, typeName)
    return child(childNode):GetComponent(typeName);
end

function findPanel(str)
    local obj = find(str);
    if obj == nil then
        error(str .. " is null");
        return nil;
    end
    return obj:GetComponent("BaseLua");
end

function getCont(id,pram)
    return PanelManager:getCont(id,pram);
end

function getError(id)
    return PanelManager:getError(id);
end

function openUIByC(id)
     PanelManager:openByC(id);
end

function closeUIByC(id)
     PanelManager:closeByC(id);
end

function doByC(id,pram)
    return PanelManager:doByC(id,pram);
end

function addBehaviour(go,behaviourName)
   return  PanelManager:addBehaviour(go,behaviourName);
end

function FunctionOpenMgr()
    return PanelManager:functionOpenMgr();
end

function flytxts()
    return PanelManager:flytxts();
end

function canlines()
    return PanelManager:canlines();
end

XmlMgr = PanelManager:xmlMgr();
KeyWord = PanelManager:getKeyWord();