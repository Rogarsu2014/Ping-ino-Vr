using UnityEngine;
using UnityEngine.InputSystem;

public class GripAnimacion : MonoBehaviour
{
    [SerializeField] InputActionReference actionGrip;
    private Animator handAnimator;
    private void Awake()
    {
        actionGrip.action.performed += GripPress;
        handAnimator = GetComponent<Animator>();
    }
    private void GripPress(InputAction.CallbackContext obj)
    {
        if(handAnimator == null)
        {
            handAnimator = GetComponent<Animator>();
        }
        handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    }
}
