using System.Collections.Generic;

namespace HRGameLogic
{
	public class Command : IData {

#region Fields
		protected uint 		ownerID;
		protected uint 		sendTag;
#endregion

#region Getter and Setter
		public uint OwnerID
		{
			get { return ownerID; }
		}

		public uint SendTag 
		{
			get { return sendTag; }
		}
#endregion

#region Constructor
		public Command(uint tag, uint ownerID)
		{
			this.ownerID = ownerID;
			this.sendTag = tag;
		}

		public Command(Dictionary<string, object> dict)
		{
			this.Deserialize(dict);
		}

		public Command()
		{
		}
#endregion

#region Implement Interface
		public virtual Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			result.Add("ownerID", ownerID);
			result.Add("sendTag", sendTag);
			
			return result;
		}

		public virtual void Deserialize(Dictionary<string, object> dict)
		{
			if(dict.ContainsKey("ownerID"))
				ownerID = System.Convert.ToUInt32(dict["ownerID"]);	

			if(dict.ContainsKey("sendTag"))
				sendTag = System.Convert.ToUInt32(dict["sendTag"]);
		}
#endregion

#region Server Side Virtual Functions
		public virtual bool Execute(ObjectState currentState, ObjectState nextState)
		{
			return true;	
		}
#endregion
	}
}
