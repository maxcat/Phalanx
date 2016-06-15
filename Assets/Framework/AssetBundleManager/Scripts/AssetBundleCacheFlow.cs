using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallCacheCollection
{
#region Fields 
	protected List<AssetBundleCache> 		cacheList;
	protected long 					spaceLeft;
#endregion

#region Getter and Setter
	public List<AssetBundleCache> CacheList
	{
		get { return cacheList; }
	}

	public long SpaceLeft
	{
		get { return spaceLeft; }
	} 
	
	public int Count
	{
		get { return cacheList.Count; }
	}
#endregion

#region Constructor
	public SmallCacheCollection(long threadhold)
	{
		spaceLeft = threadhold;
		cacheList = new List<AssetBundleCache>();
	}
#endregion

#region API
	public void AddCache(AssetBundleCache cache)
	{
		cacheList.Add(cache);
		spaceLeft -= cache.BundleSize;		
	}
#endregion
}

public class AssetBundleCacheFlow : Flow {

#region Delegate
	public delegate void OnCachingSizeChanged(long cached, long totalSize);
#endregion

#region Fields
	protected long 				bundleSizeThreadhold;
	protected long 				cacheLoaded;
	protected long 				totalSpaceNeeded;
	List<AssetBundleCache>			cacheList;
	protected OnCachingSizeChanged 		callBack;
#endregion

#region Constructor
	public AssetBundleCacheFlow(List<AssetBundleCache> cacheList, long threadhold, OnCachingSizeChanged func = null) : base ()
	{
		this.cacheList = cacheList;
		this.bundleSizeThreadhold = threadhold;
		this.totalSpaceNeeded = 0;
		this.cacheLoaded = 0;
		this.callBack = func;

		if(GameUtilities.IsEmpty(cacheList))
		{
			Debug.LogWarning("[WARNING]AssetBundleCacheFlow: cache list is empty,");
		}
		else
		{
			for(int i = 0; i < this.cacheList.Count; i ++)
			{
				totalSpaceNeeded += this.cacheList[i].BundleSize;
			}

		}
	}
#endregion

#region Sub Flows
	protected IEnumerator smallBundleCacheFlow(SmallCacheCollection collection)
	{
		for(int i = 0; i < collection.CacheList.Count; i ++)
		{
			collection.CacheList[i].LoadBundleToCache();
		}	
		yield return null;
	}

	protected override IEnumerator main()
	{
		cacheList.Sort((x, y) => x.BundleSize.CompareTo(y.BundleSize));

		int index = 0;
		List<SmallCacheCollection> smallPackList = new List<SmallCacheCollection>();

		while(index < cacheList.Count)
		{
			AssetBundleCache cache = cacheList[index];	

			if(cache.BundleSize >= bundleSizeThreadhold)
			{
				cache.Aquire();
				index ++;
				cacheLoaded += cache.BundleSize;
				if(!cache.IsCached && !cache.IsCaching)
				{
					yield return cache.LoadBundleToCacheEnumerator();
				}
				if(callBack != null)
					callBack(cacheLoaded, totalSpaceNeeded);
			}
			else 
			{
				SmallCacheCollection collection = new SmallCacheCollection(bundleSizeThreadhold);

				long chunkSize = 0;
				while(index < cacheList.Count && cache.BundleSize <= collection.SpaceLeft)
				{
					cache = cacheList[index];
					cache.Aquire();
					cacheLoaded += cache.BundleSize;
					if(!cache.IsCached && !cache.IsCaching)
					{
						collection.AddCache(cache);
					}
					index ++;
				}
				if(collection.Count > 0)
				{
					yield return smallBundleCacheFlow(collection); 
				}
				if(callBack != null)
					callBack(cacheLoaded, totalSpaceNeeded);
			}
		}
	}
#endregion

}
