using UnityEngine;
using System.Collections;


namespace NativeBuilder.XCodeEditor 
{
	public partial class XCMod {

		public ArrayList extCode {
			get {
				return (ArrayList)this._datastore["ext.code"];
			}
		}

		public ArrayList copiedFolder {
			get{
				return (ArrayList)this._datastore["ext.copiedFolder"];
			}
		}

		public Hashtable property {
			get{
				return (Hashtable)this._datastore["ext.property"];
			}
		}
	}
}
