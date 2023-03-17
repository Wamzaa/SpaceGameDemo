using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Camera camera;
    public int zoomLevel;
    public GameObject bulletPrefab;
    public GameObject pointerPrefab;

    public SpaceShip ship;
    public GameObject pointer;

    private BlasterBehaviour[] blasters;

    private float shipResist;
    private float shipFirePower;
    private Vector2 shipSpeed;
    private float shipWeight;

    void Start()
    {
        ShipFrame frame1 = new ShipFrame();
        frame1.name = "basic22frame";
        List<string> listComp1 = new List<string>();
        listComp1.Add("rafal-A12");
        //listComp1.Add("rafal-C53");
        listComp1.Add("geckoReactor");
        listComp1.Add("chameleonReactor");
        frame1.components = listComp1;


        ShipFrame frame2 = new ShipFrame();
        frame2.name = "basic22frame";
        List<string> listComp2 = new List<string>();
        listComp2.Add("geckoReactor");
        listComp2.Add("geckoReactor");
        listComp2.Add("geckoReactor");
        listComp2.Add("geckoReactor");
        frame2.components = listComp2;

        ShipFrame frame3 = new ShipFrame();
        frame3.name = "y-shaped262frame";
        List<string> listComp3 = new List<string>();
        listComp3.Add("chameleonReactor");
        listComp3.Add("geckoReactor");
        listComp3.Add("chameleonReactor");
        listComp3.Add("geckoReactor");
        listComp3.Add("chameleonReactor");
        listComp3.Add("chameleonReactor");
        listComp3.Add("rafal-C53");
        listComp3.Add("rafal-C53");
        listComp3.Add("rafal-A12");
        listComp3.Add("rafal-A12");
        frame3.components = listComp3;

        ship = new SpaceShip();
        ship.frame = frame3;

        GameObject rootShip = SpaceShipSpawner.Spawn(ship, true);
        rootShip.transform.position = this.transform.position;
        rootShip.transform.parent = this.transform;

        shipFirePower = 0.0f;
        shipSpeed = Vector2.zero;
        shipResist = 0.0f;
        shipWeight = 0.0f;
        blasters = rootShip.GetComponentsInChildren<BlasterBehaviour>();
        ComponentBehaviour[] components = rootShip.GetComponentsInChildren<ComponentBehaviour>();
        foreach (ComponentBehaviour component in components)
        {
            shipFirePower += component.firePower;
            shipSpeed += new Vector2(component.forwSpeed, component.latSpeed);
            shipResist += component.compResist;
            shipWeight += component.weight;
        }

        shipSpeed = shipSpeed / Mathf.Sqrt(shipWeight);

        pointer = Instantiate(pointerPrefab);

        foreach(BlasterBehaviour blaster in blasters)
        {
            blaster.SetNewAim(pointer.transform);
        }

        Vector3 initPosCam = new Vector3(this.transform.position.x, this.transform.position.y, transform.position.z - GetZoomDistance(zoomLevel));
        camera.transform.position = initPosCam;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.SetGameMode(!UIManager.Instance.isInGame);
        }

        if (UIManager.Instance.isInGame)
        {
            //Get Cursor projection in scene
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = GetZoomDistance(zoomLevel);
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
            /*if (playerMovement.y != 0.0f)
            {
                rotSpeed = Mathf.Sign(playerMovement.y) * 3.0f * shipSpeed.y;
            }*/
            Vector3 rotVec = transform.rotation.eulerAngles;
            rotVec.z += rotSpeed * Time.deltaTime * (-playerMovement.x);
            this.transform.rotation = Quaternion.Euler(rotVec);

            Vector3 moveVec = shipSpeed.x * Time.deltaTime * (playerMovement.y * transform.up);
            this.transform.position += moveVec;

            //CANON ----------------

            if (Input.GetMouseButton(0))
            {
                foreach (BlasterBehaviour blaster in blasters)
                {
                    blaster.Shoot();
                }
            }

            //CAMERA ---------------

            

            float scrollDelta = Input.mouseScrollDelta.y;
            if(scrollDelta != 0)
            {
                zoomLevel = (int) Mathf.Min(10, Mathf.Max(0, zoomLevel + scrollDelta));
            }

            //Get Camera position
            Vector3 vecPlayerCursor = cursorProjection - this.transform.position;
            float lenPosCam = Mathf.Min(3.0f, 0.5f * vecPlayerCursor.magnitude);               // Formula to get Camera position from cursor projection
                                                                                               // We need a value (< 1 * x) otherwise it move to fast
            Vector3 vecPosCam = transform.position + lenPosCam * vecPlayerCursor.normalized;
            vecPosCam.z = transform.position.z - GetZoomDistance(zoomLevel);

            //Move Camera
            camera.transform.position = SmoothTranslation(camera.transform.position, vecPosCam, 300.0f, Time.deltaTime);                 // Formula to get camera movement 
        }
    }

    public float GetZoomDistance(int zoomLevel)
    {
        float maxZoom = 20.0f;
        float minZoom = 3.0f;
        int maxLevel = 10;
        int minLevel = 1;

        float res = (maxZoom - minZoom) * (Mathf.Exp(zoomLevel) - Mathf.Exp(minLevel)) / (Mathf.Exp(maxLevel) - Mathf.Exp(minLevel)) + minZoom;
        Debug.Log(res);
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
