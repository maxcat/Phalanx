using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskCollectionData : MonoBehaviour {

	#region Fields
	protected Task							m_mainTask;
	#endregion

	#region Getter and Setter
	public Task mainTask
	{
		get { return m_mainTask;}
	}
	#endregion
}
