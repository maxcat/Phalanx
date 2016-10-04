using System;
using System.Collections.Generic;

namespace HRGameLogic
{
	public class CommandData : IData
	{

#region Fields
		protected Command 			data;
		protected Type 				commandType;
#endregion

#region Constructor
		public CommandData (Command data)
		{
			this.data = data;
			this.commandType = data.GetType();
		}

		public CommandData(Dictionary<string, object> dict)
		{
			this.Deserialize(dict);
		}
#endregion

#region Getter and Setter
		public Command Data
		{
			get { return data; }
		}
#endregion

#region Implemennt Interface
		public Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> result  = new Dictionary<string, object>();
			result.Add("commandType", commandType);
			result.Add("data", data.Serialize());

			return result;
		}

		public void Deserialize(Dictionary<string, object> dict)
		{
			if(dict.ContainsKey("commandType") && 
					dict.ContainsKey("data"))
			{
				this.commandType = Type.GetType((string)dict["commandType"]);
				this.data = (Command)System.Activator.CreateInstance(this.commandType, dict["data"] as Dictionary<string, object>);
			}	
		}
#endregion
	}
}

