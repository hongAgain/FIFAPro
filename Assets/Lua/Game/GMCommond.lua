module("GMCommond", package.seeall)

--GUILayout = UnityEngine.GUILayout

local switchOn = false;

function OnGUI ()
	if switchOn then
		if (GUILayout.Button("BuyItem 1")) then
			local ht = Util.GetHashTable();
			Util.FillHashTable(ht, 'Method', 'BuyItem');
			Util.FillHashTable(ht, 'itemId', '1');

			SDKMgr.CallSDK(ht);
		end

        if (GUILayout.Button("ShowLog")) then
            WindowMgr.ShowWindow(LuaConst.Const.UILogView);
            Disable();
        end

		if (GUILayout.Button("Close")) then
			Disable();
		end
	end
end

function Enable()
	switchOn = true;
end

function Disable()
	switchOn = false;
end

function OnYes()
    print("Yes");
end