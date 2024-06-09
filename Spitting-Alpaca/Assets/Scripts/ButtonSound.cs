using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
        audioSource.playOnAwake = false;  // �� �ε� �� �Ҹ��� ���� �ʵ��� ����
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound); // Ŭ�� �ÿ��� �Ҹ��� ����ǵ��� ����
    }
}
