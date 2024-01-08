using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

[RequireComponent(typeof(AudioSource))]

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField] private AI ai;
    [SerializeField] private AudioClip[] idles;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip death;

    public bool isAlive = true;
    public IEnumerator<float> idleGrowl;

    private AudioSource audioSource;

    void Start()
    {
        idleGrowl = IdleGrowlEmitter();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = .9f;
        Timing.RunCoroutine(idleGrowl.CancelWith(gameObject));
    }
    private AudioClip SelectRandomIdleClip()
    {
        return idles[Random.Range(0, idles.Length)];
    }
    private IEnumerator<float> IdleGrowlEmitter()
    {
        while(isAlive && PlayerParams.health > 0f)
        {
            AudioClip clip = SelectRandomIdleClip();
            RandomizePitch();
            RandomizeVolume();
            audioSource.PlayOneShot(clip);
            yield return Timing.WaitForSeconds(3f);
        }
    }
    public void EmitDamageSound()
    {
        audioSource.PlayOneShot(damage);
    }
    public void EmitDeathSound()
    {
        audioSource.PlayOneShot(death);
    }

    private void RandomizePitch()
    {
        audioSource.pitch = Random.Range(audioSource.pitch - .1f, audioSource.pitch + .1f);
    }

    private void RandomizeVolume()
    {
        audioSource.volume = Random.Range(audioSource.volume - .1f, audioSource.volume + 1f);
    }
}
