using UnityEngine;
using System.Collections;
using Common;
using Common.Log;
using Common.Tables;
using System.Diagnostics;

//导演类，用于管理整个游戏比赛场景

public class GameAI : MonoBehaviour
{
    public void Awake()
    {

    }

    public void Start()
    {
        if(null != AvatarManager.Instance)
            AvatarManager.Instance.BattleMode = true;
        StartCoroutine(InitClient());
    }

    public void OnDestroy()
    {
        if (null != AvatarManager.Instance)
            AvatarManager.Instance.BattleMode = false;
        GameAIDestroy();
    }

    public void Update()
    {
        MessageDispatcher.Instance.Update();
        GameAIUpdate();
    }

    #region GAME_AI_ONLY_TEST
    [Conditional("GAME_AI_ONLY")]
    protected void GameAIUpdate()
    {
        if (null != m_kLuaMgr)
            m_kLuaMgr.Update();
        
        MessageDispatcher.Instance.Update();
    }
    [Conditional("GAME_AI_ONLY")]
    protected void GameAIDestroy()
    {
        if (null != m_kLuaMgr)
            m_kLuaMgr.Destroy();
    }
    [Conditional("GAME_AI_ONLY")]
    protected void InitComponent()
    {
        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<AvatarManager>();
        AvatarManager.Instance.BattleMode = true;
        m_kLuaMgr = new LuaScriptMgr();
        m_kLuaMgr.Start();
    }
    [Conditional("GAME_AI_ONLY")]
    protected void AttachPrefabs()
    {
        ResourceManager.Instance.Load("GameAI/UIRoot", true);
        ResourceManager.Instance.Load("GameAI/Line", true);
    }
    [Conditional("GAME_AI_ONLY")]
    protected void InitTable()
    {
        ResourceManager.Instance.Load("GameAI/UIRoot", true);
        ResourceManager.Instance.Load("GameAI/Line", true);
    }
    [Conditional("GAME_AI_ONLY")]
    protected void AddDebugAIGizmos()
    {
        gameObject.AddComponent<DebugTool_AIGizmo>();
    }
    #endregion
    private IEnumerator InitClient()
    {
        LogManager.Instance.Log("Initialize start...");
        InitComponent();
        LogManager.Instance.Log("Initialize Lua Module ...");
#if GAME_AI_ONLY
        m_kMsgProc = new MessageProcessor(MessageDispatcher.Instance);
        IEnumerator it = TableManager.Instance.InitTables();
        while (it.MoveNext())
        {
            yield return it.Current;
        }
        gameObject.AddComponent<DebugGUI>();
#endif
        LogManager.Instance.Log("Initialize Table Module ...");
        gameObject.AddComponent<PLDirector>();
        gameObject.AddComponent<BaseFxPlayer>();
        LogManager.Instance.Log("Initialize end...");
        AttachPrefabs();
        yield return null;

        AddDebugAIGizmos();
    }

    private LuaScriptMgr m_kLuaMgr = null;
#if GAME_AI_ONLY
    private MessageProcessor m_kMsgProc = null;
#endif 
}
