using UnityEngine;
using System.Runtime.InteropServices;
public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    public void HelloCall()
    {
        Hello();
    }
    void Start()
    {

        //HelloString("This is a string.");
    }
}
