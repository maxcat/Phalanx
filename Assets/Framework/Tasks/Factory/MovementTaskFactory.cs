using UnityEngine;
using System.Collections;


public class MovementTaskFactory : TaskFactory {

#region Fields
	[SerializeField] protected float				gizmoTimeInterval = 0.05f;
	[SerializeField] protected bool 				relativeStartPos = false; 
	[SerializeField] protected bool 				isLocal = false;
	[SerializeField] protected GameObject				targetObj;
	[SerializeField] protected CurveData3D 				curveData3D;
#endregion

#region Editor Fields
	protected Vector3 						savedTargetObjPos = Vector3.zero;
#endregion

#region Getter and Setter
	public CurveData3D MovementData
	{
		get { return curveData3D; }
		set { curveData3D = value; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	}
	
	void OnDrawGizmosSelected () {
		drawMovemntPath(curveData3D);
	}
#endregion

#region Implement Virtual Functions
	public override Task CreateTask()
	{
		return new CurveMoveTask(this, targetObj, curveData3D, relativeStartPos, isLocal);
	}
#endregion

#region Protected Functions
	protected void init()
	{

	}
#endregion

#region Editor Help Functions
	protected void drawMovemntPath(CurveData3D  data)
	{
		if(gizmoTimeInterval <= 0f || data == null || targetObj == null)
			return;

		if(Application.isPlaying && relativeStartPos && !monitoringTask)
			return;

		if(monitoringTask && (createdTask == null || !createdTask.Running))
		{
			// save target position
			if(isLocal)
				savedTargetObjPos = targetObj.transform.localPosition;
			else
				savedTargetObjPos = targetObj.transform.position;

		}

		Transform parentTrans = targetObj.transform.parent;
		Gizmos.color = Color.red;
		float progress = 0f;

		float length = data.Length; 

		// init data
		data.Init();
		
		Vector3 drawPos = Vector3.zero;
		if(relativeStartPos)
		{
			if(monitoringTask)
				drawPos = savedTargetObjPos;
			else
				drawPos = isLocal ? targetObj.transform.localPosition : targetObj.transform.position;
		}

		drawPos += data.GetStartPosition(); 

		while(progress < length)
		{
			Vector3 nextPos = drawPos + data.GetDeltaValue(progress, gizmoTimeInterval); 
			
			if(isLocal && parentTrans != null)
				Gizmos.DrawLine(
						parentTrans.TransformPoint(drawPos), 
						parentTrans.TransformPoint(nextPos)
						);
			else
				Gizmos.DrawLine(drawPos, nextPos);

			drawPos = nextPos;
			progress += gizmoTimeInterval;
		}

		Vector3 stopPos = drawPos + data.GetDeltaValue(progress, length - progress); 

		if(isLocal && parentTrans != null)
			Gizmos.DrawLine(
					parentTrans.TransformPoint(drawPos),
					parentTrans.TransformPoint(stopPos)
					);
		else
			Gizmos.DrawLine(drawPos, stopPos);
	}

#endregion
}
