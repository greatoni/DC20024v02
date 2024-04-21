/*
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Toggle))]
public class RuneButton : MonoBehaviour
{
    public Image runeFace;
    public Toggle runeToggle;
    public UnityEvent<int,int,bool, Image> sendRune;
    public int runePosition, row;
    public SequencerWeapon sequencerWeapon;

    void Awake()
    {
        runeToggle = GetComponent<Toggle>();
        runeToggle.onValueChanged.AddListener(delegate { runeClick(); });
    }

    public void ShowRuneForce(int value)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString() ;
    }

    public void runeClick()
    {
        if (sequencerWeapon.CheckIfRunesLeft())
        {
            sendRune.Invoke(runePosition, row, runeToggle.isOn, runeFace);
        }
        if (!sequencerWeapon.CheckIfRunesLeft())
        {
            if (!runeToggle.isOn) sendRune.Invoke(runePosition, row, runeToggle.isOn, runeFace);
            else runeToggle.isOn = false;
        }
    }


    private void OnDestroy()
    {
        sendRune.RemoveAllListeners();
    }
}
*/