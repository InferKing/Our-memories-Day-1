using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;
public class NodeParser : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private UIController _controllerUI;
    [SerializeField] private AudioController _audioController;
    public DialogueGraph graph;
    public TMP_Text speaker, dialogue;
    public Image speakerImage;
    [HideInInspector] public int exitNode = -1;
    private Coroutine _parser;
    private bool _enabled = false;
    private void Start()
    {
        StartCoroutine(MainQueue());
    }

    private IEnumerator MainQueue()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));
        _controllerUI.ShowDialogue();
        foreach (BaseNode node in graph.nodes)
        {
            if (node.GetString() == "Start")
            {
                graph.current = node;
                break;
            }
        }
        yield return new WaitForSeconds(0.8f);
        _controllerUI.HideStart();
        _parser = StartCoroutine(ParseNode());
    }
    private IEnumerator ParseNode()
    {
        BaseNode b = graph.current;
        string data = b.GetString();
        List<string> dataParts = new List<string>(data.Split('/'));
        switch (dataParts[0])
        {
            case "Start":
                NextNode("exit");
                break;
            case "DialogueNode":
                _audioController.PlaySound(int.Parse(dataParts[4]), false);
                dialogue.text = "";
                _controllerUI.SetImage(int.Parse(dataParts[3]), true);
                yield return new WaitUntil(() => !_controllerUI.inTransition);
                if (dataParts[7] == "False" && dataParts[8] == "False")
                {
                    _player.good1 += int.Parse(dataParts[5]);
                    _player.good2 += int.Parse(dataParts[6]);
                }
                speaker.text = dataParts[1];
                speakerImage.sprite = b.GetSprite();
                speakerImage.SetNativeSize();
                if (speakerImage.sprite == null)
                {
                    speakerImage.color = new Color(speakerImage.color.r, speakerImage.color.g, speakerImage.color.b, 0);
                }
                else speakerImage.color = new Color(speakerImage.color.r, speakerImage.color.g, speakerImage.color.b, 1);
                Coroutine cor = StartCoroutine(CheckMouseDown(dataParts[2]));
                yield return StartCoroutine(TypeText(dataParts[2]));
                if (dataParts.Count > 9)
                {
                    _controllerUI.SetButton(dataParts.GetRange(9, dataParts.Count - 9));
                    yield return new WaitUntil(() => exitNode != -1);
                    _controllerUI.HideButtons();
                    NextNode("exit" + exitNode.ToString());
                }
                else
                {
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                    yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                    if (dataParts[7] == "True" && _player.good1 >= int.Parse(dataParts[5]) || 
                        dataParts[8] == "True" && _player.good2 >= int.Parse(dataParts[6]))
                    {
                        NextNode("exit2");
                    }
                    else
                    {
                        NextNode("exit1");
                    }
                }
                StopCoroutine(cor);
                break;
            case "End":
                _controllerUI.SetImage(6, false);
                yield return new WaitUntil(() => !_controllerUI.inTransition);
                _controllerUI.ShowFinish(dataParts[1], dataParts[2]);
                break;
        }
    }
    private IEnumerator TypeText(string s)
    {
        dialogue.text = "";
        _enabled = true;
        for (int i = 0; i < s.Length && _enabled; i++)
        {
            dialogue.text += s[i];
            if (new List<char>() { ',', '.', '?', '!' }.Contains(s[i]))
                yield return new WaitForSeconds(0.07f);
            else
                yield return new WaitForSeconds(0.03f);
        }
        _enabled = false;
    }
    private IEnumerator CheckMouseDown(string s)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        _enabled = false;
        dialogue.text = s;
    }

    public void NextNode(string fieldName)
    {
        exitNode = -1;
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
        foreach (NodePort p in graph.current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }
}
