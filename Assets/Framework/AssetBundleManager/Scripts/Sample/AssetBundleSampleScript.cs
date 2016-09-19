using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleSampleScript : MonoBehaviour {

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {
		// 1. init the asset bundle manager
		yield return AssetBundleManager.Instance.InitAsync();

		// 2. display the debug popup of assetbundle sample, comment it to disable popup.
		PopupService.Instance.ShowAssetBundleDebugPopup();

		yield return null;

		// 3. create the bundle load flow list with bundle name.
		List<string> nameList = new List<string>() {"prefab2","prefab1" };
		List<AssetBundleLoadFlow> loadFlowList = AssetBundleManager.Instance.CreateLoadingFlow(nameList);

		// 4. run the bundle load flow. 
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

		// 5. unload the bundles.
		for(int i = 0; i < loadFlowList.Count; i ++)
		{
			yield return new WaitForSeconds(3);
			loadFlowList[i].Unload(true);;
		}
	}

	// Update is called once per frame
	void Update () {



	}
#endregion

}
