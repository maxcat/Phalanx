using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(TaskCollectionData))]
public class TaskBuilderEditor : Editor {

	#region Override Editor Functions
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();

		TaskCollectionData collection = (TaskCollectionData)target;

		if(GUILayout.Button("Open Editor"))
		{
//			TaskEditorWindow.showWindow();

			TaskEditorWindow window = EditorWindow.GetWindow<TaskEditorWindow>();

			window.init(collection);
		}

	}
	#endregion
}
