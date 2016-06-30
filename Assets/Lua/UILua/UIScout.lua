module("UIScout", package.seeall)

require "Common/UnityCommonScript"
require "UILua/UIScoutAvatar"
require "Game/PageComponent"

local window = nil;
local windowComponent = nil;
local tweenPanel = nil;
local animRoot = nil;

local gacha_bg = nil;
local uiHead = nil;
local scoutRoot = nil;

local oneScout = {};
oneScout.root = nil;
oneScout.label_cd = nil;
oneScout.label_price = nil;
oneScout.label_free = nil;
oneScout.__index = oneScout;

local scoutItems = {};
local page = nil;
local scoutConfig = nil;
local topPanel = nil;
local animHexParent = nil;
local animHexRoot = nil;
local clickIdx = 0;
local againFlag = false;
local lastScoutParams = nil;
local lastScoutMsgId = nil;

function NewScoutItem(idx)

    local t =
    {
        logs       = nil,
        logs2      = nil,
        data       = nil,
        data1      = nil,
        --actTimerID = 0;
        hexRoot    = nil,
        timerID    = 0,
        hexRootPos = nil;
    };
    setmetatable(t, oneScout);
    
    function t:IsAct()
        return tostring(t.data.isAct or 0) ~= "0";
    end

    function t:IsFree()
        if (self.logs.freeTime + t.data.DTime * 1000 <= os.time() * 1000) then
            if (t.data.DFree - self.logs.freeNum > 0) then
                return true;
            end
        end

        return false;
    end
    
    function t:Init(root)
        self.root = root;
        local label_name        = TransformFindChild(window.transform, "Root/Label - Name"..idx);
        self.label_cd           = TransformFindChild(root, "Block1/Label");
        self.hexRoot            = TransformFindChild(root, "Block1/HexRoot");
        local sprite_BG         = TransformFindChild(root, "Block1/HexRoot/Sprite - BG");
        local actRoot           = TransformFindChild(root, "Block1/HexRoot/Act");
        local label_desc        = TransformFindChild(root, "Block1/HexRoot/Label - Desc");
        local sprite_currency   = TransformFindChild(root, "Block1/AutoCenter/Sprite - Currency");
        local label_cost        = TransformFindChild(root, "Block1/AutoCenter/Label - Cost");
        local sprite_currency2  = TransformFindChild(root, "Block2/AutoCenter/Sprite - Currency");
        local label_desc1       = TransformFindChild(root, "Block2/Label - Desc1");
        local label_desc2       = TransformFindChild(root, "Block2/Label - Desc2");
        self.label_free         = TransformFindChild(root, "Block2/Label - Free");
        local label_cost1       = TransformFindChild(root, "Block2/AutoCenter/Label - Price");
        local sprite_currency3  = TransformFindChild(root, "Block2/AutoCenter2/Sprite - Currency");
        local label_cost2       = TransformFindChild(root, "Block2/AutoCenter2/Label - Price");
        
        local btn_buy1          = TransformFindChild(root, "Block2/Btn - Buy1").gameObject;
        local btn_buy2          = TransformFindChild(root, "Block2/Btn - Buy2").gameObject;
        
        function t:OnClick(one)
            if (one) then
                local param = {};
                param.id = t.data._id;
                DataSystemScript.RequestWithParams(LuaConst.Const.ScoutOne, param, MsgID.tb.ScoutOne);

                if (t:IsFree()) then
                    t.logs.freeNum = t.logs.freeNum + 1;
                    t.logs.freeTime = os.time() * 1000;
                end
                t.logs.buyNum = t.logs.buyNum + 1;

                t:OnGetScoutData();
                
                lastScoutParams = param;
                lastScoutMsgId = MsgID.tb.ScoutOne;
            else
                local param = {};
                param.id = t.data1._id;
                DataSystemScript.RequestWithParams(LuaConst.Const.ScoutOne, param, MsgID.tb.ScoutTen);
                
                lastScoutParams = param;
                lastScoutMsgId = MsgID.tb.ScoutTen;
            end
            
            animHexParent = self.hexRoot.parent;
            animHexRoot = self.hexRoot;
            clickIdx = idx;
        end

        function BuyOne()
            t:OnClick(true);
        end

        function BuyTen()
            t:OnClick(false);
        end

        AddOrChangeClickParameters(btn_buy1, BuyOne);
        AddOrChangeClickParameters(btn_buy2, BuyTen);
        
        UIHelper.SetLabelTxt(label_name, t.data.name);
        
        UIHelper.SetSpriteName(sprite_BG, t.data.icon);

        if (t:IsAct()) then
            GameObjectSetActive(actRoot, true);
        else
            GameObjectSetActive(actRoot, false);
        end
        
        UIHelper.SetLabelTxt(label_desc, t.data.desc);
        
        if (t.data.mkey == LuaConst.Const.SB) then
            UIHelper.SetSpriteName(sprite_currency, "Icon_Money");
            UIHelper.SetSpriteName(sprite_currency2, "Icon_Money");
            UIHelper.SetSpriteName(sprite_currency3, "Icon_Money");
        elseif (t.data.mkey == LuaConst.Const.GB) then
            UIHelper.SetSpriteName(sprite_currency, "Icon_RMB");
            UIHelper.SetSpriteName(sprite_currency2, "Icon_RMB");
            UIHelper.SetSpriteName(sprite_currency3, "Icon_RMB");
        end
        
        UIHelper.SetLabelTxt(label_cost, tostring(t.data.mval));
        UIHelper.SetLabelTxt(label_cost1, tostring(t.data.mval));
        UIHelper.SetLabelTxt(label_cost2, tostring(t.data1.mval));
        
        local sprite1 = TransformFindChild(root, "Block2/SpriteBG/Sprite - 1");
        local sprite2 = TransformFindChild(root, "Block2/SpriteBG/Sprite - 2");
        local sprite3 = TransformFindChild(root, "Block2/SpriteBG/Sprite - 3");
        local sprite4 = TransformFindChild(root, "Block2/SpriteBG/Sprite - 4");
        
        local color = nil;
        if (idx == 1) then
            color = Color.New(204 / 255, 204 / 255, 242 / 255, 0.1);
            UIHelper.SetSpriteName(sprite3, "Detailed_bg1");
        elseif (idx == 2) then
            color = Color.New(178 / 255, 150 / 255,  82 / 255, 0.1);
            UIHelper.SetSpriteName(sprite3, "Detailed_bg2");
        else
            color = Color.New(217 / 255, 186 / 255, 140 / 255, 0.1);
            UIHelper.SetSpriteName(sprite3, "Detailed_bg3");
        end
        
        UIHelper.SetWidgetColor(sprite1, color);
        UIHelper.SetWidgetColor(sprite2, color);
        if (idx == 1) then
            color.a = 0.4;
        else
            color.a = 0.6;
        end
        UIHelper.SetWidgetColor(sprite4, color);
        
        UIHelper.SetLabelTxt(label_desc1, t.data.desc);
        UIHelper.SetLabelTxt(label_desc2, t.data1.desc);
    end

