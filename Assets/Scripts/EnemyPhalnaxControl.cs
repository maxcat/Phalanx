using UnityEngine;
using System.Collections;

public class EnemyPhalnaxControl : MonoBehaviour {
	
	
	#region Field
	Phalanx m_phalanx;
	public int			m_dataIndex;
	bool				m_collided;
	#endregion
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// update the position of the enemy
		// update the speed 
		float speed = GlobalData.Shared().speed;
		if(!m_collided)
			transform.Translate(new Vector3(0, 0, -speed));
	}
	
	void Awake () {
		// get the phalanx component
		m_phalanx = GetComponent<Phalanx>();
		
		// update the phalanx type
		m_phalanx.isPlayerPhalanx = false;
		m_collided = false;
		
		
		Test();
	}
	
	#endregion
	
	#region Public
	public void UpdatePhalanx(int infoIndex)
	{
		// tan li, pending
		m_dataIndex = infoIndex;
	}
	
	public void onCollided()
	{
		m_collided = true;
	}
	#endregion
	
	
	#region Test Purpose
	public void Test()
	{
		string prefabName = "skeletonDark";
		
		Vector3 scale = new Vector3(3, 3, 3);
		
		// x direction
		for(int row = 0; row < AppConstant.MAX_ROW; row ++)
		{
			for(int col = 0; col < AppConstant.MAX_COL; col ++)
			{
				// enemy speed will be 0
				m_phalanx.AddUnit(prefabName, false, scale, row, col, 0);
				m_phalanx.PlayAnimation(row, col, "waitingforbattle");
			}
	
			
		}
			
	}
	
	
	#endregion
}
