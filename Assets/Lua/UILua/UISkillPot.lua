--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UISkillPot", package.seeall)

require "Game/SkillUtility"
local window = nil;
local windowComponent = nil;

local m_playerId = nil;
local m_potTipsType = nil;
local m_potLv = nil;
local m_lblTicket = nil;
local m_lblMoney = nil;
local m_lockNum = nil;
local m_btnPotBegin;
local m_btnPotReplace;
local m_iFreeCount = 0;
local m_uiFreeWashCount = nil;          -- 免费次数UI
local m_uiCost = nil;                   -- 洗炼费用UI

local m_statusTb = {};
local m_lockSkillIndex = {};
local callback = nil;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    callback = params.Callback;
    --m_iFreeCount --设置可以拥有的免费次数
    BindUI();
end

function BindUI()
    local transform = window.transform;
    
    m_lockNum = 0;
    m_lockSkillIndex = {0,0,0,0}
    m_statusTb = { checkStatus = {},lockStatus = {},upStatus = {},downStatus = {},drawStatus = {},lockIcon = {},refreshIcon = {},skillIcon = {icon,lbl},skillNewIcon = {icon,lbl}}
    for i=1,4 do
        table.insert(m_statusTb.checkStatus,TransformFindChild(transform,i.."/"..i));
        table.insert(m_statusTb.lockStatus,TransformFindChild(transform,i.."/Spr_Lock"));
        table.insert(m_statusTb.upStatus,TransformFindChild(transform,i.."/Spr_Up"));
        table.insert(m_statusTb.downStatus,TransformFindChild(transform,i.."/Spr_Down"));
        table.insert(m_statusTb.drawStatus,TransformFindChild(transform,i.."/Spr_Draw"));
        table.insert(m_statusTb.lockIcon,TransformFindChild(transform,i.."/Spr_LockSkill"));
        table.insert(m_statusTb.refreshIcon,TransformFindChild(transform,i.."/Spr_Refresh"));
        local skillTemp = {}
        local newSkillTemp = {}
        skillTemp.icon = TransformFindChild(transform,i.."/Skill_Icon/Icon");
        skillTemp.lbl = TransformFindChild(transform,i.."/Skill_Icon");
        newSkillTemp.icon = TransformFindChild(transform,i.."/Skill_IconNew/Icon");
        newSkillTemp.lbl = TransformFindChild(transform,i.."/Skill_IconNew");
        table.insert(m_statusTb.skillIcon,skillTemp);
        table.insert(m_statusTb.skillNewIcon,newSkillTemp);

        Util.AddClick(m_statusTb.checkStatus[i].gameObject,BtnCheck);
        GameObjectSetActive(TransformFindChild(transform,i.."/"..i.."/Sprite"),false);
        ResetSkillIcon(i,false);       
    end

    m_uiCost = TransformFindChild(transform,"LabelCost");
    m_uiFreeWashCount = TransformFindChild(transform,"FreeWashCount");
    if m_iFreeCount > 0 then
        m_uiCost.gameObject:SetActive(false);
        m_uiFreeWashCount.gameObject:SetActive(true);
        UIHelper.SetLabelTxt(m_uiFreeWashCount,string.format(Util.LocalizeString("FreeWashCount"),m_iFreeCount))
    else
        m_uiCost.gameObject:SetActive(true);
        m_uiFreeWashCount.gameObject:SetActive(false);
    end 

    m_lblTicket = TransformFindChild(transform,"LabelCost/LabelTicket");
    m_lblMoney =  TransformFindChild(transform,"LabelCost/LabelMoney");

    UIHelper.SetLabelTxt(TransformFindChild(transform,"AllTicket/Label"),ItemSys.GetItemData(LuaConst.Const.SkillStone).num);

    m_btnPotBegin = TransformFindChild(transform,"BtnPotBegin");
    m_btnPotReplace = TransformFindChild(transform,"BtnPotReplace");
    Util.AddClick(m_btnPotBegin.gameObject,BtnPotBegin);
    Util.AddClick(m_btnPotReplace.gameObject,BtnPotReplace);
    Util.AddClick(TransformFindChild(transform,"BtnClose").gameObject,BtnClose);

    IsEnabelReplace(2);
    UpdateRefreshCost();
end


