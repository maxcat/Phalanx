using System.Collections;

public class SequentialFlow : Flow {

#region Constructor
	protected Flow 			currentFlow;
	protected Flow 			tailFlow;
#endregion

#region Constructor
	public SequentialFlow()
	{
		currentFlow = null;
		tailFlow = null;
	}
#endregion

#region Implement Virtual Functions 
	public override bool MoveNext()
	{
		isFlowRunning = true;
		if(currentFlow == null)
		{
			isFlowRunning = false;
			return false;
		}

		if(currentFlow.MoveNext())
		{
			return true;
		}
		else
		{
			if(currentFlow.NextFlow != null)
			{
				currentFlow = currentFlow.NextFlow;
				if(!currentFlow.MoveNext())
				{
					isFlowRunning = false;
					return false;
				}
				return true;
			}
			else
			{
				isFlowRunning = false;
				return false;
			}
		}
	}

	public override object Current
	{
		get { 
			return currentFlow.Current;
	       	}
	}

	public override void SetFlowSpeed(float speed)
	{
		currentFlow.SetFlowSpeed(speed);
	}

	public override void Pause()
	{
		if(!isPaused)
		{
			isPaused = true;
			if(currentFlow != null)
				currentFlow.Pause();
		}		
	}

	public override void Resume()
	{
		if(isPaused)
		{
			isPaused = false;
			if(currentFlow != null)
				currentFlow.Resume();
		}

	}
#endregion

#region Public API
	public void Add(Flow next)
	{
		if(next == null)
			return;

		if(tailFlow != null)
		{
			tailFlow.SetNext(next);
			tailFlow = next;
		}
		else
		{
			currentFlow = next;
			tailFlow = next;
		}
	}

	public void Add(IEnumerator nextEnumerator)
	{
		if(nextEnumerator == null)
			return;
		Flow next = new Flow(nextEnumerator);

		if(tailFlow != null)
		{
			tailFlow.SetNext(next);
			tailFlow = next;
		}
		else
		{
			currentFlow = next;
			tailFlow = next;
		}
	}

	public void AddToHead(Flow head)
	{
		if(head == null)
			return;

		if(currentFlow != null && currentFlow.IsFlowRunning)
		{
			head.SetNext(currentFlow.NextFlow);
			currentFlow.SetNext(head);
		}
		else
		{
			if(tailFlow != null)
				head.SetNext(currentFlow);
			else
				tailFlow = head;
			currentFlow = head;
		}
	}

	public void AddToHead(IEnumerator headEnumerator)
	{
		if(headEnumerator == null)
			return;

		Flow head = new Flow(headEnumerator);
		if(currentFlow != null && currentFlow.IsFlowRunning)
		{
			head.SetNext(currentFlow.NextFlow);
			currentFlow.SetNext(head);
		}
		else
		{
			if(tailFlow != null)
				head.SetNext(currentFlow);
			else
				tailFlow = head;
			currentFlow = head;
		}
	}
#endregion

}
