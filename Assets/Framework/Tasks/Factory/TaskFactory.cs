using UnityEngine;
using System.Collections;

public class TaskFactory : MonoBehaviour {

#region Editor Fields
	[SerializeField] protected bool 			monitoringTask = false;
	[SerializeField] protected float 			taskSpeed = 1f;
	protected Task 						createdTask;
#endregion

#region Getter and Setter
	public bool MonitoringTask
	{
		get { return monitoringTask; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMonitoringTask();	
	}
#endregion

#region Protected Functions
	protected void updateTaskSpeed()
	{
		if(createdTask != null)
		{
			if(taskSpeed < 0f)
				taskSpeed = 0f;

			createdTask.SetSpeed(taskSpeed);
		}
	}
#endregion

#region Virtual Functions
	public virtual Task CreateTask()
	{
		return null;
	}

	public virtual void OnRunTaskButtonClicked()
	{
		createdTask = CreateTask();
		createdTask.Start();
	}

	public virtual void UpdateMonitoringTask()
	{
		if(monitoringTask)
		{
			updateTaskSpeed();
		}
	}

	public virtual bool HasRecursiveStruct(TaskFactory parent = null)
	{
		return false;
	}
#endregion
}
