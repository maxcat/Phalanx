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

#region Collection Functions 
	public static bool IsEmpty(ICollection collection)
	{
		return collection == null || collection.Count <= 0;
	}

	public static bool IsEmpty(Object[] array)
	{
		return array == null || array.Length <= 0;
	}

	public static List<T> Clone<T>(this List<T> list)
	{
		if(list == null)
			return null;

		List<T> result = new List<T>();
		for(int i = 0; i < list.Count; i ++)
		{
			result.Add(list[i]);
		}
		return result;
	}

	public static void AddRangeUnique<T>(this List<T> list, List<T> input)
	{
		if(input == null)
			return;

		for(int i = 0; i < input.Count; i ++)	
		{
			T element = input[i];
			if(!list.Contains(element))
				list.Add(element);
		}
	}

	public static void AddUnique<T>(this List<T> list, T input)
	{
		if(!list.Contains(input))
			list.Add(input);
	}

	public static T GetComponentHungry<T>(this GameObject obj) where T : Component 
	{
		if(obj.GetComponent<T>() == null)
			return obj.AddComponent<T>();
		else
			return obj.GetComponent<T>();

	}

#endregion

}
