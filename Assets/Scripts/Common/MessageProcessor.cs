using System.Collections.Generic;

namespace Common
{
    public partial class MessageProcessor
    {
        private Dictionary<MessageType, object> m_kLuaHandler = new Dictionary<MessageType, object>();


        public MessageProcessor(MessageDispatcher kDispatcher)
        {
            m_kDispatcher = kDispatcher;
            InitHandler();
            InitClientHandler();
        }
        private void InitHandler()
        {
            if (null == m_kDispatcher)
                return;
            m_kDispatcher.AddHandler(MessageType.MessageHomePositionUpdate, OnRefrshPlayerHomePosition);
            //// 开始播放技能
            //m_kDispatcher.AddHandler(MessageType.SkillBegin, OnSkillBegin);
            //// 停止播放技能
            //m_kDispatcher.AddHandler(MessageType.SkillEnd, OnSkillEnd);
            // 开始播放技能
            //m_kDispatcher.AddHandler(MessageType.GenValidData, OnGenValidData);
        }

        //private void OnGenValidData(Message kInMsg)
        //{
        //    GenValidDataMessage kMsg = kInMsg as GenValidDataMessage;
        //    PVEValidData kData = new PVEValidData();
        //    kData.ActionID = kMsg.ActionID;
        //    kData.SponsorIDList = kMsg.HeroID;
        //    kData.DefenderIDList = kMsg.NpcIDList;
        //    kData.SponsorTeamScore = kMsg.Score;
        //    kData.DefendTeamScore = kMsg.NpcScore;
        //    GlobalBattleInfo.Instance.PVEDataList.Add(kData);
        //}
        //private void OnSkillBegin(Message kInMsg)
        //{
        //    SkillBeginMessage kMsg = kInMsg as SkiacllBeginMessage;
        //    LLUnit kUnit = kMsg.Unit;
        //    if (null != kUnit)
        //        kUnit.CastSkill(kMsg.SkillID);
        //}

        //private void OnSkillEnd(Message kInMsg)
        //{
        //    //SkillEndMessage kMsg = kInMsg as SkillEndMessage;
        //    //LLUnit kUnit = kMsg.Unit;
        //    //if (null != kUnit)
        //    //    kUnit.EndSkill(kMsg.SkillID);
        //}

        private void OnRefrshPlayerHomePosition(Message _msg)
        {
            RefreshHomePositionMessage _kMsg = _msg as RefreshHomePositionMessage;
            _kMsg.GetScene.RefreshOtherHomePosition();
        }

        private MessageDispatcher m_kDispatcher = null;
    }
}
