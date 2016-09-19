using UnityEngine;
using System.Collections;

public class CooldownFlow : Flow {


#region Delegate
	public delegate void 		UpdateCooldown(CooldownData data);
#endregion

#region Constructor
	protected CooldownData 		cooldown;
	protected CooldownData 		timeLeft;
	protected UpdateCooldown 	delegateFunc;
#endregion

#region Getter and Setter
	public CooldownData TimeLeft
	{
		get { return timeLeft; }
	}

	public CooldownData Cooldown
	{
		get { return cooldown; }
	}
#endregion

#region Constructor
	public CooldownFlow(CooldownData cooldown, UpdateCooldown func = null)
		: base ()
	{
		this.cooldown = cooldown;	
		this.timeLeft = cooldown;
		delegateFunc = func;
	}
#endregion

#region Implement Virtual Functions
	protected override IEnumerator main()
	{
		while(timeLeft.TotalSeconds > 0)
		{
			timeLeft.Deduct(Time.deltaTime);
			if(delegateFunc != null)
				delegateFunc(timeLeft);
			yield return null;
		}			
	}
#endregion
}
