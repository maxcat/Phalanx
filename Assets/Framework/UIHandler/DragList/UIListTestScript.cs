using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIListTestScript : MonoBehaviour {

#region Fields
	[SerializeField] protected int 					dataCount = 10;
	[SerializeField] protected bool					is2DList = false;
	[SerializeField] protected int 					subItemCount = 4;
#endregion 

#region Override MonoBehaviour 
	// Use this for initialization
	void Start () {
		List<UIListData> dataList = new List<UIListData>();

		if(!is2DList)
		{
			for(int i = 0; i < dataCount; i ++)
				dataList.Add(new UIListData());
		}
		else
		{
			for(int i = 0; i < Mathf.CeilToInt((float)dataCount / (float)subItemCount); i ++)
			{
				UI2DListData data = new UI2DListData();
				for(int j = 0; j < subItemCount; j ++)
				{
					if(i * subItemCount + j < dataCount)
						data.DataList.Add(new UIListData());
				}
				dataList.Add(data);
			}
		}

		GetComponent<UIListHandler>().UpdateData(dataList);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion
}
