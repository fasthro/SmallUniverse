using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour {
    public Text label;
    public VariableString varstr = new VariableString();

    // Use this for initialization
    void Start () {
	}

    void Update () {
        string aaa = "中国";
        string bbb = "我问问";
        //label.text = aaa + bbb;
        label.text = varstr.ConCat(aaa, bbb);
    }
}