--    function OnRefreshActTime()
--        local delta = (t.data.ETime - os.time() * 1000) / 1000;
--
--        if (delta <= 0) then
--            LuaTimer.RmvTimer(t.actTimerID);
--        else
--            local h = math.floor(delta / 60 / 60);
--            local m = math.floor((delta - h * 60 * 60) / 60);
--            local s = math.floor(delta % 60);
--
--            local str = string.format("%02d:%02d:%02d", h, m, s);
--            UIHelper.SetLabelTxt(t.label_cd, str);
--            UIHelper.SetLabelTxt(t.label_free, str);
--        end
--    end

    function t:OnGetScoutData()
        local param1 = Util.LocalizeString("dayFree");
        local param2 = t.data.DFree - self.logs.freeNum;
        local param3 = t.data.DFree;

        if (t:IsAct()) then
            LuaTimer.RmvTimer(self.timerID);
            self.timerID = LuaTimer.AddTimer(false, -1, t.OnUpdate);
            
            local tilEndTime = (t.data.ETime - os.time() * 1000) / 1000;
            local h = math.floor(tilEndTime / 60 / 60);
            local m = math.floor((tilEndTime - h * 60 * 60) / 60);
            local s = tilEndTime % 60;
            
            local str1 = string.format("[FF6666]"..Util.LocalizeString("UIScoutTip2"), string.format("[-]%02d:%02d:%02d", h, m, s));
            local str2 = string.format(Util.LocalizeString("UIScoutTip1").."[-]", string.format("%02d:%02d:%02d[FF6666]", h, m, s))
            UIHelper.SetWidgetColor(t.label_cd, "win_wb_18");
            UIHelper.SetWidgetColor(t.label_free, "win_wb_18");
            UIHelper.SetLabelTxt(t.label_cd, str1);
            UIHelper.SetLabelTxt(t.label_free, str2);
        else
            local str = "";
            if (t:IsFree()) then
                str = Util.LocalizeString("free");
                UIHelper.SetWidgetColor(t.label_cd, "win_w_20");
                UIHelper.SetWidgetColor(t.label_free, "win_w_20");
            elseif (t.data.DFree - self.logs.freeNum > 0) then
                LuaTimer.RmvTimer(self.timerID);
                self.timerID = LuaTimer.AddTimer(false, -1, t.OnUpdate);
                str = string.format("%s%d/%d", param1, param2, param3);
                UIHelper.SetWidgetColor(t.label_cd, "win_g_18");
                UIHelper.SetWidgetColor(t.label_free, "win_g_18");
            else
                str = Util.LocalizeString("nofreeTimes");
                UIHelper.SetWidgetColor(t.label_cd, "win_w_20");
                UIHelper.SetWidgetColor(t.label_free, "win_w_20");
            end

            UIHelper.SetLabelTxt(t.label_cd, str);
            UIHelper.SetLabelTxt(t.label_free, str);
        end

        if (self.logs.buyNum >= t.data.DNums) then
            --limit, disable btn
        end
    end

    function t.OnUpdate()
        
        if (t:IsAct()) then
            local tilEndTime = (t.data.ETime - os.time() * 1000) / 1000;
            local h = math.floor(tilEndTime / 60 / 60);
            local m = math.floor((tilEndTime - h * 60 * 60) / 60);
            local s = tilEndTime % 60;
            
            local str1 = string.format("[FF6666]"..Util.LocalizeString("UIScoutTip2"), string.format("[-]%02d:%02d:%02d", h, m, s));
            local str2 = string.format(Util.LocalizeString("UIScoutTip1").."[-]", string.format("%02d:%02d:%02d[FF6666]", h, m, s))
            UIHelper.SetLabelTxt(t.label_cd, str1);
            UIHelper.SetLabelTxt(t.label_free, str2);
        else
            local nextFree = t.logs.freeTime + t.data.DTime * 1000;
            local delta = (nextFree - os.time() * 1000) / 1000;
            local str = "";
            if (delta <= 0) then
                LuaTimer.RmvTimer(t.timerID);
                if (t.data.DFree - t.logs.freeNum > 0) then
                    str = Util.LocalizeString("free");
                else
                    str = Util.LocalizeString("nofreeTimes");
                end
            else
                local h = math.floor(delta / 60 / 60);
                local m = math.floor((delta - h * 60 * 60) / 60);
                local s = delta % 60;

                str = string.format("%02d:%02d:%02d", h, m, s);

                UIHelper.SetWidgetColor(t.label_cd, "win_g_18");
                UIHelper.SetWidgetColor(t.label_free, "win_g_18");
            end
            UIHelper.SetLabelTxt(t.label_cd, str);
            UIHelper.SetLabelTxt(t.label_free, str);
        end
    end

    function t:OnDestroy()
        --LuaTimer.RmvTimer(self.actTimerID);
        LuaTimer.RmvTimer(t.timerID);
    end

    return t;
