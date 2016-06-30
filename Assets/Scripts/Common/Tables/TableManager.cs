using Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
#if FIFA_CLIENT
using LuaInterface;
#endif

namespace Common.Tables
{
    public class TableManager
    {
        public static readonly TableManager Instance = new TableManager();
        private TableManager()
        {

        }
        public IEnumerator InitTables()
        {
            m_kAIConfig = new AIConfig();
            if (false == m_kAIConfig.InitTable())
            {
                LogManager.Instance.LogError("Load AIConfig Failed");
                yield break;
            }
            yield return null;

            m_kHeroTable = new HeroTable();
            if (false == m_kHeroTable.InitTable())
            {
                LogManager.Instance.LogError("Load Hero Table Failed");
                yield break;
            }
            yield return null;
            // 加载battleinfo.json
            m_kEnergyTbl = new EnergyTable();
            if (false == m_kEnergyTbl.InitTable())
            {
                LogManager.Instance.LogError("Load Energy Info Table Failed");
                yield break;
            }
            yield return null;

            //m_kCoachTable = new CoachTable();
            //if(false == m_kCoachTable.InitTable())
            //{
            //	LogManager.Instance.LogError("Load Coach Table Failed");
            //	yield break;
            //}
            //yield return null;

            m_kGroundAreaTable = new GroundAreaTable();
            if (false == m_kGroundAreaTable.InitTable())
            {
                LogManager.Instance.LogError("Load GroundArea Table Failed");
                yield break;
            }
            yield return null;

            // 传球时间与距离配置表
            m_kPassTimeTable = new PassTimeTable();
            if (false == m_kPassTimeTable.InitTable())
            {
                LogManager.Instance.LogError("Load PassTime Table Failed");
                yield break;
            }
            yield return null;

            // 加载battleinfo.json
            m_kBattleInfoTable = new BattleInfoTable();
            if (false == m_kBattleInfoTable.InitTable())
            {
                LogManager.Instance.LogError("Load Battle Info Table Failed");
                yield break;
            }
            yield return null;


            // 加载 球场球员固定站位
            m_kPosCfgTbl = new PosCfgTable();
            if (false == m_kPosCfgTbl.InitTable())
            {
                LogManager.Instance.LogError("Load KickOff Pos Table Failed");
                yield break;
            }
            yield return null;

            // 加载停球后事件概率表
            m_kEvtAfterStopTable = new EventAfterStoppingTable();
            if (false == m_kEvtAfterStopTable.InitTable())
            {
                LogManager.Instance.LogError("Load Event After Stopping Table Failed");
                yield break;
            }

            yield return null;

            m_kSensitivityFactorTable = new SensitivityFactorTable();
            if (false == m_kSensitivityFactorTable.InitTable())
            {
                LogManager.Instance.LogError("Load Sensitive Factor Table Failed");
                yield break;
            }
            yield return null;

            m_kSettlementFactorTable = new SettlementFactorTable();
            if (false == m_kSettlementFactorTable.InitTable())
            {
                LogManager.Instance.LogError("Load Settlement Factor Table Failed");
                yield break;
            }
            yield return null;

            mHotSpotTable = new HotSpotTable();
            if (false == mHotSpotTable.InitTable())
            {
                LogManager.Instance.LogError("Load HotSpot Table Failed");
                yield break;
            }
            yield return null;

            mDefenceDensityTable = new DefenceDensityTable();
            if (false == mDefenceDensityTable.InitTable())
            {
                LogManager.Instance.LogError("Load Defence Density Table Failed");
                yield break;
            }
            yield return null;

            m_kSkillAppearTbl = new SkillAppearTable();
            if (false == m_kSkillAppearTbl.InitTable())
            {
                LogManager.Instance.LogError("Load Skill Appear Table Failed");
                yield break;
            }
            yield return null;

            m_kShootTimeTbl = new ShootTimeTable();
            if (false == m_kShootTimeTbl.InitTable())
            {
                LogManager.Instance.LogError("Load Shoot Time Table Failed");
                yield break;
            }
            yield return null;

            m_kFormationTbl = new FormationTable();
            if (false == m_kFormationTbl.InitTable())
            {
                LogManager.Instance.LogError("Load Formation Table Failed");
                yield break;
            }
            yield return null;
            
            #region 传球逻辑相关配置表
            m_kAttackTacticalConfig = new AttackTacticalConfig();
            if (false == m_kAttackTacticalConfig.InitTable())
            {
                LogManager.Instance.LogError("Load AttackTactical Table Failed");
                yield break;
            }
            yield return null;

            m_kDistanceDecayConfig = new DistanceDecayTable();
            if (false == m_kDistanceDecayConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Distance Coefficient Table Failed");
                yield break;
            }
            yield return null;

            m_kDistanceCoefficientTableConfig = new DistanceCoefficientTableConfig();
            if (false == m_kDistanceCoefficientTableConfig.InitTable("Tables/Battle/DistanceCoefficient"))
            {
                LogManager.Instance.LogError("Load Distance Coefficient Table Failed");
                yield break;
            }
            yield return null;

            m_kDistanceCoefficientTableConfig1 = new DistanceCoefficientTableConfig();
            if (false == m_kDistanceCoefficientTableConfig1.InitTable("Tables/Battle/DistanceCoefficient1"))
            {
                LogManager.Instance.LogError("Load Distance Coefficient Table Failed");
                yield break;
            }
            yield return null;
            m_kInterceptCoefficientDataConfig = new InterceptCoefficientDataConfig();
            if (false == m_kInterceptCoefficientDataConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Intercept Coefficient Table Failed");
                yield break;
            }
            yield return null;

            m_kMarkCoefficientDataConfig = new MarkCoefficientDataConfig();
            if (false == m_kMarkCoefficientDataConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Mark Coefficient Table Failed");
                yield break;
            }
            yield return null;

            m_kTacticalPosCoefficientDataConfig = new TacticalPosCoefficientDataConfig();
            if (false == m_kTacticalPosCoefficientDataConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Tactical Pos Coefficient Table Failed");
                yield break;
            }
            yield return null;

            m_kHomePositionZDataConfig = new HomePositionZDataConfig();
            if (false == m_kHomePositionZDataConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Intercept Coefficient Table Failed");
                yield break;
            }
            yield return null;
            #endregion

            #region 中场开球配置表

            m_kMidKickOffPosTableConfig = new MidKickOffPosTableConfig();
            if (false == m_kMidKickOffPosTableConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Middle KickOff Pos Table Failed");
                yield break;
            }
            yield return null;
            #endregion

            #region 球场球员动画配置
            m_AniDataConfig = new AniDataConfig();
            if (!AniDataConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Player Animation Table Failed");
                yield break;
            }
            yield return null;
            m_AniCombineConfig = new AniCombineConfig();
            if (!m_AniCombineConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Player Animation Combine Table Failed");
                yield break;
            }
            yield return null;
            m_AniBeahaviorConfig = new AniBeahaviorConfig();
            if (!m_AniBeahaviorConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Player Animation Beahavior Table Failed");
                yield break;
            }
            yield return null;
            m_AniStateLayerConfig = new AniStateLayerConfig();
            if (!m_AniStateLayerConfig.InitTable())
            {
                LogManager.Instance.LogError("Load Player Animation Beahavior Table Failed");
                yield break;
            }
            yield return null;
            m_kSkillTable = new SkillTable();
            if (!m_kSkillTable.InitTable())
            {
                LogManager.Instance.LogError("Load Skill Table Failed");
                yield break;
            }
            yield return null;
            m_kCameraEffectTable = new CameraEffectTable();
            if (!m_kCameraEffectTable.InitTable())
            {
                LogManager.Instance.LogError("Load Camera Effect Table Failed");
                yield break;
            }
            yield return null;
            m_kGhostEffectTable = new GhostEffectTable();
            if (!m_kGhostEffectTable.InitTable())
            {
                LogManager.Instance.LogError("Load Ghost Effect Table Failed");
                yield break;
            }
            yield return null;
            m_kBaseFxTable = new BaseFxTable();
            if (!m_kBaseFxTable.InitTable())
            {
                LogManager.Instance.LogError("Load Based Fx Table Failed");
                yield break;
            }
            yield return null;

            m_kRaidNpcTable = new RaidNPCTable();
            if (!m_kRaidNpcTable.InitTable())
            {
                LogManager.Instance.LogError("Load RaidNpc Table Failed");
                yield break;
            }
            yield return null;

            m_kBattleTextbl = new BattleTextTable();
            if (!m_kBattleTextbl.InitTable())
            {
                LogManager.Instance.LogError("Load Battle Text Table Failed");
                yield break;
            }
            yield return null;

            m_kBattleTextCondTbl = new BattleTextCondTable();
            if (!m_kBattleTextCondTbl.InitTable())
            {
                LogManager.Instance.LogError("Load Battle Text Condition Table Failed");
                yield break;
            }
            yield return null;

            m_kConfrontationBasicTable = new ConfrontationBasicTable();
            if (!m_kConfrontationBasicTable.InitTable())
            {
                LogManager.Instance.LogError("Load Confrontation Basic Table Failed");
                yield break;
            }
            yield return null;
            #endregion

            m_kBattlePosTbl = new BattlePosTable();
            if (!m_kBattlePosTbl.InitTable())
            {
                LogManager.Instance.LogError("Load BattlePlayerPosition Basic Table Failed");
                yield break;
            }
            yield return null;
            m_goalCelebrationTbl = new GoalCelebrationTable();
            if (!m_goalCelebrationTbl.InitTable())
            {
                LogManager.Instance.LogError("Load BattlePlayerPosition Basic Table Failed");
                yield break;
            }
            yield return null;
            m_kProfessionTbl = new ProfessionTable();
            if(!m_kProfessionTbl.InitTable())
            {
                LogManager.Instance.LogError("Load Profession Table Failed");
                yield break;
            }
            yield return null;
#if FIFA_CLIENT
            m_kLoadTableFunc = LuaScriptMgr.Instance.GetLuaFunction("Config.OnConfigTemplateLoad");
            m_kGetPropFunc = LuaScriptMgr.Instance.GetLuaFunction("Config.GetProperty");
            m_kGetTableFunc = LuaScriptMgr.Instance.GetLuaFunction("Config.GetTemplate");
            if (null == m_kLoadTableFunc || null == m_kGetPropFunc)
            {
                LogManager.Instance.LogError("Function Call Failed...");
                yield break;
            }
            string strContent = ResourceManager.Instance.LoadText("Tables/Battle/profession");
            m_kLoadTableFunc.Call("profession", strContent);
#endif

            // Animation配置表
            //String strContent = ResourceManager.Instance.LoadText("Tables/Battle/Animation");
            //m_kLoadTableFunc.Call("Animation", strContent);
            //m_kAniamtionTable = new AnimationTable("Animation");
            //yield return null;

        }


