using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SnakeController : MonoBehaviour
{
    [Header("移动速度")]
    [SerializeField] public float MoveSpeed;
    [SerializeField] public float RotateSpeed;
    [SerializeField] public float bodySpeed;
    [Header("身体预制体")]
    [SerializeField] public GameObject bodyPrefab;
    [SerializeField] public Transform BodyTransform;

    private List<GameObject> _bodyParts = new List<GameObject>();

    [Header("玩家相机")]
    [SerializeField] public GameObject PlayerCamera;
    [Header("边界碰撞体")]
    [SerializeField] public GameObject BordCollider;
    [SerializeField] public UnityEvent<Vector3> onPositionChanged;
    [SerializeField] public float slerpSpeed = 5f;
    private Vector3 _lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        _bodyParts.Add(this.gameObject);
        onPositionChanged.AddListener(CameraPositionChange);
        AddBodyPart();
        AddBodyPart();
        AddBodyPart();
    }

    // Update is called once per frame
    void Update()
    {
        _lastPosition = transform.position;
        //游戏测试
        GameTest();
        //向前移动
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        //方向控制
        float RotateDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up*RotateDirection*RotateSpeed*Time.deltaTime);

        if (transform.position != _lastPosition)
        {
            onPositionChanged.Invoke(_lastPosition);
        }
        for (int i=1;i< _bodyParts.Count; i++)
        {
            Vector3 point = _bodyParts[i-1].transform.position;
            Vector3 moveDirection = point - _bodyParts[i].transform.position;

            _bodyParts[i].transform.position += moveDirection * bodySpeed * Time.deltaTime;
           
            _bodyParts[i].transform.LookAt(point);
        }
    }

    public void CameraPositionChange(Vector3 _lastPosition)
    {
        PlayerCamera.transform.position = Vector3.Lerp(PlayerCamera.transform.position, PlayerCamera.transform.position+new Vector3(transform.position.x - _lastPosition.x, 0, transform.position.z - _lastPosition.z), slerpSpeed * Time.deltaTime);
        BordCollider.transform.position = transform.position;
    }

    private void AddBodyPart()
    {
        GameObject body = Instantiate(bodyPrefab,new Vector3(transform.position.x,transform.position.y, transform.position.z),Quaternion.identity);
        _bodyParts.Insert(1, body);
        body.transform.parent = BodyTransform;
    }

    private void RemoveBodyPart()
    {
        if (_bodyParts.Count <= 1)
            return;
        GameObject body = _bodyParts[_bodyParts.Count-1];
        _bodyParts.RemoveAt(_bodyParts.Count - 1);
        Destroy(body);
    }

    private void GameTest()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddBodyPart();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveBodyPart();
        }
    }
}
