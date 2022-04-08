using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject queueButton;
    [SerializeField]
    InputField nickNameInput;

    public void QueueButton()
    {
        queueButton.SetActive(false);
        PhotonManager.singleton.Join(nickNameInput.text);
    }
}
