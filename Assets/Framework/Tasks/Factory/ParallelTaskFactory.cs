using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelTaskFactory : TaskCollectionFactory {

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
#endregion

#region Implement Virtual Functions
	public override Task CreateTask()
	{
		if(factoryList == null || factoryList.Count <= 0)
			return null;

		ParallelTasks mainTask = new ParallelTasks(this);
		for(int i = 0; i <factoryList.Count; i ++)
		{
			if(factoryList[i] != null)
				mainTask.AddTask(factoryList[i].CreateTask());	
		}
		
		return mainTask;
	}
#endregion
}
