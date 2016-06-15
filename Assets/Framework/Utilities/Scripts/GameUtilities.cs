using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

#region Collection Check
	public static bool IsEmpty(ICollection collection)
	{
		return collection == null || collection.Count <= 0;
	}

	public static bool IsEmpty(Object[] array)
	{
		return array == null || array.Length <= 0;
	}
#endregion

}
