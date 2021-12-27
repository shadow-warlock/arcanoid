using UnityEngine;

public class KeyboardController : IController
{

    public Vector2 Update(Platform platform)
    {
        if (Input.GetKey(KeyCode.A))
        {
            return Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            return Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            return Vector2.up;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            return Vector2.down;
        }
        return Vector2.zero;
    }
}
