
import 'Object'
import 'Util'
import 'AppConst'          
import 'Component'
import 'Behaviour'
import 'MonoBehaviour'        
import 'GameObject'
import 'Transform'
import 'Animation' 
import 'Application'
import 'Time'
import 'AssetBundle'   
import 'Rect'
import 'GUI'
import 'GUILayout'
import 'Camera'
import 'UIEventListener'
import 'UIBaseWindowLua'
import 'NetWorkHandler'
import 'DataSystem'
import 'GameMain'
import 'WindowMgr'
import 'UIHelper'
import 'SDKMgr'
import 'AudioMgr'
import 'LuaServerTime'
import 'UIScoutAnim'
import 'RoleHelper'
import 'CoachHelper'
import 'Common.Log.LogManager'
import 'Tutorial'
import 'DelayDeal'
import 'Common.Tables.TableWordFilter'
import 'Common.Tables.TableManager'

import 'UIRect'
import 'UIWidget'
import 'UIBasicSprite'
import 'UISprite'

import 'TweenPosition'
import 'TweenScale'
import 'TweenColor'
import 'TweenAlpha'
import 'UILuaTween'

luanet.load_assembly("UnityEngine")

--luanet.load_assembly("Assembly-CSharp")

--基础类型定义--

--UnityEngine 	= luanet.UnityEngine
GameObject 		= UnityEngine.GameObject
Vector3			= UnityEngine.Vector3
Quaternion		= UnityEngine.Quaternion
Time 			= UnityEngine.Time
Camera          = UnityEngine.Camera
Debug 			= UnityEngine.Debug
LogManager      = Common.Log.LogManager.Instance
TableWordFilter = Common.Tables.TableWordFilter.Instance
TableManager    = Common.Tables.TableManager.Instance
--Util            = luanet.Util
--WindowMgr       = luanet.WindowMgr
--UIHelper        = luanet.UIHelper
--AudioMgr        = luanet.AudioMgr
--LuaServerTime   = luanet.LuaServerTime
--DataSystem      = luanet.DataSystem
--UIScoutAnim     = luanet.UIScoutAnim
--LogManager      = luanet.Common.Log.LogManager
--function DoFileWithAssert(strFileName)
--    local str = LuaScriptMgr.Instance:LoadAsString(strFileName)
--    return assert(loadstring(str))()
--end

require("Common/Math")
require("Common/Vector2")
require("Common/Vector3")
require("Common/Vector4")
require("Common/Quaternion")
require("Common/Color")