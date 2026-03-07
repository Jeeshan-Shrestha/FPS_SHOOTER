using System;
using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI message;
    public void updateMessage(String promptMessage)
    {
        message.text = promptMessage;
    }

}