function BtnPotBegin(args)
    if m_lockNum > 3 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_NoLock") });
        return;
    elseif ItemSys.GetItemData(LuaConst.Const.SkillStone).num < Config.GetProperty(Config.HeroSkillRefresh(),tostring(m_lockNum),"item_num") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_NoSkillStone") });
        return
    elseif ItemSys.GetItemData(LuaConst.Const.SB).num < Config.GetProperty(Config.HeroSkillRefresh(),tostring(m_lockNum),"money") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_NoMoneyPot") });
        return 
    end


    local lockIndex="";
    for i=1,4 do
        if m_lockSkillIndex[i] ~= 0 then
            lockIndex = lockIndex..m_lockSkillIndex[i]..","
        end
    end
    lockIndex = string.sub(lockIndex,1,-2);
    local OnPot = function(data_)
        for k,v in pairs(data_) do
            NewSkillIcon(tonumber(k),tonumber(v));
        end
        IsEnabelReplace(1);
        UIHelper.SetLabelTxt(TransformFindChild(window.transform,"AllTicket/Label"),ItemSys.GetItemData(LuaConst.Const.SkillStone).num);
    end;
    Hero.ReqHeroSRefresh(PlayerInfoData.GetCurrPlayerId(),m_lockNum,lockIndex,OnPot);   
end


function BtnPotReplace(args)
    local OnCSRefresh = function(data_)
        for i=1,4 do
            ResetSkillIcon(i,false);
        end
        IsEnabelReplace(3);        
    end;
    Hero.ReqHeroCSRefresh(PlayerInfoData.GetCurrPlayerId(),OnCSRefresh);
end

function IsEnabelReplace(enableIndex_) -- 1 replace 2 begin
    UIHelper.EnableButton(m_btnPotReplace,false);
    UIHelper.SetWidgetColor(TransformFindChild(m_btnPotReplace,"Label"),Color.New(0, 0, 0, 1))
    UIHelper.EnableButton(m_btnPotBegin,true);
    UIHelper.SetWidgetColor(TransformFindChild(m_btnPotBegin,"Label"),Color.New(0, 0, 0, 1))

    if enableIndex_ == 1 then
        UIHelper.EnableButton(m_btnPotReplace,true);
        UIHelper.SetWidgetColor(TransformFindChild(m_btnPotReplace,"Label"),Color.New(1, 1, 1, 1))
        for i=1,4 do
            UIHelper.EnableBtn(m_statusTb.checkStatus[i].gameObject,false);
        end
    elseif enableIndex_ == 2 then
        UIHelper.EnableButton(m_btnPotBegin,true);
        UIHelper.SetWidgetColor(TransformFindChild(m_btnPotBegin,"Label"),Color.New(1, 1, 1, 1))
    end

end

function BtnCheck(obj_)
    if GameObjectActiveSelf(TransformFindChild(obj_.transform,"Sprite").gameObject) then  -- unlock
        GameObjectSetActive(TransformFindChild(obj_.transform,"Sprite"),false);
        UIHelper.SetWidgetColor(obj_.transform,Color.New(125/255, 128/255, 137/255, 1))
        RefreshLockStatus(tonumber(obj_.name),0)
        m_lockSkillIndex[tonumber(obj_.name)] = 0
        m_lockNum = m_lockNum -1;   
    else
        GameObjectSetActive(TransformFindChild(obj_.transform,"Sprite"),true);  -- lock
        UIHelper.SetWidgetColor(obj_.transform,Color.New(102/255, 204/255, 1, 1))
        RefreshLockStatus(tonumber(obj_.name),1)
        m_lockSkillIndex[tonumber(obj_.name)] = obj_.name
        m_lockNum = m_lockNum +1;   
    end

    UpdateRefreshCost();
end

