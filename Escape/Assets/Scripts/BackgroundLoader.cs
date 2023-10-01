using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackgroundLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSceneInBackground());
    }

    private IEnumerator LoadSceneInBackground()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        
        // Wait until the scene has finished loading
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Optionally: once the scene is loaded, you can initialize/setup any additional components or triggers as needed.
    }
}
