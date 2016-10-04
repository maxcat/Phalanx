using System.Collections.Generic;

namespace HRGameLogic
{
 	interface IData
	{
#region Interface Functions
		Dictionary<string, object> Serialize();
		void Deserialize(Dictionary<string, object> dict);
#endregion
	}
}

