using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blur : MonoBehaviour
{
    public Image BlurredWheel;
    public Image BlurredDashboard;
    public Image LeftNeedle;
    public Image RightNeedle;
    public Image BrakePedal;
    public Image AccelPedal;
    public static float amount = 0;
    private Color BlurColor;
    private Color VisColor;
    // Start is called before the first frame update
    void Start()
    {
        BlurColor = new Color(1,1,1,0);
        VisColor = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        BlurColor.a = amount;
        VisColor.a = 1 - amount;
        BlurredWheel.color = BlurColor;
        BlurredDashboard.color = BlurColor;
        LeftNeedle.color = VisColor;
        RightNeedle.color = VisColor;
        BrakePedal.color = VisColor;
        AccelPedal.color = VisColor;
    }
}
