--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPlayerListFilter", package.seeall)

require "UILua/UICircleListManager"

local window = nil;
local windowComponent = nil;

local circleListManager = nil;
local m_filterScrollView = nil;
local m_filterGrid = nil;

local m_filterId = 1;
local Callback = nil;
function OnStart(gameObject, params)
    Callback = params.Callback;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    m_filterScrollView = TransformFindChild(transform, "Scroll View");
    m_filterGrid = TransformFindChild(transform, "Scroll View/UIGrid");

    local btnSure = TransformFindChild(transform, "ConfirmBtn").gameObject;
    Util.AddClick(btnSure, BtnSure);
    local btnClose = TransformFindChild(transform, "Close").gameObject;
    Util.AddClick(btnClose, BtnClose);

    InitFilter();
end

function InitFilter()
	--create group items
	local groupItemDatas = {};
    for i=1,5 do
		groupItemDatas[i] = {groupID = i,groupName = "UIPlayerList_FilterType"..i};
    end
	if(circleListManager==nil)then
		circleListManager = UICircleListManager.New();
	end

    --(randomIndex, key, value, items[randomIndex].itemName);
    local OnCreateGroupItem = function( randomIndex, key, value, itemNameTrans)
         UIHelper.SetLabelTxt(itemNameTrans,Util.LocalizeString(value.groupName));
    end

	circleListManager:CreateUICircleList(m_filterScrollView,
		m_filterGrid,
		groupItemDatas,
		{
			OnCreateItem = OnCreateGroupItem,
			--( { item=items[randomIndex], data=value } )
			OnClickItem = OnClickGroupItem,
			OnSelectByDrag = OnClickGroupItem
		});
       
end

--( { item=items[randomIndex], data=value } )
function OnClickGroupItem( params )
	if(params.data ~= nil)then		
--        SetEnumPlayerType(params.data.groupID);
        m_filterId = params.data.groupID;
	end	

end

function BtnSure()
    Callback(m_filterId);

    BtnClose();
end

function BtnClose()
    ExitUIPlayerListFiler();
end

function OnDestroy()
    circleListManager:OnDestroy();

    window = nil;
    windowComponent = nil;
    circleListManager = nil;
    m_filterId = 1;
end

function ExitUIPlayerListFiler()
   windowComponent:Close();

end




