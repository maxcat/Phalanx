using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFlow : Flow {

#region Fields
	protected Dictionary<uint, ObjectState>		states;
	protected GameObject 				owner;
	protected uint 					stateTag;
	protected ObjectClientController 		controller;
#endregion

#region Constructor
	public ObjectFlow(GameObject owner, Dictionary<uint, ObjectState> states, uint startTag)
		: base ()
	{
		this.owner = owner;
		this.states = states;
		this.stateTag = startTag;

		this.controller = owner.GetComponent<ObjectClientController>();
		source = main();
	}
#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		float stateDuration = TimeStep.STATE_DURATION;

		float timeElapse = 0f;
		Vector3 previousPos = getNextPos();
		Vector3 nextPos = previousPos;
		owner.transform.localPosition = previousPos;

		while(true)
		{
			float deltaTime = Time.deltaTime;
			yield return null;
			
			timeElapse += deltaTime;

			if(timeElapse >= stateDuration)
			{
				timeElapse = timeElapse - stateDuration;
				previousPos = nextPos;
				nextPos = getNextPos();
			}
			owner.transform.localPosition = Vector3.Lerp(previousPos, nextPos, timeElapse / stateDuration);
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

		Vector3 result = state.StartPos;

		stateTag ++;

		return result;
	}
#endregion

}
