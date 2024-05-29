using System.Collections;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    float speed = 45;
    Rigidbody2D rb;

    public void SetSpeedAndDirection(float speed, Vector3 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        
        StartCoroutine(DestroyAfterDelay(2.5f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
