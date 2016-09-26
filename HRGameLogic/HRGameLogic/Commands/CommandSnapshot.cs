using System.Collections;

namespace HRGameLogic
{
	public class CommandSnapshot : Command {

#region Fields
		protected bool 				finishedInThisState = false;
		protected Command 			command;
#endregion

#region Getter and Setter
		public bool FinishedInThisState
		{
			get { return finishedInThisState; }
			set { finishedInThisState = value; }
		}

		public Command Base
		{
			get { return command; }
		}
#endregion

#region Constructor
		public CommandSnapshot(Command command, bool finishedInThisState = false)
			: base (command.SendTag, command.OwnerID)
		{
			this.command = command;
			this.finishedInThisState = finishedInThisState;	
		}
#endregion

#region Implement Virtual Functions
		public override bool Execute(ObjectState currentState, ObjectState nextState)
		{
			finishedInThisState = command.Execute(currentState, nextState);
			return finishedInThisState;
		}
#endregion
	}
}
