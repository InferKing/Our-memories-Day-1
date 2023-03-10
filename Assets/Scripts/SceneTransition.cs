using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartTrans());    
    }
    private IEnumerator StartTrans()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
