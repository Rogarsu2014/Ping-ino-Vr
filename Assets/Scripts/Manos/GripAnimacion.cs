using UnityEngine;
using UnityEngine.InputSystem;

public class GripAnimacion : MonoBehaviour
{
    [SerializeField] InputActionReference actionGrip; //Referencia a la acción de agarrar
    private Animator handAnimator; //El animador del prefab
    private void Awake() //Al inicializar el objeto
    {
        actionGrip.action.performed += GripPress; //Añade la función
        handAnimator = GetComponent<Animator>(); //Coge el animador
    }
    private void GripPress(InputAction.CallbackContext obj) //Acción que hace el grip
    {
        if(handAnimator == null) //Si no existe animador lo coge
        {
            handAnimator = GetComponent<Animator>();
        }
        handAnimator.SetFloat("Grip", obj.ReadValue<float>()); //Cambia el valor del grip en función del valor del grip
    }

    private void OnDestroy() //Cuando se destruye el objeto
    {
        actionGrip.action.performed -= GripPress; //Retira la función de la acción
    }
}
