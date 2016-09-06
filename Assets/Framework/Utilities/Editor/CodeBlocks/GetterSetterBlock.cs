using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;

public class GetterBlock : CodeBlock
{
#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		writeLine(writer, "get {");
		base.WriteToFile(writer);
		writeLine(writer, "}");
	}
#endregion
}

public class SetterBlock : CodeBlock
{
#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		writeLine(writer, "set {");
		base.WriteToFile(writer);
		writeLine(writer, "}");
	}
#endregion
}

public class GetterSetterBlock : CodeBlock {

#region Fields
	public ACCESS_LEVEL 				AccessLevel;
	public VIRTUAL_OR_OVERRIDE 			VirtualOrOverride;
	public bool 					IsStatic;
	public string 					Name;
	public string 					TypeName;
#endregion

#region Constructor
	public GetterSetterBlock(string typeName, string name, ACCESS_LEVEL level = ACCESS_LEVEL.PUBLIC, VIRTUAL_OR_OVERRIDE virtualOrOverride = VIRTUAL_OR_OVERRIDE.NONE, bool isStatic = false)
		: base ()
	{
		this.TypeName = typeName;
		this.AccessLevel = level;
		this.Name = name;
		this.VirtualOrOverride = virtualOrOverride;
		this.IsStatic = isStatic;
	}

	public GetterSetterBlock(System.Type type, string name, ACCESS_LEVEL level = ACCESS_LEVEL.PUBLIC, VIRTUAL_OR_OVERRIDE virtualOrOverride = VIRTUAL_OR_OVERRIDE.NONE, bool isStatic = false)
		: base ()
	{
		this.TypeName = type.ToString();
		this.AccessLevel = level;
		this.Name = name;
		this.VirtualOrOverride = virtualOrOverride;
		this.IsStatic = isStatic;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		StringBuilder builder = new StringBuilder();		
		builder.Append(GetAccessLevel(AccessLevel));
		if(IsStatic)
		{
			builder.Append("static ");
		}
		builder.Append(GetVirtualOrOverride(VirtualOrOverride));
		builder.Append(TypeName);
		builder.Append(CodeBlock.SPACE);
		builder.Append(Name);
		writeLine(writer, builder.ToString());
		writeLine(writer, "{");

		base.WriteToFile(writer);

		writeLine(writer, "}");
	}
#endregion
}
