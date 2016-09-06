using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstructorBlock : CodeBlock {

#region Fields
	List<ParamStringPair> 				paramList;
	List<string>	 				baseParamList;
	bool 						withBase;
	string 						className;
#endregion

#region Constructor
	public ConstructorBlock(string className, List<ParamStringPair> paramList = null, bool withBase = false, List<string> baseParamList = null)
		: base ()
	{
		this.className = className;
		this.paramList = paramList;
		this.withBase = withBase;
		this.baseParamList = baseParamList;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("public");
		builder.Append(CodeBlock.SPACE);
		builder.Append(className);
		builder.Append("(");
		builder.Append(ParamStringPair.CreateParamListString(paramList));
		builder.Append(")");
		if(withBase)
		{
			builder.Append(CodeBlock.SPACE);
			builder.Append(":");
			builder.Append(CodeBlock.SPACE);
			builder.Append("base");
			builder.Append("(");
			builder.Append(ParamStringPair.CreateBaseParamListString(baseParamList));
			builder.Append(")");
		}
		writeLine(writer, builder.ToString());
		writeLine(writer, "{");
		base.WriteToFile(writer);
		writeLine(writer, "}");
	}
#endregion
}
