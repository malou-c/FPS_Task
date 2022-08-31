using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public float LookSensitivity;

    PlayerCharacterController _playerController;

    void Start()
    {
        _playerController = GetComponent<PlayerCharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Transform playerTransform = _playerController.transform;
            float x = Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal);
            float z = Input.GetAxisRaw(GameConstants.k_AxisNameVertical);
            Vector3 move = playerTransform.right * x + playerTransform.forward * z;

            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseLookAxis(GameConstants.k_MouseAxisNameHorizontal);
    }

    public float GetLookInputsVertical()
    {
        return GetMouseLookAxis(GameConstants.k_MouseAxisNameVertical);
    }

    float GetMouseLookAxis(string mouseInputName)
    {
        if (CanProcessInput())
        {
            float i = Input.GetAxisRaw(mouseInputName);
            i *= LookSensitivity;
            return i;
        }
        return 0f;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
            return Input.GetButtonDown(GameConstants.k_ButtonNameJump);
        return false;
    }

    public bool GetFireInputDown()
    {
        if (CanProcessInput())
            return Input.GetButtonDown(GameConstants.k_ButtonNameFire);
        return false;
    } 

  
}
