using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// use partial class so that users can define their own factory function in individual popup handler.
public partial class PopupService : Singleton {

#region Fields
	[SerializeField] protected int 		popupSortOrder = 0x7fff;
	protected GameObject 			basePopupUI;
	protected GameObject 			persistPopupUI;
	protected List<GameObject>		popupStack;
	protected List<GameObject> 		persistPopupStack;
#endregion

#region Getter 
	public GameObject BasePopupUI
	{
		get 
		{
			if(basePopupUI == null)
			{
				basePopupUI = new GameObject("BasePopupUI");
				basePopupUI.transform.position = Vector3.zero;
				basePopupUI.transform.localScale = Vector3.one;
				Canvas rootCanvas = basePopupUI.AddComponent<Canvas>();
				rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
				rootCanvas.sortingOrder = popupSortOrder - 1;
			}
			return basePopupUI;
		}
	}

	public GameObject PersistPopupUI
	{
		get 
		{
			if(persistPopupUI == null)
			{
				persistPopupUI = new GameObject("PersistPopupUI");
				DontDestroyOnLoad(persistPopupUI);
				persistPopupUI.transform.position = Vector3.zero;
				persistPopupUI.transform.localScale = Vector3.one;
				Canvas persistCanvas = persistPopupUI.AddComponent<Canvas>();
				persistCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
				persistCanvas.sortingOrder = popupSortOrder;
			}
			return persistPopupUI;
		}
	}
#endregion

#region Protected Functions
	protected List<GameObject> getStack(bool isPersistPopup)
	{
		return isPersistPopup ? persistPopupStack : popupStack;	
	}

	protected int getTailIndex(bool isPersistPopup)
	{
		if(isPersist)
			return persistPopupStack == null ? 0 : persistPopupStack.Count;
		else
			return popupStack == null ? 0 : popupStack.Count;
	}

	protected IEnumerator destroyFlow(GameObject popupObj, bool isPersistPopup)
	{
		List<GameObject> stack = getStack(isPersistPopup);

		GameObject.Destroy(popupObj);
		if(stack.Count > 0)
		{
			PopupHandler parenHandler = stack[stack.Count - 1].GetComponent<PopupHandler>();

			if(parenHandler != null)
				parenHandler.OnPopupOnTopIsClosed();
		}
		yield return null;
	}

	protected void addPopup(GameObject popupObj, object data, bool isPersistPopup)
	{
		if(popupObj == null)
		{
			Debug.LogError("[ERROR]PopupService->addPopup: popup object can't be null.");
			return;
		}

		PopupHandler handler = popupObj.GetComponent<PopupHandler>();

		if(handler == null)
		{
			Debug.LogError("[ERROR]PopupService->addPopup: popup object must contain PopupHandler component.");
			return;
		}

		if(isPersistPopup)
		{
			popupObj.transform.SetParent(PersistPopupUI.transform);
			persistPopupStack.Add(popupObj);
		}
		else
		{
			popupObj.transform.SetParent(BasePopupUI.transform);
			popupStack.Add(popupObj);
		}
		popupObj.transform.SetSiblingIndex(getTailIndex(isPersistPopup));

		handler.Init(data, false);
		Task animationTask = handler.openAnimationTask;

		if(animationTask != null)
		{
			SeriesTasks mainTask = new SeriesTasks(this);
			mainTask.AddTask(ShowInvisibleBlockingFlow());
			mainTask.AddTask(animationTask);
			mainTask.AddTask(CloseInvisibleBlockingFlow());
			mainTask.Start();
		}
	}
#endregion

#region Public API
	public void CloseTopPopup(bool isPersistPopup)
	{
		List<GameObject> stack = getStack(isPersistPopup);

		if(stack != null && stack.Count > 0)
		{
			GameObject popupObj = stack[stack.Count - 1];
			stack.RemoveAt(stack.Count - 1);
			SeriesTasks mainTask = new SeriesTasks(this);

			Task animationTask = popupObj.GetComponent<PopupHandler>().closeAnimationTask;

			if(animationTask != null)
			{
				mainTask.AddTask(ShowInvisibleBlockingFlow());
				mainTask.AddTask(animationTask);
			}
			mainTask.AddTask(destroyFlow(popupObj, isPersistPopup));
			if(animationTask != null)
				mainTask.AddTask(CloseInvisibleBlockingFlow());

			mainTask.Start();
		}
	}

	public void ClearPersistPopup()
	{
		if(persistPopupStack != null)
		{
			for (int i = 0; i < persistPopupStack.Count; i ++)
				GameObject.Destroy(persistPopupStack[i]);
		}

		persistPopupStack.Clear();
	}
#endregion

#region Implement Virtual Functions
	public override void Init()
	{
		if(!isInited)
		{
			isPersist = true;
			popupStack = new List<GameObject>();
			persistPopupStack = new List<GameObject>();
			isInited = true;
		}
	}

	public override void OnSceneChanged(int level)
	{
		popupStack.Clear();	
	}

#endregion
}
