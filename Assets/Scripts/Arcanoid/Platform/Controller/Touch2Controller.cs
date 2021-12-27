using UnityEngine;

public class Touch2Controller : IController
{

    private int? click = null;

    public Vector2 Update(Platform platform)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.position.x >= Screen.width / 2)
        {
            if (touch.phase == TouchPhase.Began)
            {
                click = touch.fingerId;
                return Vector2.up;
            }
        }
        if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled && click == touch.fingerId)
        {
            click = null;
            return Vector2.down;
        }
        if (touch.phase == TouchPhase.Moved && click == touch.fingerId)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(worldPosition.x - platform.transform.position.x, 0);
        }

        return Vector2.zero;
    }
}