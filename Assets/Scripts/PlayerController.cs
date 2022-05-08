using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;

    // Awake is called before Start. Initialize properies here
    private void Awake()
    {
        //body = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xValue = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float zValue = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(xValue, 0f, zValue);
    }
}
