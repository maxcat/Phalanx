using UnityEngine;
using System.Collections;

public class UserInputHeroSelectedStateFlow : UserInputStateFlow
{
    PlayerAction action;
    GameObject selectedObject;

    public UserInputHeroSelectedStateFlow(SequentialFlow seqFlow) : base(seqFlow)
    {
        action = new HeroPlayerAction();
    }

    protected override IEnumerator main()
    {
        while (nextInputFlow == null)
        {
            yield return null;
        }

        this.sequentialFlow.Add(nextInputFlow);
        nextInputFlow = null;
    }

    public override void OnTouchEvent(RaycastHit[] hits, Touch touch)
    {

        bool result = action.TouchSelection(hits, touch, out selectedObject);
        if (!result)
        {
            if (selectedObject != null)
            {
                MapSingleton mapReceiver = selectedObject.GetComponent<MapSingleton>();

                if (mapReceiver != null)
                {
                    mapReceiver.TouchReceive(touch);

                    //TODO : remove/change highlight between objects
                }
                else
                {
                    MapSingleton.Instance.TouchClear();
                }
            }
            else
            {
                MapSingleton.Instance.TouchClear();
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            //TODO : remove highlight from object

            nextInputFlow = new UserInputIdleStateFlow(this.sequentialFlow);
            MapSingleton.Instance.TouchClear();
        }

    }
}
