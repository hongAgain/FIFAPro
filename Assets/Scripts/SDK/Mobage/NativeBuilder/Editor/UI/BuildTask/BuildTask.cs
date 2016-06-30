using UnityEngine;
using UnityEditor;
using System;

namespace NativeBuilder
{
	public class BuildTask 
	{
		public virtual void OnPreBuild() {}
		
		public virtual void OnBuild() {}

		public virtual void OnPostBuild() {}

		public virtual void OnException(Exception e)
		{
			Debug.LogException(e);
		}

		public virtual void OnFinally() {}

		public void Build(){
			try{
				this.OnPreBuild();
				this.OnBuild();
				this.OnPostBuild();
			}catch(Exception e){
				this.OnException(e);
			}
			finally{
				this.OnFinally();
			}
		}
		
	}

}


