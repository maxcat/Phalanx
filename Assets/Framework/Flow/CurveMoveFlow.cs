using UnityEngine;
using System.Collections;

public class CurveMoveFlow : Flow {

#region Fields
	 protected CurveData3D			data;
	 protected bool				isLocal;
	 protected bool 			relativeStartPos;
	 protected GameObject			owner;
#endregion

#region Constructor
	public CurveMoveFlow(GameObject targetObj, CurveData3D curveData, bool relativeStartPos, bool isLocal) : base ()
	{
		this.data = curveData;
		this.owner = targetObj;
		this.relativeStartPos = relativeStartPos;
		this.isLocal = isLocal;
	}

#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		if(data == null)
		{
			Debug.LogWarning("[Warning]Curvemain->main: curve data is null!");
			yield break;
		}

		if(owner == null)
		{
			Debug.LogWarning("[Warning]Curvemain->main: owner is null!");
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
			float deltaTime = Time.fixedDeltaTime * flowSpeed;
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
