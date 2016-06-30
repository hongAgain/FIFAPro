using System;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class FIFARandom
    {
        private static Random sRandom;

        static FIFARandom()
        {
#if GAME_AI_ONLY
            m_kRandomList.Clear();
            int seed = (int)(DateTime.Now.Ticks & 0xffffffffL);
            sRandom = new Random(seed);

            // 初始化随机数种子
            for (int i = 0; i < 200; i++)
            {
                m_kRandomList.Add(sRandom.NextDouble());
            }
            m_iRandomIdx = 0;
#endif
        }

        public static double GetRandomValue(double dFrom, double dTo)
        {
            if (m_iRandomIdx >= m_kRandomList.Count)
                m_iRandomIdx = 0;
            double iRetVal = dFrom + m_kRandomList[m_iRandomIdx++] * (dTo - dFrom);
            return iRetVal;
        }

        public delegate void OnSelect();

        public static void Select(double[] rates, OnSelect[] doSelect)
        {
            double rate = GetRandomValue(0, 1);
            double upper = 0d;
            for (int i = 0; i < rates.Length; ++i)
            {
                upper += rates[i];
                if (rate <= upper)
                {
                    doSelect[i]();
                    break;
                }
            }
        }

        public static void Select(int[] rates, OnSelect[] doSelect)
        {
            int rate = (int)GetRandomValue(0,100);

            int upper = 0;
            for (int i = 0; i < rates.Length; ++i)
            {
                upper += rates[i];
                if (rate <= upper)
                {
                    doSelect[i]();
                    break;
                }
            }
        }

        public static int GetCurRandomIdx()
        {
            if (m_iRandomIdx == m_kRandomList.Count)
                return m_kRandomList.Count - 1;
            return m_iRandomIdx - 1;
        }

        public static List<double> RandomList
        {
            get { return m_kRandomList; }
        }
        private static List<double> m_kRandomList = new List<double>();
        private static int m_iRandomIdx = 0;
    }
}