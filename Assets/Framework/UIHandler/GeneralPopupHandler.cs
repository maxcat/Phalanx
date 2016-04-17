using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneralPopupData : PopupData
{
#region Fields
	protected string 		titleStr;
	protected string 		descriptionStr;
#endregion

#region Getter and Setter
	public string TitleStr
	{
		get { return titleStr; }
	}

	public string DescriptionStr
	{
		get { return descriptionStr; }
	}
#endregion

#region Constructor
	public GeneralPopupData(string title, string description)
		: base ()
	{
		titleStr = title;
		descriptionStr = description;
	}
#endregion
}

public partial class PopupService {

#region Fields
	protected string 			GeneralPopupResourcePath = "Popup/GeneralPopup";
#endregion

#region Popup Builder
	public void ShowGeneralPopup(string title, string description, bool isPersistPopup = false)
	{
		GameObject prefab = Resources.Load(GeneralPopupResourcePath) as GameObject;
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		addPopup(instance, new GeneralPopupData(title, description), isPersistPopup);
	}
#endregion
}


public class GeneralPopupHandler : PopupHandler {

#region Fields
	[SerializeField] protected Text			titleText;
	[SerializeField] protected Text 		descriptionText;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Implement Virtual Functions
	public override void Init(PopupData data, bool isPersistPopup)
	{
		base.Init(data, isPersistPopup);

		titleText.text = (popupData as GeneralPopupData).TitleStr;
		descriptionText.text = (popupData as GeneralPopupData).DescriptionStr;
	}
#endregion
}
