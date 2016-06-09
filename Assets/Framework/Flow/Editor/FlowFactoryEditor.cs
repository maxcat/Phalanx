using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FlowFactory), true)]
public class FlowFactoryEditor : Editor {

#region Override Editor Functions
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		FlowFactory flowFactory = (FlowFactory) target;

//		if(GUILayout.Button("Open Editor"))
//		{
//			TaskEditorWindow window = EditorWindow.GetWindow<TaskEditorWindow>();
//
//			window.init(taskFactory);
//		}

		if(Application.isPlaying)
		{
			if(GUILayout.Button("Run Flow"))
			{
				flowFactory.OnRunFlowButtonClicked();
			}
		}
	}
#endregion

}
