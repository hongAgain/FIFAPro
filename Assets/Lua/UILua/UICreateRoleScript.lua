module("UICreateRoleScript", package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/HeroData"
require "Game/Role"
require "UILua/UIAvatar"
require "UILua/UIScoutAvatar"
require "UILua/UICircleListManager"

local function SortById(a, b)
    local aid = tonumber(a.id);
    local bid = tonumber(b.id);
    if (aid < bid) then
        return -1;
    elseif (aid == bid) then
        return 0;
    else
        return 1;
    end
end

local window = nil;
local windowComponent = nil;

local sortedIconTB = CommonScript.QuickSort(Config.GetTemplate(Config.IconTable()), SortById);

local curStep = nil;
local step = { go = nil, prefab = nil };
step.__index = step;
function step:Switch()
    if (self.go == nil) then
        self.go = AddChild(self.prefab, window.transform);
        self:OnInit();
    else
        self.go:SetActive(true);
        self:Active();
    end
    
    if (curStep ~= nil) then
        curStep:DeActive();
        curStep.go:SetActive(false);
    end
    
    curStep = self;
end

local step1 = { scene = nil, coaches = {}, selected = nil };
setmetatable(step1, step);

local step2 = { selected = nil };
setmetatable(step2, step);

local step3 = { ChooseName = nil, ChooseClubIcon = nil };
setmetatable(step3, step);

local step4 = { };
setmetatable(step4, step);

function step1:OnInit()
    self.scene = Util.ChangeLevelState("SelectCoach", Vector3.New(1.93, 0, 5.249));
    local cameraSaver = windowComponent:GetPrefab("CameraSaver");
    local cameraSaverClone = AddChild(cameraSaver, self.scene.transform);
    
    local coachInfo =
    {
        { id = "1004", pos = Vector3.New(-0.2557784, 0, 2.508741), rot = Vector3.New(0, 102.6247, 0), default = "idle_01", special = "special_01" },
        { id = "1001", pos = Vector3.New(0.325, 0, 0.366), rot = Vector3.New(0, 24.37092, 0), default = "idle_02", special = "special_02" },
        { id = "1010", pos = Vector3.New(-1.347, 0, 1.233), rot = Vector3.New(0, 102.8578, 0), default = "idle_03", special = "special_03" }
    };
    for i = 1, #coachInfo do
        function onCreate(go)
            go.transform.position = coachInfo[i].pos;
            go.transform.eulerAngles = coachInfo[i].rot;
            
            self.coaches[i] = go:AddComponent("CoachHelper");
            self.coaches[i]:InitUniform(i);
        end
        Util.CreateCoach(coachInfo[i].id, onCreate, coachInfo[i].default, "CoachSelectAnim");
    end
    
    local prevRoot = self.go.transform:Find("BL");
    prevRoot.gameObject:SetActive(false);
    
    local prevBtn = self.go.transform:Find("BL/Button");
    function Prev()
        prevRoot.gameObject:SetActive(false);
        local component = cameraSaverClone:GetComponent("CameraSaver");
        component.enabled = false;
        component.enabled = true;
        Camera.main.animation:Stop();
        for j = 1, 3 do
            if (self.selected ~= coachInfo[j].id) then
                self.coaches[j]:FadeIn();
            end
        end
    end
    Util.AddClick(prevBtn.gameObject, Prev);
    
    local nextBtn = self.go.transform:Find("BR/Button");
    function Next()
        Role.CreateMyRole(step1.selected);
    end
    UIHelper.SetButtonActive(nextBtn, false, true);
    Util.AddClick(nextBtn.gameObject, Next);
    local lastIdx = nil;
    
    for i = 1, 3 do
        function onClick()
            if (lastIdx ~= nil) then
                self.scene.transform:Find("show0"..lastIdx).gameObject:SetActive(false);
            end
            self.selected = coachInfo[i].id;
            lastIdx = i;
            self.scene.transform:Find("show0"..lastIdx).gameObject:SetActive(true);
            --print(self.selected);
            UIHelper.SetButtonActive(nextBtn, true, true);
            self.coaches[i]:PlayAnim(coachInfo[i].special, coachInfo[i].default);
            
            prevRoot.gameObject:SetActive(true);
            Camera.main.animation:Play("camera"..string.format("%02d", i));
            
            for j = 1, 3 do
                if (self.selected ~= coachInfo[j].id) then
                    self.coaches[j]:FadeOut();
                end
            end
        end
        local btn = self.go.transform:Find(tostring(i));
        Util.AddClick(btn.gameObject, onClick);
    end
end

function step1:Active()
    self.scene:SetActive(true);
    for i = 1, #self.coaches do
        self.coaches[i].gameObject:SetActive(true);
    end
end

function step1:DeActive()
    if (self.scene ~= nil) then
        self.scene:SetActive(false);
    end
    for i = 1, #self.coaches do
        self.coaches[i].gameObject:SetActive(false);
    end
end

function step1:OnRelease()
    self.scene = nil;
end

function step2:OnInit()
    local prevBtn = self.go.transform:Find("BL/Button");
    function Prev()
        step1:Switch();
    end
    Util.AddClick(prevBtn.gameObject, Prev);
    
    local nextBtn = self.go.transform:Find("BR/Button");
    function Next()
        step3:Switch();
    end
    Util.AddClick(nextBtn.gameObject, Next);
    
    local line_h = self.go.transform:Find("Line - H");
    local line_v = self.go.transform:Find("Line - V");
    
    local scrollViewRoot = self.go.transform:Find("Right Anchor/Scroll View");
    local gridRoot = self.go.transform:Find("Right Anchor/Scroll View/UIGrid");
    --local scrollItem = windowComponent:GetPrefab("CityItem");
    local cityData = Config.GetTemplate(Config.initAreaTB);
    local circleList = UICircleListManager.New();
    
    function CreateItem( randomIndex, key, value, itemNameTrans )
        UIHelper.SetLabelTxt(itemNameTrans, value.name);
    end
    
    --[[for i = 1, 34 do
        local city = cityData[tostring(i)];
        local clone = AddChild(scrollItem, gridRoot);
        clone.name = tostring(i);
        local label = clone.transform:Find("Label");
        UIHelper.SetLabelTxt(label, city.name);
    end]]--
    
    local line = self.go.transform:Find("Line"):GetComponent("UISprite");
    local cityLabel = self.go.transform:Find("Label - City");
    function onCenter(centerObj)
        local centerData = centerObj:GetComponent("UIEventListener").parameter.data;
        --local city = cityData[centerData.id];
        self.selected = centerData.id;
        UIHelper.TweenPositionBegin(line_h, 0.3, Vector3.New(0, tonumber(centerData.pos_y), 0));
        UIHelper.TweenPositionBegin(line_v, 0.3, Vector3.New(tonumber(centerData.pos_x), 0, 0));
        UIHelper.SetLabelTxt(cityLabel, "");
        
        function Acting(factor)
            line.fillAmount = factor;
        end
        
        function OnDone()
            UIHelper.SetLabelTxt(cityLabel, centerData.name);
        end
        UILuaTween.Begin(line.gameObject, 0.5, Acting, OnDone);
    end
    
    circleList:CreateUICircleList(scrollViewRoot, gridRoot, cityData,
    {
        OnCreateItem = CreateItem,
        OnClickItem = nil,
        OnSelectByDrag = nil,
    });
    
    UIHelper.OnCenterItem(gridRoot, onCenter);
end

function step2:Active()
end

function step2:DeActive()
end

function step2:OnRelease()
end

function step3:OnInit()
    local root = self.go.transform;
    
    local input = TransformFindChild(root, "InputName");
    local clubIcon = TransformFindChild(root, "ClubIcon");
    local btn_Random = TransformFindChild(root, "Btn_Random");
    local btn_L = TransformFindChild(root, "Btn - L");
    local btn_R = TransformFindChild(root, "Btn - R");
    local prev = TransformFindChild(root, "BL/Button");
    local next = TransformFindChild(root, "BR/Button");
    
    function OnPrev()
        step2:Switch();
    end
    Util.AddClick(prev.gameObject, OnPrev);
    
    function OnNext()
        local content = UIHelper.InputTxt(input);
        if (content == nil or string.len(content) == 0) then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("kongming") });
        else
            local bRet = TableWordFilter:FilterText(content)
            content = TableWordFilter.FilteredWords
            if true ==  bRet then
                WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("feifazhi") });
            else
                self.ChooseName = content;
                Role.RoleDetail(step3.ChooseName, step2.selected, step3.ChooseClubIcon);
            end
        end
    end
    Util.AddClick(next.gameObject, OnNext);
    
    function AutoNameCN()
        local size1 = 0;
        local size2 = 0;
        for k,v in pairs(Config.GetTemplate(Config.AutoNameTeam())) do
            size1 = size1 + 1;
        end
        for k,v in pairs(Config.GetTemplate(Config.AutoNamePlayer())) do
            size2 = size2 + 1;
        end

        local str1 = Config.GetProperty(Config.AutoNameTeam(), tostring(math.random(size1)), 'fNameCN');
        local str2 = Config.GetProperty(Config.AutoNamePlayer(), tostring(math.random(size2)), 'sNameCN');
        
        UIHelper.SetInputText(input, str1..str2);
    end
    Util.AddClick(btn_Random.gameObject, AutoNameCN);
    
    AutoNameCN();
    
    local idx = #sortedIconTB;

    function UpdateIcon()
        self.ChooseClubIcon = sortedIconTB[idx].id;
        Util.SetUITexture(clubIcon, LuaConst.Const.ClubIcon, sortedIconTB[idx].id.."_2", true);
    end
    
    function ChooseLeft()
        idx = idx + 1;
        idx = (idx - 1) % #sortedIconTB + 1;
        UpdateIcon();
    end
    Util.AddClick(btn_L.gameObject, ChooseLeft);
    
    function ChooseRight()
        idx = idx - 1;
        idx = (idx - 1) % #sortedIconTB + 1;
        UpdateIcon();
    end
    Util.AddClick(btn_R.gameObject, ChooseRight);
    
    UpdateIcon();
