using UnityEngine;
using System.Collections;

public class CurveMoveTask : Task {

#region Constructor
	public CurveMoveTask(MonoBehaviour mono, GameObject owner, CurveData3D data, bool relativeStartPos, bool isLocal)
	       	: base(mono)
	{
		coroutine = moveFlow(owner, data, relativeStartPos, isLocal);
	}
#endregion

#region Implement Virtual Functions
	public override void Draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.red;
		GUILayout.Box("Curve Movement Task");
		GUI.color = originColor;
	}
#endregion

#region Protected Functions
	protected IEnumerator moveFlow(GameObject owner, CurveData3D data, bool relativeStartPos, bool isLocal)
	{
		if(data == null)
		{
			Debug.LogWarning("[Warning]CurveMoveTask->moveFlow: curve data is null!");
			yield break;
		}

		if(owner == null)
		{
			Debug.LogWarning("[Warning]CurveMoveTask->moveFlow: owner is null!");
			yield break;
		}

		data.Init();

		float length = data.Length;

		Vector3 startPos = Vector3.zero;
		if(relativeStartPos)
			startPos = isLocal ? owner.transform.localPosition : owner.transform.position;

		startPos += data.GetStartPosition();

		if(!relativeStartPos)
		{
			if(isLocal)
				owner.transform.localPosition = startPos; 
			else
				owner.transform.position = startPos;
		}

		float timeElapse = 0f;

		while (timeElapse <= length)
		{
			float deltaTime = Time.fixedDeltaTime * taskSpeed;
			Vector3 nextPos = startPos + data.GetDeltaValue(timeElapse, deltaTime);

			if(isLocal)
				owner.transform.localPosition = nextPos;
			else
				owner.transform.position = nextPos;

			startPos = nextPos;
			timeElapse += deltaTime;

			yield return new WaitForFixedUpdate();
		}
		
		Vector3 stopPos = startPos + data.GetDeltaValue(timeElapse, length - timeElapse);

		if(isLocal)
			owner.transform.localPosition = stopPos;
		else
			owner.transform.position = stopPos;
	}
#endregion

}
