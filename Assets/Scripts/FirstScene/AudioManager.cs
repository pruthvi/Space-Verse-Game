using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    private Animator anim;
    public float fadeTiming;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);     // Ensures AudioManager stays through all levels
    }

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        anim = this.GetComponent<Animator>();
    }

    // Function called from the scene to change clip
    public void SoundTransition(AudioClip clip)
    {
        StartCoroutine(SoundTransitionCo(clip)); // Coroutine for getting music fade time
    }

    IEnumerator SoundTransitionCo(AudioClip newClip)
    {

        anim.SetBool("fadeOut", true);  // Fades music volume from 1 to 0
        yield return new WaitForSeconds(fadeTiming);    // This is animation timing  for fading and will be used through Coroutines
                                                        //  to make sure animation is completed before going further.

        audioSource.enabled = false;    // Need to disable audio source for using new clip
        audioSource.clip = newClip;     // New sound clip
        audioSource.enabled = true;

        anim.SetBool("fadeOut", false);
        anim.SetBool("fadeIn", true);   // Raising volume up from 0 to 1
        yield return new WaitForSeconds(fadeTiming);

        anim.SetBool("fadeIn", false);

    }

}
