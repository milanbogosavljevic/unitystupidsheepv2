using UnityEngine;

public class SpeedFinger : MonoBehaviour
{
    private float fingerSpeed = 90f;
    private float rotationSpeed;
    private float previousSpeed = 0f;
    private bool RotateFinger = false;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (RotateFinger)
    //     {
    //         transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    //         if(gameObject.transform.localEulerAngles.z < 2)
    //         {
    //             //RotateFinger = false;
    //             transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 4));
    //         }
    //     }
    // }

        void Update()
    {
        if (RotateFinger)
        {
            //rectTransform.Rotate( new Vector3( 0, 0, rotationSpeed * Time.deltaTime ) );
            rectTransform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            // if(gameObject.transform.localEulerAngles.z < 2)
            // {
            //     //RotateFinger = false;
            //     transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 4));
            // }
        }
    }

    public void SetFingerSpeed(string speed)
    {
        float speedDivider = float.Parse(speed) - previousSpeed;
        rotationSpeed = fingerSpeed / speedDivider;
        previousSpeed = float.Parse(speed);
    }

    public void RunAnimation(bool run)
    {
        RotateFinger = run;
    }
}
