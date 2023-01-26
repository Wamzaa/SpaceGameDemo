using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Camera camera;
    public float distCamera;
    public GameObject bulletPrefab;

    private float timer;

    void Start()
    {
        Vector3 initPosCam = new Vector3(this.transform.position.x, this.transform.position.y, transform.position.z - distCamera);
        camera.transform.position = initPosCam;
        timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //Get Cursor projection in scene
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.z = distCamera;
        Vector3 cursorProjection = camera.ScreenToWorldPoint(cursorPosition);

        //PLAYER ROTATION ---------------

        /*Vector3 dirToCursor = cursorProjection - this.transform.position;
        dirToCursor = dirToCursor.normalized;
        float newAngle = Mathf.Asin(Vector3.Dot(Vector3.Cross(transform.up, dirToCursor), Vector3.fwd));
        if (Vector3.Dot(transform.up, dirToCursor) < 0)
        {
            newAngle = Mathf.PI - newAngle;
        }
        newAngle = newAngle * 180.0f / Mathf.PI;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = SmoothRotation(rot.z, newAngle, 5.0f, Time.deltaTime);
        transform.rotation = Quaternion.Euler(rot);*/

        //PLAYER POSITION ---------------
        Vector2 playerMovement = new Vector2();

        if (Input.GetKey(KeyCode.Z))
        {
            playerMovement.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerMovement.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerMovement.x += 1.0f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            playerMovement.x -= 1.0f;
        }

        float rotSpeed = 10.0f;
        if(playerMovement.y != 0.0f)
        {
            rotSpeed = Mathf.Sign(playerMovement.y) * 50.0f;
        }
        Vector3 rotVec = transform.rotation.eulerAngles;
        rotVec.z += rotSpeed * Time.deltaTime * (- playerMovement.x);
        this.transform.rotation = Quaternion.Euler(rotVec);

        Vector3 moveVec = 5.0f * Time.deltaTime * (playerMovement.y * transform.up);
        this.transform.position += moveVec;

        //CANON ----------------

        bool isShooting;

        isShooting = Input.GetMouseButton(0);
        if(isShooting && timer > 0.2f)
        {
            timer = 0.0f;
            Vector3 dirToCursorBullet = cursorProjection - this.transform.position;
            dirToCursorBullet = dirToCursorBullet.normalized;
            float newAngleBullet = Mathf.Asin(Vector3.Dot(Vector3.Cross(Vector3.up, dirToCursorBullet), Vector3.fwd));
            if (Vector3.Dot(Vector3.up, dirToCursorBullet) < 0)
            {
                newAngleBullet = Mathf.PI - newAngleBullet;
            }
            newAngleBullet = newAngleBullet * 180.0f / Mathf.PI;
            float zerAngleBullet = GetAngleZeroCentered(newAngleBullet);
            Shoot(zerAngleBullet);
        }

        //CAMERA ---------------

        //Get Camera position
        Vector3 vecPlayerCursor = cursorProjection - this.transform.position;
        float lenPosCam = Mathf.Min(3.0f, 0.5f * vecPlayerCursor.magnitude);               // Formula to get Camera position from cursor projection
                                                                                           // We need a value (< 1 * x) otherwise it move to fast
        Vector3 vecPosCam = transform.position + lenPosCam * vecPlayerCursor.normalized;
        vecPosCam.z = transform.position.z - distCamera;

        //Move Camera
        camera.transform.position = SmoothTranslation(camera.transform.position, vecPosCam, 300.0f, Time.deltaTime);                 // Formula to get camera movement 
    }

    public void Shoot(float angle)
    {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = transform.position;
        Vector3 rot = newBullet.transform.rotation.eulerAngles;
        rot.z = angle;
        newBullet.transform.rotation = Quaternion.Euler(rot);
    }

    public float SmoothRotation(float dep, float angle, float speedRot, float dt)
    {
        float zerAngle = GetAngleZeroCentered(angle);
        float res = dep + speedRot * dt * zerAngle;
        return res;
    }

    public Vector3 SmoothTranslation(Vector3 dep, Vector3 arr, float speed, float dt)
    {
        Vector3 dist = arr - dep;
        Vector3 res = dep + speed * dt * dist;
        return res;
    }

    public float GetAngleZeroCentered(float angle)
    {
        float zerAngle = angle % (360.0f);
        if(zerAngle < -180.0f)
        {
            zerAngle += 360.0f;
        }
        else if(zerAngle > 180.0f)
        {
            zerAngle -= 360.0f;
        }
        return zerAngle;
    }
}
