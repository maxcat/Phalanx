using UnityEngine;
using System.Collections;

public class AssetBundleLoadFlow : SequentialFlow {

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
	public AssetBundleLoadFlow(AssetBundleCache cache, Flow cacheFlow)
		: base()
	{
		Add(cacheFlow);
		Add(loadFlow(cache));
	}

	public AssetBundleLoadFlow(AssetBundleCache cache)
		: base ()
	{
		Add(loadFlow(cache));
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

		Debug.Log("[INFO]AssetBundleLoadFlow->loadFlow: loading time for " + cache.BundleName + " is " + Time.deltaTime);
		output = request.asset;
	}
#endregion
}