        public object GetProperty(String strTbName, String strKey, String strColName)
        {
#if FIFA_CLIENT
            if (null == m_kGetPropFunc)
                return null;
            return m_kGetPropFunc.Call(strTbName, strKey, strColName)[0];
#else
            return null;
#endif
        }

        //public object GetTable(String strTbName)
        //{
        //    if (null == m_kGetTableFunc)
        //        return null;
        //    return m_kGetTableFunc.Call(strTbName)[0];
        //}


        public PosCfgTable PosCfgTbl
        {
            get { return m_kPosCfgTbl; }

        }
        public EventAfterStoppingTable EvtAfterStopTable
        {
            get { return m_kEvtAfterStopTable; }

        }

        public BattleInfoTable BattleInfoTable
        {
            get { return m_kBattleInfoTable; }

        }
        public HeroTable HeroTbl
        {
            get { return m_kHeroTable; }

        }

        public CoachTable CoachTable
        {
            get { return m_kCoachTable; }

        }

        public AIConfig AIConfig
        {
            get { return m_kAIConfig; }

        }

        public GroundAreaTable GroundAreaTable
        {
            get { return m_kGroundAreaTable; }

        }

        public PassTimeTable PassTimeTable
        {
            get { return m_kPassTimeTable; }
        }

        public ShootTimeTable ShootTimeTbl
        {
            get { return m_kShootTimeTbl; }
        }

