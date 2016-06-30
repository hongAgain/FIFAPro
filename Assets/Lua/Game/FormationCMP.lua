--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("FormationCMP", package.seeall)

-- require "UIAvatar"

enum_CMPType = {Type1= 1}

local ratioTb = 
{
    {x=372/699,y=456/559}
}

local tt = { };
tt.__index = tt;

function New()
    local t = {};
    setmetatable(t, tt);
    
    function t:InitFormation(formId_, transformTb_,CMPType_,playerIconTb_)
        local formId = 0;
        local cmpType = 1;
        if formId_ ~= nil then
            formId = formId_;
        else
            formId = Hero.GetFormId();
        end
        if CMPType_ ~= nil then
            cmpType = CMPType_;
        end
    
        local formData = TableManager.FormationTbl:GetItem(formId);
        --local formData = Config.GetTemplate(Config.FormTable())[formId];
        local posData = Config.GetTemplate(Config.positionTB)[tostring(formData.ID)];
         
        for i = 1, #transformTb_ do
            local pos = Vector3.New(posData[string.format("pos%d_x", i)]*ratioTb[cmpType].x, posData[string.format("pos%d_y", i)]*ratioTb[cmpType].y, 0);
            transformTb_[i].localPosition = pos;
            if playerIconTb_ ~= nil then
                InitAvatar(transformTb_[i],formData.ProList[i-1],playerIconTb_[i]);
            else    
                InitAvatar(transformTb_[i],formData.ProList[i-1]);
            end

        end
    end

    function InitAvatar(tf_, proId_,icon_)
        local sName = Config.GetProperty(Config.ProTable(), tostring(proId_), "shortName");
        local sNameLabel = TransformFindChild(tf_, "LabelPos");
        UIHelper.SetLabelTxt(sNameLabel, sName);
        local icon = TransformFindChild(tf_, "Head/Icon");
        if icon_ ~= nil then
            Util.SetUITexture(icon, LuaConst.Const.PlayerHeadIcon, icon_, true);
        end
    end

    return t;
end











