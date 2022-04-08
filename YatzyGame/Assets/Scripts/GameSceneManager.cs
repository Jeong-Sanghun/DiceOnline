using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class GameSceneManager : UnityEngine.MonoBehaviour
{
    PhotonManager photonManager;
    SceneLoadManager sceneLoadManager;
    [SerializeField]
    Text[] nickNameText;

    [SerializeField]
    DiceRoller diceRoller;

    [SerializeField]
    Text debugText;
    // Start is called before the first frame update
    void Start()
    {



    }

    public void OnPlayerConnect()
    {
        photonManager = PhotonManager.singleton;
        sceneLoadManager = SceneLoadManager.singleton;
        Debug.Log(PhotonNetwork.inRoom +" �� ��");
        // myPhotonView.RPC("OnPlayerConnectRPC", PhotonTargets.AllViaServer);
        SetNickName();
    }

    public void SetNickName()
    {
        for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
        {
            nickNameText[i].text = PhotonNetwork.playerList[i].NickName;
            
        }
        if (PhotonNetwork.countOfPlayers == 2)
        {
            //���ӽ�ŸƮ
            debugText.text = PhotonNetwork.player.NickName + " �Դϴ�";
            if (PhotonNetwork.isMasterClient)
            {
                diceRoller.TurnChange(true);
            }
            else
            {
                diceRoller.TurnChange(false);
                diceRoller.MasterStart();
            }
            
        }
        else
        {
            debugText.text = "���� ������̿���";
        }
    }
}
