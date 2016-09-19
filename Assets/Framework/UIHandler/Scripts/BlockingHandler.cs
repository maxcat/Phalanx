using UnityEngine;
using System.Collections;

public partial class PopupService 
{
#region Fields
	[SerializeField] protected int 				visibleBlockingCount = 0;
	[SerializeField] protected int 				invisibleBlockingCount = 0;
	[SerializeField] protected string 			blockResourcePath = "Popup/Blocking";
	protected GameObject 					blockingObj;
#endregion

#region Getter and Setter
	public GameObject BlockingObj
	{
		get
		{
			if(blockingObj == null)
			{
				GameObject prefab = Resources.Load(blockResourcePath) as GameObject;
				Debug.Assert(prefab != null, "[ERROR]PopupService->BlockingObj: can't find resource " + blockResourcePath);

				blockingObj = GameObject.Instantiate(prefab) as GameObject;
				blockingObj.transform.SetParent(BasePopupUI.transform);
			}
			blockingObj.transform.SetAsLastSibling();
			return blockingObj;
		}
	}
#endregion

#region Public API
	public void ShowVisibleBlocking()
	{
		BlockingHandler handler = BlockingObj.GetComponent<BlockingHandler>();
		Debug.Assert(handler != null, "[ERROR]PopupService->ShowVisibleBlocking: Blocking Prefab must contain BlockingHandler component.");
		
		blockingObj.SetActive(true);
		handler.ShowVisibleObject(true);

		visibleBlockingCount ++;
	}

	public IEnumerator ShowVisibleObjectFlow()
	{
		ShowVisibleBlocking();
		yield return null;
	}

	public void ShowInvisibleBlocking()
	{
		BlockingHandler handler = BlockingObj.GetComponent<BlockingHandler>();
		Debug.Assert(handler != null, "[ERROR]PopupService->ShowInvisibleBlocking: Blocking Prefab must contain BlockingHandler component.");

		blockingObj.SetActive(true);
		handler.ShowVisibleObject(visibleBlockingCount > 0);

		invisibleBlockingCount ++;
	}

	public IEnumerator ShowInvisibleBlockingFlow()
	{
		ShowInvisibleBlocking();
		yield return null;
	}

	public void CloseVisibleBlocking()
	{
		if(blockingObj != null)
		{
			visibleBlockingCount --;
			if(visibleBlockingCount <= 0)
			{
				visibleBlockingCount = 0;
				if(invisibleBlockingCount <= 0)
				{
					blockingObj.SetActive(false);
				}
				else
				{
					BlockingHandler handler = blockingObj.GetComponent<BlockingHandler>();
					Debug.Assert(handler != null, "[ERROR]PopupService->ShowInvisibleBlocking: Blocking Prefab must contain BlockingHandler component.");
					handler.ShowVisibleObject(false);
				}

			}
		}
	}

	public IEnumerator CloseVisibleBlockingFlow()
	{
		CloseVisibleBlocking();
		yield return null;
	}

	public void CloseInvisibleBlocking()
	{
		if(blockingObj != null)
		{
			invisibleBlockingCount --;
			if(invisibleBlockingCount <= 0)
			{
				invisibleBlockingCount = 0;
				if(visibleBlockingCount <= 0)
				{
					blockingObj.SetActive(false);
				}
			}
		}
	}

	public IEnumerator CloseInvisibleBlockingFlow()
	{
		CloseInvisibleBlocking();
		yield return null;
	}
#endregion
}

public class BlockingHandler : MonoBehaviour {

#region Fields
	[SerializeField] protected GameObject 			visibleObject;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Public API
	public void ShowVisibleObject(bool show)
	{
		if(visibleObject != null)
			visibleObject.SetActive(show);
	}
#endregion
}
