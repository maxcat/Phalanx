using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI2DListData 
{
#region Fields
	protected List<object> 		dataList;
#endregion

#region Getter and Setter
	public List<object> DataList
	{
		get { return dataList; }
		set { dataList = value; }
	}

	public int Count
	{
		get
		{
			if(dataList == null)
				return 0;
			return dataList.Count;
		}
	}
#endregion

#region Constructor
	public UI2DListData()
	{
		this.dataList = new List<object>();
	}	

	public UI2DListData (List<object> dataList)
	{
		this.dataList = dataList;
	}
#endregion 
}

public class UIListMultiItemHandler : UIListItemHandler {

#region Fields
	[SerializeField] protected List<UIListItemHandler> 				singleItemHandlerList;
#endregion

#region Getter and Setter
	public List<UIListItemHandler> SingleItemHandlerList
	{
		get 
		{ 
			if(singleItemHandlerList == null)
				singleItemHandlerList = new List<UIListItemHandler>();
			return singleItemHandlerList;
	       	}
		set { singleItemHandlerList = value; }
	}
#endregion

#region Implement Virtual Functions
	public override void UpdateData(object data)
	{
		UI2DListData data2D = new UI2DListData(); 

		if(data != null)
		{
			try
			{
				data2D = data as UI2DListData;
			}
			catch (System.InvalidCastException)
			{
				Debug.LogError("[ERROR]UIListMultiItemHandler->UpdateData: input data can not be casted to UI2DListData.");
				return;
			}
		}

		if(data2D.Count > singleItemHandlerList.Count)
		{
			Debug.LogError("[ERROR]UIListMultiItemHandler->UpdateData: input data list count exceed item count.");
			return;
		}		
		
		for(int i = 0; i < singleItemHandlerList.Count; i ++)
		{
			UIListItemHandler handler = singleItemHandlerList[i];
			if(i < data2D.Count)
			{
				handler.gameObject.SetActive(true);
				handler.UpdateData(data2D.DataList[i]);
			}
			else
			{
				handler.gameObject.SetActive(false);
			}
		}
	}
#endregion
}
