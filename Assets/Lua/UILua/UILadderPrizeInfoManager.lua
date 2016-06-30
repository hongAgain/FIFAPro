module("UILadderPrizeInfoManager", package.seeall)

require "Common/UnityCommonScript"
require "Game/PVPMsgManager"
require "UILua/UIGetItemScript"

local ladderRankInfoSettings = {
	LadderRankPrizeUIPrefabName = "LadderPrizeInfo",
	LadderRankPrizeUIPrefab = nil,

	nameColorSelected = Color.New(255/255,255/255,255/255,255/255),
	nameColorNormal = Color.New(125/255,128/255,137/255,255/255),
	nameSizeSelected = 22,
	nameSizeNormal = 18,
	nameScaleSelected = NewVector3(0.5,0.5,0.5),
	nameScaleNormal = NewVector3(0.4,0.4,0.4),

	LadderChestHint = "LadderChestHint",
	LadderChestPreview = "LadderChestPreview"
}

local prizeStatus = {
	UNREACHED = {icon = "11001_1"},
	AVAILABLE={icon = "11001_2"},
	OBTAINED={icon = "11001_3"}
};

local ladderRankPrizeUI = nil;

local rewards = {};
--0 as min,
local minLevelOnThisGroup = nil;
local maxLevel = nil;
local currentLadderLevelGroup = nil;

local buttonNext = nil;
local buttonPrev = nil;

function CreatePrizeInfo( containerTransform, windowcomponent )
	if(ladderRankPrizeUI == nil) then
		--Get prefabs
		ladderRankInfoSettings.LadderRankPrizeUIPrefab = windowcomponent:GetPrefab(ladderRankInfoSettings.LadderRankPrizeUIPrefabName);
		
		--generate ui and initialize it
		ladderRankPrizeUI = GameObjectInstantiate(ladderRankInfoSettings.LadderRankPrizeUIPrefab);
		ladderRankPrizeUI.transform.parent = containerTransform;
    	ladderRankPrizeUI.transform.localPosition = Vector3.zero;
    	ladderRankPrizeUI.transform.localScale = Vector3.one;

		BindUI();
	end

	--active it
	if(not GameObjectActiveSelf(ladderRankPrizeUI)) then
    	GameObjectSetActive(ladderRankPrizeUI.transform,true);
    end

	SetInfo();
end

function RefreshInfo()
	if(ladderRankPrizeUI ~= nil) then
		SetInfo();
	end
end

function BindUI()

	rewards = {};
	for i=1,3 do
		rewards[i] = {};
		rewards[i].transform = TransformFindChild(ladderRankPrizeUI.transform,"Reward"..i);
		rewards[i].gameObject = rewards[i].transform.gameObject;
		rewards[i].Name = TransformFindChild(rewards[i].transform,"Name");
		rewards[i].IconRoot = TransformFindChild(rewards[i].transform,"IconRoot");
		rewards[i].Icon = TransformFindChild(rewards[i].IconRoot,"Icon");
		rewards[i].selectedLight = TransformFindChild(rewards[i].transform,"Selected");
	end
	
	buttonNext = TransformFindChild(ladderRankPrizeUI.transform,"ButtonNext");
	buttonPrev = TransformFindChild(ladderRankPrizeUI.transform,"ButtonPrev");
	AddOrChangeClickParameters(buttonNext.gameObject,OnClickButtonNext,nil);
	AddOrChangeClickParameters(buttonPrev.gameObject,OnClickButtonPrev,nil);

end

function SetInfo()

	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	local localData = Config.GetTemplate(Config.LadderLevel());

	if(maxLevel == nil)then
		maxLevel = 0;
		for k,v in pairs(localData) do
			maxLevel = maxLevel+1;
		end
		maxLevel = maxLevel-1;
	end

	--temp process
	SetPrizes(1);
	-- SetPrizes(infoData.info.level-infoData.info.level%3 +1);
	-- SetPrizes(infoData.info.level-infoData.info.level%3);
end

function SetPrizes(minLv)


	if(minLevelOnThisGroup==minLv)then
		return;
	end
	minLevelOnThisGroup = minLv;
	SetPrize( 1, minLv );
	SetPrize( 2, minLv+1 );
	SetPrize( 3, minLv+2 );
	CheckButtons();
end

function CheckButtons()
	GameObjectSetActive(buttonPrev,(minLevelOnThisGroup-3 >= 1));
	GameObjectSetActive(buttonNext,(minLevelOnThisGroup+3 <= maxLevel));	
end

