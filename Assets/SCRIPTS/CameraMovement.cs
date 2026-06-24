//using UnityEngine;

//public class CameraMovement : MonoBehaviour
//{
//    [SerializeField] private float _moveSpeed;

//    private CharacterController controller;
//    private Vector3 veloc;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Vector3 moveDirection = transform.right * (Input.GetAxis("Horizontal")) + transform.forward * (Input.GetAxis("Vertical"));

//        controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
//    }


//}
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private Vector3 veloc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        veloc = new Vector3(Input.GetAxis("Horizontal"), transform.position.y , Input.GetAxis("Vertical"));

        transform.position += veloc * Time.deltaTime * _moveSpeed;
    }

}

