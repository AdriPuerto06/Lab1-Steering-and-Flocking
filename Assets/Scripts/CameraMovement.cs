
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1;

        if (Input.GetKey(KeyCode.RightArrow))
            horizontal = 1;

        if (Input.GetKey(KeyCode.UpArrow))
            vertical = 1;

        if (Input.GetKey(KeyCode.DownArrow))
            vertical = -1;

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
