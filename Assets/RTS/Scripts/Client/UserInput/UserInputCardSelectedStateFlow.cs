using UnityEngine;
using System.Collections;

public class UserInputCardSelectedStateFlow : UserInputStateFlow
{
    public UserInputCardSelectedStateFlow(SequentialFlow seqFlow) : base(seqFlow)
    {
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
        if (true) //TODO: should look for grid for touch else look hero etc.
        {
            bool touchDelivered = false;
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                MapSingleton mapReceiver = hit.transform.GetComponent<MapSingleton>();

                if (mapReceiver != null)
                {
                    mapReceiver.TouchReceive(touch);
                    touchDelivered = true;
                }
            }

            if (!touchDelivered)
            {
                MapSingleton.Instance.TouchClear();
            }
        }
    }
}
