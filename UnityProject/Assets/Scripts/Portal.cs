using UnityEngine;

public class ScenePortal : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadLevel(sceneToLoad);
        }
    }
}