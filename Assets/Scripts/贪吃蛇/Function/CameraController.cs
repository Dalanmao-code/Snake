using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("灵敏度设置")]
    [SerializeField]public float mousespeed;
    [SerializeField]public float slerpSpeed = 5f;
    [SerializeField] public float middleSpeed = 10f;
    [Header("鼠标边界设置")]
    [SerializeField] public float MaxX;
    [SerializeField] public float MaxY;


    private float RotationX = 0;
    private float RotationY = 0;
    private Vector2 IniRotation;
    // Start is called before the first frame update
    void Start()
    {
        IniRotation.x = transform.localEulerAngles.x;
        IniRotation.y = transform.localEulerAngles.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    public void CameraMove()
    {

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue != 0)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, transform.position.y + scrollValue* middleSpeed * (transform.position.y/ transform.position.y + transform.position.z),5f * Time.deltaTime), Mathf.Lerp(transform.position.z, transform.position.z + scrollValue * middleSpeed * (transform.position.z / transform.position.y + transform.position.z), 5f * Time.deltaTime));

        }
        //RotationX += transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousespeed;
        if (Input.GetMouseButton(0))
        {
            
           
            RotationX = Mathf.Lerp(RotationX, RotationX + Input.GetAxis("Mouse Y") * mousespeed, slerpSpeed * Time.deltaTime);
            RotationY = Mathf.Lerp(RotationY, RotationY - Input.GetAxis("Mouse X") * mousespeed, slerpSpeed * Time.deltaTime);
            RotationX = Mathf.Clamp(RotationX, -MaxX, MaxX);
            RotationY = Mathf.Clamp(RotationY, -MaxY, MaxY);
            transform.localEulerAngles = new Vector3(IniRotation.x + RotationX, IniRotation.y + RotationY, transform.eulerAngles.z);

        }
    }


}
