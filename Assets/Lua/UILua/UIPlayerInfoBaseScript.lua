module("UIPlayerInfoBaseScript", package.seeall)

require "UILua/UIPlayerInfoBase"
require "UILua/UIPlayerInfoAdvanced"
require "UILua/UIPlayerInfoBreak"
require "UILua/UIPlayerInfoAttribute"
require "UILua/UIRelation"
require "UILua/UIPlayerInfoPotential"
require "UILua/UIPlayerInfoPot"
require "UILua/UIUpgradePotion"
require "UILua/UIPlayerInfoSkill"
require "Game/PlayerInfoData"


local window = nil;
local windowComponent = nil;
local windowRightPart = nil;
local windowBottomPart = nil;
local windowAttribute = nil;
local windowAdvanced = nil;
local windowUpgrade = nil;
local windowBreak = nil;
local windowSkill = nil;
local windowPlayerInfo = nil;
local windowRelation = nil;
local windowPot = nil;
local windowStar = nil;
local windowAttrItem = nil;
local statusNameUI = nil;


local lbl_page = nil;
local scene = nil;
local personRoot = nil;
local guanghuan = nil;
local person = nil;
local actionTimerId = nil;

local cloneCurHeroData = nil;
local uiLevelUpFloating = nil;
local levelupFrameAnim = nil;

function OnStart(gameObject, params)
    RegisterMsgCallback()

    PlayerInfoData.InitPlayerInfoData();
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    windowRightPart = TransformFindChild(window.transform,"RightPanel");
    windowBottomPart = TransformFindChild(window.transform, "BottomPanel");
    BindUI();
    UIPlayerInfoAdvanced.RegisterOnAdvSuccess(OnAdvSuccess);
    UIPlayerInfoBreak.RegisterOnBreakSuccess(OnBreakSuccess);
    OpenPlayerInfoUI();
    TryOpenAttributeUI();
    
    scene = Util.ChangeLevelState("Lounge_PlayerInfoBase", Vector3.right * 10);

    personRoot = GameObjectInstantiate(windowComponent:GetPrefab("PersonRoot"));
    personRoot.transform.localPosition = Vector3.New(-1.32, -0.85, 0);
    
    guanghuan = TransformFindChild(personRoot.transform, "select_people").gameObject;
    Util.SetJinJieLevel(guanghuan, GetCurrHeroData().Stage());
    guanghuan:SendMessage("Use");
    
    function OnCreate(go)
        person = go;
        go.transform.parent = personRoot.transform;
        SetLayer(go, guanghuan.layer);
        
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.New(0, -114.71, 0);
            
        local helper = person:AddComponent("RoleHelper");

        local actionArr1 = { "Special_Idle_01", "Special_Idle_02" };
        local actionArr2 = { "Special_Idle_04", "Special_Idle_04" };
        
        local actionArr = nil;
        local pro = Config.GetProperty(Config.HeroTable(), PlayerInfoData.GetCurrPlayerId(), "ipos");
        if (pro == "29") then
            actionArr = actionArr2;
        else
            actionArr = actionArr1;
        end
        
        idx = 0;
        local onceFlag = true;
        function PersonAction()
            helper:Play(actionArr[idx + 1], false);
            idx = (idx + 1) % #actionArr;

            if (onceFlag) then
                onceFlag = false;
                actionTimerId = LuaTimer.AddTimer(true, -10, PersonAction);
            end
        end
        actionTimerId = LuaTimer.AddTimer(true, 2, PersonAction);
    end
    
    CommonScript.CreatePerson(PlayerInfoData.GetCurrPlayerId(), LuaConst.Const.PlayerInfoBase, "Idle", OnCreate, "AvatarAnim");
    
    cloneCurHeroData = CommonScript.DeepCopy(GetCurrHeroData());
    
    uiLevelUpFloating = WindowMgr.Create3DUI(windowComponent:GetPrefab("UILevelUpFloating"));

