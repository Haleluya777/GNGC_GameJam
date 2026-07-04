using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Unit unit;
    public Vector2 dir;

    private void Update()
    {
        if (unit.state != Unit.UnitState.Attacking)
        {
            PerformMove();
            if (dir != Vector2.zero)
                unit.state = Unit.UnitState.Moving;
            else
                unit.state = Unit.UnitState.Idle;
        }
    }

    public void PerformMove()
    {
        Vector3 movement = new Vector3(dir.x, 0f, dir.y);
        rigid.MovePosition(rigid.position + movement * 10f * Time.deltaTime);
        //rigid.velocity = Vector3.forward * Time.deltaTime * 5f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && unit.state != Unit.UnitState.Attacking)
        {
            dir = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled && unit.state != Unit.UnitState.Attacking)
        {
            dir = Vector2.zero;
            //unit.state = Unit.UnitState.Idle;
        }
    }
}
