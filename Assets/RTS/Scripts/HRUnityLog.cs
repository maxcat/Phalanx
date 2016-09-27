using UnityEngine;
using HRGameLogic;

public class HRUnityLog : HRLog {

#region Implement Virtual Functions
	public override void LogInfo(string info)
	{
		Debug.Log(info);
	}

	public override void LogWarning(string warning)
	{
		Debug.Log(warning);
	}

	public override void LogError(string error)
	{
		Debug.LogError(error);
	}
#endregion
}
