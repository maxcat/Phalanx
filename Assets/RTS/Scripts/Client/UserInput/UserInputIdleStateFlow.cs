using UnityEngine;
using System.Collections;

public class UserInputIdleStateFlow : UserInputStateFlow
{
    public UserInputIdleStateFlow(SequentialFlow seqFlow) : base(seqFlow)
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
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            CapsuleCollider receiver = hit.transform.GetComponent<CapsuleCollider>();

            if (receiver != null)
            {
                nextInputFlow = new UserInputCardSelectedStateFlow(this.sequentialFlow);
            }
        }
    }
}
