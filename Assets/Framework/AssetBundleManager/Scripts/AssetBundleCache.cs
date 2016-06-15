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
	protected bool 				isCached = false;
	protected bool 				isCaching = false;
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

	public bool IsCaching
	{
		get { return isCaching; }
	}

	public long BundleSize
	{
		get { return bundleSize; }
	}

	public double CachingTime
	{
		get { return cachingTime; }
	}
#endregion

#region Constructor
	public AssetBundleCache(string name)
	{
		bundleName = name;
	}
#endregion

#region Public API
	public void Aquire()
	{
		refCount ++;	
	}

	public void Release()
	{
		if(refCount == 1)
		{
			assetBundle = null;
			isCached = false;
		}
		refCount --;

		if(refCount < 0)
			refCount = 0;
	}

	public bool Init()
	{
		if(File.Exists(FilePath))
		{
			FileInfo info = new FileInfo(FilePath);
			bundleSize = info.Length;
			return true;
		}
		else
		{
			Debug.LogError("[ERROR]AssetBundleCache->Init: can't find file " + FilePath);
			return false;
		}
	}

	public void LoadBundleToCache()
	{
		if(isCached || isCaching)
			return;

		isCaching = true;
		string filePath = FilePath;
		System.DateTime startTime = System.DateTime.Now;
		assetBundle = AssetBundle.LoadFromFile(filePath);	
		System.DateTime endTime = System.DateTime.Now;

		cachingTime = 0;
		if(assetBundle == null)
		{
			Debug.LogError("[ERROR]AssetBundleCache->LoadBundlToCache: can't find bundle at " + filePath);
			isCaching = false;
			return;
		}

		cachingTime = (endTime - startTime).TotalSeconds;

		isCaching = false;
		isCached = true;
		Debug.Log("[INFO]AssetBundleCache->LoadBundlToCache: Load asset bundle from " + bundleName + " file size is " + bundleSize + " caching time is " + cachingTime);
	}

	public IEnumerator LoadBundleToCacheEnumerator()
	{
		if(isCaching || isCached)
			yield break;

		isCaching = true;
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
			isCaching = false;
			yield break;
		}

		cachingTime = (endTime - startTime).TotalSeconds;

		isCached = true;
		isCaching = false;
		Debug.Log("[INFO]AssetBundleCache->LoadBundleToCacheEnumerator: Load asset bundle from " + bundleName + " file size is " + bundleSize + " caching time is " + cachingTime);
	}
#endregion
}
