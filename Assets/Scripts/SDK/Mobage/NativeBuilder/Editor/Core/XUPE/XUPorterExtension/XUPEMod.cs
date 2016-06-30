using UnityEngine;
using System.Collections;
using System.IO;

namespace NativeBuilder.XCodeEditor
{
	public class XUPEMod {

		public string path {get; private set;}
		public string parentPath {get; private set;}
		public string name {get; private set;}
		public XCMod xcmod {get; private set;}

		public XUPEMod(string path){

			path = Path.GetFullPath(path);
			DirectoryInfo di = new DirectoryInfo( path );

			if( !di.Exists ) {
				throw new IOException(path + " not exsits!");
			}


			this.path = path;
			this.name = System.IO.Path.GetFileNameWithoutExtension( path );
			this.parentPath = System.IO.Path.GetDirectoryName( path );
			this.xcmod = new XCMod(Path.Combine(path, "xupe.projmods"));
		}

	}
}
