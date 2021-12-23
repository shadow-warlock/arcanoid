using UnityEngine;

public class Touch2Controller : IController
{

    private bool click = false;

    public Vector2 Update(Platform platform)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.position.x >= Screen.width / 2)
        {
            if (touch.phase == TouchPhase.Began)
            {
                click = true;
                return Vector2.up;
            }
            if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled && click)
            {
                click = false;
                return Vector2.down;
            }
            if (touch.phase == TouchPhase.Moved && click)
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