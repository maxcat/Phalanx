using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleManager : Singleton {

#region Static Fields
	static GameObject 				parentObj;
	static AssetBundleManager 			instance;
#endregion
	
#region Static Getter and Setter
	public static AssetBundleManager Instance
	{
		get {
			if (parentObj == null)
			{
				parentObj = new GameObject("AssetBundleManager");
				DontDestroyOnLoad(parentObj);
				instance = parentObj.AddComponent<AssetBundleManager>();
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

#region Fields
	[SerializeField] protected long 			bundleSizeThreadhold = 500000;
	[SerializeField] protected int 				cacheInitCountPerFrame = 100;
	protected Dictionary<string, AssetBundleCache>		cachePool;
	protected AssetBundleSummary	 			summary;
#endregion

#region Getter and Setter
	public List<AssetBundleCache> CacheList
	{
		get
		{
			List<AssetBundleCache> result = new List<AssetBundleCache>();
			if(!GameUtilities.IsEmpty(cachePool))
			{
				foreach(string key in cachePool.Keys)
				{
					result.Add(cachePool[key]);
				}
			}
			return result;
		}
	}
#endregion

#region Public API
	public AssetBundleLoadFlow CreateLoadingFlow(string name)
	{
		AssetBundleCache cache = getCache(name);
		if(cache == null)
			return null;
		AssetBundleCacheFlow cacheFlow = new AssetBundleCacheFlow(cache.CacheList, bundleSizeThreadhold);
		return new AssetBundleLoadFlow(cache, cacheFlow);
	}

	public List<AssetBundleLoadFlow> CreateLoadingFlow(List<string> nameList)
	{
		if(GameUtilities.IsEmpty(nameList))
			return null;
		AssetBundleCacheFlow cacheFlow = new AssetBundleCacheFlow(generateCachingList(nameList), bundleSizeThreadhold);
		List<AssetBundleLoadFlow> result = new List<AssetBundleLoadFlow>();
		
		for(int i = 0; i < nameList.Count; i ++)
		{
			AssetBundleCache cache = getCache(nameList[i]);
			if(cache == null)
				continue;

			if(i == 0)
			{
				result.Add(new AssetBundleLoadFlow(cache, cacheFlow));
			}	
			else
			{
				result.Add(new AssetBundleLoadFlow(cache));
			}
		}
		return result;
	}

	public IEnumerator InitAsync()
	{
		summary = new AssetBundleSummary();
		yield return summary.InitAsync();

		string[] bundleArray = summary.GetAllAssetBundles();
		cachePool = new Dictionary<string, AssetBundleCache>();

		if(!GameUtilities.IsEmpty(bundleArray))
		{
			for(int i = 0; i < bundleArray.Length; i ++)
			{
				AssetBundleCache cache = new AssetBundleCache(bundleArray[i]);
				if(cache.Init())
				{
					cachePool.Add(cache.BundleName, cache);
				}
			}	

			int index = 0;
			foreach(AssetBundleCache cache in cachePool.Values)
			{
				initDependencies(cache);
				if(index % cacheInitCountPerFrame == cacheInitCountPerFrame - 1)
				{
					yield return null;
				}
			}
		}
	}
#endregion

#region Protected Functions
	protected void initDependencies(AssetBundleCache cache)
	{
		string[] dependencyArray = summary.GetAllDependencies(cache.BundleName);
		if(!GameUtilities.IsEmpty(dependencyArray))
		{
			for(int i = 0; i < dependencyArray.Length; i ++)
			{
				AssetBundleCache dependCache = getCache(dependencyArray[i]);
				if(dependCache != null)
				{
					if(!dependCache.IsDependenciesInited)
					{
						initDependencies(dependCache);
					}
					cache.Dependencies.AddUnique(dependCache);
					cache.Dependencies.AddRangeUnique(dependCache.Dependencies);
				}
			}
		}

		cache.IsDependenciesInited = true;	
	}

	protected List<AssetBundleCache> generateCachingList(List<string> nameList)
	{
		if(GameUtilities.IsEmpty(nameList))
			return null;		

		List<AssetBundleCache> result = new List<AssetBundleCache>();
		for(int i = 0; i < nameList.Count; i ++)
		{
			AssetBundleCache cache = getCache(nameList[i]);

			if(cache != null && cache.CacheList != null)
				result.AddRange(cache.CacheList);
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
}
