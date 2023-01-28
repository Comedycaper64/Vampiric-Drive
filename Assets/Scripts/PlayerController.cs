using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int bloodFloatingCost = 5;
    [SerializeField] private int bloodShootingCost = 1;
    [SerializeField] private float movementSpeed = 50f;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float gravityWhileFloating = 0.3f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private GameObject batProjectile;
    [SerializeField] private GameObject bloodMeter;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private AudioClip dashSFX;
    [SerializeField] private AudioClip deathSFX;

    [SerializeField] private Sprite vampireIdle;
    [SerializeField] private Sprite vampireMoving;
    [SerializeField] private Sprite vampireFloating;
    [SerializeField] private Sprite vampireHanging;
    [SerializeField] private Sprite vampireDying1;
    [SerializeField] private Sprite vampireDying2;
    [SerializeField] private Sprite vampireDying3;
    [SerializeField] private Sprite vampireJumping;
    [SerializeField] private Sprite vampireSwirl1;
    [SerializeField] private Sprite vampireSwirl2;

    private GameController levelLoader;
    private UnlockingLevels levelUnlocker;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    public int dashNumber = 1;
    private bool jump = false;
    private bool crouch = false;
    private bool canMove = true;
    private bool dashing = false;
    private bool floating = false;
    private bool hanging = false;
    private bool dying = false;
    private GameObject currentHangPoint;

    private Vector2 dashDirection;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private BloodMeter bloodScript;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        bloodScript = bloodMeter.GetComponent<BloodMeter>();
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<GameController>();
        levelUnlocker = GameObject.FindGameObjectWithTag("LevelUnlocker").GetComponent<UnlockingLevels>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!dying)
        {
            if (canMove)
            {
                Move();
            }

            if (dashing)
            {
                TranslatePlayer();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !controller.m_Grounded && !floating && !hanging && (bloodScript.bloodValue >= 5))
            {
                bloodScript.SubtractBlood(bloodFloatingCost);
                floating = true;
                horizontalMove = 0;
                verticalMove = 0;
                rb.velocity = new Vector3(0, 0, 0);
                rb.gravityScale = gravityWhileFloating;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Die());
            }

            if (hanging && !currentHangPoint.GetComponent<HangPointScript>().open)
            {
                hanging = false;
                rb.gravityScale = 3;
                canMove = true;
            }

            if (controller.m_Grounded)
            {
                floating = false;
                rb.gravityScale = 3f;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && canMove && (dashNumber > 0) && !controller.m_Grounded)
            {
                canMove = false;
                StartCoroutine(Dash());
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !controller.m_Grounded && canMove && (bloodScript.bloodValue > 0))
            {
                bloodScript.SubtractBlood(bloodShootingCost);
                LaunchBat();
            }

            if (Input.GetKeyDown(KeyCode.W) && controller.m_Grounded)
            {
                jump = true;
            }
            else if (Input.GetKeyDown(KeyCode.W) && hanging)
            {
                jump = true;
                hanging = false;
                rb.gravityScale = 3;
                canMove = true;
            }

            if (hanging)
            {
                spriteRenderer.sprite = vampireHanging;
            }
            else if (floating)
            {
                spriteRenderer.sprite = vampireFloating;
            }
            else if (!controller.m_Grounded)
            {
                spriteRenderer.sprite = vampireJumping;
            }
            else if (Mathf.Abs(rb.velocity.x) > 0.05f)
            {
                spriteRenderer.sprite = vampireMoving;
            }
            else if (Mathf.Abs(rb.velocity.x) < 0.05f)
            {
                spriteRenderer.sprite = vampireIdle;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dying)
        {
            controller.Move(horizontalMove * movementSpeed * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }

    private void Move()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
    }

    public void IncreaseDashes()
    {
        dashNumber++;
    }

    public void RecoverBlood(int bloodToRecover)
    {
        bloodScript.RecoverBlood(bloodToRecover);
    }

    private Vector2 GetDashDirection()
    {
        Vector2 direction = new Vector2(0,0);

        direction.x = horizontalMove;
        direction.y = verticalMove;

        return direction;
    }

    private IEnumerator Dash()
    {
        dashNumber--;
        dashDirection = GetDashDirection();
        dashing = true;
        rb.gravityScale = 0;
        horizontalMove = 0;
        verticalMove = 0;
        rb.velocity = new Vector3(0, 0, 0);
        audioSource.PlayOneShot(dashSFX);
        yield return new WaitForSeconds(dashLength);
        floating = false;
        rb.gravityScale = 3;
        dashing = false;
        canMove = true;
    }

    private void TranslatePlayer()
    {
        transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
    }


    private void LaunchBat()
    {
        GameObject newBat = Instantiate(batProjectile, transform.position, transform.rotation);
        newBat.GetComponent<BatProjectile>().SetFlightDirection();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Damage"))
        {
            StartCoroutine(Die());
        }
        if (otherCollider.CompareTag("Hang"))
        {
            currentHangPoint = otherCollider.gameObject;
            Hang(otherCollider.transform.position, otherCollider.GetComponent<HangPointScript>());
        }
        if (otherCollider.CompareTag("Exit"))
        {
            levelUnlocker.UnlockLevel(levelLoader.GetCurrentSceneIndex());
            levelLoader.LoadNextScene();
        }
    }

    private void Hang(Vector2 hangTransform, HangPointScript hangScript)
    {
        if (hangScript.open)
        {
            hanging = true;
            rb.gravityScale = 0;
            horizontalMove = 0;
            verticalMove = 0;
            rb.velocity = new Vector3(0, 0, 0);
            canMove = false;
            transform.position = hangTransform;
        }
    }

    public IEnumerator Die()
    {
        dying = true;
        GameObject explosion = Instantiate(deathVFX, transform.position - new Vector3(0,0,-1), transform.rotation);
        Destroy(explosion, 1f);
        canMove = false;
        rb.gravityScale = 0;
        horizontalMove = 0;
        verticalMove = 0;
        rb.velocity = new Vector3(0, 0, 0);
        audioSource.PlayOneShot(deathSFX);
        spriteRenderer.sprite = vampireDying1;
        StartCoroutine(levelLoader.GetComponent<GameController>().WaitForRestart(2f));
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = vampireDying2;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = vampireDying3;
    }
}
