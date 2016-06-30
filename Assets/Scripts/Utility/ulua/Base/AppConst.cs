using UnityEngine;

public class AppConst {
    public const bool UsePbc = true;                           //PBC
    public const bool UseLpeg = true;                          //LPEG
    public const bool UsePbLua = true;                         //Protobuff-lua-gen
    public const bool UseCJson = true;                         //CJson
    public const bool UseSproto = true;                        //Sproto
    public const bool AutoWrapMode = true;                     //自动Wrap模式 

    public static string uLuaPath {
        get {
            return Application.dataPath + "/Scripts/Utility/uLua/";
        }
    }
}
