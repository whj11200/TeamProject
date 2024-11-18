using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScrollbar : MonoBehaviour
{
    [SerializeField] private Scrollbar loadingScrollbar; // Scrollbar 참조
    private float loadingDuration = 4f; // 전체 로딩 시간

    private void Start()
    {
        loadingScrollbar = GameObject.Find("UiCanvas").transform.GetChild(1).GetComponent<Scrollbar>();
        StartCoroutine(LoadNextScene("GameScene")); // 다음 씬의 이름으로 변경
    }

    private IEnumerator LoadNextScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (!asyncLoad.isDone)
        {
         
            elapsedTime += Time.deltaTime;
            loadingScrollbar.size = Mathf.Clamp01(elapsedTime / loadingDuration); 

            
            if (asyncLoad.progress >= 0.9f)
            {
               
                while (loadingScrollbar.size < 1f)
                {
                    elapsedTime += Time.deltaTime;
                    loadingScrollbar.size = Mathf.Clamp01(elapsedTime / loadingDuration);
                    yield return null; 
                }

                asyncLoad.allowSceneActivation = true; 
            }

            yield return null; 
        }
    }
}
