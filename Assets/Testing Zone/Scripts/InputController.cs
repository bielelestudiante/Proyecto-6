using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction dashAction;
    InputAction pegarAction;

    PlayerCombat playerCombat;

    [SerializeField] float speed = 5f;
    [SerializeField] float gravity = 9.81f; // Gravedad en m/s^2
    [SerializeField] float forceMultiplier = 0.1f; // Multiplicador de fuerza
    //[SerializeField] float dashDistance = 5f; // Distancia del dash
    [SerializeField] float dashCooldown = 1f; // Tiempo de espera para volver a dashear
    //[SerializeField] float dashDuration = 0.2f; // Espera del dash en segundos
    [SerializeField] float rotationSpeed = 10f;

    CharacterController characterController;
    Camera mainCamera;

    private Animator anim;
    Vector3 velocity; // Vector de velocidad para la gravedad
    bool isDashing = false; // Bandera para controlar si se est� realizando un dash
    bool isPegando = false;
    bool isMoving = false;
    internal static object instance;

    //private bool isMousePressed = false;
    //private bool canRotate = true; // Indica si el personaje puede rotar hacia la direcci�n del rat�n

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        dashAction = playerInput.actions.FindAction("Dash");
        pegarAction = playerInput.actions.FindAction("Pegar");
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main; // Obtenemos la c�mara principal
        moveAction.Enable();
        dashAction.Enable();
        anim= GetComponent<Animator>();

        playerCombat = GetComponent<PlayerCombat>();
    }

    //IEnumerator CooldownRotation()
    //{
    //canRotate = false; // El personaje no puede rotar
    //yield return new WaitForSeconds(3f); // Espera 3 segundos
    //canRotate = true; // El personaje puede rotar nuevamente
    //}
    void Update()
    {
        MovePlayer(); // Mueve al personaje en un eje fijo

        // Verifica si se ha presionado el bot�n de dash y realiza el dash si es as�
        //if (dashAction.triggered && !isDashing)
        //{
        //    StartCoroutine(Dash());
        //}

        if (pegarAction.triggered && !isPegando)
        {
            // Llamar a la funci�n de ataque del PlayerCombat
            playerCombat.Attack();
        }

        // Si el bot�n izquierdo del mouse est� presionado y si el personaje puede rotar
        //if (Mouse.current.leftButton.isPressed && !isMousePressed && canRotate)
        //{
        //isMousePressed = true;
        //RotatePlayerTowardsMouse(); // Llama a la funci�n para que el personaje mire hacia el rat�n
        //StartCoroutine(CooldownRotation()); // Inicia la corrutina de tiempo de espera
        //}

        // Restablece la variable isMousePressed cuando se suelta el bot�n izquierdo del mouse
        //if (!Mouse.current.leftButton.isPressed && isMousePressed)
        //{
        //isMousePressed = false;
        //}
    }

    //void RotatePlayerTowardsMouse()
    //{
    //    // Obtiene la posici�n del rat�n en la pantalla
    //    Vector3 mousePosition = Mouse.current.position.ReadValue();

    //    // Convierte la posici�n del rat�n de la pantalla a un rayo en el espacio del juego
    //    Ray ray = mainCamera.ScreenPointToRay(mousePosition);
    //    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    //    float rayLength;

    //    // Si el rayo intersecta el plano del suelo, calcula la longitud del rayo
    //    if (groundPlane.Raycast(ray, out rayLength))
    //    {
    //        // Obtiene el punto de intersecci�n del rayo con el plano
    //        Vector3 pointToLook = ray.GetPoint(rayLength);

    //        // Calcula la direcci�n desde el personaje hacia el punto de intersecci�n
    //        Vector3 lookDirection = pointToLook - transform.position;
    //        lookDirection.y = 0; // Asegura que el personaje no mire hacia arriba o abajo

    //        // Si la direcci�n es v�lida (no es el vector cero)
    //        if (lookDirection.sqrMagnitude > 0.001f)
    //        {
    //            // Rota el personaje hacia el rat�n
    //            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
    //            transform.rotation = lookRotation;
    //        }
    //    }
    //}

    void MovePlayer()
    {
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);

        // Si hay entrada de movimiento
        if (moveDirection.magnitude > 0.1f)
        {
            // Actualiza la variable isMoving
            isMoving = true;

            // Calcula la rotaci�n hacia la direcci�n del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            //Interpola suavemente la rotaci�n actual hacia la rotaci�n objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            

            // Normaliza el vector de movimiento para evitar la velocidad adicional
            moveDirection.Normalize();

            // Mueve al personaje en la direcci�n de entrada basada en las teclas de direcci�n o el joystick
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
        else
        {
            // Actualiza la variable isMoving
            isMoving = false;
        }

        ApplyGravity();

        // Actualiza las animaciones
        anim.SetFloat("VelX", moveDirection.x);
        anim.SetFloat("VelY", moveDirection.z);
    }


    ////IEnumerator Dash()
    ////{

    ////    if (isDashing) yield break; // Si ya se est� realizando un dash o el jugador no est� en movimiento, sale del m�todo

    ////    isDashing = true; // Indica que se est� realizando un dash

    ////    // Activar la animaci�n de Esquivar al comenzar el dash
    ////    anim.SetBool("Esquivar", true);

    ////    // Desactivar las acciones de movimiento al comenzar el dash
    ////    moveAction.Disable();

    ////    if (!isDashing) yield break; // Si ya se est� realizando un dash o el jugador no est� en movimiento, sale del m�todo
    ////    // Espera a que termine la animaci�n de Esquivar
    ////    yield return new WaitUntil(() => !anim.GetCurrentAnimatorStateInfo(0).IsName("Esquivar"));

    ////    // Desactivar la animaci�n de Esquivar al finalizar el dash
    ////    anim.SetBool("Esquivar", false);

    ////    // Espera el tiempo de cooldown antes de permitir otro dash
    ////    yield return new WaitForSeconds(dashCooldown);

    ////    isDashing = false;
    ////}
    ////public void OnDashAnimationEnd()
    ////{
    ////    // Reactivar las acciones de movimiento al finalizar el dash
    ////    moveAction.Enable();
    ////}





    void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity.y = 0f;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;
        if (rigidbody != null && !rigidbody.isKinematic)
        {
            Vector3 force = hit.moveDirection * speed * forceMultiplier;
            rigidbody.AddForce(force, ForceMode.Acceleration);
        }
    }

    void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
    }

    void OnDestroy()
    {
        moveAction.Disable();
        dashAction.Disable();
    }
}
