using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform target;
    private float lifeTime = 10f; // ����Ʈ�� ���� (10��)

    void Start()
    {
        // 10�� �Ŀ� Destroy ȣ��
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
