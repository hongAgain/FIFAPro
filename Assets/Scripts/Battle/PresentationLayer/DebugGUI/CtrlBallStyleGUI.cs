

using Common;
using UnityEngine;
public class CtrlBallStyleGUI : MonoBehaviour
{
    public void Start()
    {

    }

//     void OnGUI()
//     {
//         if (null == LLDirector.Instance.Scene)
//             return;
//         GUI.Box(new Rect(10, 200, Screen.width / 4 - 20, Screen.height - 10), "战术选择面版");
//         GUILayout.BeginArea(new Rect(10, 220, Screen.width / 4 - 20, Screen.height - 10));
//             GUILayout.BeginVertical();
//                 GUILayout.BeginHorizontal();
//                     GUILayout.Label("请选择要修改的球队");
// 
//                     bool bActive = m_kTeamColor == ETeamColor.Team_Red ? true : false;
//                     bActive = GUILayout.Toggle(bActive, "红队");
//                     if (bActive)
//                     {
//                         m_kTeamColor = ETeamColor.Team_Red;
//                         m_kTeam = LLDirector.Instance.Scene.RedTeam;
//                         m_kCtrlBallStyle = m_kTeam.CtrlBallStyle;
//                     }
//                     bActive = m_kTeamColor == ETeamColor.Team_Red ? false : true;
//                     bActive = GUILayout.Toggle(bActive, "蓝队");
//                     if (bActive)
//                     {
//                         m_kTeamColor = ETeamColor.Team_Blue;
//                         m_kTeam = LLDirector.Instance.Scene.RedTeam;
//                         m_kCtrlBallStyle = m_kTeam.CtrlBallStyle;
//                     }
//                 GUILayout.EndHorizontal();
// 
// 
//                 GUILayout.BeginHorizontal();
//                     bActive = m_kCtrlBallStyle == ECtrlBallStyle.CBS_ATTACK ? true : false;
//                     bActive = GUILayout.Toggle(bActive, "偏进攻");
//                     if (bActive)
//                     {
//                         m_kCtrlBallStyle = ECtrlBallStyle.CBS_ATTACK;
//                     }
//                     bActive = m_kCtrlBallStyle == ECtrlBallStyle.CBS_BALANCE ? true : false;
//                     bActive = GUILayout.Toggle(bActive, "偏平衡");
//                     if (bActive)
//                     {
//                         m_kCtrlBallStyle = ECtrlBallStyle.CBS_BALANCE;
//                     }
//                     bActive = m_kCtrlBallStyle == ECtrlBallStyle.CBS_DEFENCE ? true : false;
//                     bActive = GUILayout.Toggle(bActive, "偏防守");
//                     if (bActive)
//                     {
//                         m_kCtrlBallStyle = ECtrlBallStyle.CBS_DEFENCE;
//                     }
//                     if (null != m_kTeam)
//                     m_kTeam.CtrlBallStyle = m_kCtrlBallStyle;
//                 GUILayout.EndHorizontal();
//             GUILayout.EndVertical();
//         GUILayout.EndArea();
//     }


    private ECtrlBallStyle m_kCtrlBallStyle = ECtrlBallStyle.CBS_ATTACK;
    private LLTeam m_kTeam = null;
    private ETeamColor m_kTeamColor = ETeamColor.Team_Red;

}
