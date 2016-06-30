module("UIFormation", package.seeall)

require "Config"
require "Common/CommonScript"
require "Common/UnityCommonScript"
require "Common/Vector3"
require "Game/Hero"
require "UILua/UIAvatar"


local window = nil;
local windowComponent = nil;
local formationRoot = nil;
local dragDropRoot = nil;
local heroListRoot = nil;
local fieldCenter = nil;
local scrollRoot = nil;
local teamAvatarArr = {};
local teamAvatarTB = {};
local formationLabel = nil;
--local curSelectFrame = nil;
local pull_Btn = nil;
local curSelectName = nil;
local avatarMode1 = nil;
local avatarMode2 = nil;
local avatarMode3 = nil;
local teamPowerLabel = nil;
local starSprites = {};
local avatars = { };
local teamEffectAnim = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
end

function BindUI()
    --local record = require("CheckTimeCost").New();
    
    local transform = window.transform;
    formationRoot = TransformFindChild(transform, "Field/FormationRoot");
    dragDropRoot = TransformFindChild(formationRoot, "DragDropRoot");

    local autoBtn = TransformFindChild(transform, "Btn - Auto");
    Util.AddClick(autoBtn.gameObject, AutoLineup);

    local nationBtn = TransformFindChild(transform, "Sprite - NationBG");
    Util.AddPress(nationBtn.gameObject, ShowRelationNation);

    local clubBtn = TransformFindChild(transform, "Sprite - ClubBG");
    Util.AddPress(clubBtn.gameObject, ShowRelationClub);
    
    local btn_xunhua = TransformFindChild(transform, "CurHeroInfo/Texture - XunHua");
    function Xunhua()
        local tb = {};
        tb.playerId = avatarMode1.id;
        tb.callback = nil;
        WindowMgr.ShowWindow(LuaConst.Const.UITrainTalk, tb);
    end
    Util.AddClick(btn_xunhua.gameObject, Xunhua);
    
    function chooseForm(formId)
        Hero.SetFormId(formId);
        UpdateTeamFormation();
        Hero.SaveTeam();
    end
    
    function showUISelectFormation()
        WindowMgr.ShowWindow(LuaConst.Const.UISelectFormation, { chooseForm });
    end
    local selectFormBtn = TransformFindChild(transform, "Btn - Select");
    Util.AddClick(selectFormBtn.gameObject, showUISelectFormation);
    
    function PrevForm()
        local id = Hero.GetFormId();
        local prevId = tonumber(id) - 1;
        if (prevId < 1) then
            prevId = 12;
        end
        chooseForm(tostring(prevId));
    end
    local prevFormBtn = TransformFindChild(transform, "Btn - Select/Arrow - Left");
    Util.AddClick(prevFormBtn.gameObject, PrevForm);
    
    function NextForm()
        local id = Hero.GetFormId();
        local nextId = tonumber(id) + 1;
        if (nextId > 12) then
            nextId = 1;
        end
        chooseForm(tostring(nextId));
    end
    local nextFormBtn = TransformFindChild(transform, "Btn - Select/Arrow - Right");
    Util.AddClick(nextFormBtn.gameObject, NextForm);
    
    
    pull_Btn = TransformFindChild(transform, "Field/Mover/Pull_Btn");
    function OnHoverPullBtn()
        pull_Btn:SendMessage("OnClick");
    end
    UIHelper.AddDragOver(pull_Btn.gameObject, OnHoverPullBtn);
    
    function SwitchPrev()
        for _, v in pairs(avatars) do
            v:PrevMode();
        end
        avatarMode1:PrevMode();
        avatarMode2:PrevMode();
        avatarMode3:PrevMode();
    end

    function SwitchNext()
        for _, v in pairs(avatars) do
            v:NextMode();
        end
        avatarMode1:NextMode();
        avatarMode2:NextMode();
        avatarMode3:NextMode();
    end
    local switchPrev = TransformFindChild(transform, "CurHeroInfo/Btn - SwitchPrev");
    local switchNext = TransformFindChild(transform, "CurHeroInfo/Btn - SwitchNext");
    Util.AddClick(switchPrev.gameObject, SwitchPrev);
    Util.AddClick(switchNext.gameObject, SwitchNext);
    
    curSelectName = TransformFindChild(transform, "CurHeroInfo/Label - Name");
    
    --record:Record();
    
    local curHeroInfoRoot = TransformFindChild(transform, "CurHeroInfo");
    local teamIdArr = Hero.MainTeamHeroId();
