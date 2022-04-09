using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonManager : PunBehaviour
{
    public static PhotonManager singleton;

    GameSceneManager gameSceneManager;
    bool isMyRoom;
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
    private void Start()
    {
        isMyRoom = false;
    }


    public void Join(string nickName)
    {
        PhotonNetwork.player.NickName = nickName;
        PhotonNetwork.ConnectUsingSettings("Yatzy1.0");
    }

    public override void OnConnectedToMaster()
    {

        Debug.Log(PhotonNetwork.inRoom+"Ŀ��Ʈ ������");
        //PhotonNetwork.JoinOrCreateRoom("Yatzy", new RoomOptions { MaxPlayers = 2 },null);
        
    }

    public override void OnJoinedLobby()
    {

        Debug.Log(PhotonNetwork.inRoom + "�κ�����");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {

        Debug.Log(PhotonNetwork.inRoom + "������");
        SceneLoadManager.singleton.LoadGameSceneAsync((task) =>
        {
            gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
            gameSceneManager.OnMeJoin(isMyRoom);
        });
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("����!");
        //myPhotonView.RPC("OnPlayerConnectRPC", PhotonTargets.AllViaServer);
        gameSceneManager.OnOtherJoin(newPlayer);
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        //myPhotonView.RPC("OnPlayerConnectRPC", PhotonTargets.AllViaServer);
        gameSceneManager.OnOtherDisconnect(otherPlayer);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {

        Debug.Log("���̾����� ���� �����");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnCreatedRoom()
    {

        Debug.Log(PhotonNetwork.inRoom + "���� ���������");
        isMyRoom = true;
        //SceneLoadManager.singleton.LoadGameSceneAsync((task) =>
        //{
        //    GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>().OnPlayerConnect();
        //});

    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        
    }

}
