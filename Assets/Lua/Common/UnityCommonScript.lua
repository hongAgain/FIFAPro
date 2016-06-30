--[[
function log(str)
    Util.Log(str);
end
--]]

--GameObject = UnityEngine.GameObject

-- UIGrayAtlas = "UIGrayAtlas";

function print(str)
    LogManager:Log(str)
	--Util.Log(str);
end


function error(str)
    LogManager:LogError(str)
--	Util.LogError(str);
end

--[[
function warn(str)
	Util.LogWarning(str);
end


]]--
function GameObjectFind(str)
	return GameObject.Find(str);
end

function GameObjectDestroy(obj)
	GameObject.Destroy(obj);
end

function GameObjectDestroyImmediate(obj)
   GameObject.DestroyImmediate(obj)
end
function GameObjectInstantiate(prefab)
	return GameObject.Instantiate(prefab);
end

function TransformFindChild(transform, str)
	return transform:Find(str);
end

function GetComponentInChildren(childNode, typeName)
	return childNode:GetComponent(typeName);
end

function GameObjectSetActive(transform_, bActive)
    if transform_ ~= nil then
        transform_.gameObject:SetActive(bActive);
    end
end

function GameObjectActiveSelf(gameObject)
    if gameObject ~= nil then
        return gameObject.activeSelf;
    end
end

function GameObjectLocalPostion(transform_,vec3_)
    if transform_ ~= nil then
        transform_.localPosition = vec3_;
    end
end

function GetGameObjectLocalPostion(transform_)
    if transform_ ~= nil then
        return transform_.localPosition;
    end
end

function GameObjectPostion(transform_)
    if transform_ ~= nil then
        return transform_.position;
    end
end

function GameObjectLocalScale(transform_,vec3_)
    if transform_ ~= nil then
        transform_.localScale = vec3_;
    end
end

function GetGameObjectLocalScale(transform_)
    if transform_ ~= nil then
        return transform_.localScale;
    end
end

function AddChild(prefab, parent)
    local clone = GameObjectInstantiate(prefab);
    clone.transform.parent = parent;
    clone.transform.localScale = NewVector3(1, 1, 1);
    
    local layer = parent.gameObject.layer;
    SetLayer(clone, layer);
    
    return clone;
end

function SetLayer(gameObject, layer)
    gameObject.layer = layer;
    
    local t = gameObject.transform;
    local imax = t.childCount;
    for i = 1, imax do
        local child = t:GetChild(i - 1);
        SetLayer(child.gameObject, layer);
    end
end

function NewVector3(x, y, z)
    local v = Vector3.zero;
    v.x = x or 0;
    v.y = y or 0;
    v.z = z or 0;
    return v;
end
function NewVector2( x,y )
    local v = Vector2.zero
    v.x = x or 0
    v.y = y or 0
    return v
end

function IsTableEmpty( tableToTest )
    return (next(tableToTest) == nil);
end

function GetLocalizedString(localizeKey, ... )
    if(...==nil or select('#',...) == 0) then
        return Util.LocalizeString(localizeKey);
    else 
        return string.format(Util.LocalizeString(localizeKey),...);
    end 
end

function AddOrChangeClickParameters( targetGameObject, targetFunction, parameters )
    local targetListener = UIHelper.GetUIEventListener(targetGameObject);
    if(targetListener == nil) then
        targetListener = Util.AddClick(targetGameObject,targetFunction);           
    end
    targetListener.parameter = parameters;
end

function CreateUIListItemGameObjects(uiContainer, dataTable, itemPrefab, delegateOnInit)
    -- body
    local i = 1;
    for k,v in pairs(dataTable) do
        --instantiate prefab and initialize it
        local clone = GameObjectInstantiate(itemPrefab);
        clone.transform.parent = uiContainer;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localScale = Vector3.one;

        delegateOnInit(i,k,v,clone);
        i = i+1;
    end
end

function DestroyUIListItemGameObjects(itemTable)
    if(itemTable~=nil and itemTable~={})then
        for k,v in pairs(itemTable) do 
            -- if(v~=nil and v~={})then
                -- if(v.transform ~= nil)then
                    GameObjectSetActive(v.transform,false);
                -- end
                -- if(v.gameObject ~= nil)then
                    GameObjectDestroy(v.gameObject);
                -- end
            -- end
            itemTable[k] = nil;
        end
    end
    itemTable = {};
end
function InstantiatePrefab(obj_,parent_,name_)
    local clone = GameObject.Instantiate(obj_);
    if name_ ~= nil then
        clone.name = name_;
    end
    clone.transform.parent = parent_;
    clone.transform.localPosition = Vector3.zero;
    clone.transform.localScale = Vector3.one;
    return clone;
end

function InstantiateStars(parent_,obj_,allCount_,lightCount_)
    local lightTb = {};
    local objTb = {};
    for i=1,allCount_ do
        local temp = GameObject.Instantiate(obj_);
        local tempLight = TransformFindChild(temp.transform,"StarHighlight");
        temp.name = i;
        temp.transform.parent = parent_;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
        table.insert(lightTb,tempLight);
        table.insert(objTb,temp);
        if i>lightCount_ then
           GameObjectSetActive(tempLight,false);
        end
    end
    return lightTb,objTb;
end

function GetModuleRules(moduleName)
    local ruleData = Config.GetTemplate(Config.ModuleRules());
    if(ruleData==nil or ruleData[moduleName]==nil or ruleData[moduleName].content==nil)then
        return moduleName.." rules not prepared";
    end
    return ruleData[moduleName].title,ruleData[moduleName].content;
end

function SetDefaultValueForTable(tb,defaultvalue)
    local defaultTable = {__index = function ()
        return defaultvalue;
    end};
    setmetatable(tb,defaultTable);
end

function diterator(enumerator)
    if enumerator:MoveNext() == true then
        return enumerator.Current.Key, enumerator.Current.Value
    else
        return nil, nil
    end
end

function dpairs(dict)
    return diterator, dict:GetEnumerator()
end