end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",OnRefreshHeroData);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",OnRefreshHeroData);
end

function OnRefreshHeroData()
    local curHero = GetCurrHeroData();
    if (curHero.lv - cloneCurHeroData.lv ~= 0) then
        local prefab = windowComponent:GetPrefab("up");
        AddChild(prefab, person.transform);
        
        local tb = {};
        local length = curHero.attr1;
        for i = 1, #length do
            local value = curHero.attr1[i] - cloneCurHeroData.attr1[i];
            if (value ~= 0) then
                local valueName = Config.GetProperty(Config.heroAttTB, tostring(i - 1), "name");
                local v = "";
                if (value > 0) then
                    v = string.format("+%d", value);
                else
                    v = tostring(value);
                end
                table.insert(tb, valueName..v);
            end
        end
        
        local pos = { Vector3.New(-200, -20, 0), Vector3.New(-330, -101, 0), Vector3.New(-377, 28, 0) };
        for i = 1, 3 do
            local label = uiLevelUpFloating.transform:Find("Label"..i);
            local idx = math.random(1, #tb);
            UIHelper.SetLabelTxt(label, tb[idx]);
            UIHelper.Floating(label, pos[i], pos[i] + Vector3.New(0, 168, 0), 1);
            UIHelper.AlphaTweening(label, 1, 0, 1);
        end
        
        cloneCurHeroData = CommonScript.DeepCopy(GetCurrHeroData());
        
        levelupFrameAnim:SendMessage("Play");
    end
end

function OpenPlayerInfoUI()
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIPlayerInfo);

    windowPlayerInfo = AddChild(tempPrefab,windowRightPart);
    windowPlayerInfo.transform.localPosition = Vector3.New(-180, 0, 0);

    UIPlayerInfoBase.InitPlayerInfoBase(windowPlayerInfo.transform);
    
    levelupFrameAnim = windowPlayerInfo.transform:Find("CenterPanel/MiddlePart/LevelupFrameAnim");
    levelupFrameAnim:SendMessage("Pause");
end

function OpenAttributeUI()
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIPlayerInfoAttribute);

    windowAttribute = AddChild(tempPrefab, windowBottomPart);
    windowAttribute.transform.localPosition = Vector3.New(0, -326, 0);
    WindowMgr.AdjustLayer();

    local showAttrBtn = TransformFindChild(window.transform, "BottomPanel/Btn/BG");
    function OnShowAttr()
        showAttrBtn.parent.gameObject:SetActive(false);
    end
    Util.AddClick(showAttrBtn.gameObject, OnShowAttr);
    
    local closeBtn = TransformFindChild(windowAttribute.transform, "Close");
    function OnHideAttr()
        showAttrBtn.parent.gameObject:SetActive(true);
    end
    Util.AddClick(closeBtn.gameObject, OnHideAttr);
    
    UIPlayerInfoAttribute.InitPlayerInfoAttribute(windowAttribute.transform, windowComponent);
end

function OpenUpgradeUI()
-- windowUpgrade
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIUpgradePotion);

    windowUpgrade = AddChild(tempPrefab,windowRightPart);
    windowUpgrade.transform.localPosition = Vector3.New(-180, 0, 0);

    UIUpgradePotion.InitUpgradPotion(windowUpgrade.transform);

end

function OpenBreakUI()
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIPlayerInfoBreak);

    windowBreak = AddChild(tempPrefab,windowRightPart);
    windowBreak.transform.localPosition = Vector3.New(-180, 0, 0);

    UIPlayerInfoBreak.InitPlayerInfoBreak(windowBreak.transform);
end

function OpenSkillUI()
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIPlayerInfoSkill);

    windowSkill = AddChild(tempPrefab,windowRightPart);
    windowSkill.transform.localPosition = Vector3.New(-180, 0, 0);

    UIPlayerInfoSkill.InitPlayerInfoSkill(windowSkill.transform);
end

