using System.Collections.Generic;

namespace HRGameLogic
{

	public class ObjectState : IData {

#region Static Fields
		public static readonly float 				DURATION = 0.2f;
#endregion

#region Fields
		protected List<CommandSnapshot> 			commandSnapshots;
		protected HRVector2D 					startPos;
		protected HRVector2D 					endPos;
		protected uint 						stateTag;
		protected bool 						isPrediction = false;
#endregion

#region Getter and Setter
		public HRVector2D StartPos
		{
			get { return startPos; }
			set { startPos = value; }
		}

		public HRVector2D EndPos
		{
			get { return endPos; }
			set { endPos = value; }
		}

		public List<CommandSnapshot> PassOverCommands
		{
			get
			{
				return commandSnapshots.FindAll(command => !command.FinishedInThisState);
			}
		}

		public List<CommandSnapshot> CommandSnapshots
		{
			get { return commandSnapshots; }
			set { commandSnapshots = value; }
		}

		public uint StateTag
		{
			get { return stateTag; }
		}

		public bool IsPrediction
		{
			get { return isPrediction; }
			set { isPrediction = value; }
		}
#endregion

#region Constructor
		public ObjectState(uint tag)
		{
			commandSnapshots = new List<CommandSnapshot>();
			stateTag = tag;
			isPrediction = false;
		}

		public ObjectState(Dictionary<string, object> dict)
		{
			Deserialize(dict);
		}
#endregion

#region Implement Interface Functions
		public Dictionary<string, object> Serialize()
		{
			var result = new Dictionary<string, object>();

			var commandDictList = new List<Dictionary<string, object>>();
			for(int i = 0; i < commandSnapshots.Count; i ++)
			{
				commandDictList.Add(commandSnapshots[i].Serialize());	
			}
			result.Add("commandSnapshots", commandDictList);

			result.Add("startPos", startPos.ToDict());
			result.Add("endPos", endPos.ToDict());
			result.Add("stateTag", stateTag);

			return result;
		}

		public void Deserialize(Dictionary<string, object> dict)
		{
			if(dict.ContainsKey("startPos"))
				this.startPos = new HRVector2D(dict["startPos"] as Dictionary<string, object>);

			if(dict.ContainsKey("endPos"))
				this.endPos = new HRVector2D(dict["endPos"] as Dictionary<string, object>);

			if(dict.ContainsKey("stateTag"))
				this.stateTag = System.Convert.ToUInt32(dict["stateTag"]);

			if(dict.ContainsKey("commandSnapshots"))
			{
				this.commandSnapshots = new List<CommandSnapshot>();

				foreach(object obj in dict["commandSnapshots"] as List<object>)
				{
					CommandSnapshot shot = new CommandSnapshot(obj as Dictionary<string, object>);
					this.commandSnapshots.Add(shot);
				}
			}
		}
#endregion

#region Public API
		public ObjectState GenerateNextState(List<Command> commandList)
		{
			ObjectState newState = new ObjectState(stateTag + 1);	

			List<CommandSnapshot> snapshotList = new List<CommandSnapshot>();
			snapshotList.AddRange(takeCommandSnapshots(this.PassOverCommands));

			if(commandList != null)
			{ 
				snapshotList.AddRange(takeCommandSnapshots(commandList));
			}

			newState.CommandSnapshots = snapshotList;
			newState.StartPos = this.endPos;
			newState.EndPos = this.endPos;

			executeCommands(newState, snapshotList);
			return newState;
		}
#endregion

#region Virtual Functions
		protected virtual void executeCommands(ObjectState nextState, List<CommandSnapshot> snapshotList)
		{
			for(int i = 0; i < snapshotList.Count; i ++)
			{
				snapshotList[i].Execute(this, nextState);
			}
		}
#endregion

#region Protected Functions
		protected List<CommandSnapshot> takeCommandSnapshots(List<Command> commandList)
		{
			List<CommandSnapshot> snapshots = new List<CommandSnapshot>();	

			if(commandList != null)
			{
				for (int i = 0; i < commandList.Count; i ++)
				{
					snapshots.Add(new CommandSnapshot(commandList[i]));
				}
			}
			return snapshots;
		}

		protected List<CommandSnapshot> takeCommandSnapshots(List<CommandSnapshot> snapshotList)
		{
			List<CommandSnapshot> snapshots = new List<CommandSnapshot>();	

			if(snapshotList != null)
			{
				for (int i = 0; i < snapshotList.Count; i ++)
				{
					snapshots.Add(new CommandSnapshot(snapshotList[i].Base));
				}
			}
			return snapshots;
		}
#endregion
	}
}
