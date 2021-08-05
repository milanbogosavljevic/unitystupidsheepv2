using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Rigidbody2D Sun;
    private Rigidbody2D Moon;

    private Transform SunTransform;
    private Transform MoonTransform;

    private float sunSpeed = -0.2f;
    private float moonSpeed = 0.2f;
    private float alphaSpeedDivider = 150f;
    private float alphaSpeed;
    private float SunUpperPosition;
    private float SunBottomPosition;

    private bool AnimationIsActive = true;
    private bool IsDayTime = true;

    private Color DayBackgroundColor;
    private SpriteRenderer DayBackground;
    private SpriteRenderer DaySky;

    void Start()
    {
        Sun = GameObject.Find("Sun").GetComponent<Rigidbody2D>();
        Moon = GameObject.Find("Moon").GetComponent<Rigidbody2D>();
        SunTransform = GameObject.Find("Sun").transform;
        MoonTransform = GameObject.Find("Moon").transform;

        SunUpperPosition = 4f;
        SunBottomPosition = 0f;

        //DayComponents = GameObject.Find("DayComponents").GetComponentsInChildren<SpriteRenderer>();
        DayBackground = GameObject.Find("DayBackground").GetComponent<SpriteRenderer>();
        DaySky = GameObject.Find("DaySky").GetComponent<SpriteRenderer>();
        alphaSpeed = sunSpeed / alphaSpeedDivider;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (AnimationIsActive)
        {
            if (IsDayTime)
            {
                MoveSun();
                UpdateDayComponentsAlpha();
            }
            else
            {
                MoveMoon();
            }
        }
    }

    void UpdateDayComponentsAlpha()
    {
        DayBackgroundColor = DayBackground.material.color;

        if(DayBackgroundColor.a > 1)
        {
            DayBackgroundColor.a = 1;
        }
        if (DayBackgroundColor.a < 0)
        {
            DayBackgroundColor.a = 0;
        }

        DayBackgroundColor.a += alphaSpeed;
        DayBackground.material.color = DayBackgroundColor;
        DaySky.material.color = DayBackgroundColor;           
    }

    private void MoveSun()
    {
        //Debug.Log(SunTransform.position.y);
        if(SunTransform.position.y < SunBottomPosition)
        {
            IsDayTime = false;
            SwitchSunDirection();
        }

        if (SunTransform.position.y > SunUpperPosition)
        {
            SwitchSunDirection();
        }

        if (IsDayTime)
        {
            Sun.velocity = new Vector2(0f, sunSpeed);
        }
        else
        {
            Sun.velocity = new Vector2(0f, 0f);
        }
    }

    private void MoveMoon()
    {
        if (MoonTransform.position.y < SunBottomPosition)
        {
            IsDayTime = true;
            SwitchMoonDirection();
        }

        if (MoonTransform.position.y > SunUpperPosition)
        {
            SwitchMoonDirection();
        }

        if (IsDayTime)
        {
            Moon.velocity = new Vector2(0f, 0f);
        }
        else
        {
            Moon.velocity = new Vector2(0f, moonSpeed);
        }
    }

    private void SwitchSunDirection()
    {
        Vector3 pos = SunTransform.position;
        pos.y = pos.y > 0 ? SunUpperPosition : SunBottomPosition;
        SunTransform.position = pos;
        sunSpeed *= -1;
        alphaSpeed *= -1;
    }

    private void SwitchMoonDirection()
    {
        Vector3 pos = MoonTransform.position;
        pos.y = pos.y > 0 ? SunUpperPosition : SunBottomPosition;
        MoonTransform.position = pos;
        moonSpeed *= -1;
    }
}
