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
        Debug.Log(PhotonNetwork.inRoom +" 인 룸");
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
            //게임스타트
            debugText.text = PhotonNetwork.player.NickName + " 입니당";
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
            debugText.text = "아직 대기중이에요";
        }
    }
}
