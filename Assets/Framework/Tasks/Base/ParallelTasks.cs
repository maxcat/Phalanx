using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelTasks : CollectionTasks {

#region Constructor 
	public ParallelTasks(MonoBehaviour monoClass)
		: base (monoClass)
	{
	}
#endregion

	#region Implement Virtual Functions 
	public override void Start ()
	{
		running = true;
		monoClass.StartCoroutine( doTask() );
	}

	public override void Draw ()
	{
		Color originColor = GUI.color;
		GUI.color = Color.yellow;

		GUILayout.BeginVertical("box");
		{
			GUILayout.Label("Parallel Task");
			GUILayout.BeginHorizontal("box");

			foreach(Task task in taskList)
			{
				task.Draw();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();

		GUI.color = originColor;
	}
	
	protected override IEnumerator doTask ()
	{
		// start all the task inside 
		if(taskList.Count <= 0)
		{
			running = false;
			yield break;
		}

		foreach(Task task in taskList)
		{
			task.SetSpeed(taskSpeed);
			task.Start();
		}

		while(running)
		{
			// check whether got task running
			bool hasTaskRunning = false;
			foreach(Task task in taskList)
			{
				if(task != null && task.Running)
				{
					if(paused)
					{
						// pause all task
						task.Pause();
					}
					else
					{
						// resume paused task
						if(task.Paused)
							task.Resume();
					}

					// update speed
					task.SetSpeed(taskSpeed);

					hasTaskRunning = true;
				}
			}
			yield return null;
			running = hasTaskRunning;
		}

		// incase killed by use
		foreach(Task task in taskList)
		{
			if(task != null && task.Running)
			{
				// kill task
				task.Kill();
			}
		}
		// wait for one frame
		yield return null;

		// all task stopped
		// clear the list
		taskList.Clear();
	}
	#endregion


	#region Public API
	public override void AddTask(IEnumerator coroutine)
	{
		if(running)
		{
			Debug.LogError("[Error]ParallelTasks->AddTask: can't add task into running ParallelTask");
			return;
		}
		base.AddTask(coroutine);
	}

	public override void AddTask(Task task)
	{
		if(running)
		{
			Debug.LogError("[Error]ParallelTasks->AddTask: can't add task into running ParallelTask");
			return;
		}
		base.AddTask(task);
	}
	
	#endregion
	


}
