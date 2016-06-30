module("UIShopSettings",package.seeall);

ShopType = {
	EpicShop = 1,
	DismissShop = 2,
	DiamondShop = 3,
	LadderShop = 4
}

--rules for settings:
--generateSubTitleDataList : return a sorted <list>, containing list[n].titleName for showing ,
--							 targetDataList[k].titleData for calculation
--generateDataList:			 return a sorted data <list> for items,containing following data:
--							 dataList[count] = {};
-- 							 dataList[count].ID = v.item_id;
-- 							 dataList[count].Name = itemDict[v.item_id].name;
-- 							 dataList[count].Icon = itemDict[v.item_id].icon;
-- 							 dataList[count].Num = tonumber(v.num);
-- 							 dataList[count].ConditionNum = v.step;
-- 							 dataList[count].Condition = string.format(conditionSuffix,v.step);
-- 							 dataList[count].PriceIcon = itemDict[currencyID].icon;
-- 							 dataList[count].SinglePrice = tonumber(v.sub);
-- 							 dataList[count].IsSoldOut = false;
--generateStartIndexList: 	 return a <dict> saving start index of representing items and title index, 
-- 							 uses titleData as key

ShopSettings = {
	{
		titleName = "UIShopSubTitleEpic",
		currencyID = LuaConst.Const.CostDF,
		needFreshButton = false,
		needSubTitleList = true,
		prevType = ShopType.LadderShop,
		nextType = ShopType.DismissShop,
		subTitleDataList = nil,
		dataList = nil,
		startIndexList = nil,
		preRequest = nil,
		requestToBuy = function (params,onOver)
			ShopData.RequestEpicShopBuy(params,onOver);
		end,
		generateSubTitleDataList = function ()
			local subTitleDataList = {};
			local tempData = Config.GetTemplate(Config.RaidDFShop());
			for k,v in pairs(tempData) do
				local hasElement = false;
				for l,w in pairs(subTitleDataList) do
					if(w==v.step)then
						hasElement = true;
						break;
					end
				end
				if(not hasElement)then
					table.insert(subTitleDataList,v.step);
				end
		    end
		    --re-sort
		    table.sort(subTitleDataList);
		    local suffix = GetLocalizedString("UIPeakRoad_Floor");
		    local targetDataList = {};
		    for k,v in pairs(subTitleDataList) do
				targetDataList[k] = {};
				targetDataList[k].titleName = string.format(suffix,tonumber(v));
				targetDataList[k].titleData = tonumber(v);
		    end

		    return targetDataList;
		end,
		generateDataList = function (currencyID)
			local dataList = {};
			local tempData = Config.GetTemplate(Config.RaidDFShop());
			local itemDict = Config.GetTemplate(Config.ItemTable());
			local count = 1;
			local conditionSuffix = GetLocalizedString("FinishFloor");
			for k,v in pairs(tempData) do
				if(itemDict[v.item_id]==nil)then
					dataList[count] = nil;
				else
					dataList[count] = {};
					dataList[count].ID = v.item_id;
					dataList[count].Name = itemDict[v.item_id].name;
					dataList[count].Icon = itemDict[v.item_id].icon;
					dataList[count].Num = tonumber(v.num);
					dataList[count].ConditionNum = v.step;
					dataList[count].Condition = string.format(conditionSuffix,v.step);
					dataList[count].PriceIcon = itemDict[currencyID].icon;
					dataList[count].SinglePrice = tonumber(v.sub);
					dataList[count].IsSoldOut = false;
					count = count+1;
				end				
			end

		    --re-sort
			table.sort(dataList,
				function (a,b)
					if(a.ConditionNum < b.ConditionNum) then
						return true;
					elseif(a.ConditionNum == b.ConditionNum) then
						if(a.SinglePrice < b.SinglePrice) then
							return true;
						elseif(a.SinglePrice == b.SinglePrice) then
							if(a.ID < b.ID) then
								return true;
							elseif(a.ID == b.ID) then

							end
						end
					end
					return false;
				end);

			return dataList;
		end,
		generateStartIndexList = function (subTitleList,dataList)
			--startIndexList recieves subtitledata , returns item list column
			local startIndexList = {};
			for k,v in pairs(subTitleList) do	
				--default 0 for not found
				startIndexList[v.titleData] = {};
				startIndexList[v.titleData].firstIndex = 0;
				startIndexList[v.titleData].titleIndex = k;	
				for i,w in ipairs(dataList) do
					if(v.titleData == w.ConditionNum)then
						startIndexList[v.titleData] = {};
						startIndexList[v.titleData].firstIndex = i;
						startIndexList[v.titleData].titleIndex = k;
						break;
					end
				end
			end
			--key:subtitleData, value:index of the first element
			return startIndexList;
		end
	},
	{
		titleName = "UIShopSubTitleFire",
		currencyID = LuaConst.Const.HContract,
		needFreshButton = true,
		needSubTitleList = false,
		prevType = ShopType.EpicShop,
		nextType = ShopType.DiamondShop,
		subTitleDataList = nil,
		dataList = nil,
		startIndexList = nil,
		preRequest = nil,
		requestToBuy = nil,
		generateSubTitleDataList = nil,
		generateDataList = function (currencyID)
			local dataList = {};
			local tempData = Config.GetTemplate(Config.DismissShop());
			if(tempData~=nil and tempData~={})then
				local itemDict = Config.GetTemplate(Config.ItemTable());
				local count = 1;

				for k,v in pairs(tempData) do					
					if(itemDict[v.item_id]==nil)then
						dataList[count] = nil;
					else
						print(v.item_id);
						dataList[count] = {};
						dataList[count].ID = v.item_id;
						dataList[count].Name = itemDict[v.item_id].name;
						dataList[count].Icon = itemDict[v.item_id].icon;
						dataList[count].Num = tonumber(v.num);
						dataList[count].ConditionNum = nil;
						dataList[count].Condition = "";
						dataList[count].PriceIcon = itemDict[currencyID].icon;
						dataList[count].SinglePrice = tonumber(v.sub);
						dataList[count].IsSoldOut = false;
						count = count+1;
					end
				end
			    --re-sort
				table.sort(dataList,
					function (a,b)
						if(a.SinglePrice < b.SinglePrice) then
							return true;
						elseif(a.SinglePrice == b.SinglePrice) then
							if(a.ID < b.ID) then
								return true;
							end
						end
						return false;
					end);
				print(count);
			end

			return dataList;
		end,
		generateStartIndexList = nil
	},
	{
		titleName = "UIShopSubTitleDiamond",
		currencyID = LuaConst.Const.GB,
		needFreshButton = false,
		needSubTitleList = false,
		prevType = ShopType.DismissShop,
		nextType = ShopType.LadderShop,
		subTitleDataList = nil,
		dataList = nil,
		startIndexList = nil,
		preRequest = function (onOver)
			ShopData.RequestDiamondShopInfo(onOver);
		end,
		requestToBuy = function (params,onOver)
			ShopData.RequestDiamondShopBuy(params,onOver);
		end,
		generateSubTitleDataList = nil,
		generateDataList = function (currencyID)
			local dataList = {};
			local tempData = Config.GetTemplate(Config.ShopTable());
			if(tempData~=nil and tempData~={})then
				local networkData = ShopData.Get_DiamondShopInfoData();
				local itemDict = Config.GetTemplate(Config.ItemTable());
				local count = 1;

				for k,v in pairs(tempData) do					
					if(itemDict[v.item]==nil)then
						dataList[count] = nil;
					else
						dataList[count] = {};
						dataList[count].ID = v.id;
						dataList[count].Name = itemDict[v.item].name;
						dataList[count].Icon = itemDict[v.item].icon;
						dataList[count].Num = tonumber(v.limit_daily);
						dataList[count].ConditionNum = nil;
						dataList[count].Condition = "";
                        dataList[count].PriceIcon = itemDict[currencyID].icon;
						dataList[count].SinglePrice = tonumber(v.sub);
						if((v.limit_daily == 0 or v.limit_daily > networkData[v.id].dn) and 
							(v.limit == 0 or v.limit > networkData[v.id].wn) and
							v.status == 1) then
							dataList[count].IsSoldOut = false;
						else
							dataList[count].IsSoldOut = true;
						end
						count = count+1;
					end
				end
			    --re-sort
				table.sort(dataList,
					function (a,b)
						if(a.SinglePrice < b.SinglePrice) then
							return true;
						elseif(a.SinglePrice == b.SinglePrice) then
							if(a.ID < b.ID) then
								return true;
							end
						end
						return false;
					end);
			end
			return dataList;
		end,
		generateStartIndexList = nil
	},
	{
		titleName = "UIShopSubTitleLadder",
		currencyID = LuaConst.Const.Honor,
		needFreshButton = false,
		needSubTitleList = true,
		prevType = ShopType.DiamondShop,
		nextType = ShopType.EpicShop,
		subTitleDataList = nil,
		dataList = nil,
		startIndexList = nil,
		preRequest = nil,
		requestToBuy = function (params,onOver)
			ShopData.RequestLadderShopBuy(params,onOver);
		end,
		generateSubTitleDataList = function ()
			local subTitleDataList = {};
			local tempData = Config.GetTemplate(Config.LadderLevel());
			for k,v in pairs(tempData) do
				local hasElement = false;
				for l,w in pairs(subTitleDataList) do
					if(w==v.type)then
						hasElement = true;
						break;
					end
				end
				if(not hasElement)then
					subTitleDataList[v.typeID] = v.type;
				end
		    end
		    local suffix = GetLocalizedString("Rank");
		    local targetDataList = {};
		    for k,v in pairs(subTitleDataList) do
		    	targetDataList[k] = {};
				targetDataList[k].titleName = v..suffix;
				targetDataList[k].titleData = tonumber(k);
		    end
		    return targetDataList;
		end,
		generateDataList = function (currencyID)
			local currentSeason = tostring(PVPMsgManager.GetLadderSeasonData().sign);
			local dataList = {};
			local tempData = Config.GetTemplate(Config.LadderShop())[currentSeason];
			if(tempData~=nil and tempData~={})then
				local tempLevelData = Config.GetTemplate(Config.LadderLevel());
				local itemDict = Config.GetTemplate(Config.ItemTable());
				local count = 1;
				local conditionSuffix = GetLocalizedString("AfterGetSomeRank");
				for k,v in pairs(tempData) do
					if(v.id == currentSeason)then
						if(itemDict[v.item]==nil)then
							dataList[count] = nil;
						else
							dataList[count] = {};
							dataList[count].ID = v.item;
							dataList[count].Name = itemDict[v.item].name;
							dataList[count].Icon = itemDict[v.item].icon;
							dataList[count].Num = tonumber(v.maxnum);
							dataList[count].ConditionNum = v.level;
							local minRankName = tempLevelData[tostring(v.level)].name;
							dataList[count].Condition = string.format(conditionSuffix,minRankName);
							dataList[count].PriceIcon = itemDict[currencyID].icon;
							dataList[count].SinglePrice = tonumber(v.sub);
							dataList[count].IsSoldOut = false;
							count = count+1;
						end
					end				
				end
			    --re-sort
				table.sort(dataList,
					function (a,b)
						if(a.ConditionNum < b.ConditionNum) then
							return true;
						elseif(a.ConditionNum == b.ConditionNum) then
							if(a.SinglePrice < b.SinglePrice) then
								return true;
							elseif(a.SinglePrice == b.SinglePrice) then
								if(a.ID < b.ID) then
									return true;
								elseif(a.ID == b.ID) then

								end
							end
						end
						return false;
					end);
			end
			return dataList;
		end,
		generateStartIndexList = function (subTitleList,dataList)
			local startIndexList = {}
			for k,v in pairs(subTitleList) do	
				--default 0 for not found
				startIndexList[v.titleData] = {};
				startIndexList[v.titleData].firstIndex = 0;
				startIndexList[v.titleData].titleIndex = k;	
				for i,w in ipairs(dataList) do
					if((v.titleData-1)*3 <= w.ConditionNum and w.ConditionNum <= (v.titleData*3-1))then
						startIndexList[v.titleData] = {};
						startIndexList[v.titleData].firstIndex = i;
						startIndexList[v.titleData].titleIndex = k;
						break;
					end
				end
			end
			--key:subtitleData, value:index of the first element
			return startIndexList;
		end
	}	
}