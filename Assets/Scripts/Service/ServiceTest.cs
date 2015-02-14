using UnityEngine;
using System.Collections;

public class ServiceTest : MonoBehaviour {

	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
		Task testTask = new Task(this, testFlow());

		testTask.start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion


	#region Protected Functions
	protected IEnumerator testFlow()
	{
		Service.get<TaskService>();

		yield return new WaitForSeconds(10f);

		Service.remove<TaskService>();
	}
	#endregion
}
