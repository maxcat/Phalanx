using System.Collections.Generic;

namespace HRGameLogic
{
	public class ObjectManager {

#region Static Fields
		private static ObjectManager 		instance;
#endregion

#region Static Functions
		public static ObjectManager Instance
		{
			get 
			{
				if (instance == null)
					instance = new ObjectManager();
				return instance;
			}
		}
#endregion

#region Fields
		protected Dictionary<uint, ObjectController>		objectPool;
#endregion

#region Constructor
		public ObjectManager()
		{
			objectPool = new Dictionary<uint, ObjectController>();
		}
#endregion

#region Test Functions
		public void TestInit(uint tag)
		{
			HRLog.Info("=======test init=====");
			ObjectController ctrl1 = new ObjectController(1);
			ObjectController ctrl2 = new ObjectController(2);

			ctrl1.Init(tag, HRVector2D.right * -50);
			ctrl2.Init(tag, HRVector2D.right * 50);

			objectPool.Add(1, ctrl1);
			objectPool.Add(2, ctrl2);
		}
#endregion

#region Public API
		public ObjectController GetObject(uint id)
		{
			if(objectPool.ContainsKey(id))
				return objectPool[id];	

			return null;
		}

		public void AddObject(ObjectController controller)
		{
			if(controller == null)
			{
				HRLog.Error("[ERROR] ObjectManager->AddObject: input object is null.");
				return;
			}

			if(objectPool.ContainsKey(controller.ID))
			{
				HRLog.Error("[ERROR] ObjectManager->AddObject: id " + controller.ID + " already added.");
				return;
			}

			objectPool.Add(controller.ID, controller);
		}

		public void UpdateState(uint tag, uint commandDelayInState)
		{
			foreach(ObjectController controller in objectPool.Values)
			{
				controller.UpdateState(tag, commandDelayInState);
			}
		}

		public Dictionary<uint, ObjectState> GetStates(uint serverTag)
		{
			Dictionary<uint, ObjectState> result = new Dictionary<uint, ObjectState>();

			foreach(uint key in objectPool.Keys)
			{
				ObjectController controller = objectPool[key];
				ObjectState state = controller.GetState(serverTag);
				if(state != null)
					result.Add(key, state);
			}

			return result;
		}

		public ObjectStatesData GenerateStateData(uint serverStateTag)
		{
			ObjectStatesData data = new ObjectStatesData();

			foreach(uint key in objectPool.Keys)
			{
				ObjectController controller = objectPool[key];
				data.AddState(controller.ID, controller.GetState(serverStateTag));
			}

			return data;
		}
#endregion

	}
}
