using UnityEngine;
using System.Collections;

public class PopupHandler : MonoBehaviour {

#region Fields
	[SerializeField] protected object 		popupData;
	[SerializeField] protected bool 		isPersist;
	[SerializeField] protected FlowFactory 		openAnimationFlowFactory;
	[SerializeField] protected FlowFactory 		closeAnimationFlowFactory;
#endregion

#region Getter and Setter
	public Flow OpenAnimationFlow
	{
		get 
		{
			if(openAnimationFlowFactory == null)
				return null;

			return openAnimationFlowFactory.CreateFlow();
		}
	}

	public Flow CloseAnimationFlow
	{
		get 
		{
			if(closeAnimationFlowFactory == null)
				return null;

			return closeAnimationFlowFactory.CreateFlow();
		}
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Virtual Functions
	public virtual void Init(object data, bool isPersistPopup)
	{
		isPersist = isPersistPopup;
		popupData = data;
	}

	public virtual void OnPopupOnTopIsClosed()
	{
	}

	public virtual void CloseThisPopup()
	{
		Service.Get<PopupService>().CloseTopPopup(isPersist);
	}
#endregion
}
