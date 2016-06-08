using UnityEditor;

public class AssetBundleTool {
	
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
		AssetDatabase.Refresh();
	}

}
