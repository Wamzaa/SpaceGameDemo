using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBehaviour : MonoBehaviour
{
    public GameObject blasterCanon;

    private Transform aim;

    public void SetNewAim(Transform newAim)
    { 
        aim = newAim;
    }

    private void Update()
    {
        Vector3 dirToAim = aim.position - this.transform.position;
        dirToAim = dirToAim.normalized;
        float newAngle = Mathf.Asin(Vector3.Dot(Vector3.Cross(blasterCanon.transform.up, dirToAim), Vector3.fwd));
        if (Vector3.Dot(transform.up, dirToAim) < 0)
        {
            newAngle = Mathf.PI - newAngle;
        }
        newAngle = newAngle * 180.0f / Mathf.PI;
        Vector3 rot = blasterCanon.transform.rotation.eulerAngles;
        rot.z = SmoothRotation(rot.z, newAngle, 5.0f, Time.deltaTime);
        blasterCanon.transform.rotation = Quaternion.Euler(rot);
    }



    public float SmoothRotation(float dep, float angle, float speedRot, float dt)
    {
        float zerAngle = GetAngleZeroCentered(angle);
        float res = dep + speedRot * dt * zerAngle;
        return res;
    }

    public float GetAngleZeroCentered(float angle)
    {
        float zerAngle = angle % (360.0f);
        if (zerAngle < -180.0f)
        {
            zerAngle += 360.0f;
        }
        else if (zerAngle > 180.0f)
        {
            zerAngle -= 360.0f;
        }
        return zerAngle;
    }
}
