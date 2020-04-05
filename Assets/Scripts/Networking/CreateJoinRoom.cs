using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Text;

public class CreateJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField roomCodeInputField;
    [SerializeField] int maxPlayersPerRoom;
    [SerializeField] int hostSceneIndex;
    [SerializeField] int clientSceneIndex;

    public static CreateJoinRoom instance;

    int sceneIndexToLoad;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(sceneIndexToLoad);
        }
    }
    
    public void CreateRoom()
    {
        Debug.Log("Creating room now");
        int sceneIndexToLoad = hostSceneIndex;
        string roomCode = GenerateRoomCode();
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayersPerRoom };
        if (PhotonNetwork.CreateRoom(roomCode, roomOps))
            Debug.Log("Created room code " + roomCode);
    }
    public void JoinRoom()
    {
        sceneIndexToLoad = clientSceneIndex;
        PhotonNetwork.JoinRoom(roomCodeInputField.text);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Faild to join room: " + "Room " + roomCodeInputField.text);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create room FAILED");
        CreateRoom();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name);
        StartGame();
    }

    string GenerateRoomCode()
    {
        StringBuilder randomCode = new StringBuilder();

        randomCode.Append(Random.Range('A', 'Z'));
        randomCode.Append(Random.Range('A', 'Z'));
        randomCode.Append(Random.Range('A', 'Z'));
        randomCode.Append(Random.Range('A', 'Z'));

        return randomCode.ToString();

    }
}