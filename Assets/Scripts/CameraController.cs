using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 WhitePosition;
    private Quaternion WhiteRotation;

    private Vector3 BlackPosition;
    private Quaternion BlackRotation;

    void Start()
    {
        WhitePosition = transform.position;
        WhiteRotation = transform.rotation;

        BlackPosition = new Vector3(10, 25, -7.5f);
        BlackRotation = Quaternion.Euler(50, -90, 0);
    }

    public void SwitchSides(bool turn)
    {
        switch (turn)
        {
            case false:// White
                transform.position = WhitePosition;
                transform.rotation = WhiteRotation;
                break;
            case true:// Black
                transform.position = BlackPosition;
                transform.rotation = BlackRotation;
                break;
        }
    }
}