using UnityEngine;
using System.Collections;

public class PopupData
{
}

public class PopupHandler : MonoBehaviour {

#region Fields
	[SerializeField] protected PopupData 		popupData;
	[SerializeField] protected bool 		isPersist;
	[SerializeField] protected TaskFactory		openAnimationTaskFactory;
	[SerializeField] protected TaskFactory 		closeAnimationTaskFactory;
#endregion

#region Getter and Setter
	public Task openAnimationTask
	{
		get 
		{
			if(openAnimationTaskFactory == null)
				return null;

			return openAnimationTaskFactory.CreateTask();
		}
	}

	public Task closeAnimationTask
	{
		get 
		{
			if(closeAnimationTaskFactory == null)
				return null;

			return closeAnimationTaskFactory.CreateTask();
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
	public virtual void Init(PopupData data, bool isPersistPopup)
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
