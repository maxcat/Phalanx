using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskPool : MonoBehaviour {

	#region Fields
	protected List<Task> 								m_taskList;
	[SerializeField] int 								m_taskCount;
	#endregion


	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if(m_taskList != null)
			m_taskCount = m_taskList.Count;
		#endif
	}
	#endregion


	#region Getter
	public Coroutine untilDone
	{
		get
		{
			return this.StartCoroutine(checkFlow());
		}
	}

	public bool isEmpty
	{
		get 
		{
			return m_taskList == null || m_taskList.Count == 0;
		}
	}
	#endregion



	#region Public API
	public IEnumerator isRunningFlow()
	{
		while(m_taskList != null && m_taskList.Count > 0)
		{
			yield return null;
		}
	}


	public void addTask(Task task)
	{
		if(m_taskList == null)
			m_taskList = new List<Task>();

		m_taskList.Add(task);

		if(m_taskList.Count <= 1)
		{
			new Task(this, checkFlow(), true);
		}
	}

	public void pause()
	{
		if(m_taskList == null)
			return;

		foreach(Task task in m_taskList)
		{
			task.pause();
		}
	}

	public void resume()
	{
		if(m_taskList == null)
			return;

		foreach(Task task in m_taskList)
		{
			task.resume();
		}
	}

	public void setSpeed(float speed)
	{
		if(m_taskList == null)
			return;

		foreach(Task task in m_taskList)
		{
			task.setSpeed(speed);
		}
	}


	#endregion


	#region Protected Functions
	IEnumerator checkFlow()
	{
		while(m_taskList != null && m_taskList.Count > 0)
		{
			List<Task> removeTaskList = new List<Task>();
			foreach(Task task in m_taskList)
			{
				if(!task.Running)
				{
					removeTaskList.Add(task);
				}
			}

			foreach(Task task in removeTaskList)
			{
				m_taskList.Remove(task);
			}

			removeTaskList.Clear();

			yield return null;
		}
	}
	#endregion
}
