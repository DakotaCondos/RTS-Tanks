using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] GameObject objectToRotate;
    public float rpmAboutX = 0;
    public float rpmAboutY = 0;
    public float rpmAboutZ = 0;

    private void Update()
    {
        if (objectToRotate == null) { return; }
        objectToRotate.transform.Rotate(RPMDelta(rpmAboutX),
                                        RPMDelta(rpmAboutY),
                                        RPMDelta(rpmAboutZ));
    }

    private float RPMDelta(float rpm)
    {
        return (360f * rpm / 60f) * Time.deltaTime;
    }
}
