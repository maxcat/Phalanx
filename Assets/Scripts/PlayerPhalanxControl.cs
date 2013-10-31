using UnityEngine;
using System.Collections;

public class PlayerPhalanxControl : MonoBehaviour {
	
	// test hello
	#region Fields
	Phalanx m_phalanx;
	#endregion
	
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () 
	{
		Debug.Log("++++player control awake+++");
		// create the phalanx
		m_phalanx = GetComponent<Phalanx>();
		Test();
	}
	
	#endregion
	
	#region Test purpose
	
	public void Test()
	{
		string prefabName = "skeletonNormal";

		Vector3 scale = new Vector3(3, 3, 3);
		
		// x direction
		for(int row = 0; row < AppConstant.MAX_ROW; row ++)
		{
			for(int col = 0; col < AppConstant.MAX_COL; col ++)
			{
				m_phalanx.AddUnit(prefabName, true, scale, row, col);
				m_phalanx.PlayAnimation(row, col, "run");
			}
			
		}
		
	}
	
	#endregion
}
