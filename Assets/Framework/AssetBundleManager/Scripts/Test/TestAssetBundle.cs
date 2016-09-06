﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAssetBundle : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return AssetBundleManager.Instance.InitAsync();

		PopupService.Instance.ShowAssetBundleDebugPopup();

		yield return null;

		List<string> nameList = new List<string>() {"prefab2","prefab1" };

		List<AssetBundleLoadFlow> loadFlowList = AssetBundleManager.Instance.CreateLoadingFlow(nameList);

		System.DateTime startTime = System.DateTime.Now;
		for(int i = 0; i < loadFlowList.Count; i ++)
		{
			AssetBundleLoadFlow flow = loadFlowList[i];
			yield return flow;
			GameObject test = GameObject.Instantiate(flow.Output) as GameObject;
			test.name = flow.Output.name;
		}

		double loadTime = (System.DateTime.Now - startTime).TotalSeconds;
		Debug.LogError("=====load flow 1 load time is " + loadTime);

		for(int i = 0; i < loadFlowList.Count; i ++)
		{
			yield return new WaitForSeconds(3);
			loadFlowList[i].Unload(true);;
		}
	}

	// Update is called once per frame
	void Update () {



	}

}
