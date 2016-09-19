using UnityEngine;
using System.Collections;

public class DelayFlow : Flow {

#region Fields
	protected float 		delayInSecond;
#endregion

#region Constructor 
	public DelayFlow(float delayInSecond)
		: base ()
	{
		this.delayInSecond = delayInSecond;
	}
#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		float timeElapse = 0f;

		while(timeElapse < delayInSecond)
		{
			timeElapse += Time.deltaTime;
			yield return null;
		}
	}
#endregion

}
