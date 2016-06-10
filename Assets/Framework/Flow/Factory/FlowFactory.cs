using UnityEngine;
using System.Collections;

public partial class FlowFactory : MonoBehaviour {

#region Vritual Functions
	public virtual Flow CreateFlow()
	{
		return null;
	}
#endregion
}

#if UNITY_EDITOR
public partial class FlowFactory : MonoBehaviour {

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

	public bool IsMonitoringFlowStarted
	{
		get 
		{
			if(createdFlow == null)
				return false;
			return createdFlow.IsFlowRunning;
		}
	}

	public bool IsMonitoringFlowPaused
	{
		get
		{
			if(createdFlow == null)
				return false;
			return createdFlow.IsPaused;
		}
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
	public virtual void OnRunFlowButtonClicked()
	{
		createdFlow = CreateFlow();
		createdFlow.Start(this);
	}

	public virtual void OnPauseButtonClicked()
	{
		if(createdFlow != null && createdFlow.IsFlowRunning)
			createdFlow.Pause();	
	}

	public virtual void OnResumeButtonClicked()
	{
		if(createdFlow != null && createdFlow.IsFlowRunning)
			createdFlow.Resume();	
	}

	public virtual void UpdateMonitoringFlow()
	{
		if(monitoringFlow)
		{
			updateFlowSpeed();
		}
	}
#endregion

#endif
}

