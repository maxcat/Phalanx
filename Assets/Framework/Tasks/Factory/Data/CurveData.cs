using UnityEngine;
using System.Collections;

[System.Serializable]
public class CurveData  
{
#region Fields
	[SerializeField] protected AnimationCurve 			curve;
	protected float 						length;
#endregion	

#region Getter and Setter
	public float Length
	{
		get { return length; }
	}
#endregion

#region Constructor
	public CurveData(AnimationCurve curve)
	{
		this.curve = curve;
	}
#endregion

#region Virtual Functions 
	public virtual void Init()
	{
		if(curve == null || curve.keys == null || curve.keys.Length <= 0)
			length = 0f;
		else
			length = curve.keys[curve.keys.Length - 1].time - 
					curve.keys[0].time;
	}

	public virtual float GetStartValue()
	{
		if(curve == null || curve.keys == null || curve.keys.Length <= 0)
			return 0f;

		return curve.Evaluate(curve.keys[0].time);
	}
	public virtual float GetDeltaValue(float startTime, float deltaTime)
	{
		if(curve == null || curve.keys == null || curve.keys.Length <= 0)
			return 0f;

		float timeOffset = curve.keys[0].time;
		float time = startTime + deltaTime + timeOffset;
		float currentValue = curve.Evaluate(startTime + timeOffset);
		if(startTime + deltaTime <= Length)
			return curve.Evaluate(time) - currentValue;
		else
			return curve.Evaluate(curve.keys[curve.length - 1].time) - currentValue;
	}
#endregion

}

[System.Serializable]
public class CurveData3D
{
#region Fields
	[SerializeField] protected CurveData 				xData;
	[SerializeField] protected CurveData 				yData;
	[SerializeField] protected CurveData 				zData;
#endregion

#region Getter and Setter
	public CurveData XData
	{
		get { return xData; }
		set { xData = value; }
	}

	public CurveData YData
	{
		get { return yData; }
		set { yData = value; }
	}

	public CurveData ZData
	{
		get { return zData; }
		set { zData = value; }
	}

	public float Length
	{
		get 
		{
			float xLength = xData == null ? 0f : xData.Length;
			float yLength = yData == null ? 0f : yData.Length;
			float zLength = zData == null ? 0f : zData.Length;
			
			return Mathf.Max(xLength, yLength, zLength);
		}
	}
#endregion

#region Constructor
	public CurveData3D()
	{
		
	}

	public CurveData3D(CurveData xData, CurveData yData, CurveData zData)
	{
		this.xData = xData;
		this.yData = yData;
		this.zData = zData;
	}

	public CurveData3D(AnimationCurve xCurve, AnimationCurve yCurve, AnimationCurve zCurve)
	{
		xData = new CurveData(xCurve);
		yData = new CurveData(yCurve);
		zData = new CurveData(zCurve);
	}
#endregion

#region Public API
	public void Init()
	{
		xData.Init();
		yData.Init();
		zData.Init();
	}

	public Vector3 GetStartPosition()
	{
		return new Vector3(
				xData.GetStartValue(),
				yData.GetStartValue(),
				zData.GetStartValue()
				);
	}

	public Vector3 GetDeltaValue(float startTime, float deltaTime)
	{
		return Vector3.right * xData.GetDeltaValue(startTime, deltaTime) + 
			Vector3.up * yData.GetDeltaValue(startTime, deltaTime) + 
			Vector3.forward * zData.GetDeltaValue(startTime, deltaTime);
	}
#endregion
}
