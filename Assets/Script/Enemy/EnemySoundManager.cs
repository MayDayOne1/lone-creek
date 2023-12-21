using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField] private AI ai;
    [SerializeField] private AudioClip[] idles;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip death;

    public bool isAlive = true;
    public IEnumerator<float> idleGrowl;

    void Start()
    {
        idleGrowl = IdleGrowlEmitter();
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
            AudioSource.PlayClipAtPoint(clip, transform.position);
            yield return Timing.WaitForSeconds(3f);
        }
    }
    public void EmitDamageSound()
    {
        AudioSource.PlayClipAtPoint(damage, transform.position);
    }
    public void EmitDeathSound()
    {
        AudioSource.PlayClipAtPoint(death, transform.position);
    }
}
