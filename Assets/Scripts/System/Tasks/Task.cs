using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task
{	
	#region Protected Fields
	protected Coroutine			m_untilDone;
	protected bool					m_running;
	protected bool					m_paused;
	public float					m_taskSpeed = 1f;
	protected bool					m_wasKilled;
	protected MonoBehaviour			m_monoClass;
	protected IEnumerator			m_coroutine;
	#endregion 
	
	#region Getter and Setter
	public virtual bool	Running { get { return m_running; } }
	public virtual bool	Paused	{ get { return m_paused; } }

	public virtual IEnumerator coroutine {
		get {
			return m_coroutine;
		}
		set {
			m_coroutine = value;
		}
	}
	
	public virtual MonoBehaviour monoClass {
		get {
			return m_monoClass;
		}
		set {
			m_monoClass = value;
		}
	}

	
	public virtual Coroutine UntilDone
	{
		get
		{
			return m_monoClass.StartCoroutine( stillRunning() );
		}
	}

	public virtual float speed
	{
		get { return m_taskSpeed;}
	}
	#endregion
	
	#region Constructors
	public Task()
	{
		m_monoClass = Service.get<TaskService>();
	}

	public Task(IEnumerator coroutine) 
		: this(null, coroutine, false)
	{}
	
	public Task(IEnumerator coroutine, bool immediatelyStart)
		:this(null, coroutine, immediatelyStart)
	{
	}

	public Task(MonoBehaviour monoClass)
	{
		if(monoClass == null)
		{
			m_monoClass	= Service.get<TaskService>();
		}
		else 
		{
			m_monoClass = monoClass;	
		}
	}


	public Task(MonoBehaviour monoClass, IEnumerator coroutine, bool immediatelyStart)
	{
		if(monoClass == null)
		{
			m_monoClass	= Service.get<TaskService>();
		}
		else 
		{
			m_monoClass = monoClass;	
		}
		m_coroutine = coroutine;
		if(immediatelyStart)
		{
			start();	
		}
	}

	#endregion
	
	#region Statis Task Creators
	public static Task Create(IEnumerator coroutine)
	{
		return new Task(coroutine);	
	}
	
	public static Task Create(IEnumerator coroutine, bool immediatelyStart)
	{
		return new Task(coroutine, immediatelyStart);	
	}
	
	public static Task Create(MonoBehaviour monoClass, IEnumerator coroutine, bool immediatelyStart = false)
	{
		return new Task(monoClass, coroutine, immediatelyStart);
	}
	#endregion

	#region Interface
	public virtual void start()
	{
		m_running = true;
		
		m_monoClass.StartCoroutine( doTask() );
	}

	public virtual void pause()
	{
		m_paused = true;	
	}
	
	public virtual void resume()
	{
		m_paused = false;	
	}

	public virtual void setSpeed(float targetSpeed)
	{
		m_taskSpeed = targetSpeed;
	}
	
	public virtual void kill()
	{
		m_wasKilled = true;
		m_running	= false;
		m_paused	= false;
	}
	
	public virtual void kill( float delayInSeconds )
	{
		var delay = (int)(delayInSeconds * 1000);
		new System.Threading.Timer( obj =>
		                           {
			lock(this)
			{
				kill();	
			}
		}, null, delay, System.Threading.Timeout.Infinite);
	}

	protected virtual IEnumerator doTask()
	{
		while(m_running)
		{
			if(m_paused)
			{
				yield return null;
			}
			else 
			{
				bool haveNext = false;
				
				if(m_coroutine != null)
				{
					haveNext = m_coroutine.MoveNext();
				}
				
				if(haveNext)
				{
					yield return m_coroutine.Current;	
				}
				else
				{
					m_running = false;
				}
			}
		}
	}

	public virtual IEnumerator stillRunning()
	{
		while(m_running)
		{
			yield return null;	
		}
	}
	#endregion
	
	#region Public API
	public IEnumerator startAsCoroutine()
	{
		m_running = true;
		
		yield return m_monoClass.StartCoroutine( doTask() );
	}
	#endregion

}
