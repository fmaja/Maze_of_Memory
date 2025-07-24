using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal2 : MonoBehaviour
{
    public string scene;
    private bool sceneAlreadyLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !sceneAlreadyLoaded)
        {
            sceneAlreadyLoaded = true;

            // £adowanie sceny Additive
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += (op) =>
            {
                Debug.Log($"Scena {scene} za³adowana.");
            };
        }
    }
}
