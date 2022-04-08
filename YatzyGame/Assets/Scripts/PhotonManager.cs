using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonManager : PunBehaviour
{
    public static PhotonManager singleton;

    GameSceneManager gameSceneManager;

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Join(string nickName)
    {
        PhotonNetwork.player.NickName = nickName;
        PhotonNetwork.ConnectUsingSettings("Yatzy1.0");
    }

    public override void OnConnectedToMaster()
    {

        Debug.Log(PhotonNetwork.inRoom+"커넥트 마스터");
        //PhotonNetwork.JoinOrCreateRoom("Yatzy", new RoomOptions { MaxPlayers = 2 },null);
        
    }

    public override void OnJoinedLobby()
    {

        Debug.Log(PhotonNetwork.inRoom + "로비조인");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {

        Debug.Log(PhotonNetwork.inRoom + "룸조인");
        SceneLoadManager.singleton.LoadGameSceneAsync((task) =>
        {
            gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
            gameSceneManager.OnPlayerConnect();
        });
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("들어가따!");
        //myPhotonView.RPC("OnPlayerConnectRPC", PhotonTargets.AllViaServer);
        gameSceneManager.SetNickName();
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        //myPhotonView.RPC("OnPlayerConnectRPC", PhotonTargets.AllViaServer);
        gameSceneManager.SetNickName();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {

        Debug.Log("방이없으니 내가 만들어");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnCreatedRoom()
    {

        Debug.Log(PhotonNetwork.inRoom + "룸이 만들어져따");
        //SceneLoadManager.singleton.LoadGameSceneAsync((task) =>
        //{
        //    GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>().OnPlayerConnect();
        //});

    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        
    }

    [PunRPC]
    void OnPlayerConnectRPC()
    {
        gameSceneManager.SetNickName();
    }

}
