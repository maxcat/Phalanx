using UnityEngine;
using System.Collections;

public class LinkListTask : Task {


	#region Internal Fields
	protected LinkTask current;
	protected LinkTask tail;
	#endregion

	#region Getter and Setter
	public LinkTask Current
	{
		get { return current;}
	}

	public LinkTask Tail
	{
		get { return tail;}
	}

	public bool IsEmpty
	{
		get { return current == tail && tail == null;}
	}
	
	#endregion

	#region Constructor
	public LinkListTask(MonoBehaviour monoClass) : base (monoClass)
	{
		current = null;
		tail = null;
	}
	#endregion

#region Virtual Functions
	public virtual void onChildTaskComplete()
	{

	}

	public virtual void onChildTaskStart()
	{

	}
#endregion

	#region Implement Virtual Function
	public override void Start ()
	{
		running = true;	
		monoClass.StartCoroutine( doTask() );
	}
	public override void Draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.blue;
		GUILayout.Box("LinkListTask");
		GUI.color = originColor;
		
	}

	protected override IEnumerator doTask ()
	{
		if(current == null)
		{
			running = false;
			yield break;
		}
		
		// start the first task
		current.SetSpeed(taskSpeed);
		current.Start();

		
		while(running)
		{
			if(current.Running)
			{
				// current task is running
				if(paused)
				{
					// pause 
					current.Pause();
				}
				else
				{
					current.Resume();
				}

				// update speed
				current.SetSpeed(taskSpeed);
			}
			else
			{
				// current task finished
				if(paused)
				{
					// don't do anything
				}
				else
				{
					// start the next task
					if(current.next != null)
					{
						// next task available
						LinkTask previous = current;
						current = current.next;

						// clear the current reference
						previous.next = null;
						current.SetSpeed(taskSpeed);
						current.Start();

					}
					else
					{
						// all task finished
						current = null;
						tail = null;
						running = false;
					}
				}
			}
			yield return null;
		}
		
		// incase killed by the user
		if(current != null && current.Running)
		{
			current.Kill();
			current = null;
			tail = null;
			// wait for one frame
			yield return null;

		}
		
	}
	#endregion


	#region Public API
	public void AddTask(Task task)
	{
		if(task == null)
			return;

		LinkTask linkTask = new LinkTask(task);
		if(tail != null)
		{
			tail.SetNext(linkTask);
			tail = linkTask;
		}
		else
		{
			// link list is empty
			current = linkTask;
			tail = linkTask;
		}
	}

	public void AddTask(LinkTask linkTask)
	{
		if(linkTask == null)
			return;

		if(tail != null)
		{
			tail.SetNext(linkTask);
			tail = linkTask;
		}
		else
		{
			// link list is empty
			current = linkTask;
			tail = linkTask;
		}
	}

	public void AddTask(IEnumerator coroutine)
	{
		if(coroutine == null)
		{
			return;
		}
		LinkTask task = new LinkTask(monoClass, coroutine);

		if(tail != null)
		{
			tail.SetNext(task);
			tail = task;
		}
		else
		{
			// link list is empty
			current = task;
			tail = task;
		}
	}

	public void RemoteTail()
	{
		tail = current;
		current.next = null;

	}

	public void AddTaskToHead(Task task)
	{
		if(task == null)
			return;

		LinkTask linkTask = new LinkTask(task);

		if(current != null && 
			current.Running)
		{
			linkTask.SetNext(current.next);
			current.SetNext(linkTask);
		}
		else
		{
			if(tail != null)
				linkTask.SetNext(current);
			else
				tail = linkTask;

			current = linkTask;
		}
	}

	public void AddTaskToHead(LinkTask linkTask)
	{
		if(linkTask == null)
			return;

		if(current != null && 
			current.Running)
		{
			linkTask.SetNext(current.next);
			current.SetNext(linkTask);
		}
		else
		{
			if(tail != null)
				linkTask.SetNext(current);
			else
				tail = linkTask;

			current = linkTask;
		}
	}

	public void AddTaskToHead(IEnumerator coroutine)
	{
		if(coroutine == null)
		{
			return;
		}
		LinkTask linkTask = new LinkTask(monoClass, coroutine);

		if(current != null && 
			current.Running)
		{
			linkTask.SetNext(current.next);
			current.SetNext(linkTask);
		}
		else
		{
			if(tail != null)
				linkTask.SetNext(current);
			else
				tail = linkTask;

			current = linkTask;
		}
	}
	#endregion
}
