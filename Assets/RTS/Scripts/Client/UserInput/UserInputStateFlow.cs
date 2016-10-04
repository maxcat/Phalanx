using UnityEngine;
using System.Collections;

public class UserInputStateFlow : Flow
{
    public SequentialFlow sequentialFlow;

    protected UserInputStateFlow nextInputFlow = null;

    public virtual void OnTouchEvent(RaycastHit[] hits, Touch touch) { }


    public UserInputStateFlow(SequentialFlow seqFlow) : base()
    {
        sequentialFlow = seqFlow;
    }

}