function OpenPotUI()
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIPot);

    windowPot = AddChild(tempPrefab,windowRightPart);
    windowPot.transform.localPosition = Vector3.New(-180, 0, 0);

    UIPlayerInfoPot.InitPlayerInfoPot(windowPot.transform);
end

function OpenAdvancedUI()
    local tempPrefab = windowComponent:GetPrefab(LuaConst.Const.UIPlayerInfoAdavenced);

    windowAdvanced = AddChild(tempPrefab,windowRightPart);
    windowAdvanced.transform.localPosition = Vector3.New(-180, 0, 0);

    UIPlayerInfoAdvanced.InitPlayerInfoAdvanced(windowAdvanced.transform);
end

function TryOpenPlayerInfoUI()

    if windowPlayerInfo == nil then
        OpenPlayerInfoUI();
    else
        windowPlayerInfo:SetActive(true);
        UIPlayerInfoBase.RefreshPlayerInfo();
     end;

end

function TryOpenAttributeUI()
    if windowAttribute == nil then
        OpenAttributeUI();
    else
        windowAttribute:SetActive(true);
        UIPlayerInfoAttribute.RefreshAttribute();
    end;
end

function TryOpenTeamUI()
    if (windowRelation == nil) then
        local tempPrefab = windowComponent:GetPrefab("Relation");

        windowRelation = AddChild(tempPrefab,windowRightPart);
        windowRelation.transform.localPosition = NewVector3(0, -77, 0);

        WindowMgr.AdjustLayer();        
    else
        windowRelation:SetActive(true);
    end

    UIRelation.Init(windowRelation, windowComponent);

end

function TryCloseAttributeUI()
    if windowAttribute ~= nil then
        windowAttribute:SetActive(false);
     end;

end

function TryCloseEquipUI()

end

function TryCloseTeamUI()
    if (windowRelation ~= nil) then
        windowRelation:SetActive(false);
    end
end

function TryOpenUpgradeUI()
    if windowUpgrade ~= nil then 
        GameObjectSetActive(windowUpgrade.transform,true);
        UIUpgradePotion.RefreshUpgradeInfo();
    else
        OpenUpgradeUI();
    end

    UIPlayerInfoBase.IsCommonCourtBg(true);
end

function TryCloseUpgradeUI()
    if windowUpgrade ~= nil then
        GameObjectSetActive(windowUpgrade.transform,false);
    end

end

function TryOpenBreakUI()
    if windowBreak == nil then
        OpenBreakUI();
    else
        windowBreak:SetActive(true);
        UIPlayerInfoBreak.RefreshBreak();
    end

    statusNameUI = "Break";
    UIPlayerInfoBase.IsCommonCourtBg(true);
end

function TryCloseBreakUI()
    if windowBreak ~= nil then
        windowBreak:SetActive(false);
    end

end

function TryOpenAdvancedUI()
    if windowAdvanced == nil then
        OpenAdvancedUI();
    else
        windowAdvanced:SetActive(true);
        UIPlayerInfoAdvanced.RefreshAdvanced();
    end

    statusNameUI = "Advance";
    UIPlayerInfoBase.IsCommonCourtBg(true);
end

function TryCloseAdvancedUI()
    if windowAdvanced ~= nil then
        windowAdvanced:SetActive(false);
    end
end

function RequestOpenPotUI()
    TryOpenPotUI();
end

function TryOpenPotUI(data_)
     if windowPot == nil then
        OpenPotUI();
    else
        windowPot:SetActive(true);
    end

    statusNameUI = "Pot";
    UIPlayerInfoBase.IsCommonCourtBg(false);
end

function TryOpenSkillUI() 
    if windowSkill == nil then
        OpenSkillUI();
    else
        windowSkill:SetActive(true);
    end

    statusNameUI = "Skill";
    UIPlayerInfoBase.IsCommonCourtBg(false);
--    YellowPrint("TryOpenSkillUI: ".. TableManager.SkillTbl:GetItem(10001).Name);
--    YellowPrint("SkillLvUp: "..Config.GetProperty(Config.HeroSkillLvUp(),"1","item_id"));
end

