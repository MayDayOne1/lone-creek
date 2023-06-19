using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDesert : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene("SceneDesert");
        }
    }
}
