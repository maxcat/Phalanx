using System.IO;
using UnityEngine;
using System.Collections;

public class RegionBlock : CodeBlock {

#region Fields
	public string RegionDescription;
#endregion

#region Constructor
	public RegionBlock(string regionDescription)
		: base ()
	{
		this.RegionDescription = regionDescription;
		this.tabCountnoAffectedByParent = true;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		openRegion(writer);
		base.WriteToFile(writer);		
		closeRegion(writer);
	}
#endregion

#region Private Functions
	private void openRegion(StreamWriter writer)
	{
		writeLine(writer, "#region " + RegionDescription, 0);
	}

	private void closeRegion(StreamWriter writer)
	{
		writeLine(writer ,"#endregion", 0);
	}
#endregion
}
