using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public SequentialFlow stateFlow;

    
	void Start ()
    {
        stateFlow = new SequentialFlow();
        stateFlow.Add(new UserInputIdleStateFlow(stateFlow));
        stateFlow.Start(this);
	}
	
	void Update ()
    {
        //if (Input.touchCount > 0)
        if (InputHelper.GetTouches().Count > 0)
        {
            //Store the first touch detected.
            //Touch myTouch = Input.GetTouch(0);
            Touch myTouch = InputHelper.GetTouches()[0];

            Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100.0F);

            (stateFlow.CurrentFlow as UserInputStateFlow).OnTouchEvent(hits, myTouch);
        }
    }
}
