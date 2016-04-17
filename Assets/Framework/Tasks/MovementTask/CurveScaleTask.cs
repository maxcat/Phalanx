using UnityEngine;
using System.Collections;

public class CurveScaleTask : Task {

#region Constructor
	public CurveScaleTask(MonoBehaviour mono, GameObject owner, CurveData3D data, bool relativeScaling)
		:base (mono)
	{
		coroutine = scaleFlow(owner, data, relativeScaling);
	}
#endregion

#region Implement Virtual Functions
	public override void Draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.red;
		GUILayout.Box("Curve Scale Task");
		GUI.color = originColor;
	}
#endregion

#region Protected Functions
	protected IEnumerator scaleFlow(GameObject owner, CurveData3D data, bool relativeScaling)
	{
		if(data == null)
		{
			Debug.LogWarning("[Warning]CurveScaleTask->scaleFlow: curve data is null!");
			yield break;
		}

		if(owner == null)
		{
			Debug.LogWarning("[Warning]CurveScaleTask->scaleFlow: owner is null!");
			yield break;
		}

		data.Init();

		float length = data.Length;

		Vector3 startScale = Vector3.zero;
		if(relativeScaling)
			startScale = owner.transform.localScale;

		startScale += data.GetStartPosition();

		if(!relativeScaling)
			owner.transform.localScale = startScale;

		float timeElapse = 0f;

		while(timeElapse <= length)
		{
			float deltaTime = Time.fixedDeltaTime * taskSpeed;
			Vector3 nextScale = startScale + data.GetDeltaValue(timeElapse, deltaTime);

			owner.transform.localScale = nextScale;

			startScale = nextScale;
			timeElapse += deltaTime;
			yield return new WaitForFixedUpdate();
		}

		Vector3 stopScale = startScale + data.GetDeltaValue(timeElapse, length - timeElapse);
		owner.transform.localScale = stopScale;
	}
#endregion
}
