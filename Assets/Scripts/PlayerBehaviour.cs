using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Camera camera;
    public float distCamera;
    public GameObject bulletPrefab;
    public GameObject pointerPrefab;

    public SpaceShip ship;
    public GameObject pointer;

    private float timer;

    private float shipResist;
    private float shipFirePower;
    private Vector2 shipSpeed;
    private float shipWeight;

    void Start()
    {
        ShipComponent geckoReactor = new ShipComponent();
        geckoReactor.name = "geckoReactor";
        geckoReactor.latSpeed = 0.0f;
        geckoReactor.forwSpeed = 20.0f;
        geckoReactor.firePower = 0.0f;
        geckoReactor.compResist = 50.0f;
        geckoReactor.weight = 15.0f;

        ShipComponent chameleonReactor = new ShipComponent();
        chameleonReactor.name = "chameleonReactor";
        chameleonReactor.latSpeed = 7.0f;
        chameleonReactor.forwSpeed = 7.0f;
        chameleonReactor.firePower = 0.0f;
        chameleonReactor.compResist = 40.0f;
        chameleonReactor.weight = 10.0f;

        ShipComponent rafalA12 = new ShipComponent();
        rafalA12.name = "rafal-A12";
        rafalA12.latSpeed = 0.0f;
        rafalA12.forwSpeed = 0.0f;
        rafalA12.firePower = 30.0f;
        rafalA12.compResist = 40.0f;
        rafalA12.weight = 10.0f;

        ShipComponent rafalC53 = new ShipComponent();
        rafalC53.name = "rafal-C53";
        rafalC53.latSpeed = 0.0f;
        rafalC53.forwSpeed = 0.0f;
        rafalC53.firePower = 45.0f;
        rafalC53.compResist = 60.0f;
        rafalC53.weight = 20.0f;

        ShipFrame frame1 = new ShipFrame();
        frame1.name = "basic44frame";
        frame1.nbComponents = 4;
        List<ShipComponent> listComp1 = new List<ShipComponent>();
        listComp1.Add(geckoReactor);
        listComp1.Add(geckoReactor);
        listComp1.Add(rafalA12);
        listComp1.Add(rafalA12);
        frame1.components = listComp1;

        ShipFrame frame2 = new ShipFrame();
        frame2.name = "basic44frame";
        frame2.nbComponents = 4;
        List<ShipComponent> listComp2 = new List<ShipComponent>();
        listComp2.Add(chameleonReactor);
        listComp2.Add(chameleonReactor); 
        listComp2.Add(rafalC53);
        listComp2.Add(rafalC53);
        frame2.components = listComp2;

        ShipFrame frame3 = new ShipFrame();
        frame3.name = "y-shaped262frame";
        frame3.nbComponents = 8;
        List<ShipComponent> listComp3 = new List<ShipComponent>();
        listComp3.Add(chameleonReactor);
        listComp3.Add(geckoReactor);
        listComp3.Add(chameleonReactor);
        listComp3.Add(geckoReactor);
        listComp3.Add(chameleonReactor);
        listComp3.Add(chameleonReactor);
        listComp3.Add(rafalC53);
        listComp3.Add(rafalC53);
        listComp3.Add(rafalA12);
        listComp3.Add(rafalA12);
        frame3.components = listComp3;

        ship = new SpaceShip();
        ship.frame = frame2;

        shipResist = ship.GetShipResist();
        shipSpeed = ship.GetShipSpeed();
        shipFirePower = ship.GetShipFirePower();
        shipWeight = ship.GetShipWeight();

        GameObject rootShip = SpaceShipSpawner.Spawn(ship);
        rootShip.transform.position = this.transform.position;
        rootShip.transform.parent = this.transform;

        pointer = Instantiate(pointerPrefab);

        BlasterBehaviour[] blasters = rootShip.GetComponentsInChildren<BlasterBehaviour>();
        foreach(BlasterBehaviour blaster in blasters)
        {
            blaster.SetNewAim(pointer.transform);
        }

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

        pointer.transform.position = cursorProjection;

        //PLAYER POSITION & ROTATION ---------------
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

        float rotSpeed = shipSpeed.y;
        if(playerMovement.y != 0.0f)
        {
            rotSpeed = Mathf.Sign(playerMovement.y) * 3.0f * shipSpeed.y ;
        }
        Vector3 rotVec = transform.rotation.eulerAngles;
        rotVec.z += rotSpeed * Time.deltaTime * (- playerMovement.x);
        this.transform.rotation = Quaternion.Euler(rotVec);

        Vector3 moveVec = shipSpeed.x * Time.deltaTime * (playerMovement.y * transform.up);
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
