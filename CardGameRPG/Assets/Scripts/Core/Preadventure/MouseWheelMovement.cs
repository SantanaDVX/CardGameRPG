using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWheelMovement : MonoBehaviour {

    public float mouseWheelForceFactor;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.AddForce(transform.forward * mouseWheelForceFactor * Input.GetAxis("Mouse ScrollWheel"));
    }
}
