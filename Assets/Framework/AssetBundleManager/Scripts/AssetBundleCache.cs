using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleCache
{
#region Fields
	protected int 				refCount;
	protected AssetBundle 			assetBundle;
	protected string 			bundleName;
	protected List<AssetBundleCache> 	dependencies;
	protected bool 				isCached = false;
	protected long 				bundleSize;
	protected double 			cachingTime;
#endregion

#region Getter and Setter
	public AssetBundle Bundle
	{
		get { return assetBundle; }
	}

	public string BundleName
	{
		get { return bundleName; }
	}

	public string FilePath
	{
		get { return GameUtilities.AssetBundleCachePath + "/" + bundleName; }
	}

	public bool IsCached
	{
		get { return isCached; }
	}

	public long BundleSize
	{
		get { return bundleSize; }
	}

	public double CachingTime
	{
		get { return cachingTime; }
	}

	public List<AssetBundleCache> Dependencies
	{
		get { return Dependencies; }
	}
#endregion

#region Constructor
	public AssetBundleCache(string name)
	{
		bundleName = name;
	}
#endregion

#region Public API
	public void LoadBundleToCache()
	{
		string filePath = FilePath;
		System.DateTime startTime = System.DateTime.Now;
		assetBundle = AssetBundle.LoadFromFile(filePath);	
		System.DateTime endTime = System.DateTime.Now;

		cachingTime = 0;
		if(assetBundle == null)
		{
			Debug.LogError("[ERROR]AssetBundleCache->LoadBundlToCache: can't find bundle at " + filePath);
			return;
		}

		FileInfo fileInfo = new FileInfo(filePath);
		bundleSize = fileInfo.Length;
		cachingTime = (endTime - startTime).TotalSeconds;

		isCached = true;
		Debug.Log("[INFO]AssetBundleCache->LoadBundlToCache: Load asset bundle from " + bundleName + " file size is " + bundleSize + " caching time is " + cachingTime);
	}

	public IEnumerator LoadBundleToCacheEnumerator()
	{
		string filePath = FilePath;
		AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(filePath);
		System.DateTime startTime = System.DateTime.Now;

		yield return request;
		System.DateTime endTime = System.DateTime.Now;

		cachingTime = 0;

		assetBundle = request.assetBundle;
		if(assetBundle == null)
		{
			Debug.LogError("[ERROR]AssetBundleCache->LoadBundleToCacheEnumerator: can't find bundle at " + filePath);
			yield break;
		}

		FileInfo fileInfo = new FileInfo(filePath);
		bundleSize = fileInfo.Length;
		cachingTime = (endTime - startTime).TotalSeconds;

		isCached = true;
		Debug.Log("[INFO]AssetBundleCache->LoadBundleToCacheEnumerator: Load asset bundle from " + bundleName + " file size is " + bundleSize + " caching time is " + cachingTime);
	}
#endregion
}
