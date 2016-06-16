using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIListHandler : MonoBehaviour {

#region Fields
	[SerializeField] protected GameObject				itemTemplate;
	[SerializeField] protected ScrollRect 				parentScrollRect;
	[SerializeField] protected GameObject				parentObj;
	protected List<GameObject> 					itemList;
#endregion

#region Getter and Setter
	public GameObject ItemTemplate
	{
		get { return itemTemplate; }
		set { itemTemplate = value; }
	}

	public ScrollRect ParentScrollRect
	{
		get { return parentScrollRect; }
		set { parentScrollRect = value; }
	}

	public GameObject ParentObject
	{
		get { return parentObj; }
		set { parentObj = value; }
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
	public virtual void UpdateData(object[] dataList)
	{
		if(itemTemplate.activeInHierarchy)
			itemTemplate.SetActive(false);

		if(itemList == null)
			itemList = new List<GameObject>();

		int dataCount = dataList == null ? 0 : dataList.Length;
		int iterationCnt = dataCount > itemList.Count ? dataCount : itemList.Count;

		for(int i = 0; i < iterationCnt; i ++)
		{
			object data = i < dataCount ? dataList[i] : null;
			GameObject item = i < itemList.Count ? itemList[i] : null;

			if(item == null && data != null)
			{
				item = GameObject.Instantiate(itemTemplate) as GameObject;
				item.transform.SetParent(parentObj.transform);
				item.transform.localScale = Vector3.one;
				itemList.Add(item);
				item.SetActive(true);
				item.GetComponent<UIListItemHandler>().UpdateData(data);
			}
			else if (item != null && data != null)
			{
				item.SetActive(true);
				item.GetComponent<UIListItemHandler>().UpdateData(data);
			}
			else if(item != null && data == null)
			{
				item.SetActive(false);
			}
		}


	}
#endregion
}
