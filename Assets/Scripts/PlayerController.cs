using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    public int Health;
    public int maxHealth = 100;

    [Header("Movement")]
    public float speed;
    public float gravity = -9.81f;
    Vector3 velocity;  

    [Header("References")]
    public CharacterController controller;
    Animator anim;

    [Header("Aim")]
    public float scopedFOV = 20;
    private float normalFOV;
    private bool aim = false;
    public GameObject scopeOverLay;
    public Camera CameraAim;

    [Header("Texture")]
    public Texture2D crosshair;

    [Header("Ik")]
    public Transform RightHandTransform;
    public Transform LeftHandTransform;
    public Transform CameraTransform;

    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Health = maxHealth;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        Aim();
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(horizontal, 0f, vertical);
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        controller.Move(movement * speed * Time.deltaTime);

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //Moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }

        if (Input.GetKey(KeyCode.LeftShift) && !aim)
        {
            movement.Normalize();
            movement *= speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }


        //Animating
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        anim.SetFloat("InputVertical", velocityZ, 0.1f, Time.deltaTime);
        anim.SetFloat("InputHorizontal", velocityX, 0.1f, Time.deltaTime);
    }

    void Aim()
    {       
        if (Input.GetButtonDown("Fire2"))
        {
            aim = !aim;
            anim.SetBool("Aim", aim);

            if (aim)
            {
                StartCoroutine(ToggleAimOn());
            }
            else
            {
                StartCoroutine(ToggleAimOff());
            }         
        }    
    }

    private IEnumerator ToggleAimOn()
    {
        yield return new WaitForSeconds(0.25f);
        scopeOverLay.SetActive(true);
        
        normalFOV = CameraAim.fieldOfView;
        CameraAim.fieldOfView = scopedFOV;
    }
    private IEnumerator ToggleAimOff()
    {   
        yield return new WaitForSeconds(0.15f);
        scopeOverLay.SetActive(false);
   
        CameraAim.fieldOfView = normalFOV;
    }

    private void OnAnimatorIK()
    {
        // Weapon Aim at Target IK
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandTransform.position);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTransform.position);

        // Looking at target IK
        anim.SetLookAtWeight(1);
        anim.SetLookAtPosition(CameraTransform.position);
    }

    public void TakingDamage(int value)
    {
        Health -= value;

        if (Health <= 0)
        {
            anim.SetTrigger("isDeath");
            StartCoroutine(Coroutine());
        }
    }

    IEnumerator Coroutine()
    {       
        yield return new WaitForSeconds(3f);
        GameOver.ShowGameOver();
    }

}