--    local posPrefab = windowComponent:GetPrefab("UIAvatar");
    local posPrefab = Util.GetGameObject("UIAvatar");
    local clone1 = AddChild(posPrefab, curHeroInfoRoot);
    clone1.transform.localPosition = Vector3.New(1, 176, 0);
    clone1:AddComponent("UIPanel");
    UIHelper.SetPanelDepth(clone1.transform, 5);
    UIHelper.AdjustDepth(clone1, 15);
    avatarMode1 = UIAvatar.New(clone1);
    avatarMode1.mode = 1;
    avatarMode1:SwitchMode();
    avatarMode1:Warn(false);
    avatarMode1:TeamRelation(false);
    avatarMode1:Shadow(false);
    
    local clone2 = AddChild(posPrefab, curHeroInfoRoot);
    clone2.transform.localPosition = Vector3.New(-39, 167, 0);
    clone2:AddComponent("UIPanel");
    UIHelper.SetPanelAlpha(clone2, 0.3);
    UIHelper.SetPanelDepth(clone2.transform, 4);
    UIHelper.AdjustDepth(clone2, 10);
    avatarMode2 = UIAvatar.New(clone2);
    avatarMode2.mode = 2;
    avatarMode2:SwitchMode();
    avatarMode2:Warn(false);
    avatarMode2:TeamRelation(false);
    avatarMode2:Shadow(false);
    
    local clone3 = AddChild(posPrefab, curHeroInfoRoot);
    clone3.transform.localPosition = Vector3.New(42, 167, 0);
    clone3:AddComponent("UIPanel");
    UIHelper.SetPanelAlpha(clone3, 0.3);
    UIHelper.SetPanelDepth(clone3.transform, 3);
    UIHelper.AdjustDepth(clone3, 5);
    avatarMode3 = UIAvatar.New(clone3);
    avatarMode3.mode = 3;
    avatarMode3:SwitchMode();
    avatarMode3:Warn(false);
    avatarMode3:TeamRelation(false);
    avatarMode3:Shadow(false);
    
    windowComponent:AdjustSelfPanelDepth();
    --record:Print("2");
    
    teamPowerLabel = TransformFindChild(transform, "Label - Power");   
    
    for i = 1, 5 do
        starSprites[i] = TransformFindChild(transform, string.format("CurHeroInfo/Star/%d", i));
    end
    
    --record:Record();
    --BuildFormationList();
    
    local recommand = windowComponent:GetPrefab("Sprite - Recommand");
    
    --local formId = Hero.GetFormId();
    --local framePrefab = windowComponent:GetPrefab("FocusFrame");
    for i = 1, #teamIdArr do
        local clone = AddChild(posPrefab, formationRoot);
        UIHelper.AdjustDepth(clone, 1);
        clone.name = i.."";

        --local focusFrame = AddChild(framePrefab, clone.transform);
        --UIHelper.AdjustDepth(focusFrame, 3);
        --UIHelper.EnableWidget(focusFrame, false);

        Util.AddDragDrop(clone, OnDragStart, OnDrag, OnDrop, OnDragEnd, nil);
        Util.AddClick(clone, OnClickAvatar);
        UIHelper.AdjustDepth(clone, 2);
        teamAvatarArr[i] = clone;
        teamAvatarTB[clone.name] = { idx = i, id = teamIdArr[i], effect = { posFlag = nil, recommand = nil }, avatarLua = nil };
        
        local avatar = UIAvatar.New(clone);
        avatar:Init(Hero.GetHeroData2Id(teamIdArr[i]));
        avatar:Mode1();
        avatar:Warn(false);
        avatar:TeamRelation(false);
        avatars[clone.name] = avatar;
        teamAvatarTB[clone.name].avatarLua = avatar;
        
        local clone_recommand = AddChild(recommand, formationRoot);
        UIHelper.AdjustDepth(clone_recommand, 1);
        teamAvatarTB[clone.name].effect.recommand = clone_recommand;
        clone_recommand:SetActive(false);
        
        local bc = clone:AddComponent("BoxCollider");
        bc.isTrigger = true;
        bc.size = NewVector3(94, 107, 1);
    end
    --record:Print("4");

    fieldCenter = TransformFindChild(transform, "Field").localPosition;
    formationLabel = TransformFindChild(transform, "Btn - Select/Label");
    UpdateTeamFormation();

    --record:Record();
    heroListRoot = TransformFindChild(transform, "Field/Scroll View - HeroList/Mover");
    local idx = 1;
    for _, v in ipairs(Hero.GetHeroList()) do
        local flag = false;
        for i = 1, #teamIdArr do
            if (tostring(v.id) == tostring(teamIdArr[i])) then
                flag = true;
                break;
            end
        end
        if (flag == false) then
            local clone = AddChild(posPrefab, heroListRoot);
            clone.name = "UIAvatar "..string.format("%03d", idx);
            --clone:AddComponent("UIDragScrollView");
            
            UIHelper.AdjustDepth(clone, 1);

            --UIHelper.AddUIDragDropItem(clone, 1, true);

            Util.AddDragDrop(clone, OnDragStart, OnDrag, OnDrop, OnDragEnd, nil);
            Util.AddClick(clone, OnClickAvatar);
            teamAvatarTB[clone.name] = { idx = nil, id = v.id, effect = nil, avatarLua = nil };

            local bc = clone:AddComponent("BoxCollider");
            bc.isTrigger = true;
            bc.size = NewVector3(74, 74, 0);

            local avatar = UIAvatar.New(clone);
            avatar:Init(v);
            avatar:Mode1();
            avatar:Warn(false);
            avatar:TeamRelation(false);
            avatars[clone.name] = avatar;
            teamAvatarTB[clone.name].avatarLua = avatar;
            
            idx = idx + 1;
        end
    end
    --record:Print("6");
    
    RefreshHeroList();
    
    OnClickAvatar(teamAvatarArr[1]);
    
    local effectData = Hero.MainTeamEffects();
    for k, v in pairs(effectData) do
        if (v.type == 1 and v.stat == 1) then
            local texture = TransformFindChild(transform, "Texture - Club");
            Util.SetUITexture(texture, LuaConst.Const.ClubIcon, v.target.."_1", true);
            texture.localScale = Vector3.one * 0.58;
        elseif (v.type == 2 and v.stat == 1) then
            local texture = TransformFindChild(transform, "Texture - Nation");
            Util.SetUITexture(texture, LuaConst.Const.FlagIcon, v.target, true);
            texture.localScale = Vector3.one * 0.58;
        end
    end
    
    teamEffectAnim = TransformFindChild(transform, "Team");
    teamEffectAnim.gameObject:SetActive(false);
    local teamEffectRoot = TransformFindChild(transform, "TeamEffect");
    teamEffectRoot.gameObject:SetActive(false);
    --record:Record();
