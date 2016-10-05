using System.Collections.Generic;

namespace HRGameLogic
{

	public class ObjectStatesData : IData {

#region Fields
		protected Dictionary<uint, ObjectState> 			states;
#endregion

#region Constructor
		public ObjectStatesData()
		{
			states = new Dictionary<uint, ObjectState>();
		}

		public ObjectStatesData(Dictionary<string, object> dataDict)
		{
			Deserialize(dataDict);
		}
#endregion

#region Getter and Setter
		public Dictionary<uint, ObjectState> Data
		{
			get { return states; }
			set { states = value; }
		}
#endregion

#region Implement Interface
		public Dictionary<string, object> Serialize()
		{
			var result = new Dictionary<string, object>();
			var statesDict = new Dictionary<uint, Dictionary<string, object>>();
			
			foreach(uint key in states.Keys)
			{
				statesDict.Add(key, states[key].Serialize());
			}

			result.Add("states", statesDict);

			return result;
		}

		public void Deserialize(Dictionary<string, object> dict)
		{
			if(dict.ContainsKey("states"))
			{
				states = new Dictionary<uint, ObjectState>();
				var statesDict = dict["states"] as Dictionary<string, object>;
				foreach(string key in statesDict.Keys)
				{
					states.Add(System.Convert.ToUInt32(key), new ObjectState(statesDict[key] as Dictionary<string, object>));
				}	
			}	
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
