--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/4/8
--此文件由[BabeLua]插件自动生成



--endregion


--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/5
--此文件由[BabeLua]插件自动生成



--endregion


module("UIPlayerInfoPotential", package.seeall)
require "Config"
require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/Role"
require "Game/ItemSys"
require "Game/HeroData"


local strAttributeTB = 
{   "/RootLeft/CenterTrain/MiddlePart/ItemAttribute1/ValueRight/Attr1/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute1/ValueRight/Attr2/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute1/ValueRight/Attr3/lbl_attrValue",

    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute2/ValueRight/Attr1/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute2/ValueRight/Attr2/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute2/ValueRight/Attr3/lbl_attrValue",

    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute3/ValueRight/Attr1/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute3/ValueRight/Attr2/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute3/ValueRight/Attr3/lbl_attrValue",

    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute4/ValueRight/Attr1/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute4/ValueRight/Attr2/lbl_attrValue",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute4/ValueRight/Attr3/lbl_attrValue",
}
local strAttrSprTB = 
{   "/RootLeft/CenterTrain/MiddlePart/ItemAttribute1/ValueLeft/spr_ability2",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute2/ValueLeft/spr_ability2",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute3/ValueLeft/spr_ability2",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute4/ValueLeft/spr_ability2",
}
local strSprButtonTB = 
{   "/RootLeft/CenterTrain/MiddlePart/ItemAttribute1/1",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute2/2",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute3/3",
    "/RootLeft/CenterTrain/MiddlePart/ItemAttribute4/4",
}

local strCneterTrain = "/RootLeft/CenterTrain/";
local strCenterReset = "/RootLeft/CenterReset/ResetPot/";

local ParentTransform = nil;
local ThisTransform = nil;
local windowTrain = nil;
local windowReset = nil;
local windowTrainTips = nil;

local lbl_surplusCard = nil;
local lbl_surplusPot = nil;
local lbl_trainCard = nil;
local lbl_trainPot = nil;
local lbl_trainGold = nil;
local lbl_costPotGold = nil;
local lbl_costPotRMB = nil;
local lbl_resetLeftCard = nil;
local lbl_resetRightCard = nil;
local btn_resetLeft = nil;
local btn_resetRight = nil;
local btn_train1 = nil;
local btn_train2 = nil;
local btn_train3 = nil;
local btn_train4 = nil;

local m_currPot = 0;
local m_goldGetPot = 0;
local m_rmbGetPot = 0;
local m_trainType = 1;

local lblAttrValueTB = {};
local sprAttrTb = {};
local sprButtonTb = {};