end

function step3:Active()
end

function step3:DeActive()
end

function step3:OnRelease()
    
end

function step4:OnInit()
    local rootPrefab = windowComponent:GetPrefab("Root");
    local rootClone  = GameObjectInstantiate(rootPrefab);
    rootClone.transform.parent = self.go.transform;
    rootClone.transform.localScale = NewVector3(1, 1, 1);
    local step4Root  = rootClone.transform;
    local avatarPrefab = Util.GetGameObject("UIAvatar_CreateRole");
    --local glow1 = windowComponent:GetPrefab("glow1");
    
    --local uiCamera = GameObject.Find("UI Root/Camera"):GetComponent("Camera");
    --local effectCamera = GameObject.Find("UI 3D Camera"):GetComponent("Camera");
    
    local heroIdArr = CommonScript.DeepCopy(Config.GetProperty(Config.InitTeam(), "1", "hero"));
    
    for i = 1, 11 do
        local root = TransformFindChild(step4Root, string.format("%02d", i));
        local cloneAvatar = AddChild(avatarPrefab, root);
        cloneAvatar.transform.localPosition = Vector3.zero;
        
        local heroData = {};
        heroData.id = tostring(heroIdArr[i]);
        heroData.lv = 1;
        heroData.adv = 0;
        heroData.slv = 0;
        heroData.training = {};
        
        HeroData.CalcAttr(heroData);

        local uiavatar = UIAvatar.New(cloneAvatar);
        uiavatar:Mode1();
        uiavatar:Warn(false);
        uiavatar:TeamRelation(false);
        uiavatar:Shadow(false);
        uiavatar:Init(heroData);
        
