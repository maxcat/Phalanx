using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleBuilder {

#region Fields
	static List<AssetBundleBuild> 			bundleBuildList;
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
		Debug.LogError("=====list cout is " + bundleBuildList.Count);

		foreach(AssetBundleBuild build in bundleBuildList)
		{
			Debug.LogError("=====item " + build.assetBundleName + " path is " + build.assetNames[0]);
			
		}
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
					List<AssetBundleBuild> buildList = createBundleBuild(files[i]);
					if(buildList != null)
						bundleBuildList.AddRange(buildList);
				}
			}
		}
	}

	static List<AssetBundleBuild> createBundleBuild(string file)
	{
		Object obj = AssetDatabase.LoadAssetAtPath(file, typeof(Object));
		string path = file.Replace('\\', '/');	

		if(obj != null)
		{
			List<AssetBundleBuild> result = new List<AssetBundleBuild>();
			AssetBundleBuild build = new AssetBundleBuild();
			build.assetBundleName = obj.name;
			build.assetNames = new string[]{path};
			result.Add(build);

			Object[] dependList = EditorUtility.CollectDependencies(new Object[]{obj});
			if(dependList != null && dependList.Length > 0)
			{
				for(int i = 0; i < dependList.Length; i ++)
				{
					Object dependObj = dependList[i];	
					if(isAsset(dependObj))
					{
						AssetBundleBuild depBuild = new AssetBundleBuild();	
						depBuild.assetBundleName = dependObj.name;
						depBuild.assetNames = new string[]
						{
							AssetDatabase.GetAssetPath(dependObj)
						};
						result.Add(depBuild);
					}
				}
			}
			return result;
		}
		return null;
	}

	static void appendBundleBuildList(string bundleName, string source)
	{

	}	

	static bool isAsset(Object obj)
	{
		if(typeof(Texture).IsInstanceOfType(obj))	
			return true;

//		if(typeof(Sprite).IsInstanceOfType(obj))
//			return true;

		return false;
	}

#endregion

}
