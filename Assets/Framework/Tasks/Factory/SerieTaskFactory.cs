using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerieTaskFactory : TaskCollectionFactory {

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

		SeriesTasks mainTask = new SeriesTasks(this);
		for(int i = 0; i <factoryList.Count; i ++)
		{
			if(factoryList[i] != null)
			{
				mainTask.AddTask(factoryList[i].CreateTask());	
			}
		}
		
		return mainTask;
	}
#endregion
}
