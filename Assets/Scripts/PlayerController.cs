using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            var dirX = Input.GetAxisRaw("Horizontal");
            var dirY = Input.GetAxisRaw("Vertical");
            var moveVector = new Vector3(dirX, 0, dirY) * speed * Time.deltaTime;
            rb.MovePosition(transform.position + moveVector);
        }
    }
}
