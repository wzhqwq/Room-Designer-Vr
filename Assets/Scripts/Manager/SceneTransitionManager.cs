using UnityEngine;
using System.Collections;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {
  private IEnumerator transitionCoroutine;

  void Start()
  {
    transitionCoroutine = FadeController.FadeInCoroutine();
    StartCoroutine(transitionCoroutine);
  }

  public void LoadScene(string sceneName) {
    StartCoroutine(LoadSceneCoroutine(sceneName));
  }

  public IEnumerator LoadSceneCoroutine(string sceneName) {
    if (transitionCoroutine != null)
    {
      yield return transitionCoroutine;
    }
    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == sceneName)
    {
      yield break;
    }
    transitionCoroutine = DoTransition(sceneName);
    yield return transitionCoroutine;
  }

  private IEnumerator DoTransition(string sceneName) {
    
    yield return FadeController.FadeOutCoroutine();
    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    yield return FadeController.FadeInCoroutine();
    transitionCoroutine = null;
  }
}