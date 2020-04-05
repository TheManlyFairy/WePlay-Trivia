using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Utilities;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject OpenJoinRoomGroup;
    [SerializeField] GameObject StartupMessageGroup;
    [SerializeField] Image loadingIcon;

    public static UIManager instance;

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    IEnumerator Start()
    {
        while (!PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        StartupMessageGroup.SetActive(false);
        OpenJoinRoomGroup.SetActive(true);

    }
}
