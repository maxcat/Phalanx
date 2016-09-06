using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// use partial class so that users can define their own factory function in individual popup handler.
public partial class PopupService : Singleton {

#region Static Fields
	static GameObject 			parentObj;
	static PopupService 			instance;
#endregion
	
#region Static Getter and Setter
	public static PopupService Instance
	{
		get {
			if (parentObj == null)
			{
				parentObj = new GameObject("PopupService");
				DontDestroyOnLoad(parentObj);
				instance = parentObj.AddComponent<PopupService>();
				instance.Init();
			}
			return instance;
		}
	}
#endregion
	
#region Static Functions
	public static void Remove()
	{
		instance.clear();
		Destroy(parentObj);
	}
#endregion

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
		Flow animationFlow = handler.OpenAnimationFlow;

		if(animationFlow != null)
		{
			SequentialFlow mainFlow = new SequentialFlow();
			mainFlow.Add(ShowInvisibleBlockingFlow());
			mainFlow.Add(animationFlow);
			mainFlow.Add(CloseInvisibleBlockingFlow());
			mainFlow.Start(this);
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

			SequentialFlow mainFlow = new SequentialFlow();
			Flow animationFlow = popupObj.GetComponent<PopupHandler>().CloseAnimationFlow;
			if(animationFlow != null)
			{
				mainFlow.Add(ShowInvisibleBlockingFlow());
				mainFlow.Add(animationFlow);
			}
			mainFlow.Add(destroyFlow(popupObj, isPersistPopup));
			if(animationFlow != null)
				mainFlow.Add(CloseInvisibleBlockingFlow());

			mainFlow.Start(this);
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
	protected override void init()
	{
		popupStack = new List<GameObject>();
		persistPopupStack = new List<GameObject>();
	}

	protected override void clear()
	{
		popupStack.Clear();	
	}
#endregion

#region Override MonoBehaviour
	void OnLevelWasLoaded(int level)
	{
		clear();
	}
#endregion
}
