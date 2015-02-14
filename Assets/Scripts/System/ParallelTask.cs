using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelTask : Task {

	#region Fields
	protected List<Task> m_taskList;
	#endregion
	
	#region Public API
	public int flowCnt
	{
		get { return m_taskList.Count;}
	}
	#endregion
	
	#region Constructor
	public ParallelTask() : base()
	{
		m_taskList = new List<Task>();
	}
	
	public ParallelTask(MonoBehaviour mono) : base(mono)
	{
		m_taskList = new List<Task>();
		m_mono = mono;
	}
	#endregion
	
	
	#region Implement Virtual Functions
	public override void start ()
	{
		foreach(Task task in m_taskList)
		{
			task.start();
		}

		base.start ();
	}

	protected override IEnumerator doTask ()
	{
		while(m_isRunning)
		{
			if(isPaused)
			{
				// pause all flows in the list
				foreach(Task task in m_taskList)
				{
					task.pause();
				}
				yield return null;
			}
			else
			{
				// not paused
				bool hasTaskRunning = false;
				foreach(Task task in m_taskList)
				{
					if(task.isRunning)
					{
						hasTaskRunning = true;
						break;
					}
				}
				if(hasTaskRunning)
				{
					yield return null;
				}
				else
				{
					// finished
					m_isRunning = false;
				}
			}
		}
	}
	#endregion
	
	#region Public API
	public void addTask(IEnumerator coroutine)
	{
		if(m_isRunning)
		{
			Debug.LogError("[ERROR]ParallelTask=>addTask: can not dynamic add task");
			return;
		}
		m_taskList.Add(new Task(m_mono, coroutine));
	}
	
	public void addTask(Task Task)
	{
		if(m_isRunning)
		{
			Debug.LogError("[ERROR]ParallelTask=>addTask: can not dynamic add task");
			return;
		}
		m_taskList.Add(Task);
	}
	#endregion

}
