using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //看向的目标对象
    private Transform target;
    //摄像机相对目标对象 在xyz上的偏移位置
    public Vector3 offsetPos;
    //看向目标位置的y偏移值
    public float bodyHeight;

    //移动和旋转速度
    public float moveSpeed;
    public float rotationSpeed;

    private Vector3 targetPos;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        //向后偏移z坐标
        targetPos = target.position + target.forward * offsetPos.z;
        //向上偏移y坐标
        targetPos += Vector3.up * offsetPos.y;
        //左右偏移
        targetPos += target.right * offsetPos.x;
        //通过插值移动
        transform.position=Vector3.Lerp(transform.position, targetPos, moveSpeed*Time.deltaTime);
        //旋转
        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - transform.position);
        transform.rotation=Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed*Time.deltaTime);
    }

    /// <summary>
    /// 设置摄像机看向的目标对象
    /// </summary>
    /// <param name="player"></param>
    public void SetTarget(Transform player)
    {
        target = player;
    }
}
