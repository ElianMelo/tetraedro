using Unity.Entities;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private InputSystem_Actions controls;

    protected override void OnCreate()
    {
        if(!SystemAPI.TryGetSingleton<InputComponent>(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        controls = new InputSystem_Actions();
        controls.Enable();
    }

    protected override void OnUpdate() { 
        Vector2 moveVector = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        bool isPressingLMB = controls.Player.Shoot.ReadValue<float>() == 1 ? true : false;
        float forkAxis = controls.Player.Fork.ReadValue<float>();
        float leverAxis = controls.Player.Lever.ReadValue<float>();

        SystemAPI.SetSingleton(new InputComponent { mousePos = mousePosition, movement = moveVector, pressingLMB = isPressingLMB, 
            forkDirection = forkAxis, forkLever = leverAxis});
    }
}
