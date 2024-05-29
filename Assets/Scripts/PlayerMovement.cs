using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int playerID;
    [SerializeField] KeyCode downKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode upKey;
    [SerializeField] KeyCode sprintKey;
    [SerializeField] KeyCode shootKey;
    [SerializeField] GameObject projectilePrefab;

    float shootCooldown = 1f;
    float gridSize = 1.0f;
    float projectileSpeed = 5.0f;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection; 
    private bool canShoot = true;
    private bool isMoving = false;

    [Header("Rotation")]
    Vector3 rotateLeft = new Vector3(0, 0, 90);
    Vector3 rotateRight = new Vector3(0, 0, -90);
    Vector3 rotateDown = new Vector3(0, 0, 180);
    Vector3 rotateUp = new Vector3(0, 0, 0);

    [SerializeField] Canvas victoryCanvas;
    [SerializeField] Text victoryText;



    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }


    private void HandleMovement()
    {
        if (isMoving) return;

        moveDirection = Vector2.zero;

        if (Input.GetKeyDown(upKey))
        {
            moveDirection = Vector2.up;
            transform.rotation = Quaternion.Euler(rotateUp); 
        }
        else if (Input.GetKeyDown(downKey))
        {
            moveDirection = Vector2.down;
            transform.rotation = Quaternion.Euler(rotateDown); 
        }
        else if (Input.GetKeyDown(leftKey))
        {
            moveDirection = Vector2.left;
            transform.rotation = Quaternion.Euler(rotateLeft); 
        }
        else if (Input.GetKeyDown(rightKey))
        {
            moveDirection = Vector2.right;
            transform.rotation = Quaternion.Euler(rotateRight); 
        }

        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection;
            StartCoroutine(MovePlayer());
        }
    }

    private void HandleShooting()
    {
        if (canShoot && Input.GetKeyDown(shootKey))
        {
            StartCoroutine(ShootCooldown());
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            BulletMovement bulletMovement = projectile.GetComponent<BulletMovement>();
            if (bulletMovement != null)
            {
                bulletMovement.SetSpeedAndDirection(projectileSpeed, lastMoveDirection.normalized); 
            }
        }
    }

    private IEnumerator MovePlayer()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(moveDirection.x, moveDirection.y) * gridSize;

        transform.position = endPosition;
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
    }


    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }


//intento de poner canvas de win xdddd
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            HandleCollisionWithProjectile(other.gameObject);
        }
    }

    private void HandleCollisionWithProjectile(GameObject projectile)
    {
        if (gameObject.CompareTag("Player")) 
        {
            int shooterID = playerID;
  
            if (shooterID != playerID)
            {
                string winner;
                if (shooterID == 1)
                {
                    winner = "Jugador 1";
                }
                else
                {
                    winner = "Jugador 2";
                }
                victoryText.text = winner + " ha ganado!";
                victoryCanvas.enabled = true;
                Time.timeScale = 0f;
            }
        }
    }

}