function SetPrize( index, level )

	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	local maxranklevel = infoData.info.maxlv;
	local theMaxLevelOfAll = maxLevel;
	local rewardTable = infoData.info.advlog;

	--precheck the max rank
	if(level > theMaxLevelOfAll) then
		GameObjectSetActive(rewards[index].transform,false);
		return;
	else
		GameObjectSetActive(rewards[index].transform,true);
	end
	
	if(level>maxranklevel) then
		--levels never reached yet, show chest there, clicking should trigger some hints
		SetPrizeIcon(index,level,false,prizeStatus.UNREACHED);
		AddOrChangeClickParameters(rewards[index].gameObject, OnClickReward, {showHintsOnly = true,prizeLevel = level});
	elseif(level ~= 0) then
		--check whether to show chest or not
		if(rewardTable ~= nil) then
			for k,v in pairs(rewardTable) do
				if(v == level) then				
					--obtained already	
					SetPrizeIcon(index,level,false,prizeStatus.OBTAINED);
					AddOrChangeClickParameters(rewards[index].gameObject, OnClickReward, nil);
					return;
				end
			end
		end
		SetPrizeIcon(index,level,true,prizeStatus.AVAILABLE);
		AddOrChangeClickParameters(rewards[index].gameObject, OnClickReward, {prizeIndex = index,prizeLevel = level});
	else
		--unknown status
		SetPrizeIcon(index,level,false,prizeStatus.UNREACHED);
		AddOrChangeClickParameters(rewards[index].gameObject, OnClickReward, nil);
	end	
end

function SetPrizeIcon(index,level,isSelected,status)
	local localData = Config.GetTemplate(Config.LadderLevel());
	UIHelper.SetLabelTxt(rewards[index].Name,localData[tostring(level)].name);
	GameObjectSetActive(rewards[index].selectedLight,isSelected);
	UIHelper.SetSpriteName(rewards[index].Icon,status.Icon);
	if(isSelected)then
		UIHelper.SetWidgetColor(rewards[index].Name,ladderRankInfoSettings.nameColorSelected);
		UIHelper.SetLabelFontSize(rewards[index].Name,ladderRankInfoSettings.nameSizeSelected);
		GameObjectLocalScale(rewards[index].IconRoot, ladderRankInfoSettings.nameScaleSelected);
	else
		UIHelper.SetWidgetColor(rewards[index].Name,ladderRankInfoSettings.nameColorNormal);
		UIHelper.SetLabelFontSize(rewards[index].Name,ladderRankInfoSettings.nameSizeNormal);
		GameObjectLocalScale(rewards[index].IconRoot, ladderRankInfoSettings.nameScaleNormal);
	end
end

function OnClickReward(go)
	if(go==nil)then
		return;
	end
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil)then
		if(listener.parameter.showHintsOnly)then
			--show hint preview
			local advAwardData = {item = {}};
			local rawData = Config.GetTemplate(Config.LadderEndLevel());
			local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);

			local daysBeforeSeasonStart = (infoData.serv.sign-1)*infoData.serv.cycle;
			local serverStartTimeStamp = Login.GetSrvInfo(DataSystemScript.GetRegionId()).time;
			local seasonStartDayOfYear = Util.GetDayInYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
			seasonIndexThisYear = math.floor((seasonStartDayOfYear-1)/infoData.serv.cycle) + 1;

			print(""..seasonIndexThisYear..listener.parameter.prizeLevel);
			local prizeData = rawData[""..seasonIndexThisYear..listener.parameter.prizeLevel];
			if(prizeData == nil)then
				WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "表中缺少本赛季对应段为奖励" });
				return;
			elseif(not IsTableEmpty(prizeData))then
				for i=1,#prizeData.item,3 do
					if(prizeData.item[i]~="")then
						table.insert(advAwardData.item,{id=prizeData.item[i],num=prizeData.item[i+1]});
					end
				end
			end
			if(advAwardData.item == nil or #advAwardData.item==0)then
				WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "表中缺少本赛季对应段为奖励" });
				return;
			end
			WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
                m_itemTb = advAwardData,
                OnClose = nil,
				titleName = GetLocalizedString(ladderRankInfoSettings.LadderChestPreview)
            });
			return;		
		else
			local AfterRequestSuccess = function ()
				-- local advAwardData = PVPMsgManager.Get_LadderadvAwardData();
				local advAwardData = PVPMsgManager.GetPVPData(MsgID.tb.LadderadvAward);
				if(advAwardData == nil or #advAwardData==0)then
					advAwardData = {item={{id="10001",num=0}}};
					print("服务端没配奖励数据");
				end
				WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
	                m_itemTb = advAwardData,
	                OnClose = nil
                });
				
				PVPMsgManager.UpdateLadderadvAwardLogs(listener.parameter.prizeLevel);
				
				SetPrize( listener.parameter.prizeIndex, listener.parameter.prizeLevel );
			end
			PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderadvAward, LuaConst.Const.LadderadvAward, {id = listener.parameter.prizeLevel}, AfterRequestSuccess, nil );
		end
	end
end

function OnClickButtonNext()
	SetPrizes(math.min(minLevelOnThisGroup+3, maxLevel-maxLevel % 3));
end

function OnClickButtonPrev()
	SetPrizes(math.max(minLevelOnThisGroup-3, 0));
end

function OnDestroy()
	ladderRankPrizeUI = nil;
	rewards = {};
	minLevelOnThisGroup = nil;
	maxLevel = nil;
	currentLadderLevelGroup = nil;
	buttonNext = nil;
	buttonPrev = nil;
end