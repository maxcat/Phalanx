using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	
	[SerializeField] float m_speed;
	
	public float playerSpeed
	{
		get {
			return m_speed;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void updatePlayerSpeed(float input)		
	{
		m_speed = input;
	}
}
