using UnityEngine;
using System.Collections;

public class EnemyPhalnaxControl : MonoBehaviour {
	
	
	#region Field
	[SerializeField] float m_speed;
	Phalanx m_phalanx;
	public int			m_dataIndex;
	#endregion
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		// update the position of the enemy
		transform.Translate(new Vector3(0, 0, -m_speed));
	}
	
	void Awake () {
		// get the phalanx component
		m_phalanx = GetComponent<Phalanx>();
		
		Test();
	}
	
	#endregion
	
	#region Public
	public void UpdatePhalanx(int infoIndex)
	{
		// tan li, pending
		m_dataIndex = infoIndex;
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
				m_phalanx.AddUnit(prefabName, false, scale, row, col);
				m_phalanx.PlayAnimation(row, col, "waitingforbattle");
			}
	
			
		}
			
	}
	
	
	#endregion
}
