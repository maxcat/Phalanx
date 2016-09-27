using System;

namespace HRGameLogic
{
	public class HRLog
	{

#region Static Fields
		private static HRLog 			instance;
#endregion

#region Static Functions
		public static void Init(HRLog log)
		{
			instance = log;
		}

		public static void Info(string info)
		{
			if(instance != null)
				instance.LogInfo(info);
		}

		public static void Warning(string warning)
		{
			if(instance != null)
				instance.LogWarning(warning);
		}

		public static void Error(string error)
		{
			if(instance != null)
				instance.LogError(error);
		}
#endregion

#region Constructor
		public HRLog ()
		{
		}
#endregion

#region Virtual Functions
		public virtual void LogInfo(string info)
		{
		}

		public virtual void LogWarning(string warning)
		{
		}

		public virtual void LogError(string error)
		{
		}
#endregion
	}
}