end

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
    
    uiHead = GameObject.Find("UIHead(Clone)");

    Hero.animScoutOne = AnimScoutOne;
    Hero.animScoutTen = AnimScoutTen;
    Hero.RegisterRefreshScoutCB(InitRefreshScout);
    Hero.TryGetScoutConfig();
end

function OnDestroy()
    Hero.animScoutOne = nil;
    Hero.animScoutTen = nil;
    Hero.RegisterRefreshScoutCB(nil);

    for i,v in ipairs(scoutItems) do
        v:OnDestroy();
    end
    
    animRoot = nil;
    tweenPanel = nil;
    topPanel = nil;
    animHexParent = nil;
    animHexRoot = nil;
    
    ReleaseScoutEffect();
    
    uiHead = nil;
    
    page = nil;
end

function BindUI()
    local transform = window.transform;
    
    tweenPanel = TransformFindChild(transform, "Root");
    animRoot = TransformFindChild(transform, "AnimRoot");
    topPanel = TransformFindChild(transform, "TopPanel");
    
    local btn_l = TransformFindChild(transform, "Root/Btn - L").gameObject;
    function Prev()
        page:PrevPage();
        Refresh();
    end
    Util.AddClick(btn_l, Prev);
    
    local btn_r = TransformFindChild(transform, "Root/Btn - R").gameObject;
    function Next()
        page:NextPage();
        Refresh();
    end
    Util.AddClick(btn_r, Next);
