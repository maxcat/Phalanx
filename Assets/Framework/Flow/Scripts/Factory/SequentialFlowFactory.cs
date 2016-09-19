using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequentialFlowFactory : FlowCollectionFactory {

#region Implement Virtual Functions
	public override Flow CreateFlow()
	{
		if(factoryList == null || factoryList.Count <= 0)
			return null;

		SequentialFlow mainFlow = new SequentialFlow();

		for(int i = 0; i < factoryList.Count; i ++)
		{
			FlowFactory factory = factoryList[i];
			if(factory != null)
				mainFlow.Add(factory.CreateFlow());
		}

		return mainFlow;
	}
#endregion
}
