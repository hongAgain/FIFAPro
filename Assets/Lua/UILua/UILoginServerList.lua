module("UILoginServerList", package.seeall);

require "Common/UnityCommonScript"
require "Game/Role"
require "Game/DataSystemScript"

local strLocalizeServerIndexSufix = "ServerIndexSufix";
local strLocalizeServerStatusFull = "ServerStatusFull";
local strLocalizeServerStatusHalf = "ServerStatusHalf";
local strLocalizeServerStatusEmpty = "ServerStatusEmpty";

local serverListBoxPrefabName = "UILoginServerList";
local serverListBox = nil;
local LatestServerPrefabName = "LatestServerItem";
local LatestServerListRoot = nil;
local LatestServerListContainer = nil;
-- local LatestServers = nil;

local uiButtonServers = {};
local uiButtonServerGroups = {};
local uiButtonLatestServers = {};

--local uiButtonFloatArrow = nil;
--local uiButtonPrevPage = nil;
--local uiButtonNextPage = nil;

--use a parameter sid
local delegateOnSelectServer = nil;

local selectedFlag1 = nil;
local selectedFlag2 = nil;

function CreateSelectServerListBox( containerTransform, windowComponent, delegateonselectserver)
	-- body
	if(serverListBox == nil) then
		--instantiate one
		local serverListBoxPrefab = windowComponent:GetPrefab(serverListBoxPrefabName);
		--instantiate prefab and initialize it
		serverListBox = AddChild(serverListBoxPrefab, containerTransform);
    	serverListBox.transform.localPosition = Vector3.zero;

    	delegateOnSelectServer = delegateonselectserver;
        LatestServerListRoot = TransformFindChild(serverListBox.transform, "LatestServerList");
        LatestServerListContainer = TransformFindChild(serverListBox.transform, "LatestServerList/Container");
        
        selectedFlag1 = TransformFindChild(serverListBox.transform, "ServerList/Sprite - Selected");
        selectedFlag2 = TransformFindChild(serverListBox.transform, "LatestServerList/Container/Sprite - Selected");

        --uiButtonFloatArrow = TransformFindChild(serverListBox.transform,"ButtonArrowRight");
