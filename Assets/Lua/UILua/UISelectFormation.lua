module("UISelectFormation", package.seeall)

require "UILua/UICircleListManager"

local window = nil;
local windowComponent = nil;

local onSelectCB = nil;
local circleList = nil;

function OnStart(gameObject, luaTables)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    onSelectCB = luaTables[1];
    BindUI();
end

function OnDestroy()
    circleList:OnDestroy();
    circleList = nil;
end

function BindUI()
    local transform = window.transform;
    local scrollRoot = TransformFindChild(transform, "Scroll View");
    local container = TransformFindChild(scrollRoot, "Container");
    
    local close = TransformFindChild(transform, "Close");
    Util.AddClick(close.gameObject, Close);
    
    local allFormationDic = TableManager.FormationTbl:GetItemList();
    local allFormation = {};
    for k, v in dpairs(allFormationDic) do
        table.insert(allFormation, v);
    end
    
    circleList = UICircleListManager.New();
    -- circleList.itemPrefab = windowComponent:GetPrefab("Formaiton");
    circleList:CreateUICircleList(scrollRoot, container, allFormation,
            {
				OnCreateItem = OnCreateItem,
				OnClickItem = nil,
                OnSelectByDrag = nil,
			},tonumber(Hero.GetFormId()));
            
    local choose = TransformFindChild(transform, "ConfirmBtn");
    
    local function Choose ()
        local params = circleList:GetCenteredObject();
        if (params.data ~= nil)then
            if (onSelectCB ~= nil) then
                onSelectCB(params.data.id);
            end
        end
        Close();
    end
    Util.AddClick(choose.gameObject, Choose);
end

function OnCreateItem( randomIndex, key, value, itemNameTrans )
	UIHelper.SetLabelTxt(itemNameTrans, value.name);
end

function Close()
    windowComponent:Close();
end