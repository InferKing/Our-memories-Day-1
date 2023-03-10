using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : BaseNode {

	[Input] public int entry;
	[Output] public int exit1, exit2, exit3;
	public string charName;
	public string dialogueLine;
	public Sprite sprite;
    public float time;
    public int indexBack = 0, indexSound = -1, good1 = 0, good2 = 0;
    public bool checkGood1 = false, checkGood2 = false;
    public List<string> vars = new List<string>();

    public override string GetString()
    {
        string s = "DialogueNode/" + charName + "/" + dialogueLine;
        s += "/";
        s += indexBack.ToString();
        s += "/";
        s += indexSound.ToString();
        s += "/";
        s += good1.ToString();
        s += "/";
        s += good2.ToString();
        s += "/";
        s += checkGood1.ToString();
        s += "/";
        s += checkGood2.ToString();
        foreach (string v in vars)
        {
            s += "/";
            s += v;
        }
        return s;
    }
    public override Sprite GetSprite()
    {
        return sprite;
    }
    public float GetTime() => time;
}