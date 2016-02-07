using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInputHandler : MonoBehaviour {

	#region Fields
	[SerializeField] protected Camera					m_gameCamera;
	[SerializeField] protected GameObject				m_selectedBall;
	[SerializeField] protected float					m_dragDistance = 1f;

	protected bool										m_takingUserInput = false;
	protected Task										m_mainTask;
	#endregion


	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {

		takeUserInput();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region Public API
	public void takeUserInput(bool stop = false)
	{
		if(stop)
		{
			m_takingUserInput = false;
		}
		else
		{
			if(m_mainTask != null && m_mainTask.Running)
				return;

			m_takingUserInput = true;

			m_mainTask = new Task(this, mainFlow(), true);
		}
	}
	#endregion

	#region Protected Functions
	protected IEnumerator mainFlow()
	{
		bool isMouseDown = false;
		Vector3 mouseDownPos = Vector3.zero;
		List<GameObject> playerBalls = GetComponent<GameContext>().playerBalls;
		while(playerBalls != null && playerBalls.Count > 0 && m_takingUserInput)
		{
			if(Input.GetMouseButtonDown(0))
			{
				isMouseDown = true;
				mouseDownPos = Input.mousePosition;
			}
			else if(Input.GetMouseButtonUp(0))
			{
				isMouseDown = false;

				Vector3 mouseUpPos = Input.mousePosition;
				float mouseDragDistance = Vector3.Distance(mouseDownPos, mouseUpPos);
				if(mouseDragDistance <= m_dragDistance)
				{
					// no mouse dragging
					GameObject selectedBall = getSelectedBall(playerBalls, mouseUpPos);

					if(selectedBall != null)
						m_selectedBall = selectedBall;
				}
				else
				{
					// mouse dragged
					if(m_selectedBall != null)
					{
						releaseBall(mouseDownPos - mouseUpPos);
					}
				}
			}
			else
			{
				if(m_selectedBall != null && isMouseDown)
				{
					dragBall(mouseDownPos - Input.mousePosition);
				}
			}
			yield return null;
		}
	}

	protected GameObject getSelectedBall(List<GameObject> playerBalls, Vector3 mouseUpPos)
	{
		Ray ray = m_gameCamera.ScreenPointToRay(mouseUpPos);
		
		RaycastHit hit;
		
		for(int i = 0; i < playerBalls.Count; i ++)
		{
			GameObject playerBall = playerBalls[i];
			
			if(playerBall != null && playerBall.GetComponent<Collider>() != null)
			{
				if(playerBall.GetComponent<Collider>().Raycast(ray, out hit, 100f))
				{
					return playerBall;
				}
			}
		}

		return null;
	}

	protected void dragBall(Vector3 input)
	{
		m_selectedBall.GetComponent<AimingViewHandler>().drawDirectionLine(input);
	}

	protected void releaseBall(Vector3 input)
	{
		m_selectedBall.GetComponent<AimingViewHandler>().removeLine();
		m_selectedBall.GetComponent<BallControl>().releaseBall(input);
	}
	#endregion
}
