using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerieTask : Task {


	#region Fields
	protected List<Task> m_taskList;
	#endregion

	#region Public API
	public int taskCnt
	{
		get { return m_taskList.Count;}
	}
	#endregion

	#region Constructor
	public SerieTask() : base()
	{
		m_taskList = new List<Task>();
	}

	public SerieTask(MonoBehaviour mono) : base (mono)
	{
		m_taskList = new List<Task>();
		m_mono = mono;
	}
	#endregion


	#region Implement Virtual Functions
	protected override IEnumerator doTask ()
	{
		// start from index 0
		int index = 0;

		while(m_isRunning)
		{
			if(isPaused)
			{
				// paused do nothing
				if(index < m_taskList.Count)
					m_taskList[index].pause();
				yield return null;
			}
			else
			{
				if(m_taskList.Count > 0)
				{
					// no paused
					m_taskList[index].start();

					yield return m_taskList[index].untilDone;

					index ++;

					if(index >= m_taskList.Count)
					{
						m_isRunning = false;
					}
				}
				else
				{
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
			Debug.LogError("[ERROR]SerieTask=>addTask: can not dynamic add task");
			return;
		}
		m_taskList.Add(new Task(m_mono, coroutine));
	}

	public void addTask(Task task)
	{
		if(m_isRunning)
		{
			Debug.LogError("[ERROR]SerieTask=>addTask: can not dynamic add task");
			return;
		}
		m_taskList.Add(task);
	}
	#endregion
	
}
