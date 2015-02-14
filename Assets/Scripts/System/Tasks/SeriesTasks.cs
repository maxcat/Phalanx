using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeriesTasks : Task {

	#region Internal Fields
	List<Task> m_tasks;
	int m_currentTaskIndex;
	#endregion
	
	
	#region Properties
	public override Coroutine UntilDone {
		get {
			return base.UntilDone;
		}
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

	public int currentIndex
	{
		get {return m_currentTaskIndex;}
	}
	#endregion
	
	
	#region Constructor
	public SeriesTasks(MonoBehaviour monoClass) 
		: base(monoClass)
	{
		m_tasks = new List<Task>();
	}
	
	#endregion
	
	
	#region Implement Interface
	public override void start ()
	{
		m_running = true;
		if (m_monoClass == null)
		{
			kill ();
		}
		else
		{
			m_monoClass.StartCoroutine( doTask() );
		}
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
		if(m_tasks.Count <= 0)
		{
			m_running = false;
			yield break;
		}

		// start the first task
		int currentTaskIndex = 0;
		m_currentTaskIndex = 0;
		Task currentTask = m_tasks[currentTaskIndex];
		currentTask.setSpeed(m_taskSpeed);
		currentTask.start();

		while(m_running)
		{
			if(currentTask.Running)
			{
				// current task is running
				if(m_paused)
				{
					// pause 
					currentTask.pause();
				}
				else
				{
					currentTask.resume();
				}

				// update speed
				currentTask.setSpeed(m_taskSpeed);
			}
			else
			{
				// current task finished
				if(m_paused)
				{
					// don't do anything
				}
				else
				{
					// start the next task
					currentTaskIndex ++;
					m_currentTaskIndex ++;
					if(currentTaskIndex < m_tasks.Count)
					{
						// next task available
						currentTask = m_tasks[currentTaskIndex];
						currentTask.setSpeed(m_taskSpeed);
						currentTask.start();
					}
					else
					{
						// all task finished
						m_running = false;
					}
				}
			}
			yield return null;
		}

		// incase killed by the user
		if(currentTask.Running)
		{
			currentTask.kill();
			// wait for one frame
			yield return null;
		}


		// wait for one frame
		// all task finished, remove list
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
			Debug.LogError("[Error]SeriesTasks->addTask: can't add task into running SerieTasks"); 
			return;
		}
		Task task = new Task(m_monoClass, coroutine, false);
		m_tasks.Add(task);
	}
	
	public void addTask(Task task)
	{
		if(m_running)
		{
			Debug.LogError("[Error]SeriesTasks->addTask: can't add task into running SerieTasks");
			return;
		}

		if(task == null)
			return;

		m_tasks.Add(task);
	}
	
	#endregion
	



	
}
