using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    public BlockManager bM;
    public GameObject parentObject;
    public float verticalOffset = 0.018f;

    public void ButtonCameraStart()
    {
        StartCoroutine(CameraCoroutine());
    }

    IEnumerator CameraCoroutine()
    {
        while(true)
        {
            AdjustParentPosition();
            if(bM.currentLayer >= bM.maxLayer)
            {
                yield break;
            }
            yield return null;
        }
        
    }

    public void AdjustParentPosition()
    {
        parentObject.transform.position += new Vector3(0f, verticalOffset, 0f);
    }
}