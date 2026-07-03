using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform rotateChild;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Camera cam;
    public Vector2 mouseScreenPos;
    public Vector3 dir;

    public void MousePosition(InputAction.CallbackContext context)
    {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(mouseScreenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            dir = targetPoint - this.gameObject.transform.position;
            dir.y = 0;
        }
    }
}
