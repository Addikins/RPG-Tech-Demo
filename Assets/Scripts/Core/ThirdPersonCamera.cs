using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] float scrollSpeed = 6f;
    [SerializeField] Transform target = null;
    [SerializeField] float distanceFromTarget = 2f;
    [SerializeField] Vector2 distanceMinMax = new Vector2 (2f, 15f);
    [SerializeField] Vector2 verticalMinMax = new Vector2 (-5, 85);
    [SerializeField] float rotationSmoothTime = .12f;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float mouseX, mouseY, mouseScroll;

    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate () {
        CameraMovement ();
    }

    void CameraMovement () {
        mouseScroll -= Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
        mouseX += Input.GetAxis ("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis ("Mouse Y") * rotationSpeed;
        mouseScroll = Mathf.Clamp (mouseScroll, distanceMinMax.x, distanceMinMax.y);
        mouseY = Mathf.Clamp (mouseY, verticalMinMax.x, verticalMinMax.y);

        currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (mouseY, mouseX), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        distanceFromTarget = mouseScroll;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}