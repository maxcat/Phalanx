using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFlow : Flow {

#region Fields
	protected Dictionary<uint, ObjectState>		states;
	protected GameObject 				owner;
	protected uint 					stateTag;
	protected ObjectClientController 		controller;
	protected int 					positionIndex;
#endregion

#region Constructor
	public ObjectFlow(GameObject owner, Dictionary<uint, ObjectState> states, uint startTag)
		: base ()
	{
		this.owner = owner;
		this.states = states;
		this.stateTag = startTag;

		this.positionIndex = 0;
		this.controller = owner.GetComponent<ObjectClientController>();
		source = main();
	}
#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		float movementStepDuration = TimeStep.TIME_STEP_DURATION / TimeStep.MOVEMENTS_PER_STEP;

		float timeElapse = 0f;
		Vector3 previousPos = getNextPos();
		Vector3 nextPos = previousPos;
		owner.transform.localPosition = previousPos;

		while(true)
		{
			float deltaTime = Time.deltaTime;
			yield return null;
			
			timeElapse += deltaTime;

			if(timeElapse >= movementStepDuration)
			{
				timeElapse = timeElapse - movementStepDuration;
				previousPos = nextPos;
				nextPos = getNextPos();
			}
			owner.transform.localPosition = Vector3.Lerp(previousPos, nextPos, timeElapse / movementStepDuration);
		}
	}
#endregion

#region Protected Functions
	protected Vector3 getNextPos()
	{
		ObjectState state;
		if(states.ContainsKey(stateTag))
		{
			state = states[stateTag];
		}
		else if(states.ContainsKey(stateTag - 1))
		{
			uint previousStateTag = stateTag - 1;
			state = controller.PredictState(previousStateTag); 
		}
		else
		{
			Debug.LogError("[ERROR]ObjectFlow->getNextPos: can not find state for both tag " + stateTag + " and previous tag " + (stateTag - 1));
			return Vector3.zero;
		}

		Vector3 result = Vector3.zero;

		if(state.Positions.Count == 1)
		{
			result = (Vector3)state.Positions[0];
		}
		else
		{
			result = (Vector3)state.Positions[positionIndex];
		}

		if(positionIndex < TimeStep.MOVEMENTS_PER_STEP - 1)
		{
			positionIndex ++;
		}
		else
		{
			positionIndex = 0;
			stateTag ++;	
		}

		return result;
	}
#endregion

}
