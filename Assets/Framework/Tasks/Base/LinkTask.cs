using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LinkTask : Task {

	#region Internal Fields
	protected LinkTask 						nextTask;
	protected Task 							wrappedTask;
	#endregion
	
	
	#region Properties
	public override Coroutine UntilDone {
		get {
			return wrappedTask.UntilDone;
		}
	}

	public override IEnumerator Routine {
		get {
			return wrappedTask.Routine;
		}
		set {
			wrappedTask.Routine = value;
		}
	}

	public override MonoBehaviour MonoClass {
		get {
			return wrappedTask.MonoClass;
		}
		set {
			wrappedTask.MonoClass = value;
		}
	}

	public override bool Running {
		get {
			return wrappedTask.Running;
		}
	}

	public override bool Paused {
		get {
			return wrappedTask.Paused;
		}
	}

	#endregion
	
	#region Setter and Getter
	public LinkTask next
	{
		get{ return nextTask;}
		set{ nextTask = value;}
	}

	public Task WrappedTask
	{
		get {return wrappedTask;}
	}
	#endregion
	
	
	#region Constructor
	public LinkTask(MonoBehaviour monoClass) 
	{
		wrappedTask = new Task(monoClass);
		nextTask = null;
	}

	public LinkTask(MonoBehaviour monoClass, IEnumerator coroutine)
	{
		wrappedTask = new Task(monoClass, coroutine, false);
		nextTask = null;
	}

	public LinkTask(Task task)
	{
		wrappedTask = task;
		nextTask = null;
	}
	
	#endregion
	
	
	#region Implement Interface
	public override void SetSpeed (float targetSpeed)
	{
		wrappedTask.SetSpeed(targetSpeed);
	}

	public override void Start ()
	{
		wrappedTask.SetSpeed(taskSpeed);
		wrappedTask.Start();
	}

	public override IEnumerator stillRunning ()
	{
		while(wrappedTask.Running)
		{

			yield return null;	
		}
	}
	

	public override void Pause ()
	{
		wrappedTask.Pause();
	}

	public override void Resume ()
	{
		wrappedTask.Resume();
	}

	public override void Kill ()
	{
		wrappedTask.Kill ();
	}

	public override void Kill (float delayInSeconds)
	{
		wrappedTask.Kill (delayInSeconds);
	}
	

	#endregion
	
	#region Public API
	public void SetNext(LinkTask task)
	{
		nextTask = task;
	}

	public void SetNext(Task task)
	{
		LinkTask linkTask = new LinkTask(task);
		nextTask = linkTask;
	}

	public void SetNext(IEnumerator coroutine)
	{
		LinkTask task = new LinkTask(monoClass, coroutine);
		nextTask = task;
	}
	#endregion
	

}


