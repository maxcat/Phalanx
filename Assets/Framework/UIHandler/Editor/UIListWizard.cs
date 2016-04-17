using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public enum LIST_TYPE
{
	VERTICAL,
	HORIZONTAL,
}

public class UIListWizard : EditorWindow {

	#region Fields
	protected GameObject							selectedObject;
	protected string 							listName = "CustomUIList";
	protected LIST_TYPE							listType = LIST_TYPE.VERTICAL;
	protected int								subItemCountPerItem = 2;
	protected bool								multipleItemPerRowOrCol = false;
	protected ScrollRect 							scrollRect;
	protected bool 								hasTemplate;
	protected GameObject 							templateObject;
	#endregion

	#region Static Functions
	[MenuItem("GameObject/UI/Open Custom List Builder")]
	public static void OpenListBuilder()
	{
		 EditorWindow.GetWindow<UIListWizard>();
	}
	#endregion

	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnSelectionChange () {

		updateSelectedObject();
		Repaint(); 
	}

	void OnGUI () {

		drawScrollViewInfo();

		drawParentFields();	

		drawTemplateFields();

		drawListSetting();

		drawCreateButton();
	}
	#endregion

	#region Protected Functions
	protected void drawScrollViewInfo()
	{
		string infoString = "Please select a Scroll View";
		if(scrollRect == null)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(infoString);
			GUILayout.EndHorizontal();
		}	
	}

	protected void drawParentFields()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Parent Object");
		selectedObject = EditorGUILayout.ObjectField(selectedObject, typeof(GameObject), true) as GameObject;
		GUILayout.EndHorizontal();
	}

	protected void drawTemplateFields()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Has Template");
		hasTemplate = EditorGUILayout.Toggle(hasTemplate);
		GUILayout.EndHorizontal();

		if(hasTemplate)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Template");
			templateObject = EditorGUILayout.ObjectField(templateObject, typeof(GameObject), true) as GameObject;
			GUILayout.EndHorizontal();
		}
	}

	protected void drawCreateButton()
	{
		if(scrollRect != null)
		{
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Create List"))
			{
				createList();
			}
			GUILayout.EndHorizontal();
		}
	}

	protected void drawListSetting()
	{
		GUILayout.BeginHorizontal();
		listType = (LIST_TYPE)EditorGUILayout.EnumPopup("List Type", listType);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if(listType == LIST_TYPE.HORIZONTAL)
			GUILayout.Label("Multiple Item Per Column");
		else
			GUILayout.Label("Multiple Item Per Row");
		multipleItemPerRowOrCol = EditorGUILayout.Toggle(multipleItemPerRowOrCol);
		GUILayout.EndHorizontal();

		if(multipleItemPerRowOrCol)
		{
			GUILayout.BeginHorizontal();
			if(listType == LIST_TYPE.HORIZONTAL)
				GUILayout.Label("items Count Per Column");
			else
				GUILayout.Label("items Count Per Row");
			subItemCountPerItem = EditorGUILayout.IntField(subItemCountPerItem);
			GUILayout.EndHorizontal();
		}
	}

	protected void createList()
	{
		scrollRect.horizontal = listType == LIST_TYPE.HORIZONTAL;
		scrollRect.vertical = listType == LIST_TYPE.VERTICAL;

		UIListHandler listHandler = scrollRect.GetComponent<UIListHandler>();
		if(listHandler == null)
			listHandler = scrollRect.gameObject.AddComponent<UIListHandler>();

		Transform contentTrans = scrollRect.content;

		if(contentTrans == null)
		{
			Debug.LogError("Cannot find the Content GameObject");
			return;
		}

		GameObject contentObj = contentTrans.gameObject;
		var fitter = contentObj.GetComponent<ContentSizeFitter>();
		if(fitter == null)
			fitter = contentObj.AddComponent<ContentSizeFitter>();

		fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		if(listType == LIST_TYPE.HORIZONTAL)
		{
			var verticalGroup = contentObj.GetComponent<VerticalLayoutGroup>();
			if(verticalGroup != null)
				GameObject.DestroyImmediate(verticalGroup);

			var horizontalGroup = contentObj.GetComponent<HorizontalLayoutGroup>();
			if(horizontalGroup == null)
				horizontalGroup = contentObj.AddComponent<HorizontalLayoutGroup>();
			horizontalGroup.childForceExpandWidth = false;
			horizontalGroup.childForceExpandHeight = true;
		}
		else
		{
			var horizontalGroup = contentObj.GetComponent<HorizontalLayoutGroup>();
			if(horizontalGroup != null)
				GameObject.DestroyImmediate(horizontalGroup);

			var verticalGroup = contentObj.GetComponent<VerticalLayoutGroup>();
			if(verticalGroup == null)
				verticalGroup = contentObj.AddComponent<VerticalLayoutGroup>();
			verticalGroup.childForceExpandHeight = false;
			verticalGroup.childForceExpandWidth = true;
		}

		if(listHandler.ItemTemplate != null)
		{
			GameObject.DestroyImmediate(listHandler.ItemTemplate);
			listHandler.ItemTemplate = null;	
		}
		
		GameObject template = null; 
		if(multipleItemPerRowOrCol)
			template = create2dTemplate();
		else
			template = createTemplate();
		template.transform.SetParent(contentObj.transform);
		template.transform.localScale = Vector3.one;
		template.transform.localPosition = Vector3.zero;
		
		listHandler.ParentObject = contentObj;
		listHandler.ItemTemplate = template;
		listHandler.ParentScrollRect = scrollRect;
	}

	protected GameObject createTemplate()
	{
		GameObject template = null; 
		if(hasTemplate)
		{
			template = GameObject.Instantiate(templateObject) as GameObject;
			setDefaultElementSize(template);

			LayoutElement element = template.GetComponent<LayoutElement>();
			RectTransform scrollTrans = scrollRect.GetComponent<RectTransform>();
			if(listType == LIST_TYPE.VERTICAL)
			{
				scrollTrans.sizeDelta = new Vector2(element.preferredWidth, scrollTrans.sizeDelta.y);
			}
			else
			{
				scrollTrans.sizeDelta = new Vector2(scrollTrans.sizeDelta.x, element.preferredHeight);
			}
		}
		else
		{
			template = new GameObject();
			template.AddComponent<Image>();

			setDefaultElementSize(template);
		}
		template.name = "template";
		UIListItemHandler itemHandler = template.GetComponent<UIListItemHandler>();

		if(itemHandler == null)
			template.AddComponent<UIListItemHandler>();

		return template;
	}

	protected void setDefaultElementSize(GameObject subItem)
	{
		LayoutElement element = subItem.GetComponent<LayoutElement>();

		if(element == null)
		{
			element = subItem.AddComponent<LayoutElement>();
			if(listType == LIST_TYPE.VERTICAL)
			{
				element.preferredWidth = 180f;
				element.preferredHeight = 50f;
			}
			else
			{
				element.preferredWidth = 50f;
				element.preferredHeight = 180f;
			}
		}	
	}

	protected GameObject create2dTemplate()
	{
		GameObject template = new GameObject();
		LayoutElement element = template.AddComponent<LayoutElement>();

		UIListMultiItemHandler multiItemHandler = template.AddComponent<UIListMultiItemHandler>(); 

		bool hasSubLayoutElement = hasTemplate && templateObject.GetComponent<LayoutElement>() != null;
		RectTransform scrollTrans = scrollRect.GetComponent<RectTransform>();

		if(listType == LIST_TYPE.VERTICAL)
		{
			if(hasSubLayoutElement)
			{
				LayoutElement subItemElement = templateObject.GetComponent<LayoutElement>();
				element.preferredWidth = subItemElement.preferredWidth * subItemCountPerItem;
				element.preferredHeight = subItemElement.preferredHeight;
			}
			else
			{
				element.preferredWidth = 180f;
				element.preferredHeight = 50f;
			}

			scrollTrans.sizeDelta = new Vector2(element.preferredWidth, scrollTrans.sizeDelta.y);

			var horizontalGroup = template.AddComponent<HorizontalLayoutGroup>();
			horizontalGroup.childForceExpandWidth = false;
			horizontalGroup.childForceExpandHeight = true;
		}
		else
		{
			if(hasSubLayoutElement)
			{
				LayoutElement subItemElement = templateObject.GetComponent<LayoutElement>();
				element.preferredWidth = subItemElement.preferredWidth;
				element.preferredHeight = subItemElement.preferredHeight * subItemCountPerItem;				
			}
			else
			{
				element.preferredWidth = 50f;
				element.preferredHeight = 180f;
			}

			scrollTrans.sizeDelta = new Vector2(scrollTrans.sizeDelta.x, element.preferredHeight);

			var verticalGroup = template.AddComponent<VerticalLayoutGroup>();
			verticalGroup.childForceExpandHeight = false;
			verticalGroup.childForceExpandWidth = true;
		}

		for(int i = 0; i < subItemCountPerItem; i ++)
		{
			GameObject subItem = null;
			if(hasTemplate)
			{
				subItem = GameObject.Instantiate(templateObject) as GameObject;

				LayoutElement subElement = subItem.GetComponent<LayoutElement>();
				if(subElement == null)
				{
					subElement = subItem.AddComponent<LayoutElement>();
					if(listType == LIST_TYPE.VERTICAL)
					{
						subElement.preferredWidth = 180f / subItemCountPerItem;
						subElement.preferredHeight = 50f;
					}
					else
					{
						subElement.preferredHeight = 180f / subItemCountPerItem;
						subElement.preferredWidth = 50f;	
					}
				}
				else
				{
					
				}
			}
			else
			{
				subItem = new GameObject();

				LayoutElement subElement = subItem.AddComponent<LayoutElement>();

				if(listType == LIST_TYPE.VERTICAL)
				{
					subElement.preferredWidth = 180f / subItemCountPerItem;
					subElement.preferredHeight = 50f;
				}
				else
				{
					subElement.preferredHeight = 180f / subItemCountPerItem;
					subElement.preferredWidth = 50f;	
				}

			}
			UIListItemHandler subItemHandler = subItem.GetComponent<UIListItemHandler>();
			if(subItemHandler == null)
				subItemHandler = subItem.AddComponent<UIListItemHandler>();
			multiItemHandler.SingleItemHandlerList.Add(subItemHandler);
			Image subImage = subItem.GetComponent<Image>();
			if(subImage == null)
				subItem.AddComponent<Image>();

			subItem.name = i.ToString();
			subItem.transform.SetParent(template.transform);
			subItem.transform.localPosition = Vector3.zero;
			subItem.transform.localScale = Vector3.one;
		}
		template.name = "Template";

		return template;
	}

	protected void updateSelectedObject()
	{
		GameObject go = Selection.activeGameObject;

		if(go != null) 
		{
			if(selectedObject != go)
			{
				Selection.activeObject = go;
			}
			selectedObject = go;
		}
		if(selectedObject == null)
			scrollRect = null;
		else
		{
			scrollRect = selectedObject.GetComponent<ScrollRect>();

			if(scrollRect == null)
				scrollRect = selectedObject.GetComponentInChildren<ScrollRect>();
		}
	}
	#endregion
}

