using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task {

	#region Fields
	protected IEnumerator m_coroutine;
	protected MonoBehaviour m_mono;


	protected bool m_isPaused;
	protected bool m_isRunning;
	protected float m_taskSpeed;
	#endregion


	#region Getter and Setter
	public IEnumerator coroutine{
		get { return m_coroutine;}
		set { m_coroutine = value;}
	}

	public MonoBehaviour mono 
	{
		get { return m_mono;}
		set { m_mono = value;}
	}

	public virtual bool isRunning
	{
		get { return m_isRunning;}
	}

	public virtual bool isPaused
	{
		get { return m_isPaused;}
	}

	public virtual Coroutine untilDone
	{
		get
		{
			return m_mono.StartCoroutine(untilDoneFlow());
		}
	}

	public float flowSpeed
	{
		get { return m_taskSpeed;}
	}

	#endregion

	#region Constructor
	public Task()
	{
		m_coroutine = null;
		m_mono = null;

		m_isRunning = false;
		m_isPaused = false;
	}

	public Task(MonoBehaviour mono, IEnumerator coroutine = null)
	{
		m_coroutine = coroutine;
		m_mono = mono;

		m_isRunning = false;
		m_isPaused = false;
	}
	#endregion


	#region Public API
	public virtual void setSpeed(float speed)
	{
		m_taskSpeed = speed;
	}

	public virtual void start()
	{
		m_isRunning = true;

		m_mono.StartCoroutine(doTask());
	}

	public virtual void pause()
	{
		m_isPaused = true;
	}

	public virtual void resume()
	{
		m_isPaused = false;
	}

	public virtual void kill()
	{
		m_isRunning = false;
	}
	#endregion

	#region Protected Functions
	protected virtual IEnumerator doTask()
	{
		while(m_isRunning)
		{
			// skip one frame for pause at the start
			yield return null;

			if(isPaused)
			{
				// paused do nothing
				yield return null;
			}
			else
			{
				// no paused
				yield return m_coroutine.Current;

				if(!m_coroutine.MoveNext())
				{
					m_isRunning = false;
				}
			}
		}
	}

	protected virtual IEnumerator untilDoneFlow()
	{
		while(m_isRunning)
		{
			yield return null;
		}
	}
	#endregion
}
