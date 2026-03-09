using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Vector3 carVelocity;
    public float speed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessMovement(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        transform.Translate(moveDirection * speed * Time.deltaTime); 
    }
}
