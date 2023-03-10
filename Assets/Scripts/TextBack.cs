using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TextBack : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    private void Update()
    {
        RectTransform uiText = _text.GetComponent<RectTransform>();
        RectTransform uiImage = _image.GetComponent<RectTransform>();
        uiImage.sizeDelta = uiText.sizeDelta * 1.2f;
        
    }
    public void G()
    {
        
    }
}
