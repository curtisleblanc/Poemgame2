using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Gameplay variables
    [SerializeField] float movementSpeed = 6.0f;
    [SerializeField] float strafeSpeed = 2.0f;
    [SerializeField] float jumpForce = 300.0f;
    [SerializeField] float rotationSpeed = 3.0f;
    [SerializeField] float throwForce = 100.0f;

    //Working variables
    float xInput, zInput, mouseRot, rotation, aimUpDown;
    bool isOnGround, aiming;
    int ammo = 3;

    //References
    new Rigidbody rigidbody;
    [SerializeField] GameObject groundChecker;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject targeter;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] GameObject rock;
    SpriteRenderer targeterSprite;
    Transform playerView;
    Text ammoCount;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rigidbody = GetComponent<Rigidbody>();
        targeter = GameObject.Find("Targeter");
        targeterSprite = targeter.GetComponent<SpriteRenderer>();
        cam = GameObject.Find("Camera");
        playerView = GameObject.Find("PlayerView").transform;
        ammoCount = GameObject.Find("AmmoCounter").GetComponent<Text>();

        ammoCount.text = ammo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && ammo > 0)
        {
            aiming = true;
        } else
        {
            aiming = false;
        }

        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        mouseRot = Input.GetAxis("Mouse X");

        rotation = rotation + mouseRot;

        transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.x, rotation * rotationSpeed, transform.localRotation.z));

        if (Input.GetMouseButtonDown(1))
        {
            targeter.transform.position = playerView.position;
        }

        if (!aiming)
        {
            cam.transform.LookAt(playerView);

            isOnGround = Physics.CheckSphere(groundChecker.transform.position, 0.1f, whatIsGround);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isOnGround)
                {

                }
                else
                {
                    rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f));
                }
            }

            aimUpDown = 0.0f;
            targeterSprite.enabled = false;
        } else
        {
            targeterSprite.enabled = true;

            aimUpDown = aimUpDown + Input.GetAxis("Mouse Y");

            targeter.transform.position = transform.position + (transform.forward * 10.0f) + new Vector3(0.0f, aimUpDown, 0.0f);
            cam.transform.LookAt(targeter.transform);

            if (Input.GetMouseButtonDown(0))
            {
                if (ammo > 0)
                {
                    Rigidbody missile = Instantiate(rock, transform.position + transform.forward + transform.up, Quaternion.identity).GetComponent<Rigidbody>();

                    Vector3 trajectory = targeter.transform.position - transform.position;

                    missile.AddForce(trajectory * throwForce);

                    ammo--;
                    ammoCount.text = ammo.ToString();
                }
            }
        }

        Vector3 movement = transform.forward * zInput * movementSpeed + transform.right * xInput * strafeSpeed;
        rigidbody.velocity = new Vector3(movement.x, rigidbody.velocity.y, movement.z);
    }

    public bool IsJumping()
    {
        return isOnGround;
    }

    public bool IsAiming()
    {
        return aiming;
    }

    public void GiveRock()
    {
        ammo++;
        ammoCount.text = ammo.ToString();
    }
}
