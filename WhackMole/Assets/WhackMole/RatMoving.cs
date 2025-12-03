using UnityEngine;
using System.Collections;

public class RatMoving : MonoBehaviour
{
    public Transform startingPoint; // 拖放 StartingPoint 的 Transform
    public Transform endPoint;       // 拖放 EndPoint 的 Transform
    public float speed = 2.0f;       // 移動速度
    public float fixedPauseDuration = 1.0f; // 到達終點後的固定停留時間
    public float minStartPause = 2.0f; // 起始點停留的最小時間
    public float maxStartPause = 7.0f; // 起始點停留的最大時間

    private Vector3 target;
    private bool movingToEndPoint = true;
    private float randomStartPause; // 隨機的起始點停留時間

    void Start()
    {
        // 初始化位置
        transform.position = startingPoint.position;
        target = endPoint.position; // 初始目標設為 EndPoint
        randomStartPause = Random.Range(minStartPause, maxStartPause); // 隨機設置起始點的停留時間
    }

    void Update()
    {
        // 移動 Rat 到目標位置
        float step = speed * Time.deltaTime; // 計算每幀的移動步驟
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // 檢查是否到達目標
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            if (movingToEndPoint)
            {
                // 到達終點，執行固定停留
                StartCoroutine(PauseAtTarget(endPoint.position, startingPoint.position, fixedPauseDuration));
                Debug.Log("Rat reached EndPoint, pausing...");
            }
            else
            {
                // 到達起始點，執行隨機停留
                StartCoroutine(PauseAtTarget(startingPoint.position, endPoint.position, randomStartPause));
                Debug.Log("Rat reached StartingPoint, pausing...");
            }
        }
    }

    private IEnumerator PauseAtTarget(Vector3 currentTarget, Vector3 nextTarget, float pauseTime)
    {
        // 停留一段時間
        yield return new WaitForSeconds(pauseTime);
        // 反向移動
        movingToEndPoint = !movingToEndPoint;

        target = nextTarget; // 更新目標位置
    }
}