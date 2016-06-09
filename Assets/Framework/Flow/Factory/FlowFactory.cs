using UnityEngine;
using System.Collections;

public class FlowFactory : MonoBehaviour {

#region Editor Fields
	[SerializeField] protected bool 		monitoringFlow = false;
	[SerializeField] protected float 		flowSpeed = 1f;
	protected Flow 					createdFlow;
#endregion


#region Getter and Setter
	public bool MonitoringFlow
	{
		get { return monitoringFlow; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		UpdateMonitoringFlow();
	}
#endregion

#region Protected Functions
	protected void updateFlowSpeed()
	{
		if(createdFlow != null)
		{
			if(flowSpeed < 0f)
				flowSpeed = 0f;

			createdFlow.SetFlowSpeed(flowSpeed);	
		}
	}
#endregion

#region Vritual Functions
	public virtual Flow CreateFlow()
	{
		return null;
	}

	public virtual void OnRunFlowButtonClicked()
	{
		createdFlow = CreateFlow();
		Task mainTask = new Task(this, createdFlow, true);
	}

	public virtual void UpdateMonitoringFlow()
	{
		if(monitoringFlow)
		{
			updateFlowSpeed();
		}
	}
#endregion
}
