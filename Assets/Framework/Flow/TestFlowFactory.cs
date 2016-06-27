using UnityEngine;
using System.Collections;

public class TestFlowFactory : FlowFactory {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#region Implement Virtual Functions
	public override Flow CreateFlow()
	{
		return new Flow(flow1());	
	}
#endregion

#region Test Flows
	protected IEnumerator flow1()
	{
		Debug.LogError("====flow 1====");
		yield return flow2();

		Debug.LogError("======delay 1 start=====");
		yield return null;

		Debug.LogError("=====delay 2 start=====");
		yield return null;

		Debug.LogError("=====delay 3 start=====");
		yield return null;

	}

	protected IEnumerator flow2()
	{
		Debug.LogError("====flow 2====");
		yield return flow3();
	}

	protected IEnumerator flow3()
	{
		Debug.LogError("======flow 3======");
		yield return null;
	}

#endregion
}
