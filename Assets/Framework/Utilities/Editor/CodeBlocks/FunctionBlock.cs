using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParamStringPair
{
#region Fields
	public string 			ParamType;
	public string 			ParamName;
	public string 			DefaultValue;
#endregion

#region Constructor
	public ParamStringPair(string paramType, string paramName, string defaultValue = "")
	{
		this.ParamType = paramType;
		this.ParamName = paramName;
		this.DefaultValue = defaultValue;
	}

	public ParamStringPair(System.Type paramType, string paramName, string defaultValue = "")
	{
		this.ParamType = paramType.ToString();
		this.ParamName = paramName;
		this.DefaultValue = defaultValue;
	}
#endregion

#region Static Functions
	public static string CreateParamListString(List<ParamStringPair> paramList)
	{
		StringBuilder builder = new StringBuilder();
		if(paramList != null && paramList.Count > 0)
		{
			for (int i = 0; i < paramList.Count; i ++)
			{
				builder.Append(paramList[i].CodeBlockString);	
				if(i < paramList.Count - 1)
				{
					builder.Append(",");
					builder.Append(CodeBlock.SPACE);
				}
			}
			return builder.ToString();
		}
		return "";
	}

	public static string CreateBaseParamListString(List<string> baseParamList)
	{
		StringBuilder builder = new StringBuilder();
		if(baseParamList != null && baseParamList.Count > 0)
		{
			for(int i = 0; i < baseParamList.Count; i ++)
			{
				builder.Append(baseParamList[i]);
				if(i < baseParamList.Count - 1)
				{
					builder.Append(",");
					builder.Append(CodeBlock.SPACE);
				}
			}
			return builder.ToString();
		}
		return "";
	}
#endregion

#region Getter and Setter
	public string CodeBlockString
	{
		get {
			if(DefaultValue != string.Empty)
			{
				return ParamType + CodeBlock.SPACE + ParamName + CodeBlock.SPACE + CodeBlock.SPACE + "=" + CodeBlock.SPACE + DefaultValue; 
			}
			else
			{
				return ParamType + CodeBlock.SPACE + ParamName; 
			}
		}
	}
#endregion
}

public class FunctionBlock : CodeBlock {

#region Fields
	public string 				ReturnType;
	public string 				FunctionName;
	public ACCESS_LEVEL 			AccessLevel;
	public VIRTUAL_OR_OVERRIDE 		VirtualOrOverride;
	public bool 				IsStatic;
	public List<ParamStringPair> 		ParamList;
#endregion

#region Constructor
	public FunctionBlock(string returnType, string functionName, List<ParamStringPair> paramList = null, ACCESS_LEVEL level = ACCESS_LEVEL.PRIVATE, VIRTUAL_OR_OVERRIDE virtualOrOverride = VIRTUAL_OR_OVERRIDE.NONE, bool isStatic = false) : base ()
	{
		this.ReturnType = returnType;
		this.FunctionName = functionName;
		this.AccessLevel = level;
		this.IsStatic = isStatic;
		this.VirtualOrOverride = virtualOrOverride;
		this.ParamList = paramList;
	}

	public FunctionBlock(System.Type returnType, string functionName, List<ParamStringPair> paramList = null,  ACCESS_LEVEL level = ACCESS_LEVEL.PRIVATE, VIRTUAL_OR_OVERRIDE virtualOrOverride = VIRTUAL_OR_OVERRIDE.NONE, bool isStatic = false) : base ()
	{
		this.ReturnType = returnType.ToString();
		this.FunctionName = functionName;
		this.AccessLevel = level;
		this.IsStatic = isStatic;
		this.VirtualOrOverride = virtualOrOverride;
		this.ParamList = paramList;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		StringBuilder builder = new StringBuilder();		
		builder.Append(GetAccessLevel(AccessLevel));
		if(IsStatic)
		{
			builder.Append("static");
			builder.Append(CodeBlock.SPACE);
		}
		builder.Append(GetVirtualOrOverride(VirtualOrOverride));
		builder.Append(ReturnType);
		builder.Append(CodeBlock.SPACE);
		builder.Append(FunctionName);
		builder.Append("(");
		builder.Append(ParamStringPair.CreateParamListString(ParamList));
		builder.Append(")");
		writeLine(writer, builder.ToString());
		writeLine(writer, "{");

		base.WriteToFile(writer);

		writeLine(writer, "}");
	}
#endregion
}