function InitPlayerInfoPotential(parentTransform_,subTransform_)
    ParentTransform = parentTransform_;
    ThisTransform = subTransform_;

    windowTrain = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/CenterTrain");
    windowTrainTips = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/CenterTrain/TrainTips");
    windowReset = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/CenterReset");

    lbl_surplusCard = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/TopPart/Card/lbl_card");
    lbl_surplusPot = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/TopPart/Pot/lbl_point");
    lbl_trainCard = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."TopPart/Card/lbl_card");
    lbl_trainPot = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."TopPart/Pot/lbl_point");
    lbl_trainGold = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."TopPart/Gold/lbl_card");
    lbl_costPotGold = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."LeftPart/Label");
    lbl_costPotRMB = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."RightPart/Label");

    local train1 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/TrainTips/Train1/Label");
    UIHelper.SetLabelTxt(train1,Config.GetProperty(Config.HeroPotTable(),"1","tp"));
    local train2 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/TrainTips/Train2/Label");
    UIHelper.SetLabelTxt(train2,Config.GetProperty(Config.HeroPotTable(),"3","tp"));
    local train3 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/TrainTips/Train3/Label");
    UIHelper.SetLabelTxt(train3,Config.GetProperty(Config.HeroPotTable(),"2","tp"));
    local train4 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/TrainTips/Train4/Label");
    UIHelper.SetLabelTxt(train4,Config.GetProperty(Config.HeroPotTable(),"4","tp"));
    local gold3 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/TrainTips/Gold3/Label");
    UIHelper.SetLabelTxt(gold3,Config.GetProperty(Config.HeroPotTable(),"2","gb"));
    local gold4 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/TrainTips/Gold4/Label");
    UIHelper.SetLabelTxt(gold4,Config.GetProperty(Config.HeroPotTable(),"4","gb"));
    local leftGold = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."LeftPart/TopPart/lbl_left");
    UIHelper.SetLabelTxt(leftGold,Config.GetTemplate(Config.BaseTable())["gRPot"]);
    lbl_resetLeftCard = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."LeftPart/TopPart/lbl_right");
    UIHelper.SetLabelTxt(lbl_resetLeftCard,tostring(m_goldGetPot));
    local rightGold = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."RightPart/TopPart/lbl_left");
    UIHelper.SetLabelTxt(rightGold,Config.GetTemplate(Config.BaseTable())["rmbRPot"]);
    lbl_resetRightCard = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."RightPart/TopPart/lbl_right");
    UIHelper.SetLabelTxt(lbl_resetRightCard,tostring(m_rmbGetPot));

    local btnResetPot = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/BottomPart/ResetPot").gameObject;
    Util.AddClick(btnResetPot, BtnResetPot);
    local btnTrainPot = TransformFindChild(ParentTransform,ThisTransform.name.."/RootLeft/MiddlePart/TrainPot").gameObject;
    Util.AddClick(btnTrainPot, BtnTrainPot);
    btn_train1 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/Train/BtnTrain1").gameObject;
    Util.AddClick(btn_train1, BtnTrain1);
    btn_train2 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/Train/BtnTrain2").gameObject;
    Util.AddClick(btn_train2, BtnTrain2);
    btn_train3 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/Train/BtnTrain3").gameObject;
    Util.AddClick(btn_train3, BtnTrain3);
    btn_train4 = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."MiddlePart/Train/BtnTrain4").gameObject;
    Util.AddClick(btn_train4, BtnTrain4);
    local btnTrainQuit = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."BottomPart/BtnQuit").gameObject;
    Util.AddClick(btnTrainQuit, BtnTrainQuit);
    local btnTrainAccept = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."BottomPart/BtnAccept").gameObject;
    Util.AddClick(btnTrainAccept, BtnTrainAccept);
    local btnTrainClose = TransformFindChild(ParentTransform,ThisTransform.name..strCneterTrain.."BtnClose").gameObject;
    Util.AddClick(btnTrainClose, BtnTrainClose);
    local btnResetClose = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."BtnClose").gameObject;
    Util.AddClick(btnResetClose, BtnResetClose);
    btn_resetLeft = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."LeftPart/Button").gameObject;
    Util.AddClick(btn_resetLeft, BtnResetGold);
    btn_resetRight = TransformFindChild(ParentTransform,ThisTransform.name..strCenterReset.."RightPart/Button").gameObject;
    Util.AddClick(btn_resetRight, BtnResetRMB);

    for i =1,#strAttributeTB do
        table.insert(lblAttrValueTB,TransformFindChild(ParentTransform,ThisTransform.name..strAttributeTB[i]));
    end

    for j=1,#strAttrSprTB do
        table.insert(sprAttrTb,TransformFindChild(ParentTransform,ThisTransform.name..strAttrSprTB[j]))
    end

    for k=1,#strSprButtonTB do
        table.insert(sprButtonTb,TransformFindChild(ParentTransform,ThisTransform.name..strSprButtonTB[k].."/Frontground"));
        Util.AddClick(TransformFindChild(ParentTransform,ThisTransform.name..strSprButtonTB[k]).gameObject, BtnSelectType);
    end

    RefreshPotential();

    RegisterMsgCallback();
end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",RefreshPotential);
    SynSys.RegisterCallback("hero",RefreshTrain);
    SynSys.RegisterCallback("hero",RefreshReset);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",RefreshPotential);
    SynSys.UnRegisterCallback("hero",RefreshTrain);
    SynSys.UnRegisterCallback("hero",RefreshReset);
end

function RefreshPotential()
    print("..RefreshPotential");
    UIHelper.SetLabelTxt(lbl_surplusCard,ItemSys.GetItemData(LuaConst.Const.Train).num);
    UIHelper.SetLabelTxt(lbl_surplusPot,m_currPot);
end

function RefreshTrain()
    print("..RefreshTrain");
    UIHelper.SetLabelTxt(lbl_trainCard,ItemSys.GetItemData(LuaConst.Const.Train).num);
    UIHelper.SetLabelTxt(lbl_trainGold,ItemSys.GetItemData(LuaConst.Const.GB).num);
    UIHelper.SetLabelTxt(lbl_trainPot,m_currPot);

    for i=1,#strAttributeTB do
        UIHelper.SetLabelTxt(lblAttrValueTB[i],GetAttr(i));
    end
    
    for j=1,#strAttrSprTB do
        UIHelper.SetSpriteName(sprAttrTb[j],GetAttrRatingName(j));
    end

    RefreshTrainBtn();
end

