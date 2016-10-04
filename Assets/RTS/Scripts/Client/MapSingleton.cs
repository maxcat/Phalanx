using UnityEngine;
using System.Collections;

public class MapSingleton : Singleton
{
#region Static Fields
	static GameObject 			parentObj;
#endregion
#region Fields
	static MapSingleton 			instance;

    public int MapID;
    public int Width;
    public int Height;

    public Camera GameplayCamera;
    public MeshRenderer GridSquare;

    #endregion

#region Static Getter and Setter
    public static MapSingleton Instance
	{
		get {
			if (parentObj == null)
			{
                //parentObj = new GameObject("MapSingleton");
                //instance = parentObj.AddComponent<MapSingleton>();
                parentObj = GameObject.Find("MapObject"); ;
                instance = parentObj.GetComponent<MapSingleton>();
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
        MapSingleton.Instance.clear();
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
        BoxCollider collider = Instance.gameObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(Width, Height, 1);
    }
	
	protected override void clear()
	{
	}
#endregion

#region Touch Grid Functions
    public void TouchReceive(Touch touch)
    {
        Vector3 worldPos = GameplayCamera.ScreenToWorldPoint(touch.position);

        Vector3 localPos = GetComponent<Transform>().InverseTransformPoint(worldPos);

        float squareSize = GridSquare.transform.localScale.x;
        float gridPosX = Mathf.FloorToInt(localPos.x) / squareSize + squareSize / 2;
        float gridPosY = Mathf.FloorToInt(localPos.y) / squareSize + squareSize / 2;

        GridSquare.transform.localPosition = new Vector3(gridPosX, gridPosY, GridSquare.transform.position.z);

        GridSquare.enabled = true;
        Debug.Log("Touched singl grid");

        //TODO: if touch ended with phase TouchPhase.Ended do action and disappear
        if (touch.phase == TouchPhase.Ended)
        {

            GridSquare.enabled = false;
        }
    }

    public void TouchClear()
    {
        GridSquare.enabled = false;
        Debug.Log("Clear Touch");
    }
#endregion
}
