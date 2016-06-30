--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPotTips", package.seeall)


local window = nil;
local windowComponent = nil;

local m_playerId = nil;
local m_potTipsType = nil;
local m_potLv = nil;
function OnStart(gameObject, params)
    m_playerId = params.PlayerId;
    m_potTipsType = params.PotType;
    m_potLv = params.PotLv;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    if PlayerPotData.enum_PotType.TargetFinish == m_potTipsType then
        BindUIFinish();
    elseif PlayerPotData.enum_PotType.TargetReward == m_potTipsType then
        BindUIReward();
    end

end

function BindUIFinish()
    local clone = InstantiatePrefab(windowComponent:GetPrefab("FinishTarget"),window.transform).transform;
    
    UIHelper.SetLabelTxt(TransformFindChild(clone,"Content/Label1"),PlayerPotData.GetTargetDes(m_playerId,m_potLv));
    UIHelper.SetLabelTxt(TransformFindChild(clone,"Content/Label2"),PlayerPotData.GetTargetDes(m_playerId,m_potLv+1));
    UIHelper.SetLabelTxt(TransformFindChild(clone,"Content/RewardTxt"),PlayerPotData.GetTargetRewardStr(m_playerId,m_potLv));
    Util.AddClick(TransformFindChild(clone,"BtnSure").gameObject,BtnClose);
    Util.AddClick(TransformFindChild(clone,"BtnClose").gameObject,BtnClose);
end

function BindUIReward()
    local clone = InstantiatePrefab(windowComponent:GetPrefab("RewardTarget"),window.transform).transform;

    UIHelper.SetLabelTxt(TransformFindChild(clone,"Label"),PlayerPotData.GetTargetDes(m_playerId,m_potLv))
    UIHelper.SetLabelTxt(TransformFindChild(clone,"Content/RewardTxt"),PlayerPotData.GetTargetRewardStr(m_playerId,m_potLv));
    Util.AddClick(TransformFindChild(clone,"BtnSure").gameObject,BtnClose);
    Util.AddClick(TransformFindChild(clone,"BtnClose").gameObject,BtnClose);

end

function BtnClose()
    ExitUIPlayerPot();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;

end

function ExitUIPlayerPot()
   windowComponent:Close();

end