end

function OnDragStart(go)
    go.transform.parent = dragDropRoot;
    SetLayer(go, dragDropRoot.gameObject.layer);
    Util.MarkAsChanged(go);
    
    local formId = Hero.GetFormId();
--    local proArr = Config.GetProperty(Config.FormTable(), formId, "pro");
    local formDataTb = TableManager.FormationTbl:GetItem(formId);
    local proArr = formDataTb.ProList;
    local dragID = teamAvatarTB[go.name].id;
    local proId = Config.GetProperty(Config.HeroTable(), dragID, "ipos");
    
    for k, v in ipairs(teamAvatarArr) do
        LogManager:RedLog(tostring(k))
        local effect = teamAvatarTB[v.name].effect;
        if (effect ~= nil) then
            effect.posFlag:SetActive(true);
        
            if (tostring(proArr[k-1]) == proId) then
                effect.recommand:SetActive(true);
            end
        end
    end

--    if (teamAvatarTB[go.name].idx == nil) then
--        pull_Btn:SendMessage("OnClick");
--    end
end

function OnDrag(go, delta)
    --if (teamAvatarTB[go.name].idx ~= nil) then
        go.transform.localPosition = go.transform.localPosition + NewVector3(delta.x, delta.y);
    --end
end

function OnDrop(dropObj, dragObj)
    
    local aName = CommonScript.ReplaceClone(dropObj.name);
    local bName = CommonScript.ReplaceClone(dragObj.name);
    local a = teamAvatarTB[aName];
    local b = teamAvatarTB[bName];
    
    if (a ~= nil and b ~= nil) then
        if (a.idx ~= nil and b.idx ~= nil) then
            Hero.Swap(a.idx, b.idx);
        elseif (a.idx ~= nil) then
            Hero.SetInTeam(b.id, a.idx);
        elseif (b.idx ~= nil) then
            Hero.SetInTeam(a.id, b.idx);
        end
        Hero.SaveTeam();

        if (b.idx ~= nil) then
            dragObj.transform.parent = formationRoot;
            SetLayer(dragObj, formationRoot.gameObject.layer);
            Util.MarkAsChanged(dragObj);

            if (a.idx == nil) then
                pull_Btn:SendMessage("OnClick");
            end
        elseif (a.idx ~= nil) then
            dragObj.transform.parent = heroListRoot;
            SetLayer(dragObj, heroListRoot.gameObject.layer);
            Util.MarkAsChanged(dragObj);
        end

        if (a.idx ~= nil or b.idx ~= nil) then

            local id = a.id;
            a.id = b.id;
            b.id = id;

            a.avatarLua:Init(Hero.GetHeroData2Id(a.id));
            b.avatarLua:Init(Hero.GetHeroData2Id(b.id));

            for k, v in ipairs(teamAvatarArr) do
                local effect = teamAvatarTB[v.name].effect;
                if (effect ~= nil) then
                    effect.posFlag:SetActive(false);
                    effect.recommand:SetActive(false);
                end
            end

            UpdateTeamFormation();

            if (a.idx == nil or b.idx == nil) then
                RefreshHeroList();
            end
        end
    end
