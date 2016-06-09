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
		isFlowStarted = true;
		if(currentFlow == null)
		{
			isFlowStarted = false;
			return false;
		}

		if(isPaused)
			return true;

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
					isFlowStarted = false;
					return false;
				}
				return true;
			}
			else
			{
				isFlowStarted = false;
				return false;
			}
		}
	}

	public override object Current
	{
		get { 
			if(isPaused)
				return null;
			return currentFlow.Current;
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

		if(currentFlow != null && currentFlow.IsFlowStarted)
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
		if(currentFlow != null && currentFlow.IsFlowStarted)
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
