using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetBundleSummary {

#region Fields
	protected AssetBundleManifest 				manifest;
#endregion

#region Virtual Functions
	public virtual void Init()
	{
		string filePath = GameUtilities.AssetBundleCachePath + "/StreamingAssets";
		byte[] stream = File.ReadAllBytes(filePath);
		if(GameUtilities.IsEmpty(stream))
			Debug.LogError("[ERROR]AssetBundleSummary->Init: can't find " + filePath);

		AssetBundle main = AssetBundle.LoadFromMemory(stream);
		if(main == null)
			Debug.LogError("[ERROR]AssetBundleSummary->Init: can't load bundle from " + filePath);
		manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
	}

	public virtual IEnumerator InitAsync()
	{
		string filePath = GameUtilities.AssetBundleCachePath + "/StreamingAssets";
		byte[] stream = File.ReadAllBytes(filePath);
		if(GameUtilities.IsEmpty(stream))
			Debug.LogError("[ERROR]AssetBundleSummary->InitAsync: can't find " + filePath);

		AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(stream);
		yield return request;

		AssetBundle main = request.assetBundle;
		if(main == null)
			Debug.LogError("[ERROR]AssetBundleSummary->InitAsync: can't load bundle from " + filePath);
		manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
	}

	public virtual string[] GetAllAssetBundles()
	{
		return manifest.GetAllAssetBundles();
	}

	public virtual string[] GetAllAssetBundlesWithVariant()
	{
		return manifest.GetAllAssetBundlesWithVariant();
	}

	public virtual string[] GetAllDependencies(string bundleName)
	{
		return manifest.GetAllDependencies(bundleName);
	}

	public virtual string[] GetDirectDependencies(string bundleName)
	{
		return manifest.GetDirectDependencies(bundleName);
	}

	public virtual Hash128 GetAssetBundleHash(string bundleName)
	{
		return manifest.GetAssetBundleHash(bundleName);
	}
#endregion
}