end

function OnDragEnd(go)
    --Release On Free Space, Reset Position
    if (teamAvatarTB[CommonScript.ReplaceClone(go.name)].idx ~= nil) then
        go.transform.parent = formationRoot;
        SetLayer(go, formationRoot.gameObject.layer);
        Util.MarkAsChanged(go);

        UpdateTeamFormation();
    else
        go.transform.parent = heroListRoot;
        go.layer = heroListRoot.gameObject.layer;
        Util.MarkAsChanged(go);
        
        RefreshHeroList();
    end
    
    for k, v in ipairs(teamAvatarArr) do
        local effect = teamAvatarTB[v.name].effect;
        effect.posFlag:SetActive(false);
        effect.recommand:SetActive(false);
    end
end

function ShowRelationNation(go, isPress)
    local haveData = false;
    local effectData = Hero.MainTeamEffects();
    for k, v in pairs(effectData) do
        if (v.type == 2 and v.stat == 1) then
            haveData = true;
        end
    end
    isPress = isPress and haveData;
    
    teamEffectAnim.localPosition = go.transform.localPosition;
    teamEffectAnim.gameObject:SetActive(isPress);
    if (isPress) then
        teamEffectAnim.animation:Play();
    end
    
    local teamHeroId = Hero.MainTeamHeroId();
    for k, v in pairs(Hero.MainTeamEffects()) do
        if (v.type == 2) then
            local nationRelationId = k;
            local nationId = Config.GetProperty(Config.GroupCountryTable(), nationRelationId, 'target');
            
            for i = 1, #teamHeroId do
                local avatar = teamAvatarArr[i];
                local _nationId = Config.GetProperty(Config.HeroTable(), teamHeroId[i], 'country');
                teamAvatarTB[avatar.name].avatarLua:TeamRelation(tostring(nationId) == tostring(_nationId) and isPress);
