/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    public float originalMoveSpeed;

    [Header("Run Settings")]
    
    public float runSpeed = 10f;
    

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float RunCost;
    public float ChargeRate;
    

    public bool isRunning = false;

    private Coroutine recharge;
    int count = 0;

    AudioManager audioManager;
       
   
    private void Awake()
    {
       audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

  
    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
       
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

    }
    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
           audioManager.PlaySFX(audioManager.jump);
        }

    }
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
                if (pickUp != null)
                {
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }

        if ( isRunning)
        {
            Stamina -= RunCost * Time.deltaTime;
            

            if (Stamina < 0)
            {
                Stamina = 0;
                moveSpeed = originalMoveSpeed;
            }
            StaminaBar.fillAmount = Stamina / MaxStamina;

            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }

        if(isRunning && Stamina <= 0)
        {
            GetComponent<PlayerStats>().TakeDamage(0.1f);  // Damage player when stamina depletes (exp)
           
            if(count == 0)
            {
                audioManager.PlaySFX(audioManager.Damage);
                count = 1;
            }
               
            
            
        }

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {

        if (context.performed )
        {
           
            moveSpeed = runSpeed;

            isRunning = true;

           
            
            
        }
        else if (context.canceled)
        {
            
            moveSpeed = originalMoveSpeed;
            isRunning = false;
        }
    }
    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate /10f;
            count = 0;

            if (Stamina > MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(0.1f);

        }
       
    }
}
*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    public float originalMoveSpeed;

    [Header("Run Settings")]
    public float runSpeed = 10f;

    [Header("Stamina Settings")]
    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float RunCost;
    public float ChargeRate;

    [Header("Dialogue Settings")]
    public GameObject dialogueSpace;
    public TextMeshProUGUI dialogueText;
    public GameObject nextButton;
    public float wordSpeed = 0.05f;

    private string[] currentDialogue;
    private int dialogueIndex;
    private bool isTyping = false;
    private bool playerIsCloseToNPC = false;
    private Npc currentNPC;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    public bool isRunning = false;

    private Coroutine recharge;
    private int count = 0;

    // Input Actions
    private PlayerInput playerInput;
    private InputAction dialogueNextAction;
    private InputAction dialogueSkipAction;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;
        playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get dialogue input actions
        dialogueNextAction = playerInput.actions["DialogueNext"];
        dialogueSkipAction = playerInput.actions["DialogueSkip"];
    }

    private void OnEnable()
    {
        // Enable dialogue input actions
        if (dialogueNextAction != null)
        {
            dialogueNextAction.performed += OnDialogueNext;
            dialogueNextAction.Enable();
        }

        if (dialogueSkipAction != null)
        {
            dialogueSkipAction.performed += OnDialogueSkip;
            dialogueSkipAction.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable dialogue input actions
        if (dialogueNextAction != null)
        {
            dialogueNextAction.performed -= OnDialogueNext;
            dialogueNextAction.Disable();
        }

        if (dialogueSkipAction != null)
        {
            dialogueSkipAction.performed -= OnDialogueSkip;
            dialogueSkipAction.Disable();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Only allow movement if not in dialogue
        if (!dialogueSpace.activeInHierarchy)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero; // Stop movement during dialogue
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Only process look input if not in dialogue
        if (!dialogueSpace.activeInHierarchy)
        {
            lookInput = context.ReadValue<Vector2>();
        }
        else
        {
            lookInput = Vector2.zero; // Stop camera movement during dialogue
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded && !dialogueSpace.activeInHierarchy)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioManager.PlaySFX(audioManager.jump);
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed || dialogueSpace.activeInHierarchy) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
                if (pickUp != null)
                {
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (dialogueSpace.activeInHierarchy) return;

        if (context.performed)
        {
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (dialogueSpace.activeInHierarchy) return;

        if (context.performed)
        {
            moveSpeed = runSpeed;
            isRunning = true;
        }
        else if (context.canceled)
        {
            moveSpeed = originalMoveSpeed;
            isRunning = false;
        }
    }

    // Dialogue input handlers
    public void OnDialogueNext(InputAction.CallbackContext context)
    {
        if (!context.performed || !dialogueSpace.activeInHierarchy) return;

        if (!isTyping)
        {
            NextLine();
        }
    }

    public void OnDialogueSkip(InputAction.CallbackContext context)
    {
        if (!context.performed || !dialogueSpace.activeInHierarchy) return;

        if (isTyping)
        {
            SkipTyping();
        }
    }

    private void Update()
    {
        HandleMovement();

        // Only handle look if not in dialogue
        if (!dialogueSpace.activeInHierarchy)
        {
            HandleLook();
        }

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }

        // Update next button visibility
        if (dialogueSpace.activeInHierarchy && dialogueText.text == currentDialogue[dialogueIndex])
        {
            nextButton.SetActive(true);
        }

        if (isRunning)
        {
            Stamina -= RunCost * Time.deltaTime;

            if (Stamina < 0)
            {
                Stamina = 0;
                moveSpeed = originalMoveSpeed;
            }
            StaminaBar.fillAmount = Stamina / MaxStamina;

            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }

        if (isRunning && Stamina <= 0)
        {
            GetComponent<PlayerStats>().TakeDamage(0.1f);

            if (count == 0)
            {
                audioManager.PlaySFX(audioManager.Damage);
                count = 1;
            }
        }
    }

    public void HandleMovement()
    {
        // Reduce movement speed during dialogue or stop completely
        float currentMoveSpeed = dialogueSpace.activeInHierarchy ? moveSpeed * 0.1f : moveSpeed;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentMoveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 10f;
            count = 0;

            if (Stamina > MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Dialogue Methods
    public void StartDialogue(string[] dialogue, Npc npc)
    {
        if (dialogueSpace.activeInHierarchy) return;

        currentDialogue = dialogue;
        currentNPC = npc;
        dialogueIndex = 0;
        dialogueSpace.SetActive(true);
        nextButton.SetActive(false);
        StartCoroutine(Typing());

        // Switch cursor mode
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Switch to UI action map for better input handling
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void NextLine()
    {
        nextButton.SetActive(false);
        if (dialogueIndex < currentDialogue.Length - 1)
        {
            dialogueIndex++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        dialogueIndex = 0;
        dialogueSpace.SetActive(false);

        // Switch back to gameplay mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Switch back to Player action map
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        if (currentNPC != null)
        {
            currentNPC.OnDialogueEnd();
            currentNPC = null;
        }
    }

    IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in currentDialogue[dialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    private void SkipTyping()
    {
        StopAllCoroutines();
        dialogueText.text = currentDialogue[dialogueIndex];
        isTyping = false;
        nextButton.SetActive(true);
    }

    // NPC Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            playerIsCloseToNPC = true;
            Npc npc = other.GetComponent<Npc>();
            if (npc != null && !dialogueSpace.activeInHierarchy)
            {
                StartDialogue(npc.dialogue, npc);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            playerIsCloseToNPC = false;
            if (currentNPC != null && currentNPC.gameObject == other.gameObject)
            {
                ZeroText();
            }
        }
    }

    public bool CanStartDialogue()
    {
        return playerIsCloseToNPC && !dialogueSpace.activeInHierarchy;
    }
}