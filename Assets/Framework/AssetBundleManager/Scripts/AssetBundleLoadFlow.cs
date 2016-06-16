using UnityEngine;
using System.Collections;

public class AssetBundleLoadFlow : SequentialFlow {

#region Fields
	protected Object 		output;
	protected AssetBundleCache 	cache;
#endregion

#region Getter and Setter
	public Object Output
	{
		get { return output; }
	}
#endregion

#region Constructor
	public AssetBundleLoadFlow(AssetBundleCache cache, Flow dependenciesCacheFlow)
		: base()
	{
		this.cache = cache;
		Add(dependenciesCacheFlow);
		Add(loadFlow(cache));
	}

	public AssetBundleLoadFlow(AssetBundleCache cache)
		: base ()
	{ 
		this.cache = cache;
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

#region Public API
	public void Unload(bool unloadAllLoadedObjects)
	{
		output = null;
		cache.Unload(unloadAllLoadedObjects);	
		cache = null;
	}
#endregion	
}
