using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Saw : MonoBehaviour
{

    //private Rigidbody2D myBody;
    [SerializeField] private float positionLeft;
    [SerializeField] private float positionRight;
    [SerializeField] private float positionY;

    private const float RotationSpeed = 550f;
    private SoundController _soundController;

    private Collider2D SawCollider;

    private void Start()
    {
        _soundController = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
        SawCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(SawCollider.enabled)
        {
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
        }
    }

    public void PauseSaw(bool pause)
    {
        GetComponent<Collider2D>().enabled = !pause;
    }

    public void MoveSaw()
    {
        //
        _soundController.PlayClick();
        if (transform.position.x == positionLeft)
        {
            transform.position = new Vector2(positionRight, positionY);        
        }
        else
        {
            transform.position = new Vector2(positionLeft, positionY);
        }
    }
}
