--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
module("CheckUpgradeMgr", package.seeall)

local CallbackPlayer = nil;
local CallbackCoach = nil;

function SetBackup()
    Role.Set_lvBack();
    Hero.SetHeroDataBackup();
end


function CheckCoachUpgrade(cb_)
    CallbackCoach = cb_;
    if (tonumber(Role.Get_lv()) - tonumber(Role.Get_lvBack())) > 0 then
        WindowMgr.ShowWindow(LuaConst.Const.UIUpgradeCoach,{Callback=CallbackCoach});
    else
        if CallbackCoach ~= nil then
            CallbackCoach();
            CallbackCoach = nil;
        end
    end

end

function CheckPlayerUpgrade(cb_)
    CallbackPlayer = cb_;
    local upgradeTb = {};
    local dataBackup = Hero.GetHeroDataBackup();
    for k,v in pairs(Hero.MainTeamHeroId()) do
        local exp = Hero.GetHeroData()[v].exp;
        local lv = Hero.GetHeroData()[v].lv;
        if lv > dataBackup[v].lv then
            local expLv = Config.GetProperty(Config.LevelTable(), tostring(lv),"HExp");
            local expLvNext = Config.GetProperty(Config.LevelTable(), tostring(lv+1),"HExp");
            local tb = {};
            tb.lv = lv;
            tb.lvRatio = (exp-expLv)/(expLvNext-expLv);
            tb.id = v; -- player id
            table.insert(upgradeTb,tb);
        end

    end

    if #upgradeTb > 0 then
        WindowMgr.ShowWindow(LuaConst.Const.UIUpgradePlayer,{Data=upgradeTb,Callback=CallbackPlayer});
    else
        if CallbackPlayer ~= nil then
            CallbackPlayer();
            CallbackPlayer = nil;
        end
    end
end
