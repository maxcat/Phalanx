using UnityEngine;
using System.Collections;

public class GameFlow : Flow {

#region Fields
	protected ClientService 			clientService;
#endregion

#region Constructor
	public GameFlow() : base ()
	{
	}
#endregion

#region Public API
	public void SetClient(ClientService clientService)
	{
		this.clientService = clientService;
	}
#endregion
}
