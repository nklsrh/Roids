using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UITouchController : MonoBehaviour
{
    public UITouchButton btnLeft;
    public UITouchButton btnRight;
    public UITouchButton btnThrust;
    public UITouchButton btnFire;

    private PlayerController player;

    public void Setup(PlayerController player)
    {
        this.player = player;

        btnLeft.onDown = OnButtonPressedLeft;
        btnRight.onDown = OnButtonPressedRight;
        btnThrust.onDown = OnButtonPressedThrust;
        btnFire.onDown = OnButtonPressedFire;
    }

    void OnButtonPressedLeft()
    {
        player.Turn(-1);
    }
    void OnButtonPressedRight()
    {
        player.Turn(1);
    }
    void OnButtonPressedThrust()
    {
        player.Thrust(1.0f);
    }
    void OnButtonPressedFire()
    {
        player.Fire();
    }
}
