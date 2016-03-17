using UnityEngine;
using System.Collections;

public class LinkListTask : Task {


	#region Internal Fields
	LinkTask m_current;
	LinkTask m_tail;
	#endregion

	#region Getter and Setter
	public LinkTask current
	{
		get { return m_current;}
	}

	public LinkTask tail
	{
		get { return m_tail;}
	}

	public bool isEmpty
	{
		get { return m_current == m_tail && m_tail == null;}
	}
	
	#endregion

	#region Constructor
	public LinkListTask(MonoBehaviour monoClass) : base (monoClass)
	{
		m_current = null;
		m_tail = null;
	}
	#endregion

	#region Implement Virtual Function
	public override void start ()
	{

		m_running = true;
		
		m_monoClass.StartCoroutine( doTask() );
	}
	public override void draw()
	{
		Color originColor = GUI.color;
		GUI.color = Color.blue;
		GUILayout.Box("LinkListTask");
		GUI.color = originColor;
		
	}

	protected override IEnumerator doTask ()
	{
		if(m_current == null)
		{
			m_running = false;
			yield break;
		}
		
		// start the first task
		m_current.setSpeed(m_taskSpeed);
		m_current.start();

		
		while(m_running)
		{
			if(m_current.Running)
			{
				// current task is running
				if(m_paused)
				{
					// pause 
					m_current.pause();
				}
				else
				{
					m_current.resume();
				}

				// update speed
				m_current.setSpeed(m_taskSpeed);
			}
			else
			{
				// current task finished
				if(m_paused)
				{
					// don't do anything
				}
				else
				{
					// start the next task
					if(m_current.next != null)
					{
						// next task available
						LinkTask previous = m_current;
						m_current = m_current.next;

						// clear the current reference
						previous.next = null;
						m_current.setSpeed(m_taskSpeed);
						m_current.start();

					}
					else
					{
						// all task finished
						m_current = null;
						m_tail = null;
						m_running = false;
					}
				}
			}
			yield return null;
		}
		
		// incase killed by the user
		if(m_current != null && m_current.Running)
		{
			m_current.kill();
			m_current = null;
			m_tail = null;
			// wait for one frame
			yield return null;

		}
		
	}
	#endregion


	#region Public API
	public void addTask(Task task)
	{
		if(task == null)
			return;

		LinkTask linkTask = new LinkTask(task);
		if(m_tail != null)
		{
			m_tail.setNext(linkTask);
			m_tail = linkTask;
		}
		else
		{
			// link list is empty
			m_current = linkTask;
			m_tail = linkTask;
		}
	}

	public void addTask(LinkTask linkTask)
	{
		if(linkTask == null)
			return;

		if(m_tail != null)
		{
			m_tail.setNext(linkTask);
			m_tail = linkTask;
		}
		else
		{
			// link list is empty
			m_current = linkTask;
			m_tail = linkTask;
		}
	}

	public void addTask(IEnumerator coroutine)
	{
		if(coroutine == null)
		{
			return;
		}
		LinkTask task = new LinkTask(m_monoClass, coroutine);

		if(m_tail != null)
		{
			m_tail.setNext(task);
			m_tail = task;
		}
		else
		{
			// link list is empty
			m_current = task;
			m_tail = task;
		}
	}
	#endregion
}
