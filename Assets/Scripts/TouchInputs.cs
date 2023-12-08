using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class TouchInputs
{
    public static bool TouchBegan()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    public static bool TouchDragged()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved);
    }

    public static bool TouchReleased()
    {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }

    public static bool IsOverUI()
    {
        PointerEventData pointerEventData = new(EventSystem.current);

        foreach (var touch in Input.touches)
        {
            pointerEventData.position = touch.position;
            List<RaycastResult> raycastResultsTouch = new();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultsTouch);
            for (int i = 0; i < raycastResultsTouch.Count; i++)
            {
                if (raycastResultsTouch[i].gameObject.GetComponent<UIClickThrough>() != null)
                {
                    raycastResultsTouch.RemoveAt(i);
                    i--;
                }
            }

            if (raycastResultsTouch.Count > 0)
                return true;
        }

        return false;
    }
}
