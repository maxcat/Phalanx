using UnityEngine;
using HRGameLogic;
using System.Collections;

public static partial class GameUtilities {

#region Vector Convertor
	public static HRVector2D Convert(this Vector2 input)
	{
		return new HRVector2D(input.x, input.y);
	}

	public static Vector2 Convert(this HRVector2D input)
	{
		return new Vector2(input.x, input.y);
	}
#endregion
}
