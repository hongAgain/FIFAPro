using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public delegate void MessageHandlerDelegate(Message msg);

    public class MessageDispatcher
    {
        public static readonly MessageDispatcher Instance = new MessageDispatcher();
        private const int m_iQueueSize = 2;
        private Queue<Message>[] m_kMsgs = new Queue<Message>[m_iQueueSize];
        private List<MessageHandlerDelegate>[] m_kHandlers = new List<MessageHandlerDelegate>[(int)MessageType.MessageTypeCount];
        private int m_iActiveQueue = 0;

        private MessageDispatcher()
        {
            for (int i = 0; i < m_kMsgs.Length; i++)
                m_kMsgs[i] = new Queue<Message>();

            for (int i = 0; i < m_kHandlers.Length; i++)
                m_kHandlers[i] = new List<MessageHandlerDelegate>();
        }
        
        /// <summary>
        /// 发送消息到队列，立即返回，消息延后处理
        /// </summary>
        /// <param name="msg"></param>
        public void PostMessage(Message msg)
        {
            m_kMsgs[m_iActiveQueue].Enqueue(msg);
        }

        /// <summary>
        /// 立即触发消息处理函数，不再进入队列，阻塞到消息处理完成
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(Message msg)
        {
            for (int i = 0; i < m_kHandlers[(int)msg.Type].Count;i++ )
            {
                m_kHandlers[(int)msg.Type][i](msg);
            }
        }

        /// <summary>
        /// 增加消息处理函数
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="handler">处理函数</param>
        public void AddHandler(MessageType kMsgType, MessageHandlerDelegate kMsgHandler)
        {
            if (!m_kHandlers[(int)kMsgType].Contains(kMsgHandler))
                m_kHandlers[(int)kMsgType].Add(kMsgHandler);
        }

        /// <summary>
        /// 移除消息处理函数
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="handler">处理函数</param>
        public void RemoveHandler(MessageType kMsgType, MessageHandlerDelegate kMsgHandler)
        {
            m_kHandlers[(int)kMsgType].Remove(kMsgHandler);
        }

        public void Update()
        {
            var kMsgQueue = m_kMsgs[m_iActiveQueue];
            m_iActiveQueue = (m_iActiveQueue + 1) % m_iQueueSize;
            while (kMsgQueue.Count > 0)
            {
                var kMsg = kMsgQueue.Dequeue();
                for (int i = 0; i < m_kHandlers[(int)kMsg.Type].Count;i++ )
                {
                    m_kHandlers[(int)kMsg.Type][i](kMsg);
                }
            }
        }
    }
}
