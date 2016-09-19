using UnityEngine;
using System.Collections;

public class ScaleFlowFactory : FlowFactory {

#region Fields
	[SerializeField] protected CurveData3D 				curveData3D;
	[SerializeField] protected bool 				relativeScaling;
	[SerializeField] protected GameObject 				targetObj;
#endregion

#region Implement Virtual Functions
	public override Flow CreateFlow()
	{
		return new CurveScaleFlow(targetObj, curveData3D, relativeScaling);
	}
#endregion
}
