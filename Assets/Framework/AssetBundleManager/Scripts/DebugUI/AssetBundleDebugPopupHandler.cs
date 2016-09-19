using UnityEngine;
using System.Collections;

public partial class PopupService 
{
#region Fields
	protected string 			assetBundleDebugPopupPath = "Popup/DebugAssetbundleList";
#endregion

#region Popup Builder
	public void ShowAssetBundleDebugPopup()
	{
		GameObject prefab = Resources.Load(assetBundleDebugPopupPath) as GameObject;
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		addPopup(instance, null, true);
		instance.GetComponent<PopupHandler>().Init(null, true);
	}
#endregion
}

public class AssetBundleDebugPopupHandler : PopupHandler {

#region Fields
	[SerializeField] protected UIListHandler			listHandler;	
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		listHandler.UpdateData(AssetBundleManager.Instance.CacheList.ToArray());
	}
#endregion
}
