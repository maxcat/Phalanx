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
			if(!flowFactory.IsMonitoringFlowStarted)
			{
				if(GUILayout.Button("Run Flow"))
				{
					flowFactory.OnRunFlowButtonClicked();
				}
			}

			if(flowFactory.IsMonitoringFlowStarted)
			{
				if(flowFactory.IsMonitoringFlowPaused)
				{
					if(GUILayout.Button("Resume Flow"))
					{
						flowFactory.OnResumeButtonClicked();
					}
				}
				else
				{
					if(GUILayout.Button("Pause Flow"))
					{
						flowFactory.OnPauseButtonClicked();
					}
				}
			}
		}
	}
#endregion

}
