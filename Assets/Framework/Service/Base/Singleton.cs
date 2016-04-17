using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {
	
#region Fields
	[SerializeField] protected bool isPersist;
	protected bool			isInited = false;
#endregion

#region Getter and Setter
	public virtual bool IsPersist
	{
		get { return isPersist; }
	}
#endregion

#region Virtual Functions
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
}
