using UnityEngine;
using System.Collections;

public static partial class GameUtilities {

#region File Path
	public static string AssetBundleCachePath
	{
		get
		{
			return Application.streamingAssetsPath;
		}
	}
#endregion

}
