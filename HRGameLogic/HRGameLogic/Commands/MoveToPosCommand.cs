using System.Collections.Generic;

namespace HRGameLogic
{

	public class MoveToPosCommand : Command {

#region Fields
		protected HRVector2D 				destPos;
#endregion

#region Constructor
		public MoveToPosCommand(uint tag, uint ownerID, HRVector2D destPos)
			: base (tag, ownerID)
		{
			this.destPos = destPos;
		}

		public MoveToPosCommand(Dictionary<string, object> dict)
		{
			this.Deserialize(dict);
		}
#endregion

#region Implement Interface
		public override Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> result = base.Serialize();
			result.Add("destPos", destPos.ToDict());

			return result;
		}

		public override void Deserialize(Dictionary<string, object> dict)
		{
			base.Deserialize(dict);	

			if(dict.ContainsKey("destPos"))
			{
				this.destPos = 	new HRVector2D(dict["destPos"] as Dictionary<string, object>);
			}
		}
#endregion

#region Implement Virtual Functions
		public override bool Execute(ObjectState currentState, ObjectState nextState)
		{
			// TODO: read speed from unit data.
			bool finishedInThisState = false;
			float speed = 10;
			HRVector2D startPos = currentState.EndPos;	
			nextState.StartPos = startPos;

			HRVector2D direction = destPos - startPos;
			float distance = direction.magnitude;
			direction.Normalize();

			if(distance > speed)
			{
				nextState.EndPos = speed * direction + startPos;
			}
			else
			{
				nextState.EndPos = destPos;
				finishedInThisState = true;
			}
			return finishedInThisState;
		}
#endregion
	}
}