--                local focusFrame = avatar.transform:Find("FocusFrame(Clone)");
--                if (focusFrame ~= curSelectFrame) then
--                    if (tostring(nationId) == tostring(_nationId)) then
--                        UIHelper.EnableWidget(focusFrame, true and isPress);
--                    else
--                        UIHelper.EnableWidget(focusFrame, false);
--                    end
--                end
            end
            
            break;
        end
    end
    
    ShowTeamEffect(2, isPress);
end

function ShowRelationClub(go, isPress)
    local haveData = false;
    local effectData = Hero.MainTeamEffects();
    for k, v in pairs(effectData) do
        if (v.type == 1 and v.stat == 1) then
            haveData = true;
        end
    end
    isPress = isPress and haveData;
    
    teamEffectAnim.localPosition = go.transform.localPosition;
    teamEffectAnim.gameObject:SetActive(isPress);
    if (isPress) then
        teamEffectAnim.animation:Play();
    end
    
    local teamHeroId = Hero.MainTeamHeroId();
    for k, v in pairs(Hero.MainTeamEffects()) do
        if (v.type == 1) then
            local clubRelationId = k;
            local clubId = Config.GetProperty(Config.GroupClubTable(), clubRelationId, 'target');
            
            for i = 1, #teamHeroId do
                local avatar = teamAvatarArr[i];
                local _clubId = Config.GetProperty(Config.HeroTable(), teamHeroId[i], 'club');
                teamAvatarTB[avatar.name].avatarLua:TeamRelation(tostring(clubId) == tostring(_clubId) and isPress);
--                local focusFrame = avatar.transform:Find("FocusFrame(Clone)");
--                if (focusFrame ~= curSelectFrame) then
--                    local _clubId = Config.GetProperty(Config.HeroTable(), teamHeroId[i], 'club');
--                    if (tostring(clubId) == tostring(_clubId)) then
--                        UIHelper.EnableWidget(focusFrame, true and isPress);
--                    else
--                        UIHelper.EnableWidget(focusFrame, false);
--                    end
--                end
            end
            
            break;
        end
    end
    
    ShowTeamEffect(1, isPress);
end

function AutoLineup()
    local equal = Hero.AutoLineup();
    if (equal == false) then
        local teamIdArr = Hero.MainTeamHeroId();
        for i = 1, #teamAvatarArr do
            teamAvatarTB[teamAvatarArr[i].name].id = teamIdArr[i];
            teamAvatarTB[teamAvatarArr[i].name].avatarLua:Init(Hero.GetHeroData2Id(teamIdArr[i]));
        end
        UpdateTeamFormation();
        RefreshHeroList();
        Hero.SaveTeam();
    else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("BestLineup") });
    end
end

function OnClickAvatar(go)
    local id = teamAvatarTB[go.name].id;
    local heroData = Hero.GetHeroData2Id(id);
    avatarMode1:Init(heroData);
    avatarMode2:Init(heroData);
    avatarMode3:Init(heroData);
    
    UIHelper.SetLabelTxt(curSelectName, Config.GetProperty(Config.HeroTable(), id, "name"));
    
    for i = 1, heroData.star do
        UIHelper.SetSpriteName(starSprites[i], "Common_Icon_Star1");
    end
    
    for i = heroData.star + 1, 5 do
        UIHelper.SetSpriteName(starSprites[i], "Common_Icon_Star2");
    end
--    if (curSelectFrame ~= nil) then
--        UIHelper.EnableWidget(curSelectFrame, false);
--    end
--    
--    local frame = go.transform:Find("FocusFrame(Clone)");
--    if (frame ~= nil) then
--        curSelectFrame = frame;
--        UIHelper.EnableWidget(curSelectFrame, true);
--    else
--        curSelectFrame = nil;
--    end
end

