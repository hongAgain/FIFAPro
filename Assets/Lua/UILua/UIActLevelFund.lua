module("UIActLevelFund", package.seeall);

require "Config";
require "Common/UnityCommonScript";
require "Game/LevelFundData";
--显示数据来源于静态配置表
local window = nil;
local windowComponent = nil;
local itemPrefab = nil;

local scrollViewRoot = nil;
local gridRoot = nil;
local purchaseButton = nil;
local cost = nil;
local repay = nil;
local itemList = {};

local investConfig = nil;
local investRepayConfig = nil;
local currentEventInvestRepayConfig = {};

local levelFundInfo = nil;

local rewardButtonList = {};

function Init(transform, winComponent)
    window = transform.gameObject;
    windowComponent = winComponent;
    itemPrefab = windowComponent:GetPrefab("FundRewardItem");

    BindUI();
end

function OnShow()
    itemList = {};
    SetInfo();
end

function OnHide()
    DestroyUIListItemGameObjects(itemList);
end

function OnDestroy()
    DestroyUIListItemGameObjects(itemList);
end

function BindUI()
    local transform = window.transform;

    scrollViewRoot  = TransformFindChild(transform, "Container/Content/RewardsScrollView");
    gridRoot        = TransformFindChild(scrollViewRoot, "RewardsGrid");
    purchaseButton  = TransformFindChild(transform, "Container/Content/PurchaseButton");
    cost            = TransformFindChild(transform, "Container/Content/TextRoot/Cost");
    repay           = TransformFindChild(transform, "Container/Content/TextRoot/Repay");
    UIHelper.SetPanelDepth(scrollViewRoot, UIHelper.GetPanelDepth(windowComponent.transform) + 1)
    UIHelper.SetButtonActive(purchaseButton, true, false);
    AddOrChangeClickParameters(purchaseButton.gameObject, OnClickPurchaseButton);
end

function SetInfo()
    investConfig = ReadingInvestConfig();
    UIHelper.SetLabelTxt(cost, investConfig["1"].price);
    UIHelper.SetLabelTxt(repay, investConfig["1"].price * 10);

    investRepayConfig = ReadingInvestRepayConfig();

    local eventID = '1';    -- TODO
    local counter = 1;
    for k,v in pairs(investRepayConfig) do
        if (v.id == eventID) then
            currentEventInvestRepayConfig[counter] = {};
            currentEventInvestRepayConfig[counter].lv = tonumber(v.lv);
            currentEventInvestRepayConfig[counter].repay = v.repay;
            counter = counter + 1;
        end
    end

    table.sort(
        currentEventInvestRepayConfig,
        function(a, b)
            if (a.lv < b.lv) then
                return true;
            end
            return false;
        end
    );

    LevelFundData.RequestLevelFundInfo({}, OnRequestLevelFundInfoDone);
end

-------------
-- Manager --
-------------
function ReadingInvestConfig()
    local invest = Config.GetTemplate(Config.Invest());
    return invest;
end

function ReadingInvestRepayConfig()
    local investRepay = Config.GetTemplate(Config.InvestRepay());
    return investRepay;
end


local delegateOnSelect = nil;
local delegateOnDragFinish = nil;

function CreateItemList(parentTransform, windowComponent, depth, dataTable)
    CreateUIListItemGameObjects(parentTransform, dataTable, itemPrefab, OnInitItem);
end