        public SensitivityFactorTable SensitivityFactorTbl
        {
            get { return m_kSensitivityFactorTable; }
        }

        public SettlementFactorTable SettlementFactorTbl
        {
            get { return m_kSettlementFactorTable; }
        }

        public AttackTacticalConfig AttackTacticalConfig
        {
            get { return m_kAttackTacticalConfig; }
        }

        public DistanceDecayTable DistanceDecayTbl
        {
            get { return m_kDistanceDecayConfig; }
        }

        public DistanceCoefficientTableConfig DistanceCoefficientTbl
        {
            get { return m_kDistanceCoefficientTableConfig; }
        }
        public DistanceCoefficientTableConfig DistanceCoefficientTbl1
        {
            get { return m_kDistanceCoefficientTableConfig1; }
        }
        public InterceptCoefficientDataConfig InterceptCoefficientDataConfig
        {
            get { return m_kInterceptCoefficientDataConfig; }
        }
        public MarkCoefficientDataConfig MarkCoefficientDataConfig
        {
            get { return m_kMarkCoefficientDataConfig; }
        }
        public TacticalPosCoefficientDataConfig TacticalPosCoefficientDataConfig
        {
            get { return m_kTacticalPosCoefficientDataConfig; }
        }
        public MidKickOffPosTableConfig MidKickOffPosTableConfig
        {
            get { return m_kMidKickOffPosTableConfig; }
        }
        public HomePositionZDataConfig HomePositionZDataConfig
        {
            get { return m_kHomePositionZDataConfig; }
        }
        public AniDataConfig AniDataConfig
        {
            get { return m_AniDataConfig; }
        }
        public AniCombineConfig AniCombineConfig
        {
            get { return m_AniCombineConfig; }
        }
        public AniBeahaviorConfig AniBeahaviorConfig
        {
            get { return m_AniBeahaviorConfig; }
        }
        public AniStateLayerConfig AniStateLayerConfig
        {
            get { return m_AniStateLayerConfig; }
        }

