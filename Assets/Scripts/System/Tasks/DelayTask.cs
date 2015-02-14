using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayTask : Task {

	public static Coroutine delayForSecond(float delay)
	{
		DelayTask delayTask = new DelayTask(Service.get<TaskService>(), delay);
		delayTask.start();

		return delayTask.UntilDone;
	}


	#region Constructor
	public DelayTask(MonoBehaviour mono, float delayInSecond)
		:base(mono)
	{
		m_coroutine = delayFlow(delayInSecond);
	}

	public DelayTask(float delayInSecond) : base ()
	{
		m_coroutine = delayFlow(delayInSecond);
	}

	public DelayTask(int frameCount) : base ()
	{
	}
	#endregion


	#region Protected Functions
	protected IEnumerator delayFlow(float delayInSecond)
	{
		float time = 0f;

		while(time < delayInSecond)
		{
			time += Time.deltaTime;
			yield return null;
		}
	}

	protected IEnumerator delayFlow(int frameCount)
	{
		int i = 0;
		while(i < frameCount)
		{
			i ++;
			yield return null;
		}
	}
	#endregion
}
