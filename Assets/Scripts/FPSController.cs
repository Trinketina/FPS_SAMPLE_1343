using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FPSController : MonoBehaviour
{
    // references
    CharacterController controller;
    [SerializeField] GameObject cam;
    [SerializeField] Transform gunHold;
    [SerializeField] Gun initialGun;

    // stats
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float maxHealth = 20;

    // private variables
    float health;
    Vector3 velocity;
    bool grounded;
    float xRotation;
    List<Gun> equippedGuns = new List<Gun>();
    int gunIndex = 0;
    Gun currentGun = null;

    // properties
    public GameObject Cam { get { return cam; } }

    //Event Handlers
    public UnityAction PlayerInteract;
    [SerializeField] UnityEvent OnShoot;
    [SerializeField] UnityEvent<float, float> HealthChange;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // start with a gun
        AddGun(initialGun);

        health = maxHealth;
        HealthChange.Invoke(maxHealth, health);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            Movement();
            Look();

            FireGun();

            Interact(); //runs interaction event
        }
        // always go back to "no velocity"
        // "velocity" is for movement speed that we gain in addition to our movement (falling, knockback, etc.)
        Vector3 noVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.Lerp(velocity, noVelocity, 5 * Time.deltaTime);
    }
    void Interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("interacting");
            PlayerInteract?.Invoke();
        }
    }

    void Movement()
    {
        grounded = controller.isGrounded;

        if(grounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        Vector2 movement = GetPlayerMovementVector();
        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        controller.Move(move * movementSpeed * (GetSprint() ? 2 : 1) * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y += Mathf.Sqrt (jumpForce * -1 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        Vector2 looking = GetPlayerLook();
        float lookX = looking.x * lookSensitivityX * Time.deltaTime;
        float lookY = looking.y * lookSensitivityY * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    void FireGun()
    {
        if(GetPressFire() && currentGun!= null)
        {
            if (currentGun.AttemptFire())
            {
                OnShoot.Invoke();
            }
        }
    }

    void EquipGun(Gun g)
    {
        // disable current gun, if there is one
        currentGun?.gameObject.SetActive(false);

        g.gameObject.SetActive(true);
        g.transform.parent = gunHold;
        g.transform.localPosition = Vector3.zero;
        currentGun = g;
    }

    // public methods

    public void AddGun(Gun g)
    {
        // add new gun to the list
        equippedGuns.Add(g);

        // our index is the last one/new one
        gunIndex = equippedGuns.Count - 1;

        // put gun in the right place
        EquipGun(g);
    }

    public void IncreaseAmmo(int amount)
    {
        currentGun.AddAmmo(amount);
    }

    // Input methods

    bool GetPressFire()
    {
        return Input.GetButtonDown("Fire1");
    }

    bool GetHoldFire()
    {
        return Input.GetButton("Fire1");
    }

    Vector2 GetPlayerMovementVector()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    Vector2 GetPlayerLook()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    bool GetSprint()
    {
        return Input.GetButton("Sprint");
    }

    // Collision methods

    // Character Controller can't use OnCollisionEnter :D thanks Unity
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Damager>())
        {
            var collisionPoint = hit.collider.ClosestPoint(transform.position);
            var knockbackAngle = (transform.position - collisionPoint).normalized;
            velocity = (20 * knockbackAngle);

            float dam = hit.gameObject.GetComponent<Damager>().damage;
            if (health - dam <= 0 && health != .1f)
                health = .1f;
            else 
                health -= dam;
            HealthChange.Invoke(maxHealth, health);
            Debug.Log("Health: " + health);
        }
    }
}
