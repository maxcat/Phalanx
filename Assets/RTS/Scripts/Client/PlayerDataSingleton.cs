using UnityEngine;
using System.Collections;

public class PlayerDataSingleton : Singleton
{
#region Static Fields
	static GameObject 			parentObj;
#endregion
#region Fields
	static PlayerDataSingleton 			instance;

    public int PlayerID;
    public int TeamID;
#endregion
	
#region Static Getter and Setter
	public static PlayerDataSingleton Instance
	{
		get {
			if (parentObj == null)
			{
				parentObj = new GameObject("PlayerDataSingleton");
				instance = parentObj.AddComponent<PlayerDataSingleton>();
				instance.init();
			}
			return instance;
		}
	}
#endregion
	
#region Static Functions
	public static void Remove()
	{
		instance.clear();
		Destroy(parentObj);
	}
#endregion
	
#region Override MonoBehaviour
	void Awake()
	{
	}
	
	void OnApplicationPause()
	{
	}
	
	void OnLevelWasLoaded(int level)
	{
	}
#endregion
	
#region Implement Virtual Functions
	protected override void init()
	{
	}
	
	protected override void clear()
	{
	}
#endregion
}
