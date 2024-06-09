using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance = null;
    public static BackgroundMusic Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // �̱��� ������ ����Ͽ� �ϳ��� ������� ������Ʈ�� ����
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // ���� ���� �� ������� ���
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }

    public void PlayMusic()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }

    public void StopMusic()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.isPlaying)
        {
            audio.Stop();
        }
    }

    public void SetVolume(float volume)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.volume = volume;
        }
    }
}
