using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;


public class ScriptBuilder  {

#region Fields
	StreamWriter 				writer;
#endregion

#region Constructor
	public ScriptBuilder(StreamWriter writer)
	{
		this.writer = writer;
	}
#endregion

#region Public API
	public void WriteIncludes(string nameSpace)
	{
		WriteLine("using " + nameSpace + ";");
	}

	public void WriteEmptyLine()
	{
		WriteLine("");
	}

	public void OpenClass(string className, List<string> parents = null, int tabCount = 0)
	{
		if(parents != null && parents.Count > 0)
		{
			StringBuilder parenBuilder = new StringBuilder();
			parenBuilder.Append("public class " + className + " : ");
			for(int i = 0; i < parents.Count; i ++)
			{
				parenBuilder.Append(parents[i]);
				if(i < parents.Count - 1)
					parenBuilder.Append(", ");
			}
			WriteLine(parenBuilder.ToString(), tabCount);
		}
		else
		{
			WriteLine("public class " + className, tabCount);
		}
		WriteLine("{", tabCount);
	}

	public void CloseClass()
	{
		
	}

	private void WriteLine(string line, int tabCount = 0)
	{
		StringBuilder lineBuilder = new StringBuilder();
		for(int i = 0; i < tabCount; i ++)
		{
			lineBuilder.Append("\t");
		}
		lineBuilder.Append(line);


		writer.WriteLine(lineBuilder.ToString()); 
	}
#endregion
}