function ResetSkillIcon(index_,bNewSkill_)
    local id,lv = HeroData.GetHeroSkillIdLv(PlayerInfoData.GetCurrPlayerId(),index_);

    if bNewSkill_ then
        UIHelper.SetLabelTxt(m_statusTb.skillNewIcon[index_].lbl, string.format(Util.LocalizeString("UISkill_NameLv"),TableManager.SkillTbl:GetItem(id).Name,lv))
        Util.SetUITexture(m_statusTb.skillNewIcon[index_].icon,LuaConst.Const.SkillIcon,HeroData.GetHeroSkillIcon(PlayerInfoData.GetCurrPlayerId(),index_), false);
    else
        UIHelper.SetLabelTxt(m_statusTb.skillIcon[index_].lbl, string.format(Util.LocalizeString("UISkill_NameLv"),TableManager.SkillTbl:GetItem(id).Name,lv))
        
        Util.SetUITexture(m_statusTb.skillIcon[index_].icon,LuaConst.Const.SkillIcon,HeroData.GetHeroSkillIcon(PlayerInfoData.GetCurrPlayerId(),index_), false);
    end
    local kSkillItem = TableManager.SkillTbl:GetItem(id);
    if nil ~= kSkillItem then   
        UIHelper.SetWidgetColor(m_statusTb.skillIcon[index_].lbl,SkillUtility.GetSkillColor(kSkillItem.Quality));
    end
    
end

function NewSkillIcon(index_,skillId_)
    GameObjectSetActive(m_statusTb.skillNewIcon[index_].lbl,true);
    local id,lv = HeroData.GetHeroSkillIdLv(PlayerInfoData.GetCurrPlayerId(),index_);
    UIHelper.SetLabelTxt(m_statusTb.skillNewIcon[index_].lbl, string.format(Util.LocalizeString("UISkill_NameLv"),TableManager.SkillTbl:GetItem(skillId_).Name,lv))
    Util.SetUITexture(m_statusTb.skillNewIcon[index_].icon,LuaConst.Const.SkillIcon,HeroData.GetHeroSkillIcon(PlayerInfoData.GetCurrPlayerId(),index_), false);
    local istatus = 0;
    local kNewSkillItem = TableManager.SkillTbl:GetItem(skillId_);
    local kSkillItem = TableManager.SkillTbl:GetItem(id);
    if kNewSkillItem.Quality > kSkillItem.Quality then
        istatus = 2;
    elseif kNewSkillItem.Quality == kSkillItem.Quality then
        istatus = 3;
    elseif kNewSkillItem.Quality < kSkillItem.Quality then
        istatus = 3;
    end
    UIHelper.SetWidgetColor(m_statusTb.skillNewIcon[index_].lbl,SkillUtility.GetSkillColor(kNewSkillItem.Quality))
    RefreshLockStatus(index_,istatus);
end

function RefreshLockStatus(index_,iStatus_) -- 0 None 1 lock 2 up 3 draw 4 down
    GameObjectSetActive(m_statusTb.lockStatus[index_],false);    
    GameObjectSetActive(m_statusTb.upStatus[index_],false);
    GameObjectSetActive(m_statusTb.drawStatus[index_],false);
    GameObjectSetActive(m_statusTb.downStatus[index_],false);
    GameObjectSetActive(m_statusTb.lockIcon[index_],false);
    GameObjectSetActive(m_statusTb.refreshIcon[index_],true);

    if iStatus_ == 1 then
        GameObjectSetActive(m_statusTb.lockStatus[index_],true);
        GameObjectSetActive(m_statusTb.lockIcon[index_],true);
        GameObjectSetActive(m_statusTb.refreshIcon[index_],false);    
    elseif iStatus_ == 2 then   
        GameObjectSetActive(m_statusTb.upStatus[index_],true);
        GameObjectSetActive(m_statusTb.refreshIcon[index_],false);
    elseif iStatus_ == 3 then
        GameObjectSetActive(m_statusTb.drawStatus[index_],true);
        GameObjectSetActive(m_statusTb.refreshIcon[index_],false);
    elseif iStatus_ == 4 then
        GameObjectSetActive(m_statusTb.downStatus[index_],true);
        GameObjectSetActive(m_statusTb.refreshIcon[index_],false);
    end
end

function UpdateRefreshCost()
    UIHelper.SetLabelTxt(m_lblTicket,Config.GetProperty(Config.HeroSkillRefresh(),tostring(m_lockNum),"item_num"))
    UIHelper.SetLabelTxt(m_lblMoney,Config.GetProperty(Config.HeroSkillRefresh(),tostring(m_lockNum),"money"))
end

function BtnClose()
    ExitUISkillPot();
end



function OnDestroy()
    if callback ~= nil then
        callback();
        callback = nil;
    end

end


function ExitUISkillPot()
   windowComponent:Close();

end




