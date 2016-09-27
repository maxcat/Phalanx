using System.Collections.Generic;

namespace HRGameLogic
{

	public class ObjectStatesData {

#region Fields
		protected Dictionary<uint, ObjectState> 			states;
#endregion

#region Constructor
		public ObjectStatesData()
		{
			states = new Dictionary<uint, ObjectState>();
		}
#endregion

#region Getter and Setter
		public Dictionary<uint, ObjectState> Data
		{
			get { return states; }
			set { states = value; }
		}
#endregion

#region Public API
		public void AddState(uint objectID, ObjectState state)
		{
			if(states.ContainsKey(objectID))
			{
				HRLog.Warning("[WARNING]ObjectStatesData->AddState: object ID " + objectID + " already exist."); 
				states[objectID] = state;
			}
			else
			{
				states.Add(objectID, state);
			}
		}
#endregion
	}
}
