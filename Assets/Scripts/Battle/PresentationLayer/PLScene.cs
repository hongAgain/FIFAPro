using Common;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PLScene : MonoBehaviour
{
    public void Awake  ()
    {
    }
       
    public void LateUpdate()
    {
        if(false == m_bRunning)
            return;
    }

    #region BATTLE_DEBUG
    [Conditional("BATTLE_DEBUG")]
    private void DebugDraw()
    {
        // 绘制格子
        DrawRegion(m_kScene.Region, Color.black);
        foreach (var kItem in m_kScene.SubRegionList)
        {
            DrawRegion(kItem, Color.black);
        }
    }

    [Conditional("BATTLE_DEBUG")]
    private void DrawDefendBaseLine()
    {

    }
    [Conditional("BATTLE_DEBUG")]
    private void DrawLine(Vector3 kStart,Vector3 kEnd,Color kColor)
    {
        UnityEngine.Debug.DrawLine(kStart, kEnd, kColor);
    }
    [Conditional("BATTLE_DEBUG")]
    private void DrawRegion(Region kRegion,Color kColor)
    {
        Vector3 kStartPos = new Vector3((float)kRegion.Top, 0, (float)kRegion.Left);
        Vector3 kEndPos = new Vector3((float)kRegion.Top, 0, (float)kRegion.Right);
        DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = new Vector3((float)kRegion.Top, 0, (float)kRegion.Right);
        kEndPos = new Vector3((float)kRegion.Bottom, 0, (float)kRegion.Right);
        DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = new Vector3((float)kRegion.Bottom, 0, (float)kRegion.Right);
        kEndPos = new Vector3((float)kRegion.Bottom, 0, (float)kRegion.Left);
        DrawLine(kStartPos, kEndPos, kColor);

        kStartPos = new Vector3((float)kRegion.Top, 0, (float)kRegion.Left);
        kEndPos = new Vector3((float)kRegion.Bottom, 0, (float)kRegion.Left);
        DrawLine(kStartPos, kEndPos, kColor);

        Vector3 kPos = new Vector3((float)(kRegion.Bottom + kRegion.Top) / 2, 0, (float)(kRegion.Left + kRegion.Right) / 2);
        if(kRegion.ID <= 40 && kRegion.ID > 0)
        {
            GameObject kGridObj = m_kGameObjeList[(int)kRegion.ID - 1];
            kGridObj.transform.localPosition = kPos;
            string strShirtName = string.Format("Textures/Number/number_{0}", kRegion.ID);
            kGridObj.GetComponent<Renderer>().material.mainTexture = ResourceManager.Instance.LoadTexture(strShirtName) as Texture;
        }
    }

    #endregion //BATTLE_DEBUG
    public LLScene Scene
    {
        get { return m_kScene; }
        set { m_kScene = value; }
    }

    public string SceneName
    {
        get { return m_strSceneName; }
        set { m_strSceneName = value; }
    }

    public bool Running
    {
        get { return m_bRunning; }
        set { m_bRunning = value; }
    }
    private string m_strSceneName;
    private LLScene m_kScene;
    private List<GameObject> m_kGameObjeList = new List<GameObject>();
    private bool m_bRunning = true;        // 用来表示游戏是暂停还是正常运行  仅用在GamePause State
}