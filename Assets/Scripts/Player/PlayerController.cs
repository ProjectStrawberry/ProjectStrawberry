//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;

//public class PlayerController : BaseController
//{
    
    
//    public bool isGround;

//    private void OnCollisionEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Ground"))
//        {
//            isGround = true;
//        }
//    }

//    protected override void HandleAction()
//    {

//    }

//    public override void Death()
//    {
//        base.Death();
        
//    }

//    public void OnMoveInput(InputAction.CallbackContext context)
//    {
//        if (context.phase == InputActionPhase.Performed)
//        {
//            movementDirection = context.ReadValue<Vector2>();
//        }
//        else if (context.phase == InputActionPhase.Canceled)
//        {
//            movementDirection = Vector2.zero;
//        }
//    }

//    public void OnJumpInput(InputAction.CallbackContext context)
//    {
//        if (context.phase == InputActionPhase.Performed && isGround)
//        {
            
//        }
//    }
//    void OnFire(InputValue inputValue)
//    {
//        if (EventSystem.current.IsPointerOverGameObject())
//            return;

//        isAttacking = inputValue.isPressed;
//    }

//}
