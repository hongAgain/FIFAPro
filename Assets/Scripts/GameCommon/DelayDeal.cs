using UnityEngine;
using LuaInterface;
using System.Collections.Generic;

public class DelayDeal : MonoBehaviour
{
	private static DelayDeal instance;

	public class Data
	{
		public LuaFunction mCallback;
		public int mSteps;
		public int mEnqueueFrame;
		public int mCurSteps = 0;
	}

	private static Queue<Data> sDatas = new Queue<Data>();

	public static void EnqueueEvent(LuaFunction luaFunc, int steps)
	{
		var data = new Data();
		data.mCallback = luaFunc;
		data.mSteps = steps;
		data.mEnqueueFrame = Time.frameCount;
		sDatas.Enqueue(data);

		if (instance == null)
		{
			instance = FindObjectOfType<DelayDeal>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (sDatas.Count <= 0) return;

		int curFrame = Time.frameCount;

		var data = sDatas.Peek();

		if (data.mEnqueueFrame < curFrame)
		{
			if (data.mCurSteps < data.mSteps)
			{
				/*if (data.mCurSteps == 0)
				{
					Profiler.BeginSample("Create Lobby Scene");
				}
				else if (data.mCurSteps <= 11)
				{
					Profiler.BeginSample(string.Format("Create {0} Player", data.mCurSteps));
				}
				else
				{
					Profiler.BeginSample("Combine Mesh");
				}*/
				
				data.mCallback.Call(data.mCurSteps);
				
				//Profiler.EndSample();

				++data.mCurSteps;
			}

			if (data.mCurSteps >= data.mSteps)
			{
				sDatas.Dequeue();
			}
		}
	}

	void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
}