using UnityEngine;
using System.Collections;

public class TestAssetBundle : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {

		AssetBundleLoadTask task = Service.Get<AssetBundleManager>().CreateLoadingTask("prefab1");

		task.Start();

		yield return task.UntilDone;
	
		GameObject test  = GameObject.Instantiate(task.Output) as GameObject;


	}
	
	// Update is called once per frame
	void Update () {

		
	
	}

}
