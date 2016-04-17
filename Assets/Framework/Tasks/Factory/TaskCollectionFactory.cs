using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskCollectionFactory : TaskFactory {

#region Fields
	[SerializeField] protected List<TaskFactory>		factoryList;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
#endregion

#region Implement Virtual Functions
	public override bool HasRecursiveStruct(TaskFactory parent = null)
	{
		if(factoryList == null || factoryList.Count <= 0)
			return false;

		for(int i = 0; i < factoryList.Count; i ++)
		{
			TaskFactory factory = factoryList[i];

			if(factory != null)
			{
				if(factory == this)
					return true;

				if(parent != null && factory == parent)
					return true;

				if(parent != null && factory.HasRecursiveStruct(parent))
					return true;

				if(factory.HasRecursiveStruct(this))
					return true;
			}
		}
		return false;
	}
#endregion
	
}
