using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public enum ACCESS_LEVEL : int 
{
	PRIVATE  = 0,
	PROTECTED,
	PUBLIC,
}

public enum VIRTUAL_OR_OVERRIDE : int 
{
	NONE = 0,
	VIRTUAL,
	OVERRIDE,
}

public class CodeBlock
{

#region Static Configurations
	public static string 			SPACE = " ";
	public static string 			CODE_TAB = "\t";
	public static string 			FIELD_TAB = "\t\t\t";
#endregion

#region Fields
	protected int 				tabCount;
	protected List<CodeBlock> 		children;
	protected bool 				tabCountnoAffectedByParent = false;
#endregion

#region Getter and Setter
	public int TabCount
	{
		get { return tabCount; }
		set { tabCount = value; }
	}
#endregion

#region Protected Functions
	protected void writeLine(StreamWriter writer, string line, int forceTabCount)
	{
		StringBuilder lineBuilder = new StringBuilder();
		for(int i = 0; i < forceTabCount; i ++)
		{
			lineBuilder.Append(CODE_TAB);
		}
		lineBuilder.Append(line);

		writer.WriteLine(lineBuilder.ToString()); 
	}

	protected void writeLine(StreamWriter writer, string line)
	{
		StringBuilder lineBuilder = new StringBuilder();
		for(int i = 0; i < tabCount; i ++)
		{
			lineBuilder.Append(CODE_TAB);
		}
		lineBuilder.Append(line);

		writer.WriteLine(lineBuilder.ToString()); 
	}
#endregion

#region Public API 
	public void AddChild(CodeBlock child)
	{
		if(children == null)
		{
			children = new List<CodeBlock>();
		}

		children.Add(child);
	}

	public string GetAccessLevel(ACCESS_LEVEL level)
	{
		switch(level)
		{
			case ACCESS_LEVEL.PRIVATE:
				return "";
			case ACCESS_LEVEL.PUBLIC:
				return "public" + CodeBlock.SPACE;
			case ACCESS_LEVEL.PROTECTED:
				return "protected" + CodeBlock.SPACE;
		}
		return "";
	}

	public string GetVirtualOrOverride(VIRTUAL_OR_OVERRIDE input)
	{
		switch(input)
		{
			case VIRTUAL_OR_OVERRIDE.NONE:
				return "";
			case VIRTUAL_OR_OVERRIDE.OVERRIDE:
				return "override" + CodeBlock.SPACE;
			case VIRTUAL_OR_OVERRIDE.VIRTUAL:
				return "virtual" + CodeBlock.SPACE;
		}
		return "";
	}
#endregion

#region Virtual Functions
	public virtual void WriteToFile(StreamWriter writer) 
	{
		if(children != null && children.Count > 0)
		{
			for(int i = 0; i < children.Count; i ++)
			{
				CodeBlock child = children[i];
				if(!tabCountnoAffectedByParent)
					child.TabCount = tabCount + 1;
				else
					child.TabCount = tabCount;
				child.WriteToFile(writer);	
			}
		}
	}
#endregion
}
