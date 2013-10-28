using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// struct to hold the enemy phalanx information
/// may need to load from text file
/// </summary>
public class EnemyPhalanxInfo
{
	/// <summary>
	/// distane from the last enemy phalanx
	/// </summary>
	public float m_distance;
	
	// tan li, pending
	// information of the structure of the phalanx
	// e.g. unit type, formation, etc
	
	// constructor
	public EnemyPhalanxInfo(float distance)
	{
		this.m_distance = distance;
	}
}

public class EnemyControl : MonoBehaviour {
	
	
	#region Fields
	[SerializeField] GameObject[] 	m_phalanxes;
	[SerializeField] bool			m_loopEnemy;
	
	List<EnemyPhalanxInfo>			m_phalanxInfoList; 
	
	int								m_firstPhalanxIndex;
	int								m_lastPhalanxIndex;
	#endregion
	
	#region Accessor
	public List<EnemyPhalanxInfo> phalanxInfoList
	{
		get
		{
			return m_phalanxInfoList;
		}
	}
	
	public GameObject firstPhalanx
	{
		get
		{
			return m_phalanxes[m_firstPhalanxIndex];
		}
	}
	
	public GameObject lastPhalanx
	{
		get
		{
			return m_phalanxes[m_lastPhalanxIndex];
		}
	}
	#endregion
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// check the first enemy phalanx position
		if(firstPhalanx.transform.localPosition.z < AppConstant.ENEMY_UPDATE_BOUNDARY)
		{
			// hit the boundary
			// calculate the info index for update
			int infoIndex;
			if(m_loopEnemy)
			{
				// get the last phalanx data index
				infoIndex = (lastPhalanx.GetComponent<EnemyPhalnaxControl>().m_dataIndex + 1) % 
					m_phalanxInfoList.Count;
			}
			else
			{
				// get the last phalanx data index
				infoIndex = lastPhalanx.GetComponent<EnemyPhalnaxControl>().m_dataIndex ++ ;
				
				if(infoIndex >= m_phalanxInfoList.Count)
				{
					firstPhalanx.SetActive(false);
					m_firstPhalanxIndex ++;
					return;
				}
			}
			// update the phalanx
			firstPhalanx.GetComponent<EnemyPhalnaxControl>().UpdatePhalanx(infoIndex);
			// move to the end
			firstPhalanx.transform.localPosition = lastPhalanx.transform.localPosition + 
				new Vector3(0, 0, m_phalanxInfoList[infoIndex].m_distance);
			
			// update the first and last index
			m_lastPhalanxIndex = m_firstPhalanxIndex;
			m_firstPhalanxIndex = (m_firstPhalanxIndex + 1) % m_phalanxes.Length;
		}
	
	}
	
	void Awake () {
		// parse the phalanx info from file
		this.parsePhalanxInfo();
		
		// init the first and the last phalanx
		m_firstPhalanxIndex = 0;
		m_lastPhalanxIndex = m_phalanxes.Length - 1;
		
		// init the phalanxes
		this.InitPhalanx();
	}
	
	#endregion
	
	#region Internal
	void parsePhalanxInfo()
	{
		// tan li, test data
		m_phalanxInfoList = new List<EnemyPhalanxInfo>();
		m_phalanxInfoList.Add(new EnemyPhalanxInfo(20f));
		m_phalanxInfoList.Add(new EnemyPhalanxInfo(30f));
		
		
	}
	
	void InitPhalanx()
	{
		if(m_phalanxInfoList == null)
		{
			Debug.LogError("[EnemyControl] InitPhalanx: phalanx list can't be null");
			return;
		}
		
		
		if(m_phalanxInfoList.Count == 0)
		{
			Debug.LogError("[EnemyControl] InitPhalanx: phalanx list can't be empty");
			return;
		}
		
		
		Vector3 startPosition = new Vector3();
		startPosition = Vector3.zero;
		
		int count;
		if(m_phalanxInfoList.Count >= m_phalanxes.Length)
		{
			count = m_phalanxes.Length;
		}
		else
		{
			count = m_phalanxInfoList.Count;
			if(!m_loopEnemy)
				m_lastPhalanxIndex = count - 1;
		}
		for(int i = 0; i < count; i ++)
		{
			GameObject obj = m_phalanxes[i];
			
			// update the phalanx
			obj.GetComponent<EnemyPhalnaxControl>().UpdatePhalanx(i);
			
			// update position
			obj.transform.localPosition = Vector3.zero;
			if(i != 0)
			{
				startPosition += new Vector3(0, 0, m_phalanxInfoList[i].m_distance);
				obj.transform.localPosition = new Vector3(0, 0, m_phalanxInfoList[i].m_distance);
			}
		}
		
		for(int j = m_phalanxInfoList.Count; j < m_phalanxes.Length; j ++)
		{
			if(!m_loopEnemy)
			{
				// disable the unuse phalanx object
				m_phalanxes[j].SetActive(false);
			}
			else
			{
				GameObject obj = m_phalanxes[j];
				
				int infoIndex = j % m_phalanxInfoList.Count;
				// update the phalanx
				obj.GetComponent<EnemyPhalnaxControl>().UpdatePhalanx(infoIndex);
			
				// update position
				obj.transform.localPosition = Vector3.zero;
				startPosition += new Vector3(0, 0, m_phalanxInfoList[j].m_distance);
			}
		}
	}
	#endregion
	
}
