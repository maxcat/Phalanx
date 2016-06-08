using UnityEngine;
using System.Collections;

public class AssetBundleLoadTask : Task {

#region Fields
	protected Object 		output;
#endregion

#region Getter and Setter
	public Object Output
	{
		get { return output; }
	}
#endregion

#region Constructor
	public AssetBundleLoadTask(AssetBundleCache cache)
		: base(Service.Get<AssetBundleManager>())
	{
		coroutine = loadFlow(cache);	
	}
#endregion

#region Flow
	protected IEnumerator loadFlow(AssetBundleCache cache)
	{
		if(cache == null || cache.Bundle == null)
		{
			yield break;
		}

		AssetBundleRequest request = cache.Bundle.LoadAssetAsync(cache.BundleName);
		yield return request;

		Debug.Log("[INFO]AssetBundleLoadTask->loadFlow: loading time for " + cache.BundleName + " is " + Time.deltaTime);
		output = request.asset;
	}
#endregion
}
