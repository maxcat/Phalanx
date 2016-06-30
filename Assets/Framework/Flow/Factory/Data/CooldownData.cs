using UnityEngine;
using System.Collections;

[System.Serializable]
public struct CooldownData
{

#region Static Functions
	public static CooldownData Zero
	{
		get 
		{
			return new CooldownData(0, 0, 0, 0, 0, 0);
		}	
	}
#endregion

#region Fields
	[SerializeField] public long 			Year;
	[SerializeField] public long 			Month;
	[SerializeField] public long 			Day;
	[SerializeField] public long 			Hour;
	[SerializeField] public long 			Minute;
	[SerializeField] public double 			Second;
	double 						totalSeconds;
#endregion

#region Getter and Setter
	public double TotalSeconds
	{
		get { return totalSeconds; }
	}	

	public long Seconds
	{
		get { return  System.Convert.ToInt64(System.Math.Floor(totalSeconds)) % 60;}
	}
#endregion

#region Constructor
	public CooldownData(long year, long month, long day, long hour, long minute, double second)
	{
		Year = year;
		Month = month;
		Day = day;
		Hour = hour;
		Minute = minute;
		Second = second;	

		totalSeconds = Year * (12 * 31 * 24 * 60 * 60) + 
			Month * (31 * 24 * 60 * 60) + 
			Day * (24 * 60 * 60) + 
			Hour * (60 * 60) + 
			Minute * 60 + 
			Second;
	}
#endregion

#region Private Functions
	void updateFromTotalSeconds(double targetSeconds)
	{
		totalSeconds = targetSeconds;
		long totalSecondsFloor = System.Convert.ToInt64(System.Math.Floor(totalSeconds));	
		Second = (totalSeconds - totalSecondsFloor) + (totalSecondsFloor % 60);
		double tmpSeconds = targetSeconds;
		Year = System.Convert.ToInt64(System.Math.Floor(tmpSeconds / (12 * 31 * 24 * 60 * 60)));
		tmpSeconds -= Year * 12 * 31 * 24 * 60 * 60;

		Month = System.Convert.ToInt64(System.Math.Floor(tmpSeconds / (31 * 24 * 60 * 60)));
		tmpSeconds -= Month * 31 * 24 * 60 * 60;

		Day = System.Convert.ToInt64(System.Math.Floor(tmpSeconds / (24 * 60 * 60)));
		tmpSeconds -= Day * 24 * 60 * 60;

		Hour = System.Convert.ToInt64(System.Math.Floor(tmpSeconds / (60 * 60)));
		tmpSeconds -= Hour * 60 * 60;

		Minute = System.Convert.ToInt64(System.Math.Floor(tmpSeconds /  60));
	}
#endregion

#region Public API
	public void Init()
	{
		totalSeconds = Year * (12 * 31 * 24 * 60 * 60) + 
			Month * (31 * 24 * 60 * 60) + 
			Day * (24 * 60 * 60) + 
			Hour * (60 * 60) + 
			Minute * 60 + 
			Second;
	}

	public void Deduct(float deltaTime)
	{
		totalSeconds = totalSeconds - deltaTime;
		updateFromTotalSeconds(totalSeconds > 0 ? totalSeconds : 0);
	}

	public override string ToString()
	{
		Init();
		return string.Format("year: {0} month: {1} day: {2} hour: {3} minute: {4} second: {5} total seconds: {6}", Year, Month, Day, Hour, Minute, Seconds, totalSeconds);
	}

#endregion

#region Override Operator
	public static implicit operator CooldownData(long year, long month, long day, long hour, long minute, double second)
	{
		return new CooldownData(year, month, day, hour, minute, second);
	}
#endregion
}

