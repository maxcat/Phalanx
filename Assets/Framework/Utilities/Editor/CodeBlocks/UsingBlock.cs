using System.IO;
using UnityEngine;
using System.Collections;

public class UsingBlock : LineBlock {

#region Constructor
	public UsingBlock(string nameSpace)
		: base ()
	{
		this.line = "using " + nameSpace + ";";
	}
#endregion
}