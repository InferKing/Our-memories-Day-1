using UnityEngine;
public class EndNode : BaseNode
{
    [Input] public int input;
    public int numberEnd;
    public string nameEnd;
    public override string GetString()
    {
        return "End/"+ numberEnd.ToString() + "/" + nameEnd;
    }
}
