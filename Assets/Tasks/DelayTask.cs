using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayTask : Task {

	#region Constructor
	public DelayTask(MonoBehaviour mono, float delayInSecond)
		:base(mono)
	{
		m_coroutine = delayFlow(delayInSecond);
	}

	#endregion

#region Implement Virtual Functions
	public override void draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.green;
		GUILayout.Box("DelayTask");
		GUI.color = originColor;
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