function TryCloseSkillUI()
    if windowSkill ~= nil then
        windowSkill:SetActive(false);
    end
end


function TryClosePotUI()
    if windowPot ~= nil then
        windowPot:SetActive(false);
    end
end


function BindUI()
    windowStar = windowComponent:GetPrefab("Star");
    windowAttrItem = windowComponent:GetPrefab("ItemAttr");
end

function GetWindowStar()
    return windowStar;
end

function GetWindowAttrItem()
    return windowAttrItem;
end

function ToggleEvolve(name_)
    if name_ == "Upgrade" then
        TryOpenUpgradeUI();
        TryCloseAdvancedUI();
        TryCloseBreakUI();
        TryCloseSkillUI();
        TryClosePotUI();
        TryCloseTeamUI();
    elseif name_ == "Advance" then
        TryOpenAdvancedUI();
        TryCloseUpgradeUI();
        TryCloseBreakUI();
        TryCloseSkillUI();
        TryClosePotUI();
        TryCloseTeamUI();
    elseif name_ == "Break" then
        TryOpenBreakUI();
        TryCloseUpgradeUI();
        TryCloseAdvancedUI();
        TryCloseSkillUI();
        TryClosePotUI();
        TryCloseTeamUI();
    elseif name_ == "Skill" then
        TryCloseUpgradeUI();
        TryCloseAdvancedUI();
        TryCloseBreakUI();
        TryOpenSkillUI();
        TryClosePotUI();
        TryCloseTeamUI();
    elseif name_ == "Pot" then
        TryCloseUpgradeUI();
        TryCloseBreakUI();
        TryCloseAdvancedUI();
        TryCloseSkillUI();
        RequestOpenPotUI();
        TryCloseTeamUI();
    elseif name_ == "Relation" then
        TryCloseUpgradeUI();
        TryCloseAdvancedUI();
        TryCloseBreakUI();
        TryCloseSkillUI();
        TryClosePotUI();
        TryOpenTeamUI();
    else
        TryCloseUpgradeUI();
        TryCloseAdvancedUI();
        TryCloseBreakUI();
        TryCloseSkillUI();
        TryClosePotUI();
        TryCloseTeamUI();
    end
    
end

function ShowPlayerAttribute()
    window:SetActive(true);

    TryOpenPlayerInfoUI();
    TryOpenAttributeUI();
    TryCloseAdvancedUI();
    TryCloseBreakUI();
    TryCloseSkillUI();
    TryClosePotUI();
end

function BtnLeftArrow()
    local index = UIPlayerListScript.GetCurrPlayerIndex();
    if index <= 1 then
        return;
    end
    index = index - 1;

    ResetPlayerId(index);
end

function BtnRightArrow()
    local index = UIPlayerListScript.GetCurrPlayerIndex();
    if index >= UIPlayerListScript.GetMaxLivingPlayer() then
        return;
    end
    index = index + 1;

    ResetPlayerId(index);
end

function ResetPlayerId(newIndex_)
    local playerId = UIPlayerListScript.GetPlayerId2Index(newIndex_);
    UIPlayerListScript.SetCurrPlayerId(playerId);
    UIPlayerListScript.SetCurrPlayerIndex(newIndex_);

    RefreshArrowUI();
    UIPlayerInfoBase.RefreshPlayerInfo();

    UIHelper.SetLabelTxt(lbl_page,GetPage());
end

function RefreshArrowUI()
    if statusNameUI == "Advance" then
        UIPlayerInfoAdvanced.RefreshAdvanced();
    elseif statusNameUI == "Break" then
        UIPlayerInfoBreak.RefreshBreak();
    elseif statusNameUI == "Skill" then
        TryOpenSkillUI();
    elseif statusNameUI == "Pot" then
        RequestOpenPotUI();
    end
end

