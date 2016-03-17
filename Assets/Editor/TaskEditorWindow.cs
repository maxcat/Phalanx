using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public enum TASK_TYPE
{
	TASK, 
	DELAY,
	LINKLISTTASK,
	PARALLELTASK,
	SERIETASK,
}


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
	public void init(TaskCollectionData mono)
	{
		// test data
		m_mainTask = new ParallelTasks(mono);

		for(int i = 0; i < 10; i ++)
		{
			(m_mainTask as ParallelTasks).addTask(new Task(mono));
		}

		for(int i = 0; i < 3; i ++)
		{
			(m_mainTask as ParallelTasks).addTask(new LinkListTask(mono));
		}

		SeriesTasks sTask1 = new SeriesTasks(mono);
		SeriesTasks sTask2 = new SeriesTasks(mono);

		for(int i = 0; i < 2; i ++)
		{
			sTask1.addTask(new Task(mono));
		}

		ParallelTasks pTask1 = new ParallelTasks(mono);

		for(int i = 0; i < 5; i ++)
		{
			pTask1.addTask(new LinkListTask(mono));
		}

		for(int i = 0; i < 7; i ++)
		{
			sTask2.addTask(new Task(mono));
		}

		sTask1.addTask(pTask1);
		sTask1.addTask(sTask2);

		(m_mainTask as ParallelTasks).addTask(sTask1);
	}
	#endregion

	#region GUI Functions
	void OnGUI()
	{
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Text Field", myString);

		GUIStyle horizontalStyle = new GUIStyle(GUI.skin.box);


		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup ();

		GUILayout.BeginScrollView(new Vector2(0, 0), true, true);
		m_mainTask.draw();

		GUILayout.EndScrollView();
	}
	#endregion

}
