using UnityEngine;
using System.Collections;

public class SampleUIScript : MonoBehaviour {

#region Override
	// Use this for initialization
	IEnumerator Start () {
		yield return initFlow();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Protected Functions
	protected IEnumerator initFlow()
	{
		yield return new WaitForSeconds(1f);	
		PopupService service = PopupService.Instance; 
		service.Init();


		service.ShowDebugLogConsole();
		for(int i = 0; i < 2; i ++)
		{
			service.ShowGeneralPopup(string.Format("Title {0}", i), string.Format("Description {0}", i));
			yield return new WaitForSeconds(.5f);	
		}

		//service.ShowVisibleBlocking();
	}
#endregion
}
