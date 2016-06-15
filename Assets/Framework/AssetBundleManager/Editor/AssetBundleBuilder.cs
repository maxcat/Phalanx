﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleBuilder {

#region Fields
	static List<AssetBundleBuild> 			bundleBuildList;
	static List<System.Type>			assetTypeList = new List<System.Type>
	{
		typeof(Texture),
		typeof(Sprite),
		typeof(GameObject),
	};
#endregion

#region Static Functions
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
		AssetDatabase.Refresh();
	}

	[MenuItem("Assets/Buid ToBundle")]
	static void BuidlToBundle()
	{
		GatherToBundleBuild();

		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", bundleBuildList.ToArray(), BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget); 

		AssetDatabase.Refresh();
	}
#endregion

#region Private Static Functions
	static void GatherToBundleBuild()
	{
		string toBundleDir = "Assets/Resources/ToBundle";

		bundleBuildList = new List<AssetBundleBuild>();
		if(Directory.Exists(toBundleDir))
		{
			string [] files = Directory.GetFiles(toBundleDir, "*.prefab");
			if(files.Length > 0)
			{
				for(int i = 0; i < files.Length; i ++)
				{
					createBundleBuild(files[i]);
				}
			}
		}
	}

	static bool containsSameBundleBuild(string path)
	{
		for(int i = 0; i < bundleBuildList.Count; i ++)
		{
			AssetBundleBuild build = bundleBuildList[i];
			if(build.assetNames != null && build.assetNames.Length > 0)
			{
				for(int j = 0; j < build.assetNames.Length; j ++)
				{
					if(build.assetNames[j] == path)
						return true;
				}
			}
		}
		return false;
	}

	static void createBundleBuild(string file)
	{
		Object obj = AssetDatabase.LoadAssetAtPath(file, typeof(Object));
		string path = file.Replace('\\', '/');	

		if(obj != null)
		{
			if(!containsSameBundleBuild(path))
			{
				AssetBundleBuild build = new AssetBundleBuild();
				build.assetBundleName = obj.name;
				build.assetNames = new string[]{path};
				bundleBuildList.Add(build);

				Object[] dependList = EditorUtility.CollectDependencies(new Object[]{obj});
				if(dependList != null && dependList.Length > 0)
				{
					for(int i = 0; i < dependList.Length; i ++)
					{
						Object dependObj = dependList[i];	
						string dependPath = AssetDatabase.GetAssetPath(dependObj);
						if(isAsset(dependObj) && !containsSameBundleBuild(dependPath))
						{
							AssetBundleBuild depBuild = new AssetBundleBuild();	
							depBuild.assetBundleName = dependObj.name;
							depBuild.assetNames = new string[]
							{
								AssetDatabase.GetAssetPath(dependObj)
							};
							bundleBuildList.Add(depBuild);
						}
					}
				}

			}
		}
	}

	static bool isAsset(Object obj)
	{
		if(!GameUtilities.IsEmpty(assetTypeList))
		{
			for(int i = 0; i < assetTypeList.Count; i ++)
			{
				if(assetTypeList[i].IsInstanceOfType(obj))
					return true;
			}	
		}
		return false;
	}
#endregion

}
