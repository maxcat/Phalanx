using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFlow : Flow {

#region Fields
	protected Dictionary<uint, ObjectState>		states;
	protected GameObject 				owner;
	protected ObjectClientController 		controller;
	protected float 				duration;
#endregion

#region Constructor
	public ObjectFlow(GameObject owner, Dictionary<uint, ObjectState> states)
		: base ()
	{
		this.owner = owner;
		this.states = states;

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
			duration += deltaTime;
			
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
		ObjectState state = controller.GetNextState();

		if(state == null)
			return Vector3.zero;
		else
			return state.StartPos;
	}
#endregion

}
