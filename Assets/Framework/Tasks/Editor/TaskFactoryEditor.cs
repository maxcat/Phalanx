using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TaskFactory), true)]
public class TaskFactoryEditor : Editor {

	#region Override Editor Functions
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		TaskFactory taskFactory = (TaskFactory) target;

		if(GUILayout.Button("Open Editor"))
		{
			TaskEditorWindow window = EditorWindow.GetWindow<TaskEditorWindow>();

			window.init(taskFactory);
		}

		if(Application.isPlaying)
		{
			if(GUILayout.Button("Run Task"))
			{
				taskFactory.OnRunTaskButtonClicked();
			}
		}

	}
	#endregion
}
