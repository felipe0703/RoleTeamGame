using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Com.BrumaGames.Llamaradas
{
    public class LobbyMain : MonoBehaviourPunCallbacks
    {
        #region variables
        [Header("Login Panel")]
        public GameObject LoginPanel;

        public TMP_InputField PlayerNameInput;

        [Header("Selection Panel")]
        public GameObject SelectionPanel;

        [Header("Create Room Panel")]
        public GameObject CreateRoomPanel;

        public TMP_InputField RoomNameInputField;
        public TMP_InputField MaxPlayersInputField;
        public TMP_Dropdown MaxPlayers_InputField;
        public TMP_Dropdown GameTimeInputField;
        public TMP_Dropdown RoundTimeInputField;


        [Header("Join Random Room Panel")]
        public GameObject JoinRandomRoomPanel;

        [Header("Room List Panel")]
        public GameObject RoomListPanel;

        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;

        [Header("Inside Room Panel")]
        public GameObject InsideRoomPanel;

        public Button StartGameButton;
        public GameObject PlayerListEntryPrefab;

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;

        public string region;

        private int optionTime;
        private int optionR_Time;


        [Header("buttons Selection Panel")]
        public GameObject buttonRoomList;
        #endregion

        #region UNITY

        //i18n
        private string room;

        private void Awake()
        {
           

            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();

            //PlayerNameInput.text = "Jugador " + Random.Range(1000, 10000);
            //region = "";

            //inicializar el dropdown con opciones y traducción getText i18n

            
            string o_game1 = I18nManager.sharedInstance.GetText("gameTime1");
            string o_game2 = I18nManager.sharedInstance.GetText("gameTime2");
            string o_game3 = I18nManager.sharedInstance.GetText("gameTime3");
            List<string> m_GameTimeDropOptions = new List<string> { o_game1, o_game2 , o_game3 };
            GameTimeInputField.AddOptions(m_GameTimeDropOptions);

            string o_round1 = I18nManager.sharedInstance.GetText("roundTime1");
            string o_round2 = I18nManager.sharedInstance.GetText("roundTime2");
            string o_round3 = I18nManager.sharedInstance.GetText("roundTime3");
            string o_round4 = I18nManager.sharedInstance.GetText("roundTime4");
            List<string> m_RoundTimeDropOptions = new List<string> { o_round1, o_round2, o_round3, o_round4 };
            RoundTimeInputField.AddOptions(m_RoundTimeDropOptions);

        }

        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Update()
        {
            if (PhotonNetwork.LevelLoadingProgress > 0 && PhotonNetwork.LevelLoadingProgress < 1)
            {
                //Debug.Log(PhotonNetwork.LevelLoadingProgress);
            }
        }
        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            this.SetActivePanel(SelectionPanel.name);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();

            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            room = I18nManager.sharedInstance.GetText("room");
            string roomName = room + " " + Random.Range(1000, 10000);

            RoomOptions options = new RoomOptions { MaxPlayers = 8 };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public override void OnJoinedRoom()
        {
            SetActivePanel(InsideRoomPanel.name);

            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }


            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerListEntryPrefab);
                entry.transform.SetParent(InsideRoomPanel.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

                object isPlayerReady;

                if (p.CustomProperties.TryGetValue(LlamaradaGame.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }

                playerListEntries.Add(p.ActorNumber, entry);
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());

            Hashtable props = new Hashtable
            {
                {LlamaradaGame.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnLeftRoom()
        {//todo
            SetActivePanel(SelectionPanel.name);

            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

            playerListEntries.Add(newPlayer.ActorNumber, entry);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartGameButton.gameObject.SetActive(CheckPlayersReady());
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(LlamaradaGame.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
        #endregion

        #region UI CALLBACKS

        public void OnBackButtonClicked()
        {
            StartCoroutine(OnBack());
        }

        IEnumerator OnBack()
        {
            yield return new WaitForSeconds(.7f);
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            SetActivePanel(SelectionPanel.name);
        }

        public void OnCreateRoomButtonClicked()
        {
            StartCoroutine(OnCreateRoom());
        }

        public int GetMaxPlayers(int index)
        {
            int maxPlayers = 9 ;

            switch (index)
            {
                case 0: return maxPlayers = 4;
                case 1: return maxPlayers = 5;
                case 2: return maxPlayers = 6;
                case 3: return maxPlayers = 7;
                case 4: return maxPlayers = 8;
                case 5: return maxPlayers = 9;
            }

            return maxPlayers;
        }


        IEnumerator OnCreateRoom()
        {
            yield return new WaitForSeconds(.7f);

            string roomName = RoomNameInputField.text;
            roomName = (roomName.Equals(string.Empty)) ? I18nManager.sharedInstance.GetText("room") + " " + Random.Range(1000, 10000) : roomName;

            int maxPlayers = GetMaxPlayers(MaxPlayers_InputField.value);
            Debug.Log(maxPlayers);
            //byte maxPlayers;
            //byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
            //maxPlayers = (byte)Mathf.Clamp(maxPlayers, 4, 9);

            RoomOptions options = new RoomOptions { MaxPlayers = (byte)maxPlayers };
            Hashtable customRoomProperties = new Hashtable();
            customRoomProperties.Add("TIMER", GameTimeInputField.value);
            customRoomProperties.Add("R_TIME", RoundTimeInputField.value);
            options.CustomRoomProperties = customRoomProperties;
            
            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public void OnJoinRandomRoomButtonClicked()
        {
            StartCoroutine(OnJoinRandomRoom());
        }

        IEnumerator OnJoinRandomRoom()
        {
            yield return new WaitForSeconds(.7f);
            SetActivePanel(JoinRandomRoomPanel.name);

            PhotonNetwork.JoinRandomRoom();
        }

        public void OnLeaveGameButtonClicked()
        {
            StartCoroutine(OnLeaveGame());
        }

        IEnumerator OnLeaveGame()
        {
            yield return new WaitForSeconds(.7f);
            PhotonNetwork.LeaveRoom();
        }

        // tomo el nombre del jugador y lo dejo como su apodo y configura la conexion


        public void OnLoginButtonClicked()
        {
            string playerName = PlayerNameInput.text;

            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                if (region == "")
                {
                    OnLoginConnect();
                }
                else
                {
                    OnLoginConnectToRegion(region);
                }
            }
            else
            {
                Debug.LogError("Nombre de jugador invalido.");
            }
        }

        public void OnLoginConnect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        //sin usar
        public void SetRegionFromDropDown(int index)
        {
            if (index == 0)
            {
                region = "";
                return;
            }
            
            if (index == 1)
            {
                region = "eu";
            }
            else if (index == 2)
            {
                region = "sa";
            }
        }
        
        public void OnLoginConnectToRegion(string region)
        {
            PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            Debug.Log("Region: " + region);
            PhotonNetwork.ConnectToRegion(region);
        }



        public void OnRoomListButtonClicked()
        {
            StartCoroutine(OnRoomList());            
        }

        IEnumerator OnRoomList()
        {
            yield return new WaitForSeconds(.7f);
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            SetActivePanel(RoomListPanel.name);
        }

        public void OnStartGameButtonClicked()
        {
            StartCoroutine(OnStartGame());
        }

        IEnumerator OnStartGame()
        {
            yield return new WaitForSeconds(.7f);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel("Game Felipe Multiplayer");
        }

        #endregion
        
        private bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(LlamaradaGame.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void LocalPlayerPropertiesUpdated()
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
        
        public void SetActivePanel(string activePanel)
        {
            if(activePanel == "CreateRoomPanel")
                StartCoroutine(SetActivePanelCreateRoom());
            else
            {
                SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
                CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            }
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));            
            JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
            InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        }

        IEnumerator SetActivePanelCreateRoom()
        {
            yield return new WaitForSeconds(.7f);
            string activePanel = "CreateRoomPanel";
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        }

        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }

        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                roomListEntries.Add(info.Name, entry);
            }
        }



    }
}

