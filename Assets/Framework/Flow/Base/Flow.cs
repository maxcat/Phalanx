using UnityEngine;
using System.Collections;

public class Flow : IEnumerator {

#region Fields
	protected float 		flowSpeed;
	protected IEnumerator 		source;
	protected Flow 			nextFlow;
	protected bool 			isFlowStarted = false;
#endregion

#region Constructor
	public Flow(IEnumerator flow)
	{
		source = flow;
		nextFlow = null;
		flowSpeed = 1f;
	}

	public Flow()
	{
		source = main();
		nextFlow = null;
		flowSpeed = 1f;
	}
#endregion

#region Getter and Setter
	public Flow NextFlow
	{
		get { return nextFlow; }
	}

	public bool IsFlowStarted
	{
		get { return isFlowStarted; }
	}
#endregion

#region Implement Inteface
	public virtual bool MoveNext()
	{
		isFlowStarted = true;
		return source != null ? source.MoveNext() : false;
	}

	public virtual object Current
	{
		get {
			return source != null ? source.Current : null;
		}
	}

	public void Reset()
	{
	}

#endregion

#region Virtual Functions
	protected virtual IEnumerator main()
	{
		yield return null;
	}

	public virtual void SetFlowSpeed(float speed)
	{
		flowSpeed = speed;
	}
#endregion

#region Public API
	public void SetNext(Flow next)
	{
		nextFlow = next;	
	}

	public void SetNext(IEnumerator next)
	{
		if(next != null)
			nextFlow = new Flow(next);
	}
#endregion
}
