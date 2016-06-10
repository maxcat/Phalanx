using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleManager : Singleton {

#region Fields
	protected List<AssetBundleCache>		cachePool;
	protected AssetBundleManifest 			manifest;
#endregion

#region Public API
	public AssetBundleLoadTask CreateLoadingTask(string name)
	{
		AssetBundleCache cache = new AssetBundleCache(name);

		Debug.Log("[INFO] AssetBundleManager->CreateLoadingTask: bundle " + name + " hash is " + manifest.GetAssetBundleHash(name));

		foreach(string dependency in manifest.GetAllDependencies(name))
		{
			Debug.Log("dependency is " + dependency);
		}

		cache.LoadBundleToCache();
		return new AssetBundleLoadTask(cache);
	}

	public AssetBundleLoadFlow CreateLoadingFlow(string name)
	{
		AssetBundleCache cache = new AssetBundleCache(name);

		Debug.Log("[INFO] AssetBundleManager->CreateLoadingFlow: bundle " + name + " hash is " + manifest.GetAssetBundleHash(name));

		SequentialFlow cacheFlow = new SequentialFlow();
		foreach(string dependency in manifest.GetAllDependencies(name))
		{
			Debug.Log("dependency is " + dependency);
			AssetBundleCache dependCache = new AssetBundleCache(dependency);
			cacheFlow.Add(dependCache.LoadBundleToCacheEnumerator());
		}

		cacheFlow.Add(cache.LoadBundleToCacheEnumerator());
		return new AssetBundleLoadFlow(cache, cacheFlow);
	}
#endregion

#region Override MonoBehaviour
	void Update ()
	{

	}
#endregion

#region Protected Functions
	protected void loadAssetBundleInfo()
	{
		string filePath = GameUtilities.AssetBundleCachePath + "/StreamingAssets";
		byte[] stream = File.ReadAllBytes(filePath);
		AssetBundle main = AssetBundle.LoadFromMemory(stream);
		manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
	}
#endregion

#region Implement Virtual Functions
	public override void Init()
	{
		if(!isInited)
		{
			isPersist = true;
			loadAssetBundleInfo();
		}
	}
#endregion
}
