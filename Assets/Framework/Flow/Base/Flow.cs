using UnityEngine;
using System.Collections;

public class Flow : IEnumerator {

#region Fields
	protected float 		flowSpeed;
	protected IEnumerator 		source;
	protected Flow 			nextFlow;
	protected bool 			isFlowRunning = false;
	protected bool 			isPaused = false;
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

	public bool IsFlowRunning
	{
		get { return isFlowRunning; }
	}

	public bool IsPaused
	{
		get { return isPaused; }
	}
#endregion

#region Implement Inteface
	public virtual bool MoveNext()
	{
		isFlowRunning = true;

		if(source == null)
		{
			isFlowRunning = false;
			return false;
		}
		else
		{
			if(isPaused)
			{
				return true;
			}
			else
			{
				if(!source.MoveNext())
				{
					isFlowRunning = false;		
					return false;
				}
				return true;
			}
		}
	}

	public virtual object Current
	{
		get {
			if(isPaused)
				return null;
			return source != null ? source.Current : null;
		}
	}

	public void Reset()
	{
	}

#endregion

#region Virtual Functions
	public virtual void Kill()
	{
		source = null;
	}

	protected virtual IEnumerator main()
	{
		yield return null;
	}

	public virtual void SetFlowSpeed(float speed)
	{
		flowSpeed = speed;
	}

	public virtual void Pause()
	{
		if(!isPaused)
			isPaused = true;
	}

	public virtual void Resume()
	{
		if(isPaused)
			isPaused = false;
	}

	public virtual void Start(MonoBehaviour mono)
	{
		mono.StartCoroutine(this);	
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
