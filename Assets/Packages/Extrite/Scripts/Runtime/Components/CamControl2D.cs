using UnityEngine;
using UnityEngine.EventSystems;

public class CamControl2D : MonoBehaviour
{
    public float speedRegular = 5.0f;
    public float speedFast = 10.0f;

    public float zoomSpeed = 5.0f;

    public bool allowControl = true;

    void Update()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? speedFast : speedRegular;

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            allowControl = false;
        }
        else
        {
            allowControl = true;
        }

        // Pan
        if (allowControl)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }

            // Zoom
            // With the mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                Camera.main.orthographicSize -= scroll * zoomSpeed;
            }

            // With the keyboard
            if (Input.GetKey(KeyCode.Q))
            {
                Camera.main.orthographicSize += zoomSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E))
            {
                Camera.main.orthographicSize -= zoomSpeed * Time.deltaTime;
            }

            // Clamp zoom
            if (Camera.main.orthographicSize < 1.0f)
            {
                Camera.main.orthographicSize = 1.0f;
            }

            // Reset position and zoom
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
                Camera.main.orthographicSize = 5.0f;
            }
        }


    }
}
