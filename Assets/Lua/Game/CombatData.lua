module("CombatData", package.seeall)

require "Common/UnityCommonScript"
require "Game/SceneManager"

local dataTable = nil;
local resultTable = nil;
local mCombatType = nil;
local mNotifier = nil;

COMBAT_TYPE = {
    TEAMLEGEND = 1,
    PEAKROAD = 2,
    TIMERAID = 3,
    PVELEAGUE = 4,
    PVPLADDER = 5
};

function FillData(data, combatType, notifier)
    dataTable = data;
    mCombatType = combatType;
    mNotifier = notifier;
    -- Application.LoadLevel("Empty");
    SceneManager.LoadBattleScene();
end

function GetData()
    return dataTable;
end

function CombatEnd()
    if (mNotifier ~= nil) then
        mNotifier();
    end
end

function GetResultTable()
    return resultTable;
end

function GetEmptyResultTable()
    return {};
end

function PushResult(result)
    resultTable = result;
    CombatEnd();
end

function ValidPVEData()
    if nil == resultTable then  
        return false;
    end
end