function GetSelectFontColor(args_)
    if args_ == 1 then
        return Color.New(102/255,204/255,255/255,1);
    elseif args_ == 2  then
        return Color.New(255/255,255/255,255/255,1);
    end

end

function GetOffFontColor(args_)
    if args_ == 1 then
        return Color.New(125/255,128/255,137/255,1);
    elseif args_ == 2  then
        return Color.New(171/255,173/255,185/255,1);
    end

end
-- ------Data
function GetPage()
    local page = UIPlayerListScript.GetCurrPlayerIndex().. "/".. #Hero.GetHeroList();

    return page;
end


function GetCurrHeroData()
    return Hero.GetHeroData2Id(PlayerInfoData.GetCurrPlayerId());
end

function GetWindowComponent()
    return windowComponent;
end

function ExitPlayerInfoBase()

    windowComponent:Close();
end

function OnShow()
    UIUpgradePotion.OnShow();
    UIPlayerInfoPot.OnShow();

    TryOpenPlayerInfoUI();
    RefreshArrowUI();
    GameObjectSetActive(scene.transform,true)
end

function OnHide()
    UIUpgradePotion.OnHide();
    UIPlayerInfoPot.OnHide();
    GameObjectSetActive(scene.transform,false)
end

function OnDestroy()
    windowAttribute = nil;
    windowAdvanced = nil;
    windowUpgrade = nil;
    windowBreak = nil;
    windowSkill = nil;
    windowPlayerInfo = nil;
    windowRelation = nil;
    windowPot = nil;
    
    GameObjectDestroy(scene);
    scene = nil;
    
    GameObjectDestroy(personRoot);
    personRoot = nil;
    
    GameObjectDestroy(guanghuan);
    guanghuan = nil;
    
    GameObjectDestroy(person);
    person = nil;
    
    GameObjectDestroy(uiLevelUpFloating);
    uiLevelUpFloating = nil;
    
    LuaTimer.RmvTimer(actionTimerId);
    actionTimerId = nil;

    UIPlayerInfoBase.OnDestroy();
    UIPlayerInfoAttribute.OnDestroy();
    UIPlayerInfoBreak.OnDestroy();
    UIPlayerInfoAdvanced.OnDestroy();
    UIUpgradePotion.OnDestroy();
    UIPlayerInfoPot.OnDestroy();
    UIPlayerInfoPot.OnDestroy();
    PlayerInfoData.OnDestroy();
    UIRelation.OnDestroy();

    UnRegisterMsgCallback();
end

local jinjiePanelClone = nil;
local jinjieAnimClone  = nil;

function OnAdvSuccess()
    local jinjiePanel = windowComponent:GetPrefab("JinJiePanel");
    local jinjieAnim  = windowComponent:GetPrefab("JinJieAnim");
    
    jinjiePanelClone  = WindowMgr.Create3DUI(jinjiePanel);
    jinjieAnimClone   = GameObjectInstantiate(jinjieAnim);
    Util.SetJinJieLevel(jinjieAnimClone, GetCurrHeroData().Stage());
    jinjieAnimClone:SendMessage("Use");
        
    local label_name = TransformFindChild(jinjiePanelClone.transform, "R -> L/Label - Name");
    local heroName = Config.GetProperty(Config.HeroTable(), GetCurrHeroData().id, "name");
    UIHelper.SetLabelTxt(label_name, Hero.ColorHeroName(GetCurrHeroData().adv, heroName));
    
    local sprite_line = TransformFindChild(jinjiePanelClone.transform, "R -> L/Sprite - 2");
    UIHelper.SetWidgetColor(sprite_line, HeroData.GetHeroRankColor(GetCurrHeroData()));
    
    jinjiePanelClone.animation:Play();
    jinjieAnimClone.animation:Play();
    
    function Hide2DUI()        
        WindowMgr.ActiveUICamera(false);
        scene:SetActive(false);
        personRoot:SetActive(false);
        
        function OnCreate(clonePerson)
            SetLayer(clonePerson, jinjieAnimClone.layer);
            clonePerson.transform.parent = jinjieAnimClone.transform;
            clonePerson.transform.localPosition = Vector3.zero;
            clonePerson.transform.localEulerAngles = Vector3.zero;
            
            local helper = clonePerson:AddComponent("RoleHelper");
            
            function PersonAction()
                helper:Play("QiuYuanShengJi", false);
            end
            LuaTimer.AddTimer(true, 2, PersonAction);
        end

        CommonScript.CreatePerson(PlayerInfoData.GetCurrPlayerId(), LuaConst.Const.PlayerInfoBase, "Idle", OnCreate, "AvatarAnim");
    end
    
    LuaTimer.AddTimer(true, 0.5, Hide2DUI);
    
    function DestroyEffect()
        GameObjectDestroy(jinjiePanelClone);
        jinjiePanelClone = nil;
        
        GameObjectDestroy(jinjieAnimClone);
        jinjieAnimClone = nil;
        
        scene:SetActive(true);
        personRoot:SetActive(true);
        
        Util.SetJinJieLevel(guanghuan, GetCurrHeroData().Stage());
        guanghuan:SendMessage("Use");
        
        WindowMgr.ActiveUICamera(true);
    end
    
    LuaTimer.AddTimer(true, 4, DestroyEffect);
