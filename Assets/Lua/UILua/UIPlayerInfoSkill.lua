--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
module("UIPlayerInfoSkill", package.seeall)

require "Game/SkillUtility"

local ThisTransform = nil;

function InitPlayerInfoSkill(subTransform_)
    ThisTransform = subTransform_;

    Util.AddClick(TransformFindChild(ThisTransform,"BtnPot").gameObject, BtnSkillPot);
    Util.AddClick(TransformFindChild(ThisTransform,"BtnShowDetail").gameObject, BtnShowDetail);

    for i=1,4 do
        Util.AddClick(TransformFindChild(ThisTransform,i.."/"..i).gameObject, BtnSkillUpgrade);
    end

    RefreshSkillInfo();
end

function RefreshSkillInfo()
    
    local skillTipsBtn = TransformFindChild(ThisTransform,"AllTicket/Bg").gameObject;
    Util.AddClick(skillTipsBtn,OnSkillTipsClick);
    local playerId = PlayerInfoData.GetCurrPlayerId();
    UIHelper.SetLabelTxt(TransformFindChild(ThisTransform,"AllTicket/Label"),ItemSys.GetItemData(LuaConst.Const.SkillTicket).num);
    for i=1,4 do
        local id,lv = HeroData.GetHeroSkillIdLv(playerId,i);
        if id ~= nil then
            GameObjectSetActive(TransformFindChild(ThisTransform,tostring(i)),true);

            local uiIconTs = TransformFindChild(ThisTransform,i.."/Icon")
            Util.SetUITexture(uiIconTs,LuaConst.Const.SkillIcon,HeroData.GetHeroSkillIcon(playerId,i), false);
            local kSkillItem = TableManager.SkillTbl:GetItem(id);
            local kNameTransform = TransformFindChild(ThisTransform,i.."/Lbl_NameLv");
            UIHelper.SetLabelTxt(kNameTransform, string.format(Util.LocalizeString("UISkill_NameLv"),kSkillItem.Name,lv))
            local skillTbl = {}
            skillTbl.ID = id;
            skillTbl.Lv = lv;
            local kRetObj = Util.AddClick(uiIconTs.gameObject,BtnSkillInfo);
            kRetObj.parameter = skillTbl;
            kRetObj = Util.AddClick(kNameTransform.gameObject,BtnSkillInfo);
            kRetObj.parameter = skillTbl;
            UIHelper.SetWidgetColor(kNameTransform,SkillUtility.GetSkillColor(kSkillItem.Quality));
            UIHelper.SetLabelTxt(TransformFindChild(ThisTransform,i.."/Lbl_Ticket"),Config.GetProperty(Config.HeroSkillLvUp(),tostring(lv),"item_num") );
            UIHelper.SetLabelTxt(TransformFindChild(ThisTransform,i.."/Lbl_Money"),Config.GetProperty(Config.HeroSkillLvUp(),tostring(lv),"money_cost"));          
        else
            GameObjectSetActive(TransformFindChild(ThisTransform,tostring(i)),false);
        end
    end

end

function BtnSkillInfo(go)
    local iSkillID = nil
    local kLuaTbl = {};
    local kListener = UIHelper.GetUIEventListener(go);
	if(kListener~=nil and kListener.parameter~=nil )then
        kLuaTbl = kListener.parameter;
	end
    WindowMgr.ShowWindow(LuaConst.Const.UISkillInfo,kLuaTbl);
end

function OnSkillTipsClick()
end 

function BtnSkillPot()
    local OnClosePot = function()
        RefreshSkillInfo();
    end;

    WindowMgr.ShowWindow(LuaConst.Const.UISkillPot,{Callback = OnClosePot});
end

function BtnShowDetail()
    local OnClosePot = function()
        RefreshSkillInfo();
    end;

    WindowMgr.ShowWindow(LuaConst.Const.UISkillPot,{Callback = OnClosePot});
end
    

function BtnSkillUpgrade(obj_)
    index_ = obj_.name;
    local id,lv = HeroData.GetHeroSkillIdLv(PlayerInfoData.GetCurrPlayerId(),index_);
    if IsCanUpgradeSkill(lv) then
        local OnUpgrade = function(data_)
            RefreshSkillInfo();
--            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_UpgradeSuccess") });
        end;

        Hero.ReqHeroSkillLvUp(PlayerInfoData.GetCurrPlayerId(),index_,lv,OnUpgrade)
    end  
end

function IsCanUpgradeSkill(lv_)
    local stone = Config.GetProperty(Config.HeroSkillLvUp(),tostring(lv_),"item_num");
    local money = Config.GetProperty(Config.HeroSkillLvUp(),tostring(lv_),"money_cost");
    local limitPlayerLv = Config.GetProperty(Config.HeroSkillLvUp(),tostring(lv_),"player_lv");

    if Hero.GetHeroData2Id(PlayerInfoData.GetCurrPlayerId()).lv < limitPlayerLv then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_NoLv") });
        return false;
    elseif ItemSys.GetItemData(LuaConst.Const.SkillTicket).num < stone then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_NoStone") });
        return false;
    elseif ItemSys.GetItemData(LuaConst.Const.SB).num < money then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UISkill_NoMoney") });
        return false;
    end

    return true
end


function OnDestroy()

end


