using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader inst;

    [SerializeField] private GameObject facePrefab;
    private GameObject face;
    private float maxSize = 15;
    private float time = 0.5f;

    private void Awake()
    {
        if (inst == null) inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject c = GameObject.Find(NameKeys.loadSceneCanvas);
        face = Instantiate(facePrefab, Vector2.zero, Quaternion.identity);
        face.transform.SetParent(c.transform);
        face.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0);
        face.transform.DOScale(maxSize, 0);
        face.transform.DOScale(0, time);
    }

    public void LoadScene(string sceneName)
    {
        face.transform.DOScale(maxSize, time);
        DOVirtual.DelayedCall(time, () => SceneManager.LoadScene(sceneName));
       
    }
}
