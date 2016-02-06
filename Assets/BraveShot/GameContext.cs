using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameContext : MonoBehaviour {

	#region Fields
	[SerializeField] protected List<GameObject> 				m_playerBalls;
	[SerializeField] protected List<GameObject>					m_opponentBalls;
	#endregion

	#region Getter and Setter
	public List<GameObject> playerBalls
	{
		get { return m_playerBalls;}
	}

	public List<GameObject> opponentBalls
	{
		get { return m_opponentBalls;}
	}
	
	#endregion
}
