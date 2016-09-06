using System.Text;
using System.Collections;

public class FieldBlock : LineBlock {

#region Fields
	public string 				TypeName;
	public string 				Name;
	public ACCESS_LEVEL 			AccessLevel;
	public bool 				IsStatic; 
#endregion

#region Constructor
	public FieldBlock(System.Type type, string name, ACCESS_LEVEL accessLevel = ACCESS_LEVEL.PRIVATE, bool isStatic = false)
		: base ()
	{
		this.TypeName = type.ToString();
		this.Name = name;
		this.AccessLevel = accessLevel;
		this.IsStatic = isStatic;
		Init();
	}

	public FieldBlock(string typeName, string name, ACCESS_LEVEL accessLevel = ACCESS_LEVEL.PRIVATE, bool isStatic = false)
		: base ()
	{
		this.TypeName = typeName; 
		this.Name = name;
		this.AccessLevel = accessLevel;
		this.IsStatic = isStatic;
		Init();
	}
#endregion

#region Private Functions
	private void Init()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append(GetAccessLevel(AccessLevel));
		if(IsStatic)
		{
			builder.Append("static");
			builder.Append(CodeBlock.SPACE);
		}
		builder.Append(TypeName);
		builder.Append(CodeBlock.SPACE);
		builder.Append(CodeBlock.FIELD_TAB);
		builder.Append(Name);
		builder.Append(";");

		line = builder.ToString();
	}
#endregion
}
