
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float currentSpeed = 0;

    void Update()
    {
        float translation = 0;
        float rotation = 0;

        if (Input.GetKey(KeyCode.W))
            translation = speed;

        if (Input.GetKey(KeyCode.S))
            translation = -speed;

        if (Input.GetKey(KeyCode.A))
            rotation = -rotationSpeed;

        if (Input.GetKey(KeyCode.D))
            rotation = rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        currentSpeed = translation;
    }
}
