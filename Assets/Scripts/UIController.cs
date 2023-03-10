using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    [SerializeField] private Animator _startAnimation;
    [SerializeField] private TMP_Text _finishText, _title;
    [SerializeField] private Button _finishButton;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Image _background;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private TMP_Text[] _texts;
    [SerializeField] private NodeParser _nodeParser;
    [HideInInspector] public bool inTransition = false;
    [HideInInspector] public int indexBackground = 0;
    private void Start()
    {
        _finishText.gameObject.SetActive(false);
        _finishButton.gameObject.SetActive(false);
        _startAnimation.SetBool("Hide", false);
    }
    public void SetNode(int index)
    {
        _nodeParser.exitNode = index;
    }
    public void SetButton(List<string> text)
    {
        for (int i = 0; i < text.Count; i++)
        {
            _buttons[i].gameObject.SetActive(true);
            _texts[i].text = text[i];
        }
    }
    public void HideButtons()
    {
        foreach (var button in _buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
    public void SetImage(int index, bool b)
    {
        if (indexBackground == index) return;
        inTransition = true;
        StartCoroutine(StartTransition(index, b));
    }

    private IEnumerator StartTransition(int index, bool b)
    {
        _dialogueUI.SetActive(false);
        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(1);
        _background.sprite = _sprites[index];
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 1);
        yield return new WaitForSeconds(1);
        if (b) _dialogueUI.SetActive(true);
        indexBackground = index;
        inTransition = false;
    }
    public void ShowFinish(string number, string name)
    {
        _finishText.gameObject.SetActive(true);
        _finishButton.gameObject.SetActive(true);
        _finishText.text = name + "\n" + number + "/5";
        _finishButton.gameObject.SetActive(true);
    }
    public void ShowDialogue()
    {
        _dialogueUI.SetActive(true);
        _startAnimation.SetBool("Hide", true);
    }
    public void HideStart()
    {
        _title.gameObject.SetActive(false);
    }
    public void ToStart()
    {
        SceneManager.LoadScene(1);
    }
}
