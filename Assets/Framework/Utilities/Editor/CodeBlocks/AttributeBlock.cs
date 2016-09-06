using System.Collections;

public class AttributeBlock : LineBlock {

#region Constructor
	public AttributeBlock(string attribute) : base()
	{
		this.line = "[" + attribute + "]";
	}
#endregion
}
