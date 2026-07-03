using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Vector2 dir;

    private void Update()
    {
        PerformMove();
    }

    public void PerformMove()
    {
        Vector3 movement = new Vector3(dir.x, 0f, dir.y);
        rigid.MovePosition(rigid.position + movement * 5f * Time.deltaTime);
        //rigid.velocity = Vector3.forward * Time.deltaTime * 5f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("엘렐레");
        if (context.phase == InputActionPhase.Performed)
        {
            dir = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            dir = Vector2.zero;
        }
    }
}