function UpdateTeamFormation()
    local formId = Hero.GetFormId();
    local idArr = Hero.MainTeamHeroId();
    
    --local formData = Config.GetTemplate(Config.FormTable())[formId];
    local formData = TableManager.FormationTbl:GetItem(formId);
    local posData = Config.GetTemplate(Config.positionTB)[tostring(formData.id)];
    
    for i = 1, #teamAvatarArr do
        local pos = Vector3.New(posData[string.format("pos%d_x", i)], posData[string.format("pos%d_y", i)], 0);
        teamAvatarArr[i].transform.localPosition = pos;
        
        local proId1 = tostring(formData.ProList[i-1]);
        local proId2 = Config.GetProperty(Config.HeroTable(), idArr[i], "ipos");
        --print(string.format("%d %d %s %s", proId1, proId2, type(proId1), type(proId2)));

        local groupId1 = Config.GetProperty(Config.ProTable(), proId1, "group");
        local groupId2 = Config.GetProperty(Config.ProTable(), proId2, "group");
        
        avatars[teamAvatarArr[i].name]:Warn(groupId1 ~= groupId2);
        
        local effect = teamAvatarTB[teamAvatarArr[i].name].effect;
        effect.recommand.transform.localPosition = pos + Vector3.New(0, -56, 0);
        
        if (effect.posFlag ~= nil) then
            GameObjectDestroy(effect.posFlag);
            effect.posFlag = nil;
        end
        
        local dir = Config.GetProperty(Config.positionTB, tostring(formId), string.format("pos%d_dir", i));
        local prefab = windowComponent:GetPrefab(dir);
        local cloneDir = AddChild(prefab, formationRoot);
        UIHelper.AdjustDepth(cloneDir, 1);
        effect.posFlag = cloneDir;
        effect.posFlag.transform.localPosition = pos;
        cloneDir:SetActive(false);
        
        local label = TransformFindChild(cloneDir.transform, "Label");
        UIHelper.SetLabelTxt(label, Config.GetProperty(Config.ProTable(), proId1, "shortName"));
    end
    
    local formationName = formData.Name--["name"];
    UIHelper.SetLabelTxt(formationLabel, formationName);
    
    UIHelper.SetLabelTxt(teamPowerLabel, tostring(HeroData.GetTeamBattleScore()));
end

function RefreshHeroList()
    local teamIdArr = Hero.MainTeamHeroId();
    local idx = 1;
    for _, v in ipairs(Hero.GetHeroList()) do
        local flag = false;
        for i = 1, #teamIdArr do
            if (tostring(v.id) == tostring(teamIdArr[i])) then
                flag = true;
                break;
            end
        end
        if (flag == false) then
            local avatar = heroListRoot:Find("UIAvatar "..string.format("%03d", idx));
            if (avatar ~= nil) then
                local line = math.ceil(idx / 2);    -- two columns
                local col = idx % 2;
                local x = ((col == 1) and 59) or 185;
                local pos = NewVector3(x, -73 - (line - 1) * 109, 0);
                avatar.localPosition = pos;
                teamAvatarTB[avatar.name].id = v.id;
                teamAvatarTB[avatar.name].avatarLua:Init(v);
                idx = idx + 1;
            end
        end
    end
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    formationRoot = nil;
    dragDropRoot = nil;
    heroListRoot = nil;
    fieldCenter = nil;
    teamAvatarArr = {};
    teamAvatarTB = {};
    formationLabel = nil;
    --curSelectFrame = nil;
    curSelectName = nil;
    avatarMode1 = nil;
    avatarMode2 = nil;
    avatarMode3 = nil;
    teamPowerLabel = nil;
    starSprites = {};
    avatars = { };
    teamEffectAnim = nil;
end

function BuildFormationList()
    local root = window.transform;
    
    local selectFrame = TransformFindChild(root, "FormationList/Mover/SelectBG");

    local prefab = windowComponent:GetPrefab("Sprite - Formation");

    local allFormation = TableManager.FormationTbl:GetItemList();
