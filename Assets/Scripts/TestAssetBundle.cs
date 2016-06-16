using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAssetBundle : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		Service.Get<PopupService>().ShowDebugLogConsole();

		yield return Service.Get<AssetBundleManager>().InitAsync();

		List<string> nameList = new List<string>() {"prefab1", "prefab2"};

		List<AssetBundleLoadFlow> loadFlowList = Service.Get<AssetBundleManager>().CreateLoadingFlow(nameList);

		System.DateTime startTime = System.DateTime.Now;
		for(int i = 0; i < loadFlowList.Count; i ++)
		{
			AssetBundleLoadFlow flow = loadFlowList[i];
			yield return flow;
			GameObject test = GameObject.Instantiate(flow.Output) as GameObject;
		}

		double loadTime = (System.DateTime.Now - startTime).TotalSeconds;
		Debug.LogError("=====load flow 1 load time is " + loadTime);
	}
	
	// Update is called once per frame
	void Update () {

		
	
	}

}
