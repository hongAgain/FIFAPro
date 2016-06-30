using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common.Tables;
using Common.Log;
using System;

public class GoalCeleBrationFx : MonoBehaviour
{

    public enum GoalCeleBrationNum
    {
        Begin,
        Playing,
        End,
        NULL,
    }

    void Update()
    {
        if (this == null)
            return;
        switch (m_kCnum)
        {
            case GoalCeleBrationNum.Begin:
                BeginPlay();
                break;
            case GoalCeleBrationNum.Playing:
                GoalPlaying(GlobalBattleInfo.Instance.DeltaTime);
                break;
            case GoalCeleBrationNum.End:
                GoalEnd();
                break;
        }
    }
    public void Init(LLTeam _team, List<LLPlayer> _kLPlayer, GoalCelebrationData _data)
    {
        m_kCnum = GoalCeleBrationNum.Begin;
        m_kCelData = _data;
        m_kLPlayer = _kLPlayer;
        m_llTeam = _team;
        m_InvertTime = 0;
        m_animations.Clear();
        AvatarManager.Instance.BattleMode = false;
    }

    /// <summary>
    /// 初始化位置
    /// </summary>
    private void BeginPlay()
    {
        //创建角色//
        if (m_kCelData == null)
        {
            LogManager.Instance.RedLog("GoalCeleBrationFx is wrong,Beginplay");
            return;
        }

        float _angle = 0f;
        for (int i = 0; i < m_kLPlayer.Count; i++)
        {
            UInt32 uiHeroID = m_kLPlayer[i].PlayerBaseInfo.HeroID;
            bool bHost = (m_kLPlayer[i].Team.TeamColor == Common.ETeamColor.Team_Red) ? true : false;
            GameObject kRoleObj = AvatarManager.Instance.CreateRoleObjectForGoalCeleB((int)uiHeroID, "GoalPlayer", (int)m_kLPlayer[i].PlayerBaseInfo.ClubID, bHost);
            kRoleObj.transform.parent = transform;
            kRoleObj.name = uiHeroID.ToString();
            GameObject _keyObj = kRoleObj.transform.FindChild("Animation").gameObject;
            SetLayer("SceneFx", _keyObj);
            kRoleObj.transform.localPosition = MathUtil.ConverToVector3(m_kCelData.m_kPlyData[i].m_pos);
            kRoleObj.transform.localEulerAngles = new Vector3(0, (float)m_kCelData.m_kPlyData[i].m_rorateAngle + _angle, 0);
            Animation _animation = _keyObj.GetComponent<Animation>();
            m_animations.Add(_animation);
        }
        m_CameraObj = transform.FindChild("Camera").gameObject;
        if(m_CameraObj!=null)
        {
            m_AnimationCamera = m_CameraObj.GetComponent<Animation>();
            m_CameraObj.transform.localPosition = MathUtil.ConverToVector3(m_kCelData.m_kCameraPosition);
            m_CameraObj.transform.localEulerAngles = MathUtil.ConverToVector3(m_kCelData.m_fCameraAngle);
        }
        m_kCnum = GoalCeleBrationNum.Playing;
    }

    private void GoalPlaying(float _ftime)
    {

        if (m_kCelData.m_AniTime - m_InvertTime > 0)
        {
            if (m_InvertTime == 0)
            {
                for (int i = 0; i < m_animations.Count; i++)
                {
                    m_animations[i].Play(m_kCelData.m_kPlyData[i].m_clipData.AniName);

                    m_animations[i][m_kCelData.m_kPlyData[i].m_clipData.AniName].speed = m_kCelData.m_kPlyData[i].m_clipData.AniSpeed * GlobalBattleInfo.Instance.PlaySpeed;
                    LogManager.Instance.RedLog("GoalCeleBrationFx,PlayerAnimationName==="+m_kCelData.m_kPlyData[i].m_clipData.AniName);
                }

                if (m_AnimationCamera != null&&m_kCelData.m_cameAniName!=string.Empty)
                {
                    m_AnimationCamera.Play(m_kCelData.m_cameAniName);
                    m_AnimationCamera[m_kCelData.m_cameAniName].speed = 1.2f * GlobalBattleInfo.Instance.PlaySpeed;
                    LogManager.Instance.RedLog("GoalCeleBrationFx,Camera AnimationName===" + m_kCelData.m_cameAniName);
                }
            }
        }
        else
        {
            m_kCnum = GoalCeleBrationNum.End;
        }
        m_InvertTime += _ftime;
    }


    private void GoalEnd()
    {
        DestoryGoal();
    }
    public void DestoryGoal()
    {
        if (m_playerObjs != null)
        {
            foreach (GameObject item in m_playerObjs)
            {
                Destroy(item);
            }
            m_playerObjs.Clear();
            m_animations.Clear();
        }
        m_kCnum = GoalCeleBrationNum.End;
        Resources.UnloadUnusedAssets();
    }
    public void SetLayer(string strLayer, GameObject _gameObj)
    {
        Transform kTransform = _gameObj.transform.FindChild("Position");
        Util.SetParentAndAllChildrenToLayer(kTransform, strLayer);
        kTransform = _gameObj.transform.FindChild("All_001");
        Util.SetParentAndAllChildrenToLayer(kTransform, strLayer);
    }

    /// 摄像机相机Animtion组件//
    private Animation m_AnimationCamera = null;
    private GameObject m_CameraObj = null;

    private GoalCeleBrationNum m_kCnum = GoalCeleBrationNum.Begin;
    private GoalCelebrationData m_kCelData = new GoalCelebrationData();
    private List<LLPlayer> m_kLPlayer = new List<LLPlayer>();
    private LLTeam m_llTeam = null;
    private float m_InvertTime = 0f;
    private List<Animation> m_animations = new List<Animation>();
    private List<GameObject> m_playerObjs = new List<GameObject>();

    private List<AnimationState> m_states = new List<AnimationState>();
}
