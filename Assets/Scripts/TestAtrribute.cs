using UnityEngine;
using System.Collections;

public class TestData
{
#region Fields
	public int 			PublicInt;
	protected float 		protectedFloat;
#endregion	

#region Functions

	public void PublicFunction()
	{

	}

	protected void protectedFunction()
	{
		
	}

	private void privateFunction()
	{

	}
#endregion
}


public class TestAtrribute : MonoBehaviour {



	// Use this for initialization
	void Start () {
		foreach(var field in typeof(TestData).GetFields())
		{
			Debug.LogError("Field " + field);
		}

		foreach(var member in typeof(TestData).GetMembers())
		{
			Debug.LogError("Member " + member);
		}

		foreach(var method in typeof(TestData).GetMethods())
		{
			Debug.LogError("Methods " + method);
		}

		foreach(var property in typeof(TestData).GetProperties())
		{
			Debug.LogError("Property " + property);	
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
