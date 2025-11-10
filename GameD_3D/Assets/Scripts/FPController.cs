
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    private string[] currentDialogue;
    private int dialogueIndex;
    private bool isTyping = false;
    private bool playerIsCloseToNPC = false;
    private Npc currentNPC;
    private bool isPaused = false;

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
    private InputAction pauseAction;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;
        playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get input actions
        dialogueNextAction = playerInput.actions["DialogueNext"];
        dialogueSkipAction = playerInput.actions["DialogueSkip"];
        pauseAction = playerInput.actions["Pause"];
    }

    private void OnEnable()
    {
        // Enable input actions
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

        if (pauseAction != null)
        {
            pauseAction.performed += OnPause;
            pauseAction.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable input actions
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

        if (pauseAction != null)
        {
            pauseAction.performed -= OnPause;
            pauseAction.Disable();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Always allow movement, even during dialogue
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Only process look input if not in dialogue (to prevent camera movement during dialogue)
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
        if (context.performed && controller.isGrounded && !isPaused)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioManager.PlaySFX(audioManager.jump);
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed || isPaused) return;

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
        if (isPaused) return;

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
        if (isPaused) return;

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

    // Pause input handler
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    // Dialogue input handlers
    public void OnDialogueNext(InputAction.CallbackContext context)
    {
        if (!context.performed || !dialogueSpace.activeInHierarchy || isPaused) return;

        if (!isTyping)
        {
            NextLine();
        }
    }

    public void OnDialogueSkip(InputAction.CallbackContext context)
    {
        if (!context.performed || !dialogueSpace.activeInHierarchy || isPaused) return;

        if (isTyping)
        {
            SkipTyping();
        }
    }

    private void Update()
    {
        // Only process gameplay updates if not paused
        if (!isPaused)
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
    }

    public void HandleMovement()
    {
        // Slightly reduce movement speed during dialogue for better readability
        float currentMoveSpeed = dialogueSpace.activeInHierarchy ? moveSpeed * 0.7f : moveSpeed;

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

    // Pause Menu Methods
    public void Pause()
    {
        if (dialogueSpace.activeInHierarchy) return; // Can't pause during dialogue

        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

        // Switch cursor mode
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Switch to UI action map
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        // Switch back to gameplay cursor mode
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Switch back to Player action map
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Dialogue Methods
    public void StartDialogue(string[] dialogue, Npc npc)
    {
        if (dialogueSpace.activeInHierarchy || isPaused) return;

        currentDialogue = dialogue;
        currentNPC = npc;
        dialogueIndex = 0;
        dialogueSpace.SetActive(true);
        nextButton.SetActive(false);
        StartCoroutine(Typing());

        // Keep cursor locked during dialogue since player can still move
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Stay in Player action map to allow movement during dialogue
        // No need to switch action maps since we want to maintain movement control
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

        // Ensure cursor remains locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
            if (npc != null && !dialogueSpace.activeInHierarchy && !isPaused)
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
            // Don't automatically end dialogue when player moves away
            // Let the player finish reading the dialogue at their own pace
        }
    }

    public bool CanStartDialogue()
    {
        return playerIsCloseToNPC && !dialogueSpace.activeInHierarchy && !isPaused;
    }
}