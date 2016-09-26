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
