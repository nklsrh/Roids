using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UITouchButton : Button
{
    public System.Action onDown;

    private bool isDown = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        isDown = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        isDown = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        isDown = false;
    }


    void Update()
    {
        if (isDown && onDown != null)
        {
            onDown.Invoke();
        }
    }
}
