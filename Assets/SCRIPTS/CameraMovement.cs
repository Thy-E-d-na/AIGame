
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _zoomSpeed;
    private Vector3 veloc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CamRotate();
        Zoom();
    }
    void Move()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        veloc = transform.forward * inputY + transform.right * inputX;
        transform.position += veloc * Time.deltaTime * _moveSpeed;
    }

    void CamRotate()
    {
        if(Input.GetMouseButton(2))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, _rotateSpeed * mouseX * Time.deltaTime);
        }
    }
    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(Mathf.Abs(scroll) > 0.01f)
        {
            var dir = -transform.forward + transform.up;
            transform.position += -dir * scroll * _zoomSpeed;
        }
    }
}

