using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ClassBlock : CodeBlock {

#region Fields
	string 					className;
	List<System.Type> 			parents;
	Dictionary<string, RegionBlock> 	regionBlockMap;
#endregion

#region Constructor
	public ClassBlock(string className, List<System.Type> parents = null)
		: base ()
	{
		this.className = className;
		this.parents = parents;
		this.regionBlockMap = new Dictionary<string, RegionBlock>();
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		openClass(writer);
		base.WriteToFile(writer);
		closeClass(writer);
	}
#endregion

#region Private Functions
	private RegionBlock getRegionBlock(string regionName)
	{
		if(!regionBlockMap.ContainsKey(regionName))
		{
			RegionBlock block = new RegionBlock(regionName);
			regionBlockMap.Add(regionName, block);
			this.AddChild(block);
		}
		return regionBlockMap[regionName];
	}
#endregion

#region Private Funtions
	private void openClass(StreamWriter writer)
	{
		if(parents != null && parents.Count > 0)
		{
			StringBuilder parenBuilder = new StringBuilder();
			parenBuilder.Append("public class " + className + " : ");
			for(int i = 0; i < parents.Count; i ++)
			{
				parenBuilder.Append(parents[i].ToString());
				if(i < parents.Count - 1)
					parenBuilder.Append(", ");
			}
			writeLine(writer, parenBuilder.ToString());
		}
		else
		{
			writeLine(writer, "public class " + className);
		}
		writeLine(writer, "{");
	}

	private void closeClass(StreamWriter writer)
	{
		writeLine(writer, "}");		
	}
#endregion

#region Public API
	public void AddConstructor(string regionName, List<ParamStringPair> paramList = null, bool withBase = false, List<string> baseParamList = null)
	{
		AddBlock(regionName, new ConstructorBlock(className, paramList, withBase, baseParamList));
	}

	public void AddBlock(string regionName, CodeBlock block)
	{
		RegionBlock region = getRegionBlock(regionName);
		region.AddChild(block);
	}
#endregion
}