end

local shenxingPanelClone = nil;
local shenxingAnimClone = nil;

function OnBreakSuccess()
    local shenxingPanel = windowComponent:GetPrefab("ShenXingPanel");
    local shenxingAnim  = windowComponent:GetPrefab("ShenXingAnim");
    
    shenxingPanelClone  = WindowMgr.Create3DUI(shenxingPanel);
    shenxingAnimClone   = GameObjectInstantiate(shenxingAnim);
    
    local addStar = TransformFindChild(shenxingPanelClone.transform, "L -> R/ExplosePanel/Star6");
    local max = GetCurrHeroData().slv;
    for i = 1, 5 do
        local star = TransformFindChild(shenxingPanelClone.transform, "L -> R/Star"..i);
        
        if (i == max + 1) then
            addStar.localPosition = star.localPosition;
        end
        if (i <= max) then
            UIHelper.SetSpriteNameNoPerfect(star, "Common_Icon_Star_Big1");
        else
            UIHelper.SetSpriteNameNoPerfect(star, "Common_Icon_Star_Big2");
        end
    end

    shenxingPanelClone.animation:Play();
    shenxingAnimClone.animation:Play();
    
    function Hide2DUI()        
        WindowMgr.ActiveUICamera(false);
        scene:SetActive(false);
        personRoot:SetActive(false);
        
        function OnCreate(clonePerson)
            SetLayer(clonePerson, shenxingAnimClone.layer);
            clonePerson.transform.parent = shenxingAnimClone.transform;
            clonePerson.transform.localPosition = Vector3.zero;
            clonePerson.transform.localEulerAngles = Vector3.zero;
            
            local helper = clonePerson:AddComponent("RoleHelper");
            
            function PersonAction()
                helper:Play("QiuYuanShengJi", false);
            end
            LuaTimer.AddTimer(true, 2, PersonAction);
        end

        CommonScript.CreatePerson(PlayerInfoData.GetCurrPlayerId(), LuaConst.Const.PlayerInfoBase, "Idle", OnCreate, "AvatarAnim");
    end
    
    LuaTimer.AddTimer(true, 0.5, Hide2DUI);
    
    function DestroyEffect()
        GameObjectDestroy(shenxingPanelClone);
        shenxingPanelClone = nil;
        
        GameObjectDestroy(shenxingAnimClone);
        shenxingAnimClone = nil;
        
        scene:SetActive(true);
        personRoot:SetActive(true);
        
        UIPlayerInfoBreak.RefreshBreak();
        WindowMgr.ActiveUICamera(true);
    end
    
    LuaTimer.AddTimer(true, 5, DestroyEffect);
end
