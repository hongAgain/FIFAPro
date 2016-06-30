
module("UISkillInfo", package.seeall)
require "Game/SkillUtility"
local window = nil;
local windowComponent = nil;
local skillTbl = nil
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    skillTbl = windowComponent.mLuaTables;
    BindUI();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
end

function BindUI()
    local kTransform = window.transform;

    local kCloseBtnObj = TransformFindChild(kTransform, "BG/TopCloseBtn").gameObject;
    Util.AddClick(kCloseBtnObj,OnCloseClick);
    kCloseBtnObj = TransformFindChild(kTransform, "BG/CloseBtn").gameObject;
    Util.AddClick(kCloseBtnObj,OnCloseClick);
    local iSkillLV = 1
    -- 设置技能等级
    local kLvObj = TransformFindChild(kTransform, "BG/SkillIconInfo/LV"); 
    if(nil ~= skillTbl.Lv) then
        iSkillLV = skillTbl.Lv
    end

    UIHelper.SetLabelTxt(kLvObj,string.format("Lv:%d",iSkillLV))
    if(nil ~= skillTbl.ID) then
        local kSkillItem = TableManager.SkillTbl:GetItem(skillTbl.ID);
        if nil ~= kSkillItem then   
            local kIconTs = TransformFindChild(kTransform, "BG/SkillIconInfo/Icon"); 
            Util.SetUITexture(kIconTs,LuaConst.Const.SkillIcon,kSkillItem.IconName,false);

            local kNameTs = TransformFindChild(kTransform, "BG/SkillIconInfo/Name"); 
            UIHelper.SetLabelTxt(kNameTs, kSkillItem.Name)
            UIHelper.SetWidgetColor(kNameTs,SkillUtility.GetSkillColor(kSkillItem.Quality));

            -- 技能效果描述
            local kDescTs = TransformFindChild(kTransform, "BG/SkillDescBG/SkillDesc");
            UIHelper.SetLabelTxt(kDescTs, GenDesc(kSkillItem,iSkillLV,kSkillItem.Desc) ) 
        else
            LogManager:RedLog(string.format("SkillID:%d",skillTbl.ID))
        end
    end 
    
end

function GenDesc(kInItem,kInLv,kInStrVal)
    if nil == kInItem or nil == kInLv or nil == kInStrVal then  
        return ""
    end 
    local _iIdx = string.find(kInStrVal,"{")
    local _strText = ""
    local _iPosX = 0
    local _iParamIdx = 0
    while(nil ~=kInStrVal and nil ~= _iIdx)
    do
        local _strPreMsg = string.sub(kInStrVal,0,_iIdx-1)
        local iVal = kInItem.AttriList[_iParamIdx*3+1]+kInItem.AttriList[_iParamIdx*3+2]*kInLv
        _strText= string.format("%s%s%d",_strText,_strPreMsg,iVal)
        local _strPostMsg = string.sub(kInStrVal,_iIdx+1,kInStrVal.Length)
        _iIdx = string.find(_strPostMsg,"}")
        local _strMidMsg = string.sub(_strPostMsg,0,_iIdx-1)
        kInStrVal = string.sub(_strPostMsg,_iIdx+1,_strPostMsg.Length)
        _iIdx = string.find(kInStrVal,"{")
    end
    _strText= string.format("%s%s",_strText,kInStrVal)
    return _strText;
end

function OnCloseClick()
    windowComponent:Close();
end

