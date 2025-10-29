using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public TextMeshPro nameText;

    [SerializeField]
    public Cooldown cooldown;

    [Header("Inputs")]
    [Space]
    public InputAction playerMovementControls;
    public InputAction playerEchoControl;
    public InputAction playerActionControl;
    public InputAction playerAttackControl;
    public InputAction playerMapControl;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float unequipedMoveSpeed = 6f;
    private float currentMoveSpeed = 4f;
    private Vector2 moveDirection = Vector2.zero;
    public bool wantMap = false;

    [Header("Inventory")]
    public SC_Item torche;
    public Transform objectHolder;
    public GameObject itemObj;
    private bool closeToItem = false;
    private GameObject itemCloseToPlayer;

    [Header("Echos")]
    public float echoCooldown = 40f;
    public AudioClip deathEcho;
    public GameObject echoObject;
    public bool isRecording = false;
    public float recordingTime = 0f;

    private void Start()
    {
        itemObj = Instantiate<GameObject>(torche.itemPrefab);
        itemObj.GetComponent<CircleCollider2D>().enabled = false;
        itemObj.transform.SetParent(objectHolder, false);
        nameText.text = GameManager.instance.currentPlayerName;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRecording)
        {
            recordingTime += Time.deltaTime;
        }

        if(!wantMap)
        {
            moveDirection = playerMovementControls.ReadValue<Vector2>();
        }
        else
        {
            moveDirection = Vector2.zero;
        }
        if (itemObj)
        {
            currentMoveSpeed = moveSpeed;
        }
        else
        {
            currentMoveSpeed = unequipedMoveSpeed;
        }
        animator.SetFloat("X", moveDirection.x);
        animator.SetFloat("Y", moveDirection.y);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * currentMoveSpeed, moveDirection.y * currentMoveSpeed);
    }

    public void RecordEcho(InputAction.CallbackContext context)
    {
        if (cooldown.IsCoolingDown) return;

        isRecording = true;
        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 10;

        deathEcho = Microphone.Start(device, false, lengthSec, sampleRate);
    }

    public void StopEchoRecord(InputAction.CallbackContext context)
    {
        if (cooldown.IsCoolingDown) return;

        Microphone.End(null);

        isRecording = false;
        recordingTime = 0f;
        GameObject echo = Instantiate<GameObject>(echoObject, transform.position, transform.rotation);
        echo.GetComponent<EchoListener>().CreateEcho(deathEcho);
        GameManager.instance.AddDeathVocal(deathEcho);

        deathEcho = null;
        cooldown.StartCooldown(echoCooldown);
    }



    // Actions

    public void Action(InputAction.CallbackContext context)
    {
        if (playerActionControl != null)
        {
            if (itemObj)
            {
                DropItem();
            }
            else
            {
                TakeItem();
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        animator.SetBool("kick", true);
    }

    public void DropItem()
    {
        if(itemObj)
        {
            itemObj.transform.SetParent(null);
            itemObj.GetComponent<CircleCollider2D>().enabled = true;
            itemObj = null;
        }
    }

    public void TakeItem()
    {
        if (closeToItem && itemCloseToPlayer)
        {
            itemObj = itemCloseToPlayer;
            itemObj.transform.SetParent(objectHolder);
            itemObj.GetComponent<CircleCollider2D>().enabled = false;
            itemObj.transform.transform.localPosition = Vector3.zero;
            closeToItem = false;
        }
    }

    public void OpenMap(InputAction.CallbackContext context)
    {
        wantMap = true;
    }

    public void CloseMap(InputAction.CallbackContext context)
    {
        wantMap=false;
    }



    // PlayerActions

    private void OnEnable()
    {
        playerMovementControls.Enable();
        playerEchoControl.Enable();
        playerActionControl.Enable();
        playerAttackControl.Enable();
        playerMapControl.Enable();



        playerActionControl.performed += Action;
        playerEchoControl.performed += RecordEcho;
        playerEchoControl.canceled += StopEchoRecord;
        playerAttackControl.performed += Attack;
        playerMapControl.performed += OpenMap;
        playerMapControl.canceled += CloseMap;
    }

    private void OnDisable()
    {
        playerMovementControls.Disable();
        playerEchoControl.Disable();
        playerActionControl.Disable();
        playerAttackControl.Disable();
        playerMapControl.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Torche")
        {
            closeToItem = true;
            itemCloseToPlayer = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        closeToItem = false;
        itemCloseToPlayer = null;
    }
}
