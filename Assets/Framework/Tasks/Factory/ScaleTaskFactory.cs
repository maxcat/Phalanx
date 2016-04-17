using UnityEngine;
using System.Collections;

public class ScaleTaskFactory : TaskFactory {

#region Fields
	[SerializeField] protected CurveData3D 				curveData3D;
	[SerializeField] protected bool 				relativeScaling;
	[SerializeField] protected GameObject 				targetObj;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Implement Virtual Functions
	public override Task CreateTask()
	{
		return new CurveScaleTask(this, targetObj, curveData3D, relativeScaling);
	}
#endregion
}
