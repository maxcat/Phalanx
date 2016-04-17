using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectionTasks : Task {

#region Fields
	protected List<Task> 					taskList;
#endregion

#region Getter and Setter
	public List<Task> TaskList
	{
		get { return taskList; }
	}

	public int TaskCnt
	{
		get 
		{
			if(taskList != null)
				return taskList.Count;

			return 0;
		}
	}
#endregion

#region Constructor
	public CollectionTasks(MonoBehaviour monoClass)
		: base (monoClass)
	{
		taskList = new List<Task>();
	}
#endregion

#region Implement Virtual Functions
	public override bool HasRecursiveStruct(Task parent = null)
	{
		if(taskList == null || taskList.Count <= 0)
			return false;

		for(int i = 0; i < taskList.Count; i ++)
		{
			Task task = taskList[i];

			if(task != null)
			{
				if(task == this)
					return true;

				if(parent != null && task == parent)
					return true;

				if(parent != null && task.HasRecursiveStruct(parent))
					return true;

				if(task.HasRecursiveStruct(this))
					return true;
			}
		}
		return false;
	}
#endregion

#region Virtual Functions
	public virtual void AddTask(IEnumerator coroutine)
	{
		if(coroutine == null)
			return;

		Task task = new Task(monoClass, coroutine, false);
		taskList.Add(task);
	}

	public virtual void AddTask(Task task)
	{
		if(task == null)
			return;

		taskList.Add(task);
	}
#endregion

}
