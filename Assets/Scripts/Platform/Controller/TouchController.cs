using UnityEngine;

public class TouchController : IController
{

    private bool click = false;

    public Vector2 Update(Platform platform)
    {
        if (Input.mousePosition.x >= Screen.width / 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                click = true;
                return Vector2.up;
            }
            if (Input.GetMouseButtonUp(0) && click)
            {
                click = false;
                return Vector2.down;
            }
            if (Input.GetMouseButton(0) && click)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return new Vector2(worldPosition.x - platform.transform.position.x, 0);
            }
        }else if (click)
        {
            click = false;
            return Vector2.down;
        }

        return Vector2.zero;
    }
}