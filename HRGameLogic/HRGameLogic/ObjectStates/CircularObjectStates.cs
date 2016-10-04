using System.Collections.Generic;

namespace HRGameLogic
{

	public class CircularObjectStates 
	{
#region Fields
		protected Dictionary<uint, ObjectState>		 	objectStates;
		protected int 						size = 10;
		protected uint 						lastTag = 0;
#endregion

#region Constructor
		public CircularObjectStates(int size = 10)
		{
			objectStates = new Dictionary<uint, ObjectState>(); 
			this.size = size;
		}
#endregion

#region Public API
		public void Append(ObjectState state)
		{
			if(objectStates.Count == 0)
			{
				objectStates.Add(state.StateTag, state);
				lastTag = state.StateTag;
			}

			if(objectStates.Count > 0)	
			{
				if(state.StateTag - lastTag == 1)
				{
					lastTag ++;
					objectStates.Add(state.StateTag, state);
					if(objectStates.Count >= size)
					{
						objectStates.Remove(lastTag - (uint)size);
					}	
				}	
				else
				{
					HRLog.Warning("[WARNING]CircularObjectStates->Append: last receive time state is " + lastTag + " input time state is " + state.StateTag);
					return;
				}
			}
		}

		public ObjectState Get(uint tag)
		{
			if(objectStates.ContainsKey(tag))
			{
				return objectStates[tag];
			}	

			return null;
		}

		public List<ObjectState> GetLastStates(int count)
		{
			if(count > size || count > objectStates.Count)
				return null;

			List<ObjectState> result = new List<ObjectState>();
			for(int i = 0; i < count; i ++)
			{
				result.Add(objectStates[lastTag + 1 - (uint)count]);
			}
			return result;
		}
#endregion

	}
}

