using System.IO;
using System.Collections;

public class LineBlock : CodeBlock {

#region Fields
	protected string 				line;
#endregion

#region Constructor
	public LineBlock(string line)
		: base ()
	{
		this.line = line;
	}

	public LineBlock()
		: base ()
	{
		this.line = string.Empty;
	}
#endregion

#region Implement Virtual Functions
	public override void WriteToFile(StreamWriter writer)
	{
		writeLine(writer, line);
	}
#endregion
}
