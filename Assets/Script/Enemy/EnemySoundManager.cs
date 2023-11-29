using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField] private AI ai;
    [SerializeField] private AudioClip[] idles;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip death;

    public bool isAlive = true;
    public IEnumerator idleGrowl;

    void Start()
    {
        idleGrowl = IdleGrowlEmitter();
        StartCoroutine(idleGrowl);
    }
    private AudioClip SelectRandomIdleClip()
    {
        return idles[Random.Range(0, idles.Length)];
    }
    private IEnumerator IdleGrowlEmitter()
    {
        while(isAlive)
        {
            AudioClip clip = SelectRandomIdleClip();
            AudioSource.PlayClipAtPoint(clip, transform.position);
            yield return new WaitForSeconds(3f);
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
