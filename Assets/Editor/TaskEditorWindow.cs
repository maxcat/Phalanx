//using UnityEngine;
//using UnityEditor;
//using UnityExtensions;
//using System.Collections;
//using System.Collections.Generic;
//using GumiBase;
//
//public enum TASK_TYPE
//{
//	TASK, 
//	DELAY,
//	LINKLISTTASK,
//	PARALLELTASK,
//	SERIETASK,
//}
//
//
//public class TaskEditorWindow : EditorWindow {
//
//	#region Fields
//	string myString = "Hello World";
//	bool groupEnabled;
//	bool myBool = true;
//	float myFloat = 1.23f;
//
//
//	protected Color							m_taskColor = Color.red;
//	protected Color							m_delayColor = Color.green;
//	protected Color							m_linklistColor = Color.blue;
//
//	protected Color							m_parallelColor = Color.yellow;
//	protected Color							m_serieColor = Color.magenta;
//
//	protected Task							m_mainTask;
//	#endregion
//
//	#region Static Functions
//	public static void showWindow()
//	{
//		EditorWindow.GetWindow(typeof(TaskEditorWindow));
//	}
//	#endregion
//
//	#region Public API
//	public void init(TaskCollectionData mono)
//	{
//		// test data
//		m_mainTask = new ParallelTasks(mono);
//
//		for(int i = 0; i < 10; i ++)
//		{
//			(m_mainTask as ParallelTasks).addTask(new Task(mono));
//		}
//
//		for(int i = 0; i < 3; i ++)
//		{
//			(m_mainTask as ParallelTasks).addTask(new LinkListTask(mono));
//		}
//
//		SeriesTasks sTask1 = new SeriesTasks(mono);
//		SeriesTasks sTask2 = new SeriesTasks(mono);
//
//		for(int i = 0; i < 2; i ++)
//		{
//			sTask1.addTask(new Task(mono));
//		}
//
//		ParallelTasks pTask1 = new ParallelTasks(mono);
//
//		for(int i = 0; i < 5; i ++)
//		{
//			pTask1.addTask(new LinkListTask(mono));
//		}
//
//		for(int i = 0; i < 7; i ++)
//		{
//			sTask2.addTask(new Task(mono));
//		}
//
//		sTask1.addTask(pTask1);
//		sTask1.addTask(sTask2);
//
//		(m_mainTask as ParallelTasks).addTask(sTask1);
//	}
//	#endregion
//
//	#region GUI Functions
//	void OnGUI()
//	{
//		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
//		myString = EditorGUILayout.TextField ("Text Field", myString);
//
//		GUIStyle horizontalStyle = new GUIStyle(GUI.skin.box);
//
//
//		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
//		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
//		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
//		EditorGUILayout.EndToggleGroup ();
//
//		GUILayout.BeginScrollView(new Vector2(0, 0), true, true);
//		distributeTaskDrawer(m_mainTask);
//
//		GUILayout.EndScrollView();
//	}
//	#endregion
//
//	#region Protected Functions
//	protected void drawSingleTask()
//	{
//		Color originColor = GUI.color;
//		GUI.color = m_taskColor;
//		GUILayout.Box("Task");
//		GUI.color = originColor;
//	}
//
//	protected void drawDelayTask()
//	{
//		Color originColor = GUI.color;
//		GUI.color = m_delayColor;
//		GUILayout.Box("Delay Task");
//		GUI.color = originColor;
//	}
//
//	protected void drawLinkListTask()
//	{
//		Color originColor = GUI.color;
//		GUI.color = m_linklistColor;
//		GUILayout.Box("LinkListTask");
//		GUI.color = originColor;
//	}
//
//	protected void drawParallelTask(ParallelTasks mainTask)
//	{
//		if(mainTask.taskCnt <= 0)
//			return;
//		
//		Color originColor = GUI.color;
//		GUI.color = m_parallelColor;
//
//		EditorGUILayout.BeginVertical("box");
//		{
//			EditorGUILayout.LabelField("Parallel Task");
//
//			EditorGUILayout.BeginHorizontal("box");
//
//			foreach(Task task in mainTask.taskList)
//			{
//				distributeTaskDrawer(task);
//			}
//
//			EditorGUILayout.EndHorizontal();
//		}
//		EditorGUILayout.EndVertical();
//
//		GUI.color = originColor;
//	}
//
//	protected void drawSerieTask(SeriesTasks mainTask)
//	{
//		if(mainTask.taskCnt <= 0)
//			return;
//
//		Color originColor = GUI.color;
//		GUI.color = m_serieColor;
//
//		EditorGUILayout.BeginVertical("box");
//
//		EditorGUILayout.LabelField("Serie Task");
//
//		foreach(Task task in mainTask.taskList)
//		{
//			distributeTaskDrawer(task);
//		}
//
//		EditorGUILayout.EndVertical();
//
//		GUI.color = originColor;
//	}
//
//	protected void distributeTaskDrawer(Task task)
//	{
//		if(typeof(LinkListTask).IsInstanceOfType(task))
//		{
//			drawLinkListTask();
//		}
//		else if(typeof(DelayTask).IsInstanceOfType(task))
//		{
//			drawDelayTask();
//		}
//		else if(typeof(ParallelTasks).IsInstanceOfType(task))
//		{
//			drawParallelTask(task as ParallelTasks);
//		}
//		else if(typeof(SeriesTasks).IsInstanceOfType(task))
//		{
//			drawSerieTask(task as SeriesTasks);
//		}
//		else
//		{
//			drawSingleTask();
//		}
//	}
//	#endregion
//}
