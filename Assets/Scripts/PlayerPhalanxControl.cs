using UnityEngine;
using System.Collections;

public class PlayerPhalanxControl : MonoBehaviour {
	
	// test hello
	#region Fields
	Phalanx m_phalanx;
	[SerializeField] float m_speed;
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
		// create the phalanx
		m_phalanx = GetComponent<Phalanx>();
		
		// update the phalanx type
		m_phalanx.isPlayerPhalanx = true;
		Test();
		
		// init the speed of the player phalanx
		GlobalData.Shared().speed = m_speed;
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
			for(int col = 0; col < 	AppConstant.MAX_COL; col ++)
			{
				m_phalanx.AddUnit(prefabName, true, scale, row, col, m_speed);
				m_phalanx.PlayAnimation(row, col, "run");
				
				// add the first row to the collided units
				if(row == 0)
					CollidedUnits.shared().init(col, m_phalanx.getUnit(row, col));
			}
			
			
			
		}
		
	}
	
	#endregion
}