--		uiButtonPrevPage = TransformFindChild(serverListBox.transform,"ButtonArrowUp");
--		uiButtonNextPage = TransformFindChild(serverListBox.transform,"ButtonArrowDown");
--		Util.AddClick(uiButtonPrevPage.gameObject,OnClickPrevServerGroup);
--		Util.AddClick(uiButtonNextPage.gameObject,OnClickNextServerGroup);
        
        uiButtonServers = {};
        local serverItemRoot = TransformFindChild(serverListBox.transform, "ServerList");
        local serverItemPrefab = windowComponent:GetPrefab("ServerItem");
    	for i = 1, Login.NR_PER_PAGE do
    		local uiButtonServer = AddChild(serverItemPrefab, serverItemRoot).transform;
            local x = i - 1 - math.floor((i - 1) / 3) * 3;
            local y = math.floor((i - 1) / 3);
            uiButtonServer.localPosition = Vector3.New(-342 + x * 236, 157 - y * 62, 0);
    		-- local buttonServerListener = Util.AddClick(uiButtonServer.gameObject, delegateonselectserver);
    		-- buttonServerListener.parameter = i;
            uiButtonServers[i] = uiButtonServer;
    	end
        
        uiButtonServerGroups = {};
        local serverGroupPrefab = windowComponent:GetPrefab("ServerGroupItem");
        local serverGroupRoot = TransformFindChild(serverListBox.transform, "ServerGroupList");
    	for i = 1, Login.GetMaxPageNum() do
    		local uiButtonServerGroup = AddChild(serverGroupPrefab, serverGroupRoot).transform;
            uiButtonServerGroup.localPosition = Vector3.New(0, 130 - (i - 1) * 68, 0);
    		-- local buttonServerGroupListener = Util.AddClick(uiButtonServer.gameObject,delegateonselectserver);
    		-- buttonServerListener.parameter = i;
            uiButtonServerGroups[i] = uiButtonServerGroup;
    	end
	end

	--activate it
	if(not GameObjectActiveSelf(serverListBox)) then
    	GameObjectSetActive(serverListBox.transform, true);
    end

	SetServers();
    Login.RegisterPageChanged(SetServers);

    local latestServerTable = Login.GetRecentLoginServerRole();
    if (latestServerTable ~= nil) then
        selectedFlag2.gameObject:SetActive(true);
        local latestServerPrefab = windowComponent:GetPrefab(LatestServerPrefabName);
        
        for i,v in ipairs(latestServerTable) do
            
            local clone = nil;
            if (i <= #uiButtonLatestServers) then
                clone = uiButtonLatestServers[i];
            else
                clone = AddChild(latestServerPrefab, LatestServerListContainer);
                clone.transform.localPosition = Vector3.zero;
                uiButtonLatestServers[i] = clone;
            end

            local uiLatestServerButton = TransformFindChild(clone.transform, "Button");
            local uiLatestServerName   = TransformFindChild(clone.transform, "ServerName");
            local uiLatestServerStatus = TransformFindChild(clone.transform, "ServerStatus");
            local uiLatestServerLevel  = TransformFindChild(clone.transform, "Level");

            UIHelper.SetLabelTxt(uiLatestServerName,v.sname);
            --use a active value for this
            if(v.sid == Login.GetServerSum()) then
                UIHelper.SetLabelTxt(uiLatestServerStatus, Util.LocalizeString(strLocalizeServerStatusFull));
                UIHelper.SetWidgetColor(uiLatestServerStatus, "win_r_24");
            elseif(v.sid == Login.GetServerSum() - 1) then
                UIHelper.SetLabelTxt(uiLatestServerStatus, Util.LocalizeString(strLocalizeServerStatusHalf));
                UIHelper.SetWidgetColor(uiLatestServerStatus, "win_o_24");
            else
                UIHelper.SetLabelTxt(uiLatestServerStatus, Util.LocalizeString(strLocalizeServerStatusEmpty));
                UIHelper.SetWidgetColor(uiLatestServerStatus, "win_g_24");
            end
            
            UIHelper.SetLabelTxt(uiLatestServerLevel, "Lv."..v.lv);
            UIHelper.SetDragScrollViewTarget(uiLatestServerButton, LatestServerListRoot);
            Util.AddClick(uiLatestServerButton.gameObject, OnClickServerButton);
            AddOrChangeClickParameters( uiLatestServerButton.gameObject, OnClickServerButton, { host = nil,
                                                                                                sid = v.sid,
                                                                                                sname = v.sname } );
            
            if (v.sid == DataSystemScript.GetRegionId()) then
                selectedFlag2.parent = clone.transform;
                selectedFlag2.localPosition = Vector3.New(60, 0, 0);
            end
        end
        UIHelper.RepositionGrid(LatestServerListContainer, LatestServerListRoot);
        UIHelper.RefreshPanel(LatestServerListRoot);
    else
        --GameObjectSetActive(uiButtonFloatArrow.gameObject,false);
        selectedFlag2.gameObject:SetActive(false);
    end
end

function SetServers()
    -- body
    --local ServerPageNum = Login.GetMaxPageNum();
    --local ServerPageCur = Login.GetCurPage();
    local ServersInThisPage = Login.GetCurPageSrvInfo();
    local ServerNumInThisPage = #ServersInThisPage;
    
    --set info
    for i = 1, #uiButtonServers do
        if(i <= ServerNumInThisPage) then
            GameObjectSetActive(uiButtonServers[i].gameObject, true);
            local serverName = TransformFindChild(uiButtonServers[i], "ServerName");
            local serverStatus = TransformFindChild(uiButtonServers[i], "ServerStatus");
            UIHelper.SetLabelTxt(serverName, ServersInThisPage[i].name);
            if (i == Login.GetServerSum()) then
                UIHelper.SetLabelTxt(serverStatus, Util.LocalizeString(strLocalizeServerStatusFull));
                UIHelper.SetWidgetColor(serverStatus, "win_r_24");
            elseif (i == Login.GetServerSum() - 1) then
                UIHelper.SetLabelTxt(serverStatus, Util.LocalizeString(strLocalizeServerStatusHalf));            
                UIHelper.SetWidgetColor(serverStatus, "win_o_24");
            else
                UIHelper.SetLabelTxt(serverStatus, Util.LocalizeString(strLocalizeServerStatusEmpty));            
                UIHelper.SetWidgetColor(serverStatus, "win_g_24");
            end
            AddOrChangeClickParameters( serverName.gameObject, OnClickServerButton, { host = ServersInThisPage[i].host,
                                                                        sid = ServersInThisPage[i].id,
                                                                        sname = ServersInThisPage[i].name});
        else
            GameObjectSetActive(uiButtonServers[i].gameObject, false);
        end
    end

    --set info
    local strServerIndexSufix = Util.LocalizeString(strLocalizeServerIndexSufix);
    for i = 1, #uiButtonServerGroups do
        local serverGroupName = TransformFindChild(uiButtonServerGroups[i], "ServerGroupName");
        local serverGroupBG = TransformFindChild(uiButtonServerGroups[i], "ServerGroupBG");
        local minSidInGroup = tostring(math.max((i - 1) * Login.NR_PER_PAGE, 1));
        local maxSidInGroup = tostring(i * Login.NR_PER_PAGE);
        UIHelper.SetLabelTxt(serverGroupName, string.format(strServerIndexSufix, minSidInGroup, maxSidInGroup));
        AddOrChangeClickParameters( serverGroupBG.gameObject, OnClickServerGroupButton, { page = i });
    end

    --set page buttons
    --GameObjectSetActive(uiButtonPrevPage.gameObject,(minPage > 1));
    --GameObjectSetActive(uiButtonNextPage.gameObject,(maxPage < ServerPageNum));
    
    local srvId = DataSystemScript.GetRegionId();
    local pageIdx = math.ceil(srvId / Login.NR_PER_PAGE);
    for i = 1, #uiButtonServerGroups do
        local serverGroupName = TransformFindChild(uiButtonServerGroups[i], "ServerGroupName");
        local serverGroupBG = TransformFindChild(uiButtonServerGroups[i], "ServerGroupBG");
        if (pageIdx == i) then
            UIHelper.SetWidgetColor(serverGroupName, "win_b_24");
            UIHelper.SetWidgetColor(serverGroupBG, "win_b_24");
        else
            UIHelper.SetWidgetColor(serverGroupName, "win_wa_24");
            UIHelper.SetWidgetColor(serverGroupBG, "win_wa_24");
        end
    end
    
    local idxInPage = srvId - (pageIdx - 1) * Login.NR_PER_PAGE;
    selectedFlag1.parent = uiButtonServers[idxInPage];
    selectedFlag1.localPosition = Vector3.New(19, 23, 0);
    
    for i = 1, #uiButtonServers do
        local serverName = TransformFindChild(uiButtonServers[i], "ServerName");
        
        if (idxInPage == i) then
            local serverStatus = TransformFindChild(uiButtonServers[i], "ServerStatus");

            UIHelper.SetWidgetColor(serverName, "win_b_24");
            UIHelper.SetWidgetColor(serverStatus, "win_b_24");
        else
            UIHelper.SetWidgetColor(serverName, Color.New(1, 1, 1, 1));
        end
    end
end

function OnClickServerButton(go)
    --print("OnClickServerButton");
    local listener = UIHelper.GetUIEventListener(go);
    if(listener == nil) then
        print ("listener is nil");
    end

    delegateOnSelectServer(listener.parameter.host, listener.parameter.sid, listener.parameter.sname);
    if (GameObjectActiveSelf(serverListBox)) then
        GameObjectSetActive(serverListBox.transform, false);
    end
end

function OnClickServerGroupButton(go)
    --print("OnClickServerGroupButton");
    local listener = UIHelper.GetUIEventListener(go);
    local page = listener.parameter.page;
    Login.SwitchToPage(page);
end

--function OnClickPrevServerGroup()
--    print("OnClickPrevServerGroup");
--    Login.SwitchToPrevPage();
--end
--
--function OnClickNextServerGroup()
--    print("OnClickNextServerGroup");
--    Login.SwitchToNextPage();
--end

function OnDestroy()
    -- body
    Login.UnRegisterPageChanged(SetServers);
    for _, v in ipairs(uiButtonServers) do
        GameObjectDestroy(v);
    end
    uiButtonServers = nil;
    
    for _, v in ipairs(uiButtonServerGroups) do
        GameObjectDestroy(v);
    end
    uiButtonServerGroups = nil;
    
    delegateOnSelectServer = nil;
    
    serverListBox = nil;
    LatestServerListRoot = nil;
    LatestServerListContainer = nil;
    
    selectedFlag1 = nil;
    selectedFlag2 = nil;
end