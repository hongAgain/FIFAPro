module("UIHintSettings",package.seeall);

require "Config"
require "Game/ItemSys"
require "Game/MsgID"
require "Game/GameMailData"

-- HintSettingType = {
-- 	Simple,
-- 	Complex
-- }

--functions with params needed is all called by specific hint widgets
HintSettings = {
	-- PlayerCanBreak = {
	-- 	HintDataKey = nil,
	-- 	CheckHintStatus = function (params)
	-- 		--do what you are interested
	-- 		local playerID = params[1];
	-- 		local pieceID = Config.GetProperty(Config.HeroFragTable(),playerID,"item");
	-- 		local playerStar = Hero.GetCurrStars(playerID);
	-- 		local subSoul = Config.GetProperty(Config.HeroSlvTable(),tostring(playerStar+1),"subSoul");
	-- 		local subMoney = Config.GetProperty(Config.HeroSlvTable(),tostring(playerStar+1),"subMoney");
	-- 		return ((subSoul~=nil)
	-- 				and (subMoney~=nil)
	-- 				and (ItemSys.GetItemData(pieceID).num >= subSoul) 
	-- 				and (ItemSys.GetItemData(LuaConst.Const.SB).num >= subMoney));
	-- 	end,
	-- 	RelatedMsgID = {
	-- 		MsgID.tb.ReqEmailListMsg
	-- 	},
	-- 	CheckMsgContent = function (_data, msgID, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;			
	-- 	end,
	-- 	CheckMsgCache = function (cache, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;
	-- 	end
	-- },
	-- PlayerCanAdvance = {
	-- 	HintDataKey = nil,
	-- 	CheckHintStatus = function (params)
	-- 		--do what you are interested
	-- 		local playerID = params[1];
	-- 		local playerMod = Config.GetProperty(Config.HeroTable(), playerID, "advMod");
	-- 		local playerQuality = Hero.GetCurrQuality(playerID);
	-- 		local subTable = Config.GetProperty(Config.HeroAdvTable(),playerMod..playerQuality,"sub");
	-- 		local needShow = true;
	-- 		for i=1,#subTable do
	-- 			local subItemID = subTable[i];
	-- 			local subItemNum = subTable[i+1];
	-- 			if(subItemNum>0 and ItemSys.GetItemData(subItemID).num < subItemNum)then
	-- 				needShow = false;
	-- 			end
	-- 			i = i+2;
	-- 		end
	-- 		return needShow;
	-- 	end,
	-- 	RelatedMsgID = {
	-- 		MsgID.tb.ReqEmailListMsg
	-- 	},
	-- 	CheckMsgContent = function (_data, msgID, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;			
	-- 	end,
	-- 	CheckMsgCache = function (cache, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;
	-- 	end
	-- },
	-- PlayerCanBeSummoned = {
	-- 	HintDataKey = nil,
	-- 	CheckHintStatus = function (params)
	-- 		--do what you are interested
	-- 		local playerID = params[1];
	-- 		local pieceID = Config.GetProperty(Config.HeroFragTable(),playerID,"item");
	-- 		local initialStar = Config.GetProperty(Config.HeroTable(), playerID, "islv");
	-- 		local subSoul = Config.GetProperty(Config.HeroSlvTable(),tostring(initialStar),"subSoul");
	-- 		local subMoney = Config.GetProperty(Config.HeroSlvTable(),tostring(initialStar),"subMoney");
	-- 		return ((subSoul~=nil)
	-- 				and (subMoney~=nil)
	-- 				and (ItemSys.GetItemData(pieceID).num >= subSoul) 
	-- 				and (ItemSys.GetItemData(LuaConst.Const.SB).num >= subMoney));
	-- 	end,
	-- 	RelatedMsgID = {
	-- 		MsgID.tb.ReqEmailListMsg
	-- 	},
	-- 	CheckMsgContent = function (_data, msgID, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;			
	-- 	end,
	-- 	CheckMsgCache = function (cache, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;
	-- 	end
	-- },
	-- CoachCanBeSummoned = {

	-- },
	-- DailyTaskCompleteOne = {
	-- --need data in lobby
	-- },
	-- AchievementCompleteOne = {
	-- --need data in lobby
	-- },
	-- HavePrivateMessage = {
	-- --need data in lobby
	-- },
	-- HaveNewActivity = {
	-- --need data in lobby
	-- },
	-- HaveLobbyMailHint = {
	-- --need data in lobby
	-- 	HintDataKey = "1100",
	-- 	CheckHintStatus = function (params)
	-- 		if(HintSettings["HaveNewMail"].CheckHintStatus(nil))then
	-- 			return true;				
	-- 		end
	-- 		return HintManager.MsgDataContainValidKey(HintDataKey);
	-- 	end
	-- },
	HaveNewMail = {
		HintDataKey = "1100",
		CheckHintStatus = function (params, hintDataKey)
			return GameMailData.HaveUnreadMail() or HintManager.MsgDataContainValidKey(hintDataKey);
		end,
		RelatedMsgID = {
			MsgID.tb.ReqEmailListMsg,
			MsgID.tb.GetFuncTips
		},
		CheckMsgContent = function (_data, msgID, params, hintDataKey)
			local needShowHint = false;
			if(msgID == MsgID.tb.ReqEmailListMsg)then
				if(_data~=nil)then
					for k,v in pairs(_data.rows) do
						if(v.stat==0)then
							needShowHint = true;
							break;
						end
					end
					if(not needShowHint)then
						--if no new letter, check remaining letter in list
						needShowHint = _data.total > 0 and (_data.start+_data.num-2)<_data.total;
					end
				end				
			elseif(msgID == MsgID.tb.GetFuncTips)then
				needShowHint = tonumber(_data[hintDataKey])>0;
			end
			return needShowHint;			
		end,
		CheckMsgCache = nil
	},
	-- HaveLobbyFriendHint = {
	-- --need data in lobby
	-- 	HintDataKey = "1200",
	-- 	CheckHintStatus = function (params)
	-- 		if(HintSettings["HaveNewMail"].CheckHintStatus(nil))then
	-- 			return true;				
	-- 		end
	-- 		return HintManager.MsgDataContainValidKey(HintDataKey);
	-- 	end
	-- },
	-- HaveNewFriendAdded = {
	-- --need data in lobby
	-- },
	-- HaveEnergyFromFriend = {
	-- 	HintDataKey = "1201",
	-- 	CheckHintStatus = function (params)
	-- 		return false;
	-- 	end,
	-- 	RelatedMsgID = {
	-- 		MsgID.tb.ReqEmailListMsg,
	-- 		MsgID.tb.GetFuncTips
	-- 	},
	-- 	CheckMsgContent = function (_data, msgID, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;			
	-- 	end,
	-- 	CheckMsgCache = nil
	-- },
	-- HaveNewFriendApplication = {
	-- 	HintDataKey = "1202",
	-- 	CheckHintStatus = function (params)
	-- 		return false;
	-- 	end,
	-- 	RelatedMsgID = {
	-- 		MsgID.tb.ReqEmailListMsg,
	-- 		MsgID.tb.GetFuncTips
	-- 	},
	-- 	CheckMsgContent = function (_data, msgID, params)
	-- 		local needShowHint = false;

	-- 		return needShowHint;			
	-- 	end,
	-- 	CheckMsgCache = nil
	-- },
	-- HaveCheckInsToMake = {
	-- --need data in lobby
	-- },
	-- HaveLeagueMatchReward = {
	-- --need data in lobby
	-- },
	-- HaveDailyCupResult = {
	-- --need data in lobby
	-- },
	-- HaveDailyCupReward = {
	-- --need data in lobby
	-- },
	-- HaveScoutChance = {
	-- --need data in lobby
	-- },	
	HavePrivateChatMsgs = 
	{
		--the machanism of reqing msgs is always to get the latest, 
		-- so basically we only need to check newly merged data
		HintDataKey = "1000",
		CheckHintStatus = nil,
		RelatedMsgID = {
			MsgID.tb.GetChatMsgList,
			MsgID.tb.GetFuncTips
		},
		CheckMsgContent = function (_data, msgID, params, hintDataKey)
			local needShowHint = false;
			if(msgID == MsgID.tb.GetChatMsgList)then
				if(_data~=nil)then
					local myUid = Role.Get_uid();
					for k,v in pairs(_data.msgList) do
						if(v.receiver == myUid)then
							needShowHint = true;
							break;
						end
					end
				end				
			elseif(msgID == MsgID.tb.GetFuncTips)then
				needShowHint = tonumber(_data[hintDataKey])>0;
			end
			return needShowHint;			
		end,
		CheckMsgCache = nil
	}
}