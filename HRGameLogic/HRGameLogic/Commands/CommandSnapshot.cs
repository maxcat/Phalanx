using System.Collections;
using System.Collections.Generic;

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

		public CommandSnapshot(Dictionary<string, object> dict)
		{
			this.Deserialize(dict);
		}
#endregion

#region Implement Interface
		public override Dictionary<string, object> Serialize()
		{
			var result = base.Serialize();
			var commandDict = command.Serialize();

			result.Add("command", commandDict);
			result.Add("commandType", command.GetType());
			result.Add("finishedInThisState", finishedInThisState);

			return result;
		}

		public override void Deserialize(Dictionary<string, object> dict)
		{
			base.Deserialize(dict);	

			if(dict.ContainsKey("command") && 
					dict.ContainsKey("commandType"))
			{
				System.Type commandType = (System.Type)dict["commandType"];	
				this.command = (Command)System.Activator.CreateInstance(commandType); 

				this.command.Deserialize(dict["command"] as Dictionary<string, object>);
			}

			if(dict.ContainsKey("finishedInThisState"))
			{
				this.finishedInThisState = (bool)dict["finishedInThisState"];	
			}
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
