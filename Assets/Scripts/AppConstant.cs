using UnityEngine;
using System.Collections;

public class AppConstant  {
	
	#region Phalanx Constants
	/// <summary>
	/// The unit size
	/// all player or enemy unit are consist of one or more unit
	/// </summary>
	public static readonly float 	UNIT_SIZE					= 		1f;
	
	/// <summary>
	/// The the interval between units
	/// </summary>
	public static readonly float 	UNIT_INTERVAL				=		0.3f;		
	public static readonly int 		MAX_COL						= 		5;
	public static readonly int		MAX_ROW						= 		5;
	public static readonly float 	ENEMY_UPDATE_BOUNDARY		=		-20f;
	#endregion
	
}
