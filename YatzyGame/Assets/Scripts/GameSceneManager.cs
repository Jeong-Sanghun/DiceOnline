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
    Text myNickName;
    [SerializeField]
    Text enemyNickName;

    [SerializeField]
    DiceRoller diceRoller;

    [SerializeField]
    Text debugText;
    // Start is called before the first frame update
    void Start()
    {



    }

    public void OnMeJoin(bool isNewRoom)
    {
        photonManager = PhotonManager.singleton;
        sceneLoadManager = SceneLoadManager.singleton;
        Debug.Log(PhotonNetwork.inRoom +" ¿Œ ∑Î");
        myNickName.text = PhotonNetwork.player.NickName;
        if (!isNewRoom)
        {
            for(int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
            {
                if(PhotonNetwork.playerList[i] != PhotonNetwork.player)
                {
                    enemyNickName.text = PhotonNetwork.playerList[i].NickName;
                }
            }
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
        // myPhotonView.RPC("OnPlayerConnectRPC", PhotonTargets.AllViaServer);
    }

    public void OnOtherJoin(PhotonPlayer player)
    {
        enemyNickName.text = player.NickName;
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

    public void OnOtherDisconnect(PhotonPlayer player)
    {
        diceRoller.GameInit();
        enemyNickName.text = null;
    }
}
