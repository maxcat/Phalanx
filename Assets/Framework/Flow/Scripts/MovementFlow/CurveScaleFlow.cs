using UnityEngine;
using System.Collections;

public class CurveScaleFlow : Flow {

#region Fields
	protected GameObject 			owner;
	protected CurveData3D 			data;
	protected bool 				relativeScaling;
#endregion

#region Constructor
	public CurveScaleFlow(GameObject targetObj, CurveData3D curveData, bool relativeScaling) : base ()
	{
		this.owner = targetObj;
		this.data = curveData;
		this.relativeScaling = relativeScaling;
	}
#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		if(data == null)
		{
			Debug.LogWarning("[Warning]CurveScaleFlow->main: curve data is null!");
			yield break;
		}

		if(owner == null)
		{
			Debug.LogWarning("[Warning]CurveScaleFlow->main: owner is null!");
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
			float deltaTime = Time.fixedDeltaTime * flowSpeed;
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
