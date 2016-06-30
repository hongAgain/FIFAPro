using System;
using System.Collections.Generic;

namespace Common.Tables
{
    public class TableWordFilter
    {
        public static readonly TableWordFilter Instance = new TableWordFilter();
        #region PublicFunction
        private TableWordFilter()
        {
            Init();
        }

        public void Init()
        {
            Clear();
            ParseContent();
        }
        public void Clear()
        {
            if (_firstCharMap != null)
            {
                _firstCharMap.Clear();
                _firstCharMap = null;
            }

            if (_keywordsTable != null)
            {
                for (int i = 0; i < _keywordsTable.Count; ++i)
                {
                    if (_keywordsTable[i] != null)
                    {
                        _keywordsTable[i].Clear();
                        _keywordsTable[i] = null;
                    }
                }
                _keywordsTable.Clear();
                _keywordsTable = null;
            }
        }

        public bool FilterText(string text)
        {
            bool keyWordFound = false;
            char[] filteredStr = text.ToCharArray();
            int i = 0;
            while (i < text.Length)
            {
                char firstC = text[i];
                if (!_firstCharMap.ContainsKey(firstC))
                {
                    ++i;
                    continue;
                }
                else
                {
                    int bitset = _firstCharMap[firstC];
                    if (bitset == 0)
                        bitset = 0;
                    int tries = text.Length - i;
                    if (tries > _maxFilterLength)
                        tries = _maxFilterLength;
                    bool keyWordFoundThis = false;
                    for (int b = 1; b <= tries; ++b)
                    {
                        if ((bitset & 1 << b) == 0)
                            continue;

                        if (_keywordsTable[b] == null || _keywordsTable[b].Count == 0)
                            continue;

                        string subStr = text.Substring(i, b);
                        if (_keywordsTable[b].ContainsKey(subStr))
                        {
                            for (int f = i; f < i + b; ++f)
                            {
                                filteredStr[f] = '*';
                            }
                            i += b;
                            keyWordFoundThis = true;
                            keyWordFound = true;
                            break;
                        }
                    }

                    if (!keyWordFoundThis)
                        ++i;
                }
            }

            m_strConvertedValue = new string(filteredStr);
            return keyWordFound;
        }

        public bool ParseContent()
        {
            JsonTable kTable = DataManager.Instance.ReadJsonTable("Tables/Common/ForbidWord") as JsonTable;
            if (null == kTable)
                return false;

            _maxFilterLength = 0;
            List<string> keyWords = new List<string>();
            foreach (var kItem in kTable.ItemList)
            {
                _maxFilterLength = Math.Max(_maxFilterLength, kItem.Key.Length);
                keyWords.Add(kItem.Key);
            }

            _keywordsTable = new List<Dictionary<string, bool>>();
            for (int i = 0; i < _maxFilterLength + 1; ++i)
                _keywordsTable.Add(new Dictionary<string, bool>());

            _firstCharMap = new Dictionary<char, int>();
            for (int i = 0; i < keyWords.Count; ++i)
            {
                int len = keyWords[i].Length;
                if (_keywordsTable[len].ContainsKey(keyWords[i]))
                    continue;
                else
                    _keywordsTable[len].Add(keyWords[i], false);

                char firstC = keyWords[i][0];
                Int32 bitset = 0;
                if (_firstCharMap.ContainsKey(firstC))
                {
                    bitset = _firstCharMap[firstC] | (1 << len);
                }
                else
                {
                    bitset = 1 << len;
                }
                _firstCharMap[firstC] = bitset;
            }
            return true;
        }
        #endregion
        public string FilteredWords
        {
            get { return m_strConvertedValue; }
        }
        #region PrivateMember
        string m_strConvertedValue;
        int _maxFilterLength;
        Dictionary<char, Int32> _firstCharMap;                //首字符-32位bitset， 标示是否有以此字符开始，对应长度的敏感词字串
        List<Dictionary<string, bool>> _keywordsTable;       // 敏感词表[敏感词长] - bool没用
        #endregion
    }

}