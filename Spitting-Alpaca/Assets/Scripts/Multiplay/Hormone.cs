using UnityEngine;
using System.Collections;
using Photon.Pun; // Photon ���

public class Hormone : MonoBehaviourPunCallbacks
{
    public float scanRadius = 10.0f;
    public GameObject markerPrefab; // �÷��̾� �Ӹ� ���� ��� ��Ŀ ������

    // ������ ��� �Լ�
    public void UseHormone()
    {
        if (!photonView.IsMine)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scanRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Player �±׸� ���� ��ü�� ��Ŀ ���� (���ÿ����� ����)
                ShowMarkerOverPlayer(hitCollider.gameObject);
            }
        }
    }

    [PunRPC]
    void ShowMarkerOverPlayerRPC(int photonViewID)
    {
        GameObject player = PhotonView.Find(photonViewID).gameObject;
        ShowMarkerOverPlayer(player);
    }

    void ShowMarkerOverPlayer(GameObject player)
    {
        if (photonView.IsMine && player.GetComponent<PhotonView>().Owner != PhotonNetwork.LocalPlayer)
        {
            Vector3 markerPosition = player.transform.position + new Vector3(0, 1, 0); // Y������ 1 ���� �ø���
            GameObject marker = Instantiate(markerPrefab, markerPosition, Quaternion.identity, player.transform);
            StartCoroutine(RemoveMarker(marker));
        }
    }

    IEnumerator RemoveMarker(GameObject marker)
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(marker);
    }
}
