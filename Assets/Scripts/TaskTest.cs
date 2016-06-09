using UnityEngine;
using System.Collections;

public class TaskTest : MonoBehaviour {

	IEnumerator flow;
	int 		frameCnt = 0;
	int    		maxFramCnt = 2;
	[SerializeField] protected bool			conditionFit = true;
	SequentialFlow mainFlow;
	float 					time;
	bool 					dynamicAdded = false;

	void Awake ()
	{

	}

	// Use this for initialization
	void Start () {

		mainFlow = new SequentialFlow();
		mainFlow.Add(new DelayFlow(10f));
		mainFlow.Add(helloFlow());
		Task main = new Task(this, mainFlow, true);

	}

	// Update is called once per frame
	void Update () {

		if(frameCnt < maxFramCnt)
		{
			Debug.LogError("+++frame " + frameCnt);
			frameCnt ++;
		}

	}

	protected IEnumerator testFlow()
	{
		Debug.LogError("=====test flow 1 start=====");
		yield return null; 

		if(conditionFit)
			yield return conditionFlow();
		Debug.LogError("====test flow 1 end====");
	}

	protected IEnumerator helloFlow()
	{
		Debug.LogError("*****hello**********");
		yield return null;
	}

	protected IEnumerator testFlow1()
	{
		Debug.LogError("=====test flow 2 start=====");
		yield return new WaitForSeconds(10) ;	
		Debug.LogError("=====test flow 2 end======");
	}

	protected IEnumerator testFlow2()
	{
		Debug.LogError("=====test flow 3 start=====");
		yield return new WaitForSeconds(2);
		Debug.LogError("=====test flow 3 end======");
	}

	protected IEnumerator conditionFlow()
	{
		Debug.LogError("====condtion flow start=====");
		yield return new WaitForSeconds(2);
		Debug.LogError("=====condition flow end=====");
	}

	protected IEnumerator combineFow()
	{
		yield return testFlow();
		yield return testFlow1();

	}
}


