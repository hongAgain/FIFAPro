module("SynSys", package.seeall)

require("CommonScript")


local mTypeKey = {
    Item = "item",
    Hero = "hero",
    Info = "info",
    Coach = "coach"
}
local mRefreshItemListCB = {};
local mRefreshHeroListCB = {};
local mRefreshInfoListCB = {};
local mRefreshCoachListCB = {};

function OnInit()
end

function OnRelease()

end

function RegisterCallback(_type, callBack)
    if (_type == mTypeKey.Item) then
        table.insert(mRefreshItemListCB, callBack);
    elseif _type == mTypeKey.Hero then
        table.insert(mRefreshHeroListCB,callBack);
    elseif _type == mTypeKey.Info then
        table.insert(mRefreshInfoListCB,callBack);
    elseif _type == mTypeKey.Coach then
        table.insert(mRefreshCoachListCB,callBack);
    end
end

function UnRegisterCallback(_type, callBack)
    if (_type == mTypeKey.Item and mRefreshItemListCB ~= nil) then
        CommonScript.TableRemoveValue(mRefreshItemListCB, callBack);
    elseif _type == mTypeKey.Hero and mRefreshHeroListCB ~= nil then
        CommonScript.TableRemoveValue(mRefreshHeroListCB, callBack);
    elseif _type == mTypeKey.Info and mRefreshInfoListCB ~= nil then
        CommonScript.TableRemoveValue(mRefreshInfoListCB, callBack);
    elseif _type == mTypeKey.Coach and mRefreshCoachListCB ~= nil then
        CommonScript.TableRemoveValue(mRefreshCoachListCB, callBack);
    end
end

function Handle(cache)
    local itemFlag = false;
    local heroFlag = false;
    local infoFlag = false;
    local coachFlag = false;

    for _, v in ipairs(cache) do
        if (v["type"] == mTypeKey.Item) then
            itemFlag = true;
            ItemSys.UpdateItemData(v["key"], v["val"]);
        elseif v["type"] == mTypeKey.Hero then
            heroFlag = true;
            local heroValue = v["val"];
            -- do data
            if heroValue ~= nil then
                Hero.SetHeroData(heroValue["id"],heroValue);
            else
                Hero.SetHeroData(v["key"],heroValue);
            end
        elseif v["type"] == mTypeKey.Info then
            infoFlag = true;
            local infoValue = v["val"];
            -- do data
            Role.SetRoleData(v["key"],infoValue); 
        elseif v["type"] == mTypeKey.Coach then
            coachFlag = true;
            local coachValue = v["val"];
            -- do data
            CoachData.AddOrUpdateCoachData(coachValue);
        end
    end
    
    if (itemFlag) then
        for _, v in ipairs(mRefreshItemListCB) do
            v();
        end
    end
    if (heroFlag) then
        for _, v in ipairs(mRefreshHeroListCB) do
            v();
        end
    end
    if (infoFlag) then
        for _, v in ipairs(mRefreshInfoListCB) do
            v();
        end
    end
    if (coachFlag) then
        for _, v in ipairs(mRefreshCoachListCB) do
            v();
        end
    end
end