function OnTrainPreview(data_)
    local index = 1;

    for i = m_trainType*3-2,m_trainType*3 do
        local strAdd = "";
        if data_["pot_status"][index] ~= 0 then
            strAdd = "[00FF00]+"..data_["pot_status"][index].."[-]";
        end
        index = index + 1;
        UIHelper.SetLabelTxt(lblAttrValueTB[i],GetAttr(i)..strAdd);
    end

end

function RefreshReset()
    local strPotLeft = string.format(Util.LocalizeString("ResetGoldTips"),GetCostPot());
    local strPotRight = string.format(Util.LocalizeString("ResetRMBTips"),GetCostPot());

    UIHelper.SetLabelTxt(lbl_costPotGold,strPotLeft);
    UIHelper.SetLabelTxt(lbl_costPotRMB,strPotRight);

    UIHelper.SetLabelTxt(lbl_resetLeftCard,tostring(m_goldGetPot));
    UIHelper.SetLabelTxt(lbl_resetRightCard,tostring(m_rmbGetPot));

    if GetCostPot() > 0 and ItemSys.GetItemData(LuaConst.Const.SB).num > tonumber(Config.GetTemplate(Config.BaseTable())["gRPot"]) then
        UIHelper.EnableBtn(btn_resetLeft,true);
    else
        UIHelper.EnableBtn(btn_resetLeft,false);
    end

    if GetCostPot() > 0 and ItemSys.GetItemData(LuaConst.Const.GB).num > tonumber(Config.GetTemplate(Config.BaseTable())["rmbRPot"]) then
        UIHelper.EnableBtn(btn_resetRight,true);
    else
        UIHelper.EnableBtn(btn_resetRight,false);
    end
end

function BtnSelectType(gameObject_)
    m_trainType = tonumber(gameObject_.name);
    for i=1,#strSprButtonTB do
        if i == m_trainType then
            GameObjectSetActive(sprButtonTb[i],true);
        else
            GameObjectSetActive(sprButtonTb[i],false);
        end
    end

end


function BtnTrainPot()
    if windowTrain ~= nil then
        GameObjectSetActive(windowTrain,true);
        RefreshTrain();
    end

end

function BtnResetPot()
    if windowReset ~= nil then
        GameObjectSetActive(windowReset,true);
        RefreshReset();
    end

end

function BtnTrainClose()
    if windowTrain ~= nil then
        GameObjectSetActive(windowTrain,false);
    end

    RefreshPotential();
end

function BtnResetClose()
    if windowReset ~= nil then
        GameObjectSetActive(windowReset,false);
    end

    RefreshPotential();
end

-- Send Message
function BtnTrainAccept()
    Hero.ReqHeroCpot(PlayerInfoData.GetCurrPlayerId(),m_trainType,OnSurplusPot);
end

function BtnTrainQuit()
    BtnTrainClose();
end

function BtnTrain(type_)
    Hero.ReqHeroPot(PlayerInfoData.GetCurrPlayerId(),m_trainType,type_,OnTrainPreview);
end

function BtnTrain1()
    if ItemSys.GetItemData(LuaConst.Const.Train).num < Config.GetProperty(Config.HeroPotTable(),"1","tp") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtTrain") });
        return;
    end

    BtnTrain(1);
end

function BtnTrain2()
    if ItemSys.GetItemData(LuaConst.Const.Train).num < Config.GetProperty(Config.HeroPotTable(),"3","tp") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtTrain") });
        return;
    end

    BtnTrain(3);
end

function BtnTrain3()
    if ItemSys.GetItemData(LuaConst.Const.Train).num < Config.GetProperty(Config.HeroPotTable(),"2","tp") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtTrain") });
        return;
    elseif ItemSys.GetItemData(LuaConst.Const.GB).num < Config.GetProperty(Config.HeroPotTable(),"2","gb") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtRMB") });
        return;
    end

    BtnTrain(2);
end

function BtnTrain4()
    if ItemSys.GetItemData(LuaConst.Const.Train).num < Config.GetProperty(Config.HeroPotTable(),"4","tp") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtTrain") });
        return;
    elseif ItemSys.GetItemData(LuaConst.Const.GB).num < Config.GetProperty(Config.HeroPotTable(),"4","gb") then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtRMB") });
        return;
    end

    BtnTrain(4);
end

function BtnResetGold()
    if ItemSys.GetItemData(LuaConst.Const.SB).num < tonumber(Config.GetTemplate(Config.BaseTable())["gRPot"]) then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtGold") });
        return;
    end

    Hero.ReqHeroRpot(PlayerInfoData.GetCurrPlayerId(),0,OnResetGold);
end

function BtnResetRMB()
    if ItemSys.GetItemData(LuaConst.Const.GB).num < tonumber(Config.GetTemplate(Config.BaseTable())["rmbRPot"]) then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtRMB") });
        return;
    end

     Hero.ReqHeroRpot(PlayerInfoData.GetCurrPlayerId(),1,OnResetRMB);
end

function OnResetGold(data_)
    print("..OnResetGold");
    m_goldGetPot = 0;
    m_rmbGetPot = 0;
    RefreshReset();
    
end

function OnResetRMB(data_)
    print("..OnResetRMB");
    m_goldGetPot = 0;
    m_rmbGetPot = 0;
    RefreshReset();

end
-- Data

function RefreshTrainBtn()
    local trainNum = ItemSys.GetItemData(LuaConst.Const.Train).num;
    local rmbNum = ItemSys.GetItemData(LuaConst.Const.GB).num;

    if trainNum >= Config.GetProperty(Config.HeroPotTable(),"1","tp") then
        UIHelper.EnableBtn(btn_train1,true);
    else
        UIHelper.EnableBtn(btn_train1,false);
    end
    if trainNum >= Config.GetProperty(Config.HeroPotTable(),"3","tp") then
        UIHelper.EnableBtn(btn_train2,true);
    else
        UIHelper.EnableBtn(btn_train2,false);
    end

    if trainNum >= Config.GetProperty(Config.HeroPotTable(),"2","tp") and rmbNum >= Config.GetProperty(Config.HeroPotTable(),"2","gb") then
        UIHelper.EnableBtn(btn_train3,true);
    else
        UIHelper.EnableBtn(btn_train3,false);
    end
    if trainNum >= Config.GetProperty(Config.HeroPotTable(),"4","tp") and rmbNum >= Config.GetProperty(Config.HeroPotTable(),"4","gb") then
        UIHelper.EnableBtn(btn_train4,true);
    else
        UIHelper.EnableBtn(btn_train4,false);
    end

end

function GetAttr(index_)

    return HeroData.GetAttr(Hero.GetHeroData2Id(PlayerInfoData.GetCurrPlayerId()),index_);
end


function GetAttrRatingName(index_)
    
    local ratingValue = GetAddRating(index_);
    for i=7,1,-1 do
        local rating,name = GetHeroRatingValue(index_,i);
        if ratingValue <= rating then
            return name;
        end
    end

end

function GetAddRating(index_)
    local addAttrTb = {};
    local addAttr = 0;
    for i=1,15 do
        addAttr = addAttr + GetAttr(i);
        if math.fmod(i,3) == 0 then
            table.insert(addAttrTb,addAttr);
            addAttr = 0;
        end
    end
    return addAttrTb[index_];
end

function GetHeroRatingValue(index_,id_)
    local ratingTb = {};
    for i=1,7 do
        local valueTb = {};
        table.insert(valueTb,Config.GetProperty(Config.HeroAttrRatingTable(),tostring(i),"shoot"));
        table.insert(valueTb,Config.GetProperty(Config.HeroAttrRatingTable(),tostring(i),"tissue"));
        table.insert(valueTb,Config.GetProperty(Config.HeroAttrRatingTable(),tostring(i),"defend"));
        table.insert(valueTb,Config.GetProperty(Config.HeroAttrRatingTable(),tostring(i),"guard"));
        table.insert(valueTb,Config.GetProperty(Config.HeroAttrRatingTable(),tostring(i),"qua"));
        table.insert(valueTb,Config.GetProperty(Config.HeroAttrRatingTable(),tostring(i),"name"));
        table.insert(ratingTb,valueTb);

    end

    return ratingTb[id_][index_],ratingTb[id_][6];
end


function OnSurplusPot(data_)
    m_currPot = m_currPot - data_['pot'];
    SetGoldGetPot(data_);
    SetRMBGetPot(data_);

end
function SetSurplusPot(data_)
    m_currPot = data_['pot'];
end

function SetGoldGetPot(data_)
    m_goldGetPot = data_['sb_r'];
end

function SetRMBGetPot(data_)
    m_rmbGetPot = data_['gb_r'];
end

function GetCostPot()
    local trainTB = Hero.GetHeroData2Id(PlayerInfoData.GetCurrPlayerId())["training"];
    local pot = 0;
    for k,v in pairs(trainTB) do
        pot = pot + v;
    end

    return pot;
end

function OnHide()


end

function OnShow()


end


function OnDestroy()
    lblAttrValueTB = {};
    sprAttrTb = {};


    UnRegisterMsgCallback();
end












