using UnityEngine;

public class TestUserInputManager : MonoBehaviour {

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if(hit.transform.gameObject == this.gameObject)
				{
					gameObject.GetComponent<ClientService>().OnReceiveInput(hit.point);
				}
			}
		}	
	}
#endregion
}
