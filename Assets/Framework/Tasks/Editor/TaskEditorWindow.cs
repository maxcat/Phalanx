using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TaskEditorWindow : EditorWindow {

	#region Fields
	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	protected Task							m_mainTask;
	#endregion

	#region Static Functions
	public static void showWindow()
	{
		EditorWindow.GetWindow(typeof(TaskEditorWindow));
	}
	#endregion

	#region Public API
	public void init(TaskFactory factory)
	{
		if(factory.HasRecursiveStruct())
			Debug.LogError("[ERROR]TaskEditorWindow->init: factory has recursive structure.");
		else
			m_mainTask = factory.CreateTask();
	}
	#endregion

	#region GUI Functions
	void OnGUI()
	{
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Text Field", myString);

		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup ();

		if(m_mainTask != null)
		{
			GUILayout.BeginScrollView(new Vector2(0, 0), true, true);
			m_mainTask.Draw();

			GUILayout.EndScrollView();
		}

	}
	#endregion

}
