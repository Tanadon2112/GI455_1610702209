using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Find : MonoBehaviour
{
    public string[] Data5;
    public Text Pim;
    public Text Show;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finddata()
    {
        for (int t = 0; t <= 4; t++)
        {
            if (Pim.text == Data5[t])
            {
                Show.text = "[" + "<color=green>"+ Pim.text +"</color>" + "] is found.";
                return;
            }
            else
            {
                Show.text = "[" + "<color=red>" + Pim.text + "</color>" + "] is not found"; 
                return;
            }

        }
    }
}
