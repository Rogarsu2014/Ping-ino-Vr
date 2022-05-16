using UnityEngine;
using UnityEngine.InputSystem;

public class GripAnimacion : MonoBehaviour
{
    [SerializeField] InputActionReference actionGrip; //Referencia a la acci�n de agarrar
    private Animator handAnimator; //El animador del prefab
    private void Awake() //Al inicializar el objeto
    {
        actionGrip.action.performed += GripPress; //A�ade la funci�n
        handAnimator = GetComponent<Animator>(); //Coge el animador
    }
    private void GripPress(InputAction.CallbackContext obj) //Acci�n que hace el grip
    {
        if(handAnimator == null) //Si no existe animador lo coge
        {
            handAnimator = GetComponent<Animator>();
        }
        handAnimator.SetFloat("Grip", obj.ReadValue<float>()); //Cambia el valor del grip en funci�n del valor del grip
    }

    private void OnDestroy() //Cuando se destruye el objeto
    {
        actionGrip.action.performed -= GripPress; //Retira la funci�n de la acci�n
    }
}
