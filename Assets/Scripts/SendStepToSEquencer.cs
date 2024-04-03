using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SendStepToSEquencer : MonoBehaviour
{

    public UnityEvent<Step> sendRune;
    public Step step = new Step();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onButtonClick()
    {
        sendRune.Invoke(step);
    }
    
}