        public SkillTable SkillTbl
        {
            get { return m_kSkillTable; }
        }
        public BaseFxTable BaseFxTbl
        {
            get { return m_kBaseFxTable; }
        }
        public CameraEffectTable CameraEffectTbl
        {
            get { return m_kCameraEffectTable; }
        }

        public GhostEffectTable GhostEffectTbl
        {
            get { return m_kGhostEffectTable; }
        }

        public RaidNPCTable RaidNpcTbl
        {
            get { return m_kRaidNpcTable; }
        }

        public BattleTextTable BattleTextTbl
        {
            get { return m_kBattleTextbl; }
        }
        public BattleTextCondTable BattleTextCondTbl
        {
            get { return m_kBattleTextCondTbl; }
        }
        public ConfrontationBasicTable ConfrontationBasicTbl
        {
            get { return m_kConfrontationBasicTable; }
        }

        public EnergyTable EnergyTbl
        {
            get { return m_kEnergyTbl; }
        }

        public FormationTable FormationTbl
        {
            get { return m_kFormationTbl; }
        }

        public SkillAppearTable SkillAppearTbl
        {
            get { return m_kSkillAppearTbl; }
        }
        
        public HotSpotTable hotSpot { get { return mHotSpotTable; } }
        public DefenceDensityTable defenceDensity { get { return mDefenceDensityTable; } }


        public BattlePosTable BattlePosTbl
        {
            get
            {
                return m_kBattlePosTbl;
            }
        }

        public GoalCelebrationTable GoalCelebrationTb
        {
            get { return m_goalCelebrationTbl; }
        }

        public ProfessionTable ProfessionTbl
        {
            get { return m_kProfessionTbl; }
        }

        private ProfessionTable m_kProfessionTbl = null;
        private EventAfterStoppingTable m_kEvtAfterStopTable = null;
        private PosCfgTable m_kPosCfgTbl = null;
        private BattleInfoTable m_kBattleInfoTable = null;
        private HeroTable m_kHeroTable = null;
        private CoachTable m_kCoachTable = null;
        private AIConfig m_kAIConfig = null;
        private GroundAreaTable m_kGroundAreaTable = null;
        private PassTimeTable m_kPassTimeTable = null;
        private SensitivityFactorTable m_kSensitivityFactorTable = null;
        private SettlementFactorTable m_kSettlementFactorTable = null;
        private AttackTacticalConfig m_kAttackTacticalConfig = null;
        private DistanceDecayTable m_kDistanceDecayConfig = null;
        private DistanceCoefficientTableConfig m_kDistanceCoefficientTableConfig = null;
        private DistanceCoefficientTableConfig m_kDistanceCoefficientTableConfig1 = null;
        private InterceptCoefficientDataConfig m_kInterceptCoefficientDataConfig = null;
        private MarkCoefficientDataConfig m_kMarkCoefficientDataConfig = null;
        private TacticalPosCoefficientDataConfig m_kTacticalPosCoefficientDataConfig = null;
        private MidKickOffPosTableConfig m_kMidKickOffPosTableConfig = null;
        private HomePositionZDataConfig m_kHomePositionZDataConfig = null;
        private SkillTable m_kSkillTable = null;
        private CameraEffectTable m_kCameraEffectTable = null;
        private GhostEffectTable m_kGhostEffectTable = null;
        private BaseFxTable m_kBaseFxTable = null;
        private RaidNPCTable m_kRaidNpcTable = null;
        private HotSpotTable mHotSpotTable = null;
        private DefenceDensityTable mDefenceDensityTable = null;
        private BattleTextTable m_kBattleTextbl = null;
        private BattleTextCondTable m_kBattleTextCondTbl = null;
        private EnergyTable m_kEnergyTbl = null;
        private ConfrontationBasicTable m_kConfrontationBasicTable = null;
        private ShootTimeTable m_kShootTimeTbl = null;
        private FormationTable m_kFormationTbl = null;          // 球队阵型配置
        private SkillAppearTable m_kSkillAppearTbl = null;
        #if FIFA_CLIENT
        private LuaFunction m_kGetTableFunc = null;
        private LuaFunction m_kLoadTableFunc = null;
        private LuaFunction m_kGetPropFunc = null;
        #endif
        private Dictionary<String, String> m_kTableList = new Dictionary<String, String>();


        #region 比赛动画相关配置
        protected AniDataConfig m_AniDataConfig = null;
        protected AniCombineConfig m_AniCombineConfig = null;
        protected AniBeahaviorConfig m_AniBeahaviorConfig = null;
        protected AniStateLayerConfig m_AniStateLayerConfig = null;
        #endregion

        #region 战斗球员跑动位置配置
        private BattlePosTable m_kBattlePosTbl = null;
        #endregion

        #region 庆祝动画配置表
        private GoalCelebrationTable m_goalCelebrationTbl = null;
        #endregion
    }
}