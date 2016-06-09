using System.Collections;

public class Flow : IEnumerator {

#region Fields
	protected float 		flowSpeed;
	protected IEnumerator 		source;
	protected Flow 			nextFlow;
	protected bool 			isFlowStarted = false;
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

	public bool IsFlowStarted
	{
		get { return isFlowStarted; }
	}

	public bool IsPaused
	{
		get { return isPaused; }
	}
#endregion

#region Implement Inteface
	public virtual bool MoveNext()
	{
		isFlowStarted = true;

		if(source == null)
		{
			isFlowStarted = false;
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
					isFlowStarted = false;		
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
