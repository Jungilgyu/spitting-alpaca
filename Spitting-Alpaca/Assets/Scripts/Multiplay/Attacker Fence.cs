using System.Collections;
using Photon.Pun;
using UnityEngine;

public class AttackerFence : MonoBehaviourPun
{
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        // �ʱ� ��ġ�� �����մϴ�.
        initialPosition = transform.position;

        // ��ǥ ��ġ�� �ʱ� ��ġ���� �Ʒ��� ���� �Ÿ� ����
        targetPosition = initialPosition + new Vector3(0, -20, 0);

        // 5�� �Ŀ� ��Ÿ���� ������ �ڷ�ƾ ����
        if (photonView.IsMine)
        {
            StartCoroutine(LowerFenceAfterDelay(10f));
        }
    }

    IEnumerator LowerFenceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        photonView.RPC("LowerFence", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void LowerFence()
    {
        StartCoroutine(LowerFenceCoroutine());
    }

    IEnumerator LowerFenceCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 2f; // �������� �ð�

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
