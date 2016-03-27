using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {
	
#region Fields
	[SerializeField] protected bool m_isPersist;
	protected bool			m_isInited = false;
#endregion

#region Getter and Setter
	public virtual bool isPersist
	{
		get { return m_isPersist; }
	}
#endregion

#region Virtual Functions
	public virtual void init()
	{
		if(!m_isInited)
		{
			// only init the singleton once.
		}

	}

	public virtual void onSceneChanged(int level)
	{
		
	}

	public virtual void preRemoved()
	{
		
	}

	public virtual void onGamePaused()
	{
		
	}
#endregion
}