--    local allFormation = Config.GetTemplate(Config.FormTable());
    local idx = 0;
    for k, v in pairs(allFormation) do
        local clone = AddChild(prefab, scrollRoot);
        local _transform = clone.transform;
        
        _transform.localPosition = Vector3.New(-444, 210 - 67 * idx, 0);

        local label_name = TransformFindChild(_transform, "Label");
        UIHelper.SetLabelTxt(label_name, v.name);
        
        function ChooseFormation(go)
            local listener = UIHelper.GetUIEventListener(go);
            selectFrame.localPosition = go.transform.localPosition + Vector3.New(0, 34, 0);
            
            Hero.SetFormId(listener.parameter.id);
            UpdateTeamFormation();
        end

        local listener = Util.AddClick(clone, ChooseFormation);
        listener.parameter = v;

        if (v.ID == Hero.GetFormId()) then
            selectFrame.localPosition = clone.transform.localPosition + Vector3.New(0, 34, 0);
        end
        
        idx = idx + 1;
    end
end

function ShowTeamEffect(teamType, active) --"1" is club "2" is country
    
    local teamEffectRoot= TransformFindChild(window.transform, "TeamEffect");
    teamEffectRoot.gameObject:SetActive(active);
    
    if (active) then
        local title         = TransformFindChild(teamEffectRoot, "Label - Title");
        local effectTitle   = TransformFindChild(teamEffectRoot, "Label - Effect");
        local effect1       = TransformFindChild(teamEffectRoot, "Label - Effect1");
        local effect2       = TransformFindChild(teamEffectRoot, "Label - Effect2");
        local effect3       = TransformFindChild(teamEffectRoot, "Label - Effect3");
        local spClub        = TransformFindChild(teamEffectRoot, "Sprite - Club");
        local spCountry     = TransformFindChild(teamEffectRoot, "Sprite - Country");

        spClub.gameObject:SetActive(teamType == 1);
        spCountry.gameObject:SetActive(teamType == 2);

        local teamEffect = {};
        for k, v in pairs(Hero.MainTeamEffects()) do
            if (v.type == teamType and v.stat == 1) then
                table.insert(teamEffect, v);
            end
        end

        function SortById(a, b)
            local aid = tonumber(a.id);
            local bid = tonumber(b.id);
            if (aid > bid) then
                return 1;
            elseif (aid == bid) then
                return 0;
            else
                return -1;
            end
        end

        if (#teamEffect > 0) then
            local sortedEffect = CommonScript.QuickSort(teamEffect, SortById);
            local activeEffect = sortedEffect[1];
            local name = Config.GetProperty(Config.GroupClubTable(), activeEffect.id, "name");
            UIHelper.SetLabelTxt(title, string.format(Util.LocalizeString("squareBrackets"), name));
            local level = Config.GetProperty(Config.GroupClubTable(), activeEffect.id, "level");
            UIHelper.SetLabelTxt(effectTitle, Util.LocalizeString("UIPlayer_TeamRelation"..level));

            local att = Config.GetProperty(Config.GroupClubTable(), activeEffect.id, "att");
            for i = 1, #att, 3 do
                local attType  = att[i];
                local attValue = att[i + 1];
                local attRate  = att[i + 2];

                local strDesc = "";
                if (attType ~= -1) then
                    local strAttName = Config.GetProperty(Config.heroAttTB, tostring(attType), "name");
                    local strAttValue = tostring(attValue);
                    if (attValue > 0) then
                        strAttValue = "+"..strAttValue;
                    end
                    local strAttRate = "";
                    if (attRate > 0) then
                        strAttRate = "%";
                    end
                    strDesc = string.format("%s%d%s", strAttName, strAttValue, strAttRate);
                end

                if (i == 1) then
                    UIHelper.SetLabelTxt(effect1, strDesc);
                elseif (i == 4) then
                    UIHelper.SetLabelTxt(effect2, strDesc);
                elseif (i == 7) then
                    UIHelper.SetLabelTxt(effect3, strDesc);
                end
            end
        else

        end
    end

    local curHeroInfoRoot = TransformFindChild(window.transform, "CurHeroInfo");
    curHeroInfoRoot.gameObject:SetActive(active == false);
end