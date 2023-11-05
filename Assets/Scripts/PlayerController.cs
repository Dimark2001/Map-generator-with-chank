using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) return;
        var dirX = Input.GetAxis("Horizontal");
        var dirY = Input.GetAxis("Vertical");
        var moveVector = new Vector3(dirX, 0, dirY) * speed / 100;
        rb.MovePosition(transform.position + moveVector);
    }
}
