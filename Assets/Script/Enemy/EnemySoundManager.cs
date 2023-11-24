using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField] private AI ai;
    [SerializeField] private AudioClip idle1;
    [SerializeField] private AudioClip idle2;
    [SerializeField] private AudioClip idle3;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip death;

    public bool isAlive = true;
    public IEnumerator idleGrowl;

    void Start()
    {
        idleGrowl = IdleGrowlEmitter();
        StartCoroutine(idleGrowl);
    }
    private AudioClip SelectIdleClip()
    {
        int randomClipNumber = Random.Range(1, 4);
        return randomClipNumber switch
        {
            1 => idle1,
            2 => idle2,
            3 => idle3,
            _ => null,
        };
    }
    private IEnumerator IdleGrowlEmitter()
    {
        while(isAlive)
        {
            AudioClip clip = SelectIdleClip();
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
