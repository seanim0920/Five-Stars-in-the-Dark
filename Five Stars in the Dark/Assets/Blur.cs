using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blur : MonoBehaviour
{
    public Image Wheel;
    public Image Dashboard;
    public Image BlurredWheel;
    public Image BlurredDashboard;
    public Image LeftNeedle;
    public Image RightNeedle;
    public Image BrakePedal;
    public Image AccelPedal;
    public static float amount = 0;
    private Color BlurColor;
    private Color SmallColor;
    private Color VisColor;
    // Start is called before the first frame update
    void Start()
    {
        amount = 0;
        BlurColor = new Color(1,1,1,0);
        SmallColor = new Color(1, 1, 1, 1);
        VisColor = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        BlurColor.a = Mathf.Sqrt(amount);
        SmallColor.a = 1 - amount;
        VisColor.a = Mathf.Pow(1-amount,2) + 0.5f;
        Wheel.color = VisColor;
        Dashboard.color = VisColor;
        BlurredWheel.color = BlurColor;
        BlurredDashboard.color = BlurColor;
        LeftNeedle.color = SmallColor;
        RightNeedle.color = SmallColor;
        BrakePedal.color = SmallColor;
        AccelPedal.color = SmallColor;
    }
}
