using UnityEngine;
using System.Collections;

public partial class Flow : IEnumerator {

#region Fields
	protected float 		flowSpeed;
	protected IEnumerator 		source;
	protected Flow 			nextFlow;
	protected bool 			isFlowRunning = false;
	protected bool 			isPaused = false;

	protected IEnumerator 		sourcePerIternation;
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

#region Protected Functions 
	protected IEnumerator moveNext(IEnumerator parentSource)
	{
		if(typeof(IEnumerator).IsInstanceOfType(parentSource.Current))
		{
			IEnumerator currentObject = parentSource.Current as IEnumerator;
			IEnumerator child = moveNext(currentObject);
			if(child != null)
			{
				return child; 
			}
			else 
			{
				if(parentSource.MoveNext())
				{
					return parentSource;
				}
				else
				{
					return null;
				}
			}
		}
		else
		{
			if(parentSource.MoveNext())
			{
				return parentSource;
			}
			else
			{
				return null;
			}
		}
	}
#endregion

#region Implement Inteface
	public virtual bool MoveNext()
	{
		isFlowRunning = true;

		sourcePerIternation = source;
		if(sourcePerIternation == null)
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
				sourcePerIternation = moveNext(source);
				if(sourcePerIternation == null)
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
			return sourcePerIternation != null ? sourcePerIternation.Current : null;
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

#if UNITY_EDITOR
public partial class Flow : IEnumerator
{
#region Virtual Functions
	public virtual void Draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.red;
		GUILayout.Box("Flow");
		GUI.color = originColor;
	}
#endregion
}
#endif

