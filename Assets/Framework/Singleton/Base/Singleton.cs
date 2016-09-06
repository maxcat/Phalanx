using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

#region Remove 
	[SerializeField] protected bool isPersist;
	protected bool			isInited = false;

	public virtual bool IsPersist
	{
		get { return isPersist; }
	}

	public virtual void Init()
	{
		if(!isInited)
		{
			// only init the singleton once.
		}

	}

	public virtual void OnSceneChanged(int level)
	{

	}

	public virtual void PreRemoved()
	{

	}

	public virtual void OnGamePaused()
	{

	}
#endregion

#region Virtual Functions
	protected virtual void init()
	{
		
	}

	protected virtual void clear()
	{

	}
#endregion
}
