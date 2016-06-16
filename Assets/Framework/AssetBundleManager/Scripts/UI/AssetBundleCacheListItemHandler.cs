using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AssetBundleCacheListItemHandler : UIListItemHandler {

#region Fields
	[SerializeField] protected Text 			bundleNameLabel;
	[SerializeField] protected Text 			refCntLabel;
	[SerializeField] protected Text 			bundleSizeLabel;
	[SerializeField] protected Text 			isCachedLabel;
	[SerializeField] protected Text 			pathLabel;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Implement Virtual Functions
	public override void UpdateData(object data)
	{
		if(data != null)
		{
			try
			{
				AssetBundleCache cache = data as AssetBundleCache;
				bundleNameLabel.text = "Bundle Name: " + cache.BundleName;
				pathLabel.text = "Path: " + cache.FilePath;
				bundleSizeLabel.text = "Bundle Size: " + cache.BundleSize;
				refCntLabel.text = "Reference Count: " + cache.RefCount;
				isCachedLabel.text = "Is Cached: " + cache.IsCached;
			}
			catch (System.InvalidCastException)
			{
				Debug.LogError("[ERROR]AssetBundleCacheListItemHandler->UpdateData: can not cast " + data + " to AssetBundleCache.");
				return;
			}
		}

	}
#endregion
}
