using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on UIButtonSound.");
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogError("No AudioSource or AudioClip assigned to UIButtonSound!");
        }
    }
}