--        if (i < 11) then
--            local particle = GameObjectInstantiate(glow1);
--            particle.transform.parent = root;
--            Util.SyncObjPos(cloneAvatar, particle, uiCamera, effectCamera);
--        end
        
        if (i == 11) then
            local scoutAvatarGO = TransformFindChild(step4Root, "UIAvatarScout");
            local scoutAvatar = UIScoutAvatar.New(scoutAvatarGO);
            scoutAvatar:ToggleFragment(false);
            scoutAvatar:Init(heroData.id);
        end
    end
    
    windowComponent:AdjustSelfPanelDepth();
    
    function Login()
        Role.GetRoleData();
        windowComponent:Close();
    end
    
    step4Root.animation:Play();
    LuaTimer.AddTimer(false, 5, Login);
end

function step4:Active()
end

function step4:DeActive()
end

function step4:OnRelease()
    
end

function CreateTeamEffect()
    Util.ClearLevelState();
    OnRelease();
    step4:Switch();
end

function OnStart(gameObject, params)
	window = gameObject;
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    
    Role.onInitRoleSuccess = CreateTeamEffect;
    
    step1.prefab = windowComponent:GetPrefab("Step1");
    step2.prefab = windowComponent:GetPrefab("Step2");
    step3.prefab = windowComponent:GetPrefab("Step3");
    step4.prefab = windowComponent:GetPrefab("Step4");
    
    if (params.startIdx == 1) then
        step1:Switch();
    else
        step2:Switch();
    end
end

function OnRelease()
    step1:OnRelease();
    step2:OnRelease();
    step3:OnRelease();
    step4:OnRelease();
end

function OnDestroy()
    Role.onInitRoleSuccess = nil;
    curStep = nil;
end
