using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    public GameObject parentObject;
    public float verticalOffset = 1.0f;

    void Update()
    {
        AdjustParentPosition();
    }

    public void AdjustParentPosition()
    {
        parentObject.transform.position += new Vector3(0f, verticalOffset, 0f);
    }
}
