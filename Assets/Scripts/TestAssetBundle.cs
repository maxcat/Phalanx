using UnityEngine;
using System.Collections;

public class TestAssetBundle : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {

		AssetBundleLoadFlow loadFlow = Service.Get<AssetBundleManager>().CreateLoadingFlow("prefab1");

		yield return loadFlow;
	
		GameObject test  = GameObject.Instantiate(loadFlow.Output) as GameObject;

	}
	
	// Update is called once per frame
	void Update () {

		
	
	}

}