function OnInitItem(index, key, value, cloneGameObject)
    itemList[value.lv] = cloneGameObject;
    local transform     = cloneGameObject.transform;
    local titleLabel    = TransformFindChild(transform, "TextRoot/Title");
    local describeLabel = TransformFindChild(transform, "TextRoot/Describe");
    local itemNumLabel  = TransformFindChild(transform, "TextRoot/ItemNum");
    local icon          = TransformFindChild(transform, "IconRoot/Icon");
    local rewardButton  = TransformFindChild(transform, "RewardButton");

    rewardButtonList[value.lv] = rewardButton;
   	AddOrChangeClickParameters(
        rewardButton.gameObject,
        OnClickRewardButton,
        {
            lv = value.lv
        }
    );

    transform.localPosition = NewVector3(0, -tonumber(-224 + key * 112), 0);
    UIHelper.SetLabelTxt(titleLabel, value.lv.."级奖励");
    UIHelper.SetLabelTxt(describeLabel, "到达"..value.lv.."级即可领取");
    UIHelper.SetLabelTxt(itemNumLabel, "x"..value.repay[2]);
    Util.SetUITexture(icon, LuaConst.Const.ItemIcon, 10001, true); -- TODO: change 10001 to real value
    UIHelper.SetButtonActive(rewardButton, true, false);

    if (levelFundInfo ~= nil) then
        if (levelFundInfo["1"] ~= nil) then
            if (levelFundInfo["1"]["log"] ~= nil) then
                for k,v in pairs(levelFundInfo["1"]["log"]) do
                    if (k == tostring(value.lv)) then
                        SetRewardClaimed(rewardButton);
                    end
                end
            end
        end
    end

    if (value.lv > Role.Get_lv()) then
        SetUnReach(rewardButton);
    end

    if (levelFundInfo == nil) then
        SetUnBuy(rewardButton);
    end
end

function OnRequestLevelFundInfoDone()
    levelFundInfo = LevelFundData.Get_LevelFundInfoData();

    CreateItemList(
        gridRoot,
        windowComponent,
        UIHelper.GetPanelDepth(windowComponent.transform) + 1,
        currentEventInvestRepayConfig
    );
    -- UIHelper.RefreshPanel(scrollViewRoot);

    if (levelFundInfo == nil) then
        return;
    end
    if (levelFundInfo["1"] ~= nil) then
        SetPurchased();
    end
end

function OnClickPurchaseButton()
    print("UIActLevelFund -> OnClickPurchaseButton");
    LevelFundData.RequestLevelFundBuy({ id=1 }, OnPurchaseResponse); -- TODO: set real id
end

function OnPurchaseResponse()
    local purchaseResponse = LevelFundData.Get_LevelFundBuyData();
    if (purchaseResponse == nil) then
        return;
    end
    SetPurchased();

    levelFundInfo = {};
    levelFundInfo["1"] = {};
    DestroyUIListItemGameObjects(itemList);
    CreateItemList(
        gridRoot,
        windowComponent,
        UIHelper.GetPanelDepth(windowComponent.transform) + 1,
        currentEventInvestRepayConfig
    );
end

local currentClickRewardLevel = nil;
function OnClickRewardButton(item)
    local lv = UIHelper.GetUIEventListener(item).parameter.lv;
    currentClickRewardLevel = lv;
    LevelFundData.RequestLevelFundRepay({ id=1, lv=lv }, OnRewardResponse);
end

function OnRewardResponse()
    local rewardResponse = LevelFundData.Get_LevelFundRepayData();
    SetRewardClaimed(rewardButtonList[currentClickRewardLevel]);
end

function SetPurchased()
    UIHelper.SetButtonActive(purchaseButton, false, false);
    local label = TransformFindChild(purchaseButton.gameObject.transform, "Label");
    UIHelper.SetLabelTxt(label, "已购买");
end

function SetRewardClaimed(rewardButton)
    UIHelper.SetButtonActive(rewardButton, false, false);
    local label = TransformFindChild(rewardButton, "Label");
    UIHelper.SetLabelTxt(label, "已领取");
    UIHelper.SetWidgetColor(label, "win_w_30");
end

function SetUnReach(rewardButton)
    UIHelper.SetButtonActive(rewardButton, false, false);
    local label = TransformFindChild(rewardButton, "Label");
    UIHelper.SetLabelTxt(label, "未达成");
    UIHelper.SetWidgetColor(label, "win_w_30");
end

function SetUnBuy(rewardButton)
    UIHelper.SetButtonActive(rewardButton, false, false);
    local label = TransformFindChild(rewardButton, "Label");
    UIHelper.SetLabelTxt(label, "领取");
    UIHelper.SetWidgetColor(label, "win_w_30");
end
