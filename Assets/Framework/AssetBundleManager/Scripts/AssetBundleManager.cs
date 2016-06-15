﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleManager : Singleton {

#region Fields
	[SerializeField] protected long 			bundleSizeThreadhold = 500000;
	protected Dictionary<string, AssetBundleCache>		cachePool;
	protected AssetBundleSummary	 			summary;
#endregion

#region Public API
	public AssetBundleLoadFlow CreateLoadingFlow(string name)
	{
		AssetBundleCache cache = getCache(name);
		if(cache == null)
			return null;
		AssetBundleCacheFlow cacheFlow = new AssetBundleCacheFlow(generateCachingList(name), bundleSizeThreadhold);
		return new AssetBundleLoadFlow(cache, cacheFlow);
	}

	public void Unload(string bundleName)
	{
		
	}

	public IEnumerator InitAsync()
	{
		summary = new AssetBundleSummary();
		yield return summary.InitAsync();

		string[] bundleArray = summary.GetAllAssetBundles();
		cachePool = new Dictionary<string, AssetBundleCache>();

		if(!GameUtilities.IsEmpty(bundleArray))
		{
			for(int i = 0; i <bundleArray.Length; i ++)
			{
				AssetBundleCache cache = new AssetBundleCache(bundleArray[i]);
				if(cache.Init())
				{
					cachePool.Add(cache.BundleName, cache);
				}
			}	
		}
	}
#endregion

#region Override MonoBehaviour
	void Update ()
	{

	}
#endregion

#region Protected Functions
	protected List<AssetBundleCache> generateCachingList(string name)
	{
		AssetBundleCache cache = getCache(name); 
		if(cache == null)
			return null;

		List<AssetBundleCache> result = new List<AssetBundleCache>();
		result.Add(cache);

		string[] dependencyArray = summary.GetAllDependencies(name);
		if(!GameUtilities.IsEmpty(dependencyArray))
		{
			for(int i = 0; i < dependencyArray.Length; i ++)
			{
				List<AssetBundleCache> dependencyCachList = generateCachingList(dependencyArray[i]);
				if(!GameUtilities.IsEmpty(dependencyCachList))
					result.AddRange(dependencyCachList);
			}
		}
		
		return result;
	}
	
	protected AssetBundleCache getCache(string name)
	{
		if(GameUtilities.IsEmpty(cachePool))
		{
			Debug.LogError("[ERROR]AssetBundleManager->getCache: InitAsync not finished yet.");
			return null;
		}	

		if(cachePool.ContainsKey(name))
		{
			return cachePool[name];
		}
		else
		{
			Debug.LogError("[ERROR]AssetBundleManager->getCache: " + name + " is not inside the cache pool.");
			return null;
		}
	}
#endregion

#region Implement Virtual Functions
	public override void Init()
	{
		if(!isInited)
		{
			isPersist = true;

		}
#endregion
	}
}
