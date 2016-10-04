using UnityEngine;
using System.Collections;

public class CameraSizeControll : MonoBehaviour {

    public int topMargin = 0;
    public int bottomMargin = 0;
    public int sideMargin = 0;

	// Use this for initialization
	void Start ()
    {
        Camera camera = GetComponent<Camera>();
        
        camera.orthographicSize = MapSingleton.Instance.Height/2 + Mathf.CeilToInt((topMargin + bottomMargin)/2);


        float windowAspect = (float)Screen.width / (float)Screen.height;
        float heightSize = camera.orthographicSize * 2;
        float widthSize = windowAspect * heightSize;
        float prefferedWidthSize = MapSingleton.Instance.Width + sideMargin*2;

        if (widthSize < prefferedWidthSize)
        {
            float finalHeightSize = (heightSize * prefferedWidthSize) / widthSize;

            camera.orthographicSize = Mathf.CeilToInt(finalHeightSize / 2);
        }


    }
	
}
