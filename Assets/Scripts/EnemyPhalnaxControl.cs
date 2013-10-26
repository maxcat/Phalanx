using UnityEngine;
using System.Collections;

public class EnemyPhalnaxControl : MonoBehaviour {
	
	
	#region Field
	[SerializeField] float m_speed;
	Phalanx m_phalanx;
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
	
	
	#region Test Purpose
	public void Test()
	{
		string prefabName = "skeletonDark";
		
		float unitSize = 1f;
		float interval = 0;
		// vector3.zero as center
		
		int colCnt = 5;
		int rowCnt = 5;
		
		Vector3 scale = new Vector3(3, 3, 3);
		
		float width = colCnt * unitSize + (colCnt - 1) * interval;		
		float height = rowCnt * unitSize + (rowCnt - 1) * interval;
		
		float startX = - (width / 2 - unitSize / 2);
		
		float startZ = height / 2 - unitSize /  2;
		
		// x direction
		for(int row = 0; row < rowCnt; row ++)
		{
			for(int col = 0; col < colCnt; col ++)
			{
				m_phalanx.AddUnit(prefabName, new Vector3(startX, 0, startZ), Vector3.back, scale);
				
				// update startX
				startX += (interval + unitSize);
			}
			// update startZ
			startZ -= (interval + unitSize);
			// reset start X
			startX = - (width / 2 - unitSize / 2);
			
		}
		
		for(int i = 0; i < m_phalanx.unitList.Count; i ++)
		{
			m_phalanx.PlayAnimation(i, "run");
		}
		
	}
	
	
	#endregion
}
