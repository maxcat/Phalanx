using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelTasks : Task {

	#region Internal Fields
	List<Task> m_tasks;
	#endregion


	#region Properties
	public override Coroutine UntilDone {
		get {
			return base.UntilDone;
		}
	}

	#endregion


	#region Constructor
	public ParallelTasks(MonoBehaviour monoClass) 
		: base(monoClass)
	{
		m_tasks = new List<Task>();
	}
	
	#endregion

	#region Setter and Getter
	public int taskCnt
	{
		get 
		{
			if (m_tasks == null)
				return 0;
			else
				return m_tasks.Count;
		}
	}

	public List<Task> taskList
	{
		get { return m_tasks; }
	}
	#endregion


	#region Implement Interface
	public override void start ()
	{
		m_running = true;
		
		m_monoClass.StartCoroutine( doTask() );

		
	}

	public override void draw ()
	{
		Color originColor = GUI.color;
		GUI.color = Color.yellow;

		GUILayout.BeginVertical("box");
		{
			GUILayout.Label("Parallel Task");
			GUILayout.BeginHorizontal("box");

			foreach(Task task in m_tasks)
			{
				task.draw();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();

		GUI.color = originColor;
	}
	
	public override void pause ()
	{
		base.pause ();
	}

	public override void resume ()
	{
		base.resume ();
	}

	public override void kill ()
	{
		base.kill ();
	}

	public override void kill (float delayInSeconds)
	{
		base.kill (delayInSeconds);
	}



	public override IEnumerator stillRunning ()
	{
		while(m_running)
		{
			yield return null;	
		}
	}

	protected override IEnumerator doTask ()
	{
		// start all the task inside 
		if(m_tasks.Count <= 0)
		{
			m_running = false;
			yield break;
		}

		foreach(Task task in m_tasks)
		{
			task.setSpeed(m_taskSpeed);
			task.start();
		}

		while(m_running)
		{
			// check whether got task running
			bool hasTaskRunning = false;
			foreach(Task task in m_tasks)
			{
				if(task != null && task.Running)
				{
					if(m_paused)
					{
						// pause all task
						task.pause();
					}
					else
					{
						// resume paused task
						if(task.Paused)
							task.resume();
					}

					// update speed
					task.setSpeed(m_taskSpeed);

					hasTaskRunning = true;
				}
			}
			yield return null;
			m_running = hasTaskRunning;
		}

		// incase killed by use
		foreach(Task task in m_tasks)
		{
			if(task != null && task.Running)
			{
				// kill task
				task.kill();
			}
		}
		// wait for one frame
		yield return null;

		// all task stopped
		// clear the list
		m_tasks.Clear();
	}
	#endregion

	#region Internal Functions

	#endregion


	#region Public API
	public void addTask(IEnumerator coroutine)
	{
		if(m_running)
		{
			Debug.LogError("[Error]ParallelTasks->addTask: can't add task into running ParallelTask");
			return;
		}
		Task task = new Task(m_monoClass, coroutine, false);
		m_tasks.Add(task);
	}

	public void dynamicAddTask(IEnumerator coroutine)
	{
		Task task = new Task(m_monoClass, coroutine, false);

		m_tasks.Add(task);

		// start the task
		task.start();
	}

	public void addTask(Task task)
	{
		if(m_running)
		{
			Debug.LogError("[Error]ParallelTasks->addTask: can't add task into running ParallelTask");
			return;
		}
		if(task == null)
			return;

		m_tasks.Add(task);
	}

	public void dynamicAddTask(Task task)
	{
		if(task == null)
			return;

		m_tasks.Add (task);

		// start the tast 
		task.start();
	}
	
	#endregion
	


}
