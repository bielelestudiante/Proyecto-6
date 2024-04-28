using UnityEngine;

public class CombatController : MonoBehaviour
{
    public float meleeRange = 2f; // Alcance del golpe cuerpo a cuerpo
    public LayerMask targetLayer; // Capa de los objetivos contra los que se puede golpear

    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Detectar la direcci�n hacia donde apunta el rat�n
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0f; // Ignorar la componente Y para que el personaje no mire hacia arriba o abajo

            // Girar el personaje hacia la direcci�n del rat�n
            transform.rotation = Quaternion.LookRotation(targetDirection);
        }

        // Realizar un ataque cuerpo a cuerpo cuando se presione el bot�n de ataque
        if (Input.GetButtonDown("Fire1")) // Cambiar "Fire1" seg�n el input que uses para el ataque
        {
            MeleeAttack();
        }
    }

    void MeleeAttack()
    {
        // Detectar los objetivos dentro del alcance del golpe
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRange, targetLayer);

        // Realizar el ataque en todos los objetivos dentro del alcance
        foreach (Collider collider in hitColliders)
        {
            EnemyHealthSystem enemyHealth = collider.GetComponent<EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                // Aplicar da�o al enemigo
                enemyHealth.TakeDamage(20); // Ajusta "damageAmount" seg�n la cantidad de da�o que quieras que haga el ataque
            }
        }

        // Activar la animaci�n de ataque en el Animator
        animator.SetTrigger("Attack");
    }
}
