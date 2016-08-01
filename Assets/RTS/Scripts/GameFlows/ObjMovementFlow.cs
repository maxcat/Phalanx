using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMovementFlow : ObjectFlow {

#region Fields
	protected int 				positionIndex = 0;
#endregion

#region Constructor
	public ObjMovementFlow(GameObject owner, Dictionary<uint, ObjectState> states, uint startTag)
		: base (owner, states, startTag)
	{
		positionIndex = 0;
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
			uint previousStateTag=  stateTag - 1;
			ObjectState previousState = states[previousStateTag];	

			// find all unfinished movement commands
			Command movementCommand = previousState.PassOverCommands.Find(command => typeof(MoveToPosCommand).IsInstanceOfType(command));

			if(movementCommand != null)
			{
				state = new ObjectState(stateTag);
				movementCommand.Execute(previousState, state);
			}
			else
			{
				state = new ObjectState(stateTag);
				state.Positions = new List<Vector2>();
				state.Positions.Add(previousState.Positions[previousState.Positions.Count - 1]);
			}

			// TODO: need test
			Debug.Log("[INFO]ObjMovementFlow->getNextPos: predict state for tag " + stateTag);
			states.Add(stateTag, state);
		}
		else
		{
			Debug.LogError("[ERROR]ObjMovementFlow->getNextPos: can not find state for both tag " + stateTag + " and tag " + (stateTag - 1));
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
