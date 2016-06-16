using UnityEngine;
using System.Collections;

public class TestBundleController : MonoBehaviour {

	[SerializeField] protected GameObject 			prefab;
	// Use this for initialization
	void Start () {

		GameObject test = GameObject.Instantiate(prefab) as GameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
