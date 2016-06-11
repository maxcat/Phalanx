using UnityEngine;
using UnityEditor;
using System.Collections;

public class FlowEditorWindow : EditorWindow {

#region Fields
	protected Flow targetFlow;
#endregion

#region Static Functions
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FlowEditorWindow));
	}
#endregion

#region Public API
	public void Init(FlowFactory factory)
	{
		targetFlow = factory.CreateFlow();
	}
#endregion

#region OnGUI()
	void OnGUI()
	{
		if(targetFlow != null)
		{
			GUILayout.BeginScrollView(new Vector2(0, 0), true, true);
			targetFlow.Draw();
			GUILayout.EndScrollView();
		}
	}
#endregion
}
