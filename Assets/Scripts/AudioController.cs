using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private AudioSource m_Source;
    [HideInInspector] private int _playing = -1;

    public void PlaySound(int index, bool loop)
    {
        if (_playing == index || index == -1) return;
        if (m_Source.isPlaying) m_Source.Stop();
        _playing = index;
        m_Source.clip = _clips[index];
        m_Source.loop = loop;
        m_Source.Play();
        StartCoroutine(SetIndex());
    }
    private IEnumerator SetIndex()
    {
        yield return new WaitUntil(() => !m_Source.isPlaying);
        _playing = -1;
    }
}
