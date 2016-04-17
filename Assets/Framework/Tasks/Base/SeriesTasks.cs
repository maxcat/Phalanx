using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeriesTasks : CollectionTasks {

	#region Internal Fields
	protected int 					currentTaskIndex;
	#endregion
	
	
#region Getter and Setter
	public int CurrentIndex
	{
		get {return currentTaskIndex;}
	}
#endregion
	
#region Constructor
	public SeriesTasks(MonoBehaviour monoClass)
		: base(monoClass)
	{
	}
#endregion
	
	#region Implement Interface
	public override void Start ()
	{
		running = true;
		if (monoClass == null)
		{
			Kill ();
		}
		else
		{
			monoClass.StartCoroutine( doTask() );
		}
	}

	public override void Draw ()
	{
		Color originColor = GUI.color;
		GUI.color = Color.magenta;

		GUILayout.BeginVertical("box");
		GUILayout.Label("Serie Task");

		foreach(Task task in taskList)
		{
			task.Draw();
		}

		GUILayout.EndVertical();

		GUI.color = originColor;
	}
	
	protected override IEnumerator doTask ()
	{
		if(taskList.Count <= 0)
		{
			running = false;
			yield break;
		}

		// start the first task
		currentTaskIndex = 0;
		Task currentTask = taskList[currentTaskIndex];
		currentTask.SetSpeed(taskSpeed);
		currentTask.Start();

		while(running)
		{
			if(currentTask.Running)
			{
				// current task is running
				if(paused)
				{
					// pause 
					currentTask.Pause();
				}
				else
				{
					currentTask.Resume();
				}

				// update speed
				currentTask.SetSpeed(taskSpeed);
			}
			else
			{
				// current task finished
				if(paused)
				{
					// don't do anything
				}
				else
				{
					// start the next task
					currentTaskIndex ++;
					if(currentTaskIndex < taskList.Count)
					{
						// next task available
						currentTask = taskList[currentTaskIndex];
						currentTask.SetSpeed(taskSpeed);
						currentTask.Start();
					}
					else
					{
						// all task finished
						running = false;
					}
				}
			}
			yield return null;
		}

		// incase killed by the user
		if(currentTask.Running)
		{
			currentTask.Kill();
			// wait for one frame
			yield return null;
		}


		// wait for one frame
		// all task finished, remove list
		taskList.Clear();


	}
	#endregion

	#region Public API
	public override void AddTask(IEnumerator coroutine)
	{
		if(running)
		{
			Debug.LogError("[Error]SeriesTasks->AddTask: can't add task into running SerieTasks"); 
			return;
		}
		base.AddTask(coroutine);
	}
	
	public override void AddTask(Task task)
	{
		if(running)
		{
			Debug.LogError("[Error]SeriesTasks->AddTask: can't add task into running SerieTasks");
			return;
		}
		base.AddTask(task);
	}
	
	#endregion
	



	
}
