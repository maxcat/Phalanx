using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public partial class PopupService {

#region Fields
	protected string 			debugLogConsoleResourcePath = "Popup/DebugLogConsole";
#endregion

#region Popup Builder
	public void ShowDebugLogConsole()
	{
		GameObject prefab = Resources.Load(debugLogConsoleResourcePath) as GameObject;
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		addPopup(instance, null, true);
		instance.GetComponent<PopupHandler>().Init(null, true);
	}
#endregion
}

public class LogData
{
#region Fields
	public string 							LogString;
	public string 							StackTrace;
	public LogType 							Type;
#endregion

#region Constructor
	public LogData(string logString, string stackTrace, LogType type)
	{
		LogString = logString;
		StackTrace = stackTrace;
		Type = type;	
	}
#endregion

#region Override Functions
	public override string ToString()
	{
		switch(Type)
		{
			case LogType.Log:
				return "<color=#ffffffff>" + StackTrace + "</color>";
			case LogType.Warning:
				return "<color=#ffff00ff>" + StackTrace + "</color>";
			default:
				return "<color=#ff0000ff>" + StackTrace + "</color>";

		}
	}
#endregion
}

public class DebugLogConsoleHandler : PopupHandler {

#region Fields
	[SerializeField] protected Text 				label;
	[SerializeField] protected int 					maxLogCount = 9999;
	[SerializeField] protected ScrollRect				scrollRect;
	protected bool 							isLocked = true;
	[SerializeField] protected bool					infoEnabled = true;
	[SerializeField] protected bool 				warningEnabled = true;
	[SerializeField] protected bool 				errorEnabled = true;
	[SerializeField] protected bool 				isRunningInBackground = false;
	[SerializeField] protected GameObject 				backgroundRunningObj;
	
	protected List<LogData>						logDataList;
#endregion

#region Override  MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isLocked)
			scrollRect.normalizedPosition = Vector2.zero;
	}

	void OnEnable () {
		Application.logMessageReceived += onLogMessageReceived;
	}

	void OnDisable () {
		Application.logMessageReceived -= onLogMessageReceived;
	}
#endregion

#region Protected Functions
	protected void showLog()
	{
		if(logDataList == null)
			return;
		string result = string.Empty; 
		for(int i = 0; i < logDataList.Count; i ++)
		{
			LogData data = logDataList[i];

			switch(data.Type)
			{
				case LogType.Log:
					if(infoEnabled)
						result += data.ToString();
					break;
				case LogType.Warning:
					if(warningEnabled)
						result += data.ToString();
					break;
				default:
					if(errorEnabled)
						result += data.ToString();
					break;
			}
		}
		if(string.IsNullOrEmpty(result))
			label.text = " ";
		else
			label.text = result;
	}
#endregion

#region Log Event Handler
	protected void onLogMessageReceived(string logString, string stackTrace, LogType type)
	{
		if(logDataList == null)
			logDataList = new List<LogData>();

		if(logDataList.Count >= maxLogCount)
			logDataList.RemoveAt(0);

		logDataList.Add(new LogData(logString, stackTrace, type));

		showLog();
	}

#endregion

#region UI Event Handler
	public void OnClicked()
	{
		Debug.Log("DebugLogConsoleHandler clicked");
		Debug.LogWarning("DebugLogConsoleHandler clicked");
		Debug.LogError("DebugLogConsoleHandler clicked");
	}

	public void OnEnableError(bool enabled)
	{
		errorEnabled = enabled;		
		showLog();
	}

	public void OnEnableWarning(bool enabled)
	{
		warningEnabled = enabled;
		showLog();
	}

	public void OnEnableInfo(bool enabled)
	{
		infoEnabled = enabled;
		showLog();
	}

	public void OnLock(bool enabled)
	{
		isLocked = enabled;
	}

	public void OnClearClicked()
	{
		if(logDataList != null)
		{
			logDataList.Clear();
			showLog();
		}
	}

	public void OnCloseClicked()
	{
		CloseThisPopup();
	}

	public void OnRunningInBackground(bool enabled)
	{
		backgroundRunningObj.GetComponent<GraphicRaycaster>().enabled = !enabled;
		foreach(Selectable selectable in backgroundRunningObj.GetComponentsInChildren<Selectable>())
		{
			selectable.interactable = !enabled;
		}	
	}
#endregion
}
