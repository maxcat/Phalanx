using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task
{	
	#region Protected Fields
	protected Coroutine			untilDone;
	protected bool					running;
	protected bool					paused;
	protected float					taskSpeed = 1f;
	protected bool					wasKilled;
	protected MonoBehaviour			monoClass;
	protected IEnumerator			coroutine;
	#endregion 
	
	#region Getter and Setter
	public virtual bool	Running { get { return running; } }
	public virtual bool	Paused	{ get { return paused; } }

	public virtual IEnumerator Routine {
		get {
			return coroutine;
		}
		set {
			coroutine = value;
		}
	}
	
	public virtual MonoBehaviour MonoClass {
		get {
			return monoClass;
		}
		set {
			monoClass = value;
		}
	}

	
	public virtual Coroutine UntilDone
	{
		get
		{
			return monoClass.StartCoroutine( stillRunning() );
		}
	}

	public virtual float Speed
	{
		get { return taskSpeed;}
	}
	#endregion
	
	#region Constructors
	public Task()
	{}

	public Task(IEnumerator coroutine) 
		: this(null, coroutine, false)
	{}
	
	public Task(IEnumerator coroutine, bool immediatelyStart)
		:this(null, coroutine, immediatelyStart)
	{
	}

	public Task(MonoBehaviour monoClass)
	{
		this.monoClass = monoClass;	
	}


	public Task(MonoBehaviour monoClass, IEnumerator coroutine, bool immediatelyStart)
	{
		this.monoClass = monoClass;	
		this.coroutine = coroutine;
		if(immediatelyStart)
		{
			Start();	
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
	public virtual void Start()
	{
		running = true;
		
		monoClass.StartCoroutine( doTask() );
	}

	public virtual void Draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.red;
		GUILayout.Box("Task");
		GUI.color = originColor;
		
	}

	public virtual void Pause()
	{
		paused = true;	
	}
	
	public virtual void Resume()
	{
		paused = false;	
	}

	public virtual void SetSpeed(float targetSpeed)
	{
		taskSpeed = targetSpeed;
	}
	
	public virtual void Kill()
	{
		wasKilled = true;
		running	= false;
		paused	= false;
	}
	
	public virtual void Kill( float delayInSeconds )
	{
		var delay = (int)(delayInSeconds * 1000);
		new System.Threading.Timer( obj =>
		                           {
			lock(this)
			{
				Kill();	
			}
		}, null, delay, System.Threading.Timeout.Infinite);
	}

	protected virtual IEnumerator doTask()
	{
		while(running)
		{
			if(paused)
			{
				yield return null;
			}
			else 
			{
				bool haveNext = false;
				
				if(coroutine != null)
				{
					haveNext = coroutine.MoveNext();
				}
				
				if(haveNext)
				{
					yield return coroutine.Current;	
				}
				else
				{
					running = false;
				}
			}
		}
	}

	public virtual IEnumerator stillRunning()
	{
		while(running)
		{
			yield return null;	
		}
	}

	public virtual bool HasRecursiveStruct(Task parent = null)
	{
		return false;
	}
	#endregion

}
