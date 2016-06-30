module("GameNewsData", package.seeall);

local gameNewsData = nil;

local gameNewsDelegateFunc = nil;

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.GameNews, OnReqGameNewsList);

end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GameNews, OnReqGameNewsList);

end

--no need to update every time you get into the shop
function RequestGameNewsList( delegatefunc )

    gameNewsDelegateFunc = delegatefunc;
    if(gameNewsData == nil) then
        DataSystemScript.RequestWithParams(LuaConst.Const.GameNews, nil, MsgID.tb.GameNews);
    else
        if (gameNewsDelegateFunc ~= nil) then
            gameNewsDelegateFunc();
            gameNewsDelegateFunc = nil;
        end
    end
end

function OnReqGameNewsList(code_, data_)
    print(".. OnReqGameNewsList!!!");
    gameNewsData = data_;
    if (gameNewsDelegateFunc ~= nil) then
        gameNewsDelegateFunc();
        gameNewsDelegateFunc = nil;
    end
end

function Get_GameNewsData()
    return gameNewsData;
end

function Get_OneGameNewsData(index)
    return gameNewsData and gameNewsData[index];
end

function Get_GameNewsCount()
    print(gameNewsData and #gameNewsData or 0);
    return gameNewsData and #gameNewsData or 0;
end