end

function InitRefreshScout(scoutData)
    page = PageComponent.New(Refresh);
    page:SetMax(math.ceil(#scoutData / 2));
    scoutConfig = scoutData;
    scoutRoot = {};
    
    for i = 1, 3 do
        local root = TransformFindChild(window.transform, string.format("Root/Scout%d/ScoutMover", i));
        table.insert(scoutRoot, root);
    end
    
    if (#scoutConfig % 2 ~= 0) then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIScoutTip3") });
    else
        Refresh();
    end
end

function Refresh()
    local default = { totNum = 0, buyNum = 0, buyTime = 0, freeNum = 0, freeTime = 0 };
    for k = 1, 6, 2 do
        
        local idx = math.ceil(k / 2);
        
        local clone = scoutRoot[idx];
        
        local oneScout = scoutItems[idx];
        if (oneScout == nil) then
            oneScout = NewScoutItem(idx);
            table.insert(scoutItems, oneScout);
        else
            oneScout:OnDestroy();
        end
        
        local i = k + (page.cur - 1) * 6;
        if (i <= #scoutConfig) then
            clone.gameObject:SetActive(true);
            oneScout.data  = scoutConfig[i];
            oneScout.data1 = scoutConfig[i + 1];
            oneScout.logs  = scoutConfig[k].logs or default;
            oneScout.logs2 = scoutConfig[k + 1].logs or default;
            oneScout:Init(clone);
            oneScout:OnGetScoutData();
        else
            local label_name = TransformFindChild(window.transform, "Root/Label - Name"..idx);
            UIHelper.SetLabelTxt(label_name, "");
            clone.gameObject:SetActive(false);
        end
    end
end

function OnHide()
    ReleaseScoutEffect();
end

function OnShow()
end

function AnimScoutOne(data)
    Animate(data, {"center"}, animRoot, "AnimPosAndAlpha", "1Anim");
end

function AnimScoutTen(data)
    Animate(data, {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}, animRoot, "AnimScaleAndAlpha", "10Anim");
end

local newCachedRootNames = nil;
function Animate(data, rootNames, animRoot, rootAnimPrefabName, clipName)
    newCachedRootNames = rootNames;
    
    local bools = {};
    for i = 1, #data do
        bools[i] = false;
        local d = data[i];
        local type = d["type"];
        if (type == "hero") then
            local rateName = HeroData.GetHeroRatingName(d["id"]);
            if (rateName == "S") then
                bools[i] = true;
            end
        end
    end
    
    local avatarPrefab = Util.GetGameObject("UIAvatarScout");
    local rootAnimPrefab = windowComponent:GetPrefab(rootAnimPrefabName);
    for i = 1, #data do
        local parent = TransformFindChild(animRoot, rootNames[i]);
        
        for i = 1, parent.childCount do
            local child = parent:GetChild(i - 1);
            GameObjectDestroy(child.gameObject);
        end
        
        local anim  = AddChild(rootAnimPrefab, parent)
        anim.transform.localPosition = Vector3.zero;
        local clone = AddChild(avatarPrefab, anim.transform);
        clone.transform.localPosition = Vector3.zero;
        
        local avatarData = UIScoutAvatar.New(clone);
        if (data[i]["type"] == "item") then
            local heroId = Config.GetProperty(Config.ItemTable(), data[i].id, "useArgs");
            avatarData:Init(heroId);
            avatarData:ToggleFragment(true);
        elseif (data[i]["type"] == "hero") then
            avatarData:Init(data[i].id);
            avatarData:ToggleFragment(false);
        end
    end
    
    windowComponent:AdjustSelfPanelDepth();
    
    for i = 1, #data do
        local parent = TransformFindChild(animRoot, rootNames[i]);
        parent.gameObject:SetActive(false);
    end
    
    if (gacha_bg == nil) then
        local prefab_gacha_hero_bg = windowComponent:GetPrefab("gachaBG");
        gacha_bg = GameObjectInstantiate(prefab_gacha_hero_bg);
        gacha_bg.transform.localPosition = Vector3.New(0, 0, -17.08);
        gacha_bg.transform.localScale = Vector3.New(2.5, 2.5, 1);
    end
    gacha_bg:SetActive(false);
    
    local _3dUI = nil;
    local hero = nil;
    function ShowImportant(idx)
        gacha_bg:SetActive(true);
        gacha_bg.animation:Play();
        --gacha_bg.transform:Find("gacha_ten").animation:Play();
        
        local heroId = nil;
        if (data[idx].type == "item") then
            heroId = Config.GetProperty(Config.ItemTable(), data[idx].id, "useArgs");
        elseif (data[idx].type == "hero") then
            heroId = data[idx].id;
        else
            heroId = "";
        end
        
        function onCreate(go)
            hero = go;
            SetLayer(hero, gacha_bg.layer);
            hero.transform.position = Vector3.New(0.3, -0.833, -17.929);
            hero.transform.localEulerAngles = Vector3.New(-3.637207, -169.9796, -0.6422119);
            hero.transform.localScale = Vector3.New(0.99, 0.99, 0.99);
            
            _3dUI = WindowMgr.Create3DUI(windowComponent:GetPrefab("Panel"));
            local root          = _3dUI.transform;
            local spEvaluate    = TransformFindChild(root, "Sprite - Evaluate");
            local lbEvaluate    = TransformFindChild(root, "Label - Evaluate");
            local lbName        = TransformFindChild(root, "Label - Name");
            local lbPro         = TransformFindChild(root, "Label - Pro");
            local texCountry    = TransformFindChild(root, "Texture - Country");
            local texClub       = TransformFindChild(root, "Texture - Club");
            
            UIHelper.SetSpriteNameNoPerfect(spEvaluate, HeroData.GetHeroRatingName(heroId));
            UIHelper.SetLabelTxt(lbEvaluate, "("..HeroData.GetHeroRating(heroId)..")");
            
            local heroName = Config.GetProperty(Config.HeroTable(), heroId, "name");
            UIHelper.SetLabelTxt(lbName, heroName);
            
            local proId = Config.GetProperty(Config.HeroTable(), heroId, "ipos");
            local proName = Config.GetProperty(Config.ProTable(), proId, "shortName");
            UIHelper.SetLabelTxt(lbPro, proName);
            
            Util.SetUITexture(texCountry, LuaConst.Const.FlagIcon, HeroData.GetHeroCountryIcon(heroId), false);
            Util.SetUITexture(texClub, LuaConst.Const.ClubIcon, HeroData.GetHeroClubIcon(heroId), false);
            
            local group = tonumber(Config.GetProperty(Config.ProTable(), proId, "group"));
            local anim = "";
            local ballVisible = true;
            if (group <= 3) then
                anim = "ChouKa_QianFeng";
                ballVisible = true;
            elseif (group <= 8) then
                anim = "QiuYuanXuanZe_02";
                ballVisible = true;
            elseif (group <= 11) then
                anim = "Special_Idle_02";
                ballVisible = false;
            elseif (group == 12) then
                anim = "ChouKa_ShouMenYuan";
                ballVisible = true;
            end
            
            hero:AddComponent("RoleHelper"):Play(anim, ballVisible);
        end
        
        CommonScript.CreatePerson(heroId, LuaConst.Const.Scout, "Idle", onCreate, "ScoutAnim");
    end

    function HideImportant()
        if (hero ~= nil) then
            GameObjectDestroy(hero);
            hero = nil;
        end
        
        if (_3dUI ~= nil) then
            GameObjectDestroy(_3dUI);
            _3dUI = nil;
        end
        
        gacha_bg:SetActive(false);
    end
    
    function Tween1()
        if (clickIdx == 1) then
            topPanel.localPosition = Vector3.New(-300, -26);
        elseif (clickIdx == 2) then
            topPanel.localPosition = Vector3.New(0, -26);
        else
            topPanel.localPosition = Vector3.New(300, -26);
        end
        
        animHexRoot = AddChild(animHexRoot.gameObject, topPanel);
        animHexRoot.transform.localPosition = Vector3.zero;
        animHexRoot.transform:Find("Texture - OutLight").gameObject:SetActive(true);
        topPanel:Find("Texture").gameObject:SetActive(true);
        
        uiHead:SendMessage("MoveUpwardHide");
        tweenPanel.animation:Play("FadeOut");
    end
    
    if (againFlag == false) then
        LuaTimer.AddTimer(true, 0.5, Tween1);
    end
    
    function Tween2()
        topPanel.gameObject:SendMessage("Begin");
    end
    
    if (againFlag == false) then
        LuaTimer.AddTimer(true, 0.75, Tween2);
    end
    
    function Tween3()
        animHexRoot.animation:Play("FlyAway");
    end
    
    if (againFlag == false) then
        LuaTimer.AddTimer(true, 1.25, Tween3);
    end
    
    local btn_again_one = TransformFindChild(animRoot, "Btn - AgainOne").gameObject;
    local btn_again_ten = TransformFindChild(animRoot, "Btn - AgainTen").gameObject;
    local btn_confirm = TransformFindChild(animRoot, "Btn - Confirm").gameObject;
    local shadowSprite = TransformFindChild(animRoot, "Sprite - Shadow").gameObject;
    
    function Tween4()
        --[[animHexRoot.parent = animHexParent;
        animHexRoot.localPosition = Vector3.zero;
        Util.MarkAsChanged(animHexRoot.gameObject);]]--
        GameObjectDestroy(animHexRoot);
        animHexRoot = nil;
        topPanel:Find("Texture").gameObject:SetActive(false);

        --topPanel.gameObject:SendMessage("Refresh");
        UIScoutAnim.BeginAnimate(animRoot.gameObject, clipName, bools, rootNames, ShowImportant, HideImportant);
    end
    
    function CloseScoutAnim()
        againFlag = false;
        lastScoutParams = nil;
        lastScoutMsgId = nil;
        btn_again_one:SetActive(false);
        btn_again_ten:SetActive(false);
        btn_confirm:SetActive(false);
        shadowSprite:SetActive(false);
        
        for i = 1, #newCachedRootNames do
            local parent = TransformFindChild(animRoot, newCachedRootNames[i]);
        
            for j = 1, parent.childCount do
                local child = parent:GetChild(j - 1);
                GameObjectDestroy(child.gameObject);
            end
        end
        
        uiHead:SendMessage("RevertOrgPos");
        tweenPanel.animation:Play("FadeIn");
        
        HideImportant();
    end
    
    if (againFlag == false) then
        LuaTimer.AddTimer(true, 1.85, Tween4);
    else
        Tween4();
    end
    
    function BuyAgain()
        againFlag = true;
        DataSystemScript.RequestWithParams(LuaConst.Const.ScoutOne, lastScoutParams, lastScoutMsgId);
    end
    
    AddOrChangeClickParameters(btn_again_one, BuyAgain);
    AddOrChangeClickParameters(btn_again_ten, BuyAgain);
    AddOrChangeClickParameters(btn_confirm, CloseScoutAnim);
end

function ReleaseScoutEffect()
    
    if (gacha_bg ~= nil) then
        GameObjectDestroy(gacha_bg);
    end
    gacha_bg = nil;
    
end
