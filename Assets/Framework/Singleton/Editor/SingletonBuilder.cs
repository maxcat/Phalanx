using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SingletonBuilder {

#region Static Functions
	[MenuItem("Assets/Create/Persist Singleton")]
	public static void CreatePersistSingleton()
	{
		createFile(true, "NewPersistSingleton");
	}

	[MenuItem("Assets/Create/Singleton")]
	public static void CreateSingleton()
	{
		createFile(false, "NewSingleton");
	}
#endregion

#region Private Functions
	private static void createFile(bool isPersist, string defaultFileName)
	{
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);

		string filePath = EditorUtility.SaveFilePanel("Create Singleton", path, defaultFileName, "cs");

		string fileName = Path.GetFileName(filePath); 
		fileName = fileName.Replace(".cs", "");
		using (StreamWriter outfile = new StreamWriter(filePath))
		{
			CodeBlock mainBlock = new CodeBlock();
			mainBlock.TabCount = -1;

			AddUsingBlocks(mainBlock);
			mainBlock.AddChild(new LineBlock());
			
			mainBlock.AddChild(createClassBlock(fileName, isPersist));

			mainBlock.WriteToFile(outfile);
		}

		AssetDatabase.Refresh();
	}

	private static void AddUsingBlocks(CodeBlock mainBlock)
	{
		mainBlock.AddChild(new UsingBlock("UnityEngine"));
		mainBlock.AddChild(new UsingBlock("System.Collections"));
	}

	private static ClassBlock createClassBlock(string fileName, bool isPersist)
	{
		List<System.Type> parents = new List<System.Type>();
		parents.Add(typeof(Singleton));
		ClassBlock block = new ClassBlock(fileName, parents);
		{
			block.AddBlock("Static Fields", new FieldBlock
					(
					 "GameObject", 
					 "parentObj", 
					 ACCESS_LEVEL.PRIVATE, 
					 true)
					);
			block.AddBlock("Fields", new FieldBlock
					(
					 fileName, 
					 "instance", 
					 ACCESS_LEVEL.PRIVATE, 
					 true)
					);

			block.AddChild(new LineBlock());
			block.AddBlock("Static Getter and Setter", createGetterBlock(fileName, isPersist));
			block.AddChild(new LineBlock());
			block.AddBlock("Static Functions", createRemoveFunctionBlock());
			block.AddChild(new LineBlock());

			FunctionBlock awakeBlock = new FunctionBlock("void", "Awake");
			FunctionBlock applicationPauseBlock = new FunctionBlock("void", "OnApplicationPause");
			var monoParamList = new List<ParamStringPair>();
			monoParamList.Add(new ParamStringPair("int", "level"));
			FunctionBlock onLevelWasLoadedBlock = new FunctionBlock("void", "OnLevelWasLoaded", monoParamList);
			block.AddBlock("Override MonoBehaviour", awakeBlock);
			block.AddBlock("Override MonoBehaviour", new LineBlock());
			block.AddBlock("Override MonoBehaviour", applicationPauseBlock);
			block.AddBlock("Override MonoBehaviour", new LineBlock());
			block.AddBlock("Override MonoBehaviour", onLevelWasLoadedBlock);

			block.AddChild(new LineBlock());
			block.AddBlock("Implement Virtual Functions", new FunctionBlock("void", "init", null, ACCESS_LEVEL.PROTECTED, VIRTUAL_OR_OVERRIDE.OVERRIDE));
			block.AddBlock("Implement Virtual Functions", new LineBlock());
			block.AddBlock("Implement Virtual Functions", new FunctionBlock("void", "clear", null, ACCESS_LEVEL.PROTECTED, VIRTUAL_OR_OVERRIDE.OVERRIDE));
		}

		return block;
	}

	private static CodeBlock createGetterBlock(string fileName, bool isPersist)
	{
		var block = new GetterSetterBlock(fileName, "Instance", ACCESS_LEVEL.PUBLIC, VIRTUAL_OR_OVERRIDE.NONE, true);
		var getterBlock = new GetterBlock();
		var ifBlock = new IfBlock("parentObj == null");
		ifBlock.AddChild(new LineBlock("parentObj = new GameObject(\"" + fileName + "\");"));
		if(isPersist)
			ifBlock.AddChild(new LineBlock("DontDestroyOnLoad(parentObj);"));
		ifBlock.AddChild(new LineBlock("instance = parentObj.AddComponent<" + fileName + ">();"));
		ifBlock.AddChild(new LineBlock("instance.init();"));
		getterBlock.AddChild(ifBlock);
		getterBlock.AddChild(new LineBlock("return instance;"));

		block.AddChild(getterBlock);

		return block;
	}

	private static CodeBlock createRemoveFunctionBlock()
	{
		var block = new FunctionBlock("void", "Remove", null, ACCESS_LEVEL.PUBLIC, VIRTUAL_OR_OVERRIDE.NONE, true);
		block.AddChild(new LineBlock("instance.clear();"));
		block.AddChild(new LineBlock("Destroy(parentObj);"));
		return block;
	}
#endregion
}
