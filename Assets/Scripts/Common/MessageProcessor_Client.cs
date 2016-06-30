using BehaviourTree;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common
{
    public partial class MessageProcessor
    {
        [Conditional("FIFA_CLIENT")]
        private void InitClientHandler()
        {
            if (null == m_kDispatcher)
                return;
            m_kDispatcher.AddHandler(MessageType.CreateScene, OnCreateScene);
            m_kDispatcher.AddHandler(MessageType.DestroyScene, OnDestroyScene);
            m_kDispatcher.AddHandler(MessageType.ExitBattle, OnExitBattle);
            m_kDispatcher.AddHandler(MessageType.CreateTeam, OnCreateTeam);
            m_kDispatcher.AddHandler(MessageType.DestroyTeam, OnDestroyTeam);
            m_kDispatcher.AddHandler(MessageType.CreatePlayer, OnCreatePlayer);
            m_kDispatcher.AddHandler(MessageType.RemovePlayer, OnRemovePlayer);
            m_kDispatcher.AddHandler(MessageType.CreateBall, OnCreateBall);
            m_kDispatcher.AddHandler(MessageType.DestroyBall, OnDestroyBall);
            m_kDispatcher.AddHandler(MessageType.PlayAnimation, OnPlayAnimation);
            m_kDispatcher.AddHandler(MessageType.CameraAniEnable, OnCameraAniEnable);
            m_kDispatcher.AddHandler(MessageType.CreateBattleDesc, OnCreateBattleDesc);
            m_kDispatcher.AddHandler(MessageType.DestroyBattleDesc, OnDestroyBattleDesc);
            m_kDispatcher.AddHandler(MessageType.CreateGoalAni, OnCreateGoalAni);
            m_kDispatcher.AddHandler(MessageType.DestroyGoalAni, OnDestroyGoalAni);
            m_kDispatcher.AddHandler(MessageType.ChangeBallPosState, OnChangeBallPos);
            m_kDispatcher.AddHandler(MessageType.MessageHomePositionUpdate, OnRefrshPlayerHomePosition);

            // 相机特效
            m_kDispatcher.AddHandler(MessageType.CameraFxBegin, OnCameraFxBegin);
            // 相机特效
            m_kDispatcher.AddHandler(MessageType.CameraFxEnd, OnCameraFxEnd);
            // 残影特效
            m_kDispatcher.AddHandler(MessageType.GhostFxBegin, OnGhostFxBegin);
            // 残影特效
            m_kDispatcher.AddHandler(MessageType.GhostFxEnd, OnGhostFxEnd);
            // 暂停游戏
            m_kDispatcher.AddHandler(MessageType.PauseBattle, OnBattlePause);
            // 继续游戏
            m_kDispatcher.AddHandler(MessageType.ResumeBattle, OnBattleResume);
            // 退出战斗
            m_kDispatcher.AddHandler(MessageType.ExitBattle, OnExitBattle);
            // 播放特效
            m_kDispatcher.AddHandler(MessageType.BaseFxBegin, OnBaseFxBegin);
            // 结束播放特效
            m_kDispatcher.AddHandler(MessageType.BaseFxEnd, OnBaseFxEnd);
            // 帧冻结
            m_kDispatcher.AddHandler(MessageType.FrameFrozenBegin, OnFrameFrozenBegin);
            // 帧冻结
            m_kDispatcher.AddHandler(MessageType.FrameFrozenEnd, OnFrameFrozenEnd);
            // 更新控球率
            m_kDispatcher.AddHandler(MessageType.CtrlBallRate, OnUpdateCtrlRate);
            // 更新控球率
            m_kDispatcher.AddHandler(MessageType.BattleText, OnBattleText);

            //球的显示隐藏//
            m_kDispatcher.AddHandler(MessageType.BallVisable, OnBallVisable);
            //球的显示隐藏//
            m_kDispatcher.AddHandler(MessageType.ChangeAniAngle, OnAniAngleChange);
            //进球之后的庆祝场景//
            m_kDispatcher.AddHandler(MessageType.GoalCelebration, OnPlayGoalCeleBration);
            //销毁庆祝场景//
            m_kDispatcher.AddHandler(MessageType.DestoryCeleBration, OnDestoryGoalCeleBration);
            //隐藏战斗UI//
            m_kDispatcher.AddHandler(MessageType.HideBattleUI, OnHideBattleUI);
            //显示战斗UI//
            m_kDispatcher.AddHandler(MessageType.ShowBattleUI, OnShowBattleUI);
            //隐藏进球UI//
            m_kDispatcher.AddHandler(MessageType.HideInGoalUI, OnHideInGoalUI);
            //显示进球UI//
            m_kDispatcher.AddHandler(MessageType.ShowInGoalUI, OnShowInGoalUI);
            //销毁战前UI//
            m_kDispatcher.AddHandler(MessageType.DestroyPerBattleUI, OnDestroyPerBattleUI);
            //显示战前UI//
            m_kDispatcher.AddHandler(MessageType.ShowPerBattleUI, OnShowPerBattleUI);
        }

        private void OnHideBattleUI(Message kMsg)
        {
            if (null == UIBattle.Instance)
                return;
            if (UIBattle.Instance.Is2DModel)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.gameObject.transform.FindChild("CommonControls");
            if (null == kTransform || null == kTransform.gameObject)
                return;
            kTransform.gameObject.SetActive(false);
        }
        private void OnShowBattleUI(Message kInMsg)
        {
            if (null == UIBattle.Instance)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.gameObject.transform.FindChild("CommonControls");
            if (null == kTransform || null == kTransform.gameObject)
                return;
            kTransform.gameObject.SetActive(true);
        }

        private void OnHideInGoalUI(Message kMsg)
        {
            if (null == UIBattle.Instance)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.SkillRoot.FindChild("Celebrate");
            if (null == kTransform || null == kTransform.gameObject)
                return;
            UIBattle.Instance.ShowCelebrateUI(false, null);
        }
        private void OnShowInGoalUI(Message kInMsg)
        {
            if (null == UIBattle.Instance)
                return;
            ShowInGoalUIMessage kMsg = kInMsg as ShowInGoalUIMessage;
            if (null == kMsg.Unit)
                return;
            UIBattle.Instance.ShowCelebrateUI(true, kMsg.Unit);
        }
        private void OnFrameFrozenBegin(Message kInMsg)
        {
            FrameFrozenBeginMessage kMsg = kInMsg as FrameFrozenBeginMessage;
            List<LLUnit> kUnitList = kMsg.UnitList;
            for (int i = 0; i < kUnitList.Count; i++)
            {
                PLPlayer kPlayer = GetPlayer(kUnitList[i]);
                AnimationPlayer kAniPlayer = kPlayer.transform.gameObject.GetComponent<AnimationPlayer>();
                if (null == kAniPlayer)
                    continue;
                kAniPlayer.ScaleTime((float)kMsg.ScaleTime);
            }
            // 开始记录帧冻结
         //   UnityEngine.Time.timeScale *= (float)kMsg.ScaleTime;
            //    
        }
        private void OnFrameFrozenEnd(Message kInMsg)
        {
            FrameFrozenEndMessage kMsg = kInMsg as FrameFrozenEndMessage;
            // 开始记录帧冻结
            //  UnityEngine.Time.timeScale /= (float)kMsg.ScaleTime;

            List<LLUnit> kUnitList = kMsg.UnitList;
            for (int i = 0; i < kUnitList.Count; i++)
            {
                PLPlayer kPlayer = GetPlayer(kUnitList[i]);
                AnimationPlayer kAniPlayer = kPlayer.transform.gameObject.GetComponent<AnimationPlayer>();
                if (null == kAniPlayer)
                    continue;
                kAniPlayer.ScaleTime(1/ (float)kMsg.ScaleTime);
            }
        }

        public void OnCreateScene(Message kInMsg)
        {
            CreateSceneMessage kMsg = kInMsg as CreateSceneMessage;
            LogManager.Instance.Log("BeginCreate Scene");
            UnityEngine.GameObject kObj = ResourceManager.Instance.Load(kMsg.SceneName, true) as UnityEngine.GameObject;
            m_kScene = kObj.AddComponent<PLScene>();
            m_kScene.SceneName = kMsg.SceneName;
            m_kScene.Scene = kMsg.Scene;
            LogManager.Instance.Log("EndCreate Scene");
        }

        public void OnDestroyScene(Message kInMsg)
        {
            if(null != m_kScene && null != m_kScene.gameObject)
                UnityEngine.GameObject.Destroy(m_kScene.gameObject);

            UnityEngine.GameObject kObj = UnityEngine.GameObject.FindWithTag("GameAI"); 
            if(kObj)
            {
                BaseFxPlayer kFxPlayer = kObj.GetComponent<BaseFxPlayer>();
                if (null != kFxPlayer)
                    UnityEngine.GameObject.Destroy(kFxPlayer);
            }
        }

        public void OnExitBattle(Message kInMsg)
        {
            PLDirector.Instance.OnExit();
        }

        public void OnCreateTeam(Message kInMsg)
        {
            CreateTeamMessage kMsg = kInMsg as CreateTeamMessage;
            ETeamColor kType = kMsg.Team.TeamColor;
            UnityEngine.GameObject kTeamObj = new UnityEngine.GameObject(kType.ToString());
            switch(kMsg.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    m_kRedTeam = kTeamObj.AddComponent<PLTeam>();
                    m_kRedTeam.Team = kMsg.Team;
                    break;
                case ETeamColor.Team_Blue:
                    m_kBlueTeam = kTeamObj.AddComponent<PLTeam>();
                    m_kBlueTeam.Team = kMsg.Team;
                    break;
                default:
                    break;
            }
        }

        public void OnDestroyTeam(Message kInMsg)
        {
            DestroyTeamMessage kMsg = kInMsg as DestroyTeamMessage;
            ETeamColor kType = kMsg.Team.TeamColor;
            PLTeam kTeam = null;
            switch (kMsg.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    kTeam = m_kRedTeam;
                    break;
                case ETeamColor.Team_Blue:
                    kTeam = m_kBlueTeam;
                    break;
                default:
                    break;

            }
            if (null == kTeam || null == kTeam.gameObject)
                return;
            UnityEngine.GameObject.Destroy(kTeam.gameObject);
        }

        public void OnCreatePlayer(Message kInMsg)
        {
            CreatePlayerMessage kMsg = kInMsg as CreatePlayerMessage;
            ETeamColor kType = kMsg.Unit.Team.TeamColor;

            PLTeam kTeam = null;
            switch(kMsg.Unit.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    kTeam = m_kRedTeam;
                    break;
                case ETeamColor.Team_Blue:
                    kTeam = m_kBlueTeam;
                    break;
                default:
                    break;
            }

            if (null == kTeam || null == kTeam.gameObject)
                return;
            kTeam.CreatePlayer(kMsg.Unit, kMsg.IsGoalKeeper);
        }
        public void OnRemovePlayer(Message kInMsg)
        {
            RemovePlayerMessage kMsg = kInMsg as RemovePlayerMessage;
            ETeamColor kType = kMsg.TeamColor;
            PLTeam kTeam = null;
            switch (kMsg.Unit.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    kTeam = m_kRedTeam;
                    break;
                case ETeamColor.Team_Blue:
                    kTeam = m_kBlueTeam;
                    break;
                default:
                    break;
            }
            if (null == kTeam || null == kTeam.gameObject)
                return;
            kTeam.RemovePlayer(kMsg.Unit, kMsg.IsGoalKeeper);
        }


        public void OnCreateBall(Message kInMsg)
        {
            if (null != m_kBall)
                return;
            CreateBalllMessage kMsg = kInMsg as CreateBalllMessage;
            UnityEngine.GameObject kObj = ResourceManager.Instance.Load("Char/soccer_ball", true) as UnityEngine.GameObject;
            m_kBall = kObj.AddComponent<PLBall>();
            m_kBall.Ball = kMsg.Ball;
        }

        public void OnDestroyBall(Message kInMsg)
        {
            if (null == m_kBall)
                return;
            UnityEngine.GameObject.Destroy(m_kBall.gameObject);
            m_kBall = null;
        }

        public void OnPlayAnimation(Message kInMsg)
        {
            PlayAniMessage kMsg = kInMsg as PlayAniMessage;
            LLUnit kPlayer = kMsg.Player;
            PLTeam kTeam = null;
            switch (kPlayer.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    kTeam = m_kRedTeam;
                    break;
                case ETeamColor.Team_Blue:
                    kTeam = m_kBlueTeam;
                    break;
                default:
                    break;
            }
            if (null == kTeam)
                return;
            PLPlayer kPLPlayer = kTeam.GetPlayer(kPlayer);
            if (null != kPLPlayer)
                kPLPlayer.PlayAnimation(kMsg.AniClipData);
        }

        public void OnCameraAniEnable(Message kInMsg)
        {
            UnityEngine.GameObject kCameraObj = UnityEngine.GameObject.FindGameObjectWithTag("MainCamera");
            if (null == kCameraObj)
                return;

            UnityEngine.Animation kAnimation = kCameraObj.GetComponent<UnityEngine.Animation>();
            if (null != kAnimation)
            {
                kAnimation.enabled = true;
                kAnimation.Play("camerabegining_animation");
            }
        }

        public void OnCreateBattleDesc(Message kInMsg)
        {
            CreateBattleDescMessage kMsg = kInMsg as CreateBattleDescMessage;
            if (null == UIBattle.Instance || null == UIBattle.Instance.SkillRoot)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.SkillRoot.FindChild("BattleDesc");
            if(null != kTransform && null != kTransform.gameObject)
            {
                kTransform.gameObject.SetActive(true);
                UnityEngine.Transform kLabel = kTransform.FindChild("Label");
                UIHelper.SetLabelTxt(kLabel,kMsg.Desc);
                UnityEngine.Animation kAnimation = kTransform.GetComponent<UnityEngine.Animation>();
                if (kAnimation)
                    kAnimation.Play("kaishi_in");
            }

        }

        public void OnDestroyBattleDesc(Message kInMsg)
        {
            DestroyBattleDescMessage kMsg = kInMsg as DestroyBattleDescMessage;
            if (null == UIBattle.Instance || null == UIBattle.Instance.SkillRoot)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.SkillRoot.FindChild("BattleDesc");
            if (null != kTransform && null != kTransform.gameObject)
            {
                UnityEngine.Animation kAnimation = kTransform.GetComponent<UnityEngine.Animation>();
                if (kAnimation)
                    kAnimation.Stop();
                kTransform.gameObject.SetActive(false);
            }
        }

        public void OnCreateGoalAni(Message kInMsg)
        {
            CreateGoalAniMessage kMsg = kInMsg as CreateGoalAniMessage;
            if (null == UIBattle.Instance || null == UIBattle.Instance.SkillRoot)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.SkillRoot.FindChild("GoalRoot");
            if (null != kTransform && null != kTransform.gameObject)
            {
                kTransform.gameObject.SetActive(true);
                UnityEngine.Animation kAnimation = kTransform.GetComponent<UnityEngine.Animation>();
                if (kAnimation)
                    kAnimation.Play("Goal");
            }
        }

        public void OnDestroyGoalAni(Message kInMsg)
        {
            DestroyGoalAniMessage kMsg = kInMsg as DestroyGoalAniMessage;
            if (null == UIBattle.Instance || null == UIBattle.Instance.SkillRoot)
                return;
            UnityEngine.Transform kTransform = UIBattle.Instance.SkillRoot.FindChild("GoalRoot");
            if (null != kTransform && null != kTransform.gameObject)
            {
                UnityEngine.Animation kAnimation = kTransform.GetComponent<UnityEngine.Animation>();
                if (kAnimation)
                    kAnimation.Stop();
                kTransform.gameObject.SetActive(false);
            }
        }
        public void OnChangeBallPos(Message kInMsg)
        {
            ChangeBallPosMessage kMsg = kInMsg as ChangeBallPosMessage;
            m_kBall.transform.localPosition = new UnityEngine.Vector3((float)kMsg.Pos.X, (float)kMsg.Pos.Y, (float)kMsg.Pos.Z);
        }

        public void OnBaseFxBegin(Message kInMsg)
        {
            BaseFxBeginMessage kMsg = kInMsg as BaseFxBeginMessage;
            PLPlayer kPlayer = GetPlayer(kMsg.Unit);
            if (null == kPlayer)
                return;
            SkillItem kSkillItem = TableManager.Instance.SkillTbl.GetItem(kMsg.SkillID);
            if(null != kSkillItem)
                BaseFxPlayer.Instance.BeginBaseFx(kPlayer,kMsg.ID, kSkillItem.Name);
        }

        public void OnBaseFxEnd(Message kInMsg)
        {
            BaseFxEndMessage kMsg = kInMsg as BaseFxEndMessage;
            PLPlayer kPlayer = GetPlayer(kMsg.Unit);
            if (null == kPlayer)
                return;

            BaseFxPlayer.Instance.EndBaseFx(kPlayer,kMsg.ID);
        }

        public void OnCameraFxBegin(Message kInMsg)
        {
            CameraFxBeginMessage kMsg = kInMsg as CameraFxBeginMessage;
            if (null == m_kFxCamera)
            {
                m_kFxCamera = ResourceManager.Instance.Load("Battle/FxCamera", true) as UnityEngine.GameObject;
            }
                
                
            if (null == m_kFxCamera)
                return;
            m_kFxCamera.SetActive(true);
            CameraFx kCameraFx = m_kFxCamera.GetComponent<CameraFx>();
            if (null != kCameraFx)
            {
                kCameraFx.Unit = GetPlayer(kMsg.Unit);
                //List<PLPlayer> kPlayerList = new List<PLPlayer>();
                //for(int i = 0;i < kMsg.UnitList.Count;i++)
                //{
                //    PLPlayer kPlayer = GetPlayer(kMsg.UnitList[i]);
                //    if(null != kPlayer)
                //        kPlayerList.Add(kPlayer);
                //}
                kCameraFx.Init(kMsg.IDList, kMsg.SkillTime, kMsg.SkillID);
            }
                
        }

        public void OnCameraFxEnd(Message kInMsg)
        {
            CameraFxEndMessage kMsg = kInMsg as CameraFxEndMessage;
            if (null == m_kFxCamera)
                return;
            m_kFxCamera.SetActive(false);
            CameraFx kCameraFx = m_kFxCamera.GetComponent<CameraFx>();
            if (null != kCameraFx)
                kCameraFx.UnInit();
            UnityEngine.GameObject.Destroy(m_kFxCamera);
            m_kFxCamera = null;
        }

        public PLPlayer GetPlayer(LLUnit kUnit)
        {
            if (null == kUnit)
                return null;
            PLTeam kTeam = null;
            switch (kUnit.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    kTeam = m_kRedTeam;
                    break;
                case ETeamColor.Team_Blue:
                    kTeam = m_kBlueTeam;
                    break;
                default:
                    break;
            }
            if (null == kTeam)
                return null;
            return kTeam.GetPlayer(kUnit);
        }
        public void OnGhostFxBegin(Message kInMsg)
        {
            // 查找对应的PL层对象
            GhostFxBeginMessage kMsg = kInMsg as GhostFxBeginMessage;
            PLPlayer kPlayer = GetPlayer(kMsg.Unit);
            if (null == kPlayer)
                return;
            kPlayer.AddGhostFx(kMsg.ID);
        }

        public void OnGhostFxEnd(Message kInMsg)
        {
            GhostFxEndMessage kMsg = kInMsg as GhostFxEndMessage;
            PLPlayer kPlayer = GetPlayer(kMsg.Unit);
            if (null == kPlayer)
                return;
            kPlayer.RemoveGhosFx();
        }

        public void OnBattlePause(Message kInMsg)
        {
            if(null != m_kRedTeam)
                m_kRedTeam.Pause();
            if (null != m_kBlueTeam)
                m_kBlueTeam.Pause();
            if(null != m_kScene)
                m_kScene.Running = false;
        }

        // 继续游戏
        public void OnBattleResume(Message kInMsg)
        {
            if (null != m_kRedTeam)
                m_kRedTeam.Resume();
            if (null != m_kBlueTeam)
                m_kBlueTeam.Resume();
            if (null != m_kScene)
                m_kScene.Running = true;
        }

        public void OnUpdateCtrlRate(Message kInMsg)
        {
            CtrlBallRateMessage kMsg = kInMsg as CtrlBallRateMessage;
            if(null != UIBattle.Instance)
                UIBattle.Instance.OnCtrlBallRate(kMsg.RedTime, kMsg.BlueTime);
        }

        public void OnBallVisable(Message kInMsg)
        {
            BallVisableMeassage _kMsg = kInMsg as BallVisableMeassage;
            //场景球的控制//
            m_kBall.gameObject.SetActive(!_kMsg.Visable);
            PLPlayer _player = GetPlayer(_kMsg.Kuint);
            //动画球的控制//
            _player.AniPlayer.SetBallVisable(_kMsg.Visable);
        }

        public void OnAniAngleChange(Message kInMsg)
        {
            AniAngleChangeMeassage kMsg = kInMsg as AniAngleChangeMeassage;
            LLUnit kPlayer = kMsg.Unit;
            PLTeam kTeam = null;
            switch (kPlayer.Team.TeamColor)
            {
                case ETeamColor.Team_Red:
                    kTeam = m_kRedTeam;
                    break;
                case ETeamColor.Team_Blue:
                    kTeam = m_kBlueTeam;
                    break;
                default:
                    break;
            }
            if (null == kTeam)
                return;
            PLPlayer kPLPlayer = kTeam.GetPlayer(kPlayer);
            if (null != kPLPlayer)
                kPLPlayer.ChangeAnimationAngle(kMsg.Angle);

        }

        public void OnBattleText(Message kInMsg)
        {
            BattleTextMessage kMsg = kInMsg as BattleTextMessage;
            BattleTextCondItem kCondItem = TableManager.Instance.BattleTextCondTbl.GetItem(kMsg.CondID);
            if(null == kCondItem)
            {
                LogManager.Instance.Log(string.Format("Battle Text Condition Table : Item {0} is not exisit",kMsg.CondID));
                return;
            }
            int iRandomVal = (int)(FIFARandom.GetRandomValue(0, 100));
            int iVal = 0;
            int iTextID = -1;
            for(int i = 0;i < kCondItem.RateList.Count;i++)
            {
                iVal += kCondItem.RateList[i];
                if(iRandomVal < iVal)
                {
                    iTextID = kCondItem.CombineList[i];
                    break;
                }
            }

            if(-1 == iTextID)
            {
                LogManager.Instance.Log("Battle Text ID Retrive failed");
                return;
            }

            BattleTextItem kTextItem = TableManager.Instance.BattleTextTbl.GetItem(iTextID);
            if(null == kTextItem)
            {
                LogManager.Instance.Log(string.Format("Battle Text Table : Item {0} is not exisit", iTextID));
                return;
            }
            else
            {
                LogManager.Instance.Log(string.Format("Battle Text  : {0}", kTextItem.Text));
                if (null != UIBattle.Instance)
                    UIBattle.Instance.EnqueueReportData(kMsg.Unit, kTextItem);
            }

            // 显示结果
            //FIFARandom.Select(new int[] { iDribblePr, iPassPr, iShootPr }, new FIFARandom.OnSelect[] { DribbleFunc, PassBallFunc, ShootFunc });
        }
        public void OnPlayGoalCeleBration(Message kInMsg)
        {
            BattleGoalCeleBrationMsg _kmsg = kInMsg as BattleGoalCeleBrationMsg;
            if(null != UIBattle.Instance)
                UIBattle.Instance.OnPlayGoalCeleBration(_kmsg.GData, _kmsg.Unit);
        }

        public void OnDestoryGoalCeleBration(Message kInMsg)
        {
            if (null != UIBattle.Instance)
                UIBattle.Instance.OnDestoryGoalCeleBration();
        }

        public void OnDestroyPerBattleUI(Message kMsg)
        {
            if (null != UIBattle.Instance)
                UIBattle.Instance.OnDestroyPreBattleUI();
        }

        public void OnShowPerBattleUI(Message kInMsg)
        {
            ShowPreBattleUIMessage kMsg = kInMsg as ShowPreBattleUIMessage;
            if (null != UIBattle.Instance)
                UIBattle.Instance.OnShowPerBattleUI(kMsg.RedTeam,kMsg.BlueTeam);
        }

        private PLScene m_kScene = null;
        private PLTeam m_kRedTeam = null;
        private PLTeam m_kBlueTeam = null;
        private PLBall m_kBall = null;
        private UnityEngine.GameObject m_kFxCamera = null;
    }
}
