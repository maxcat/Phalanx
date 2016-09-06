using System.IO;
using UnityEngine;
using System.Collections;

public class IfBlock : CodeBlock {

#region Fields
	string 				conditions;
#endregion

#region Constructor
	public IfBlock(string conditions) : base ()
	{
		this.conditions = conditions;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		writeLine(writer, "if (" + conditions + ")");
		writeLine(writer, "{");
		base.WriteToFile(writer);
		writeLine(writer, "}");
	}
#endregion
}

public class ElseBlock : CodeBlock {

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		writeLine(writer, "else");
		writeLine(writer, "{");
		base.WriteToFile(writer);
		writeLine(writer, "}");
	}
#endregion
}

public class ElseIfBlock : CodeBlock
{

#region Fields
	string 				conditions;
#endregion

#region Constructor
	public ElseIfBlock(string conditions) : base ()
	{
		this.conditions = conditions;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		writeLine(writer, "else if (" + conditions + ")");
		writeLine(writer, "{");
		base.WriteToFile(writer);
		writeLine(writer, "}");
	}
#endregion
}
