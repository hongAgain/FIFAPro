using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListNetWorkHandler : NetWorkHandler
{
    private LinkedList<WWW> mRequestList = new LinkedList<WWW>();

    public delegate void HandleWWW(WWW www);
    private HandleWWW handleWWW;

    public delegate void Callback();
    private Callback onNoReq;

    void Start()
    {
        StartCoroutine(Pump());
    }

    IEnumerator Pump()
    {
        while (true)
        {
            WWW www = null;
            if (mRequestList.Count > 0)
            {
                www = mRequestList.First.Value;
                float t = Time.realtimeSinceStartup;
                //NGUIDebug.Log("Begin url = " + www.url);
                yield return www;
                //NGUIDebug.Log("End url = " + www.url);
                mRequestList.RemoveFirst();

				if (www.responseHeaders.Count > 0)
				{
					if (string.IsNullOrEmpty(www.error))
					{
//						Debug.LogError("======Header start======");
//						foreach(KeyValuePair<string,string> v in www.responseHeaders)
//						{
//							Debug.LogError(v.Key+":"+v.Value);
//						}
//						Debug.LogError("======Header over======");
						handleWWW(www);
					}
					else
					{
						mRequestList.AddFirst(new WWW(www.url));
					}
				}
				else
				{
					mRequestList.AddFirst(new WWW(www.url));
				}

                if (mRequestList.Count == 0)
                {
                    onNoReq();
                }

                //强制两个请求之间时间间隔大于500毫秒 服务器要求
                while (true)
                {
                    float delta = Time.realtimeSinceStartup - t;
                    if (delta <= 0.5f)
                    {
                        yield return null;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public void Request(string url,Dictionary<string,string> header)
    {
        WWW www = new WWW(url,null,header);
        mRequestList.AddLast(www);
    }

    public void SetHandler(HandleWWW del)
    {
        handleWWW = del;
    }

    public void SetOnNoReq(Callback del)
    {
        onNoReq = del;
    }
}