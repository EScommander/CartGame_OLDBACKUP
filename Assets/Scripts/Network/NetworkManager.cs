/* NetworkManager.cs
 * Coded by: Jeramy Zapotosky
 * GAM 52 
 * 
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Make a web build to test out on a single machine
// Web build should be the opposite of what you are working on.

public class NetworkManager : MonoBehaviour 
{
	private const string typeName = "MyUniqueGameType-NORCOGAM52_JER";
	private string gameName = "Echo Chat!";

	private HostData[] hostList; // USED FOR CLIENT ONLY
	private Vector2 scrollPosition;
	private Vector2 scrollPosition2;

	private List<NetworkPlayer> clients = new List<NetworkPlayer>();

	private List<string> conversation = new List<string>();

	private static NetworkManager instance_;

	public GameObject carPrefab;

	public bool isOnline = false;

	public bool countDownStarted = false;
	public static bool gameStarted = false;

	public int minPlayers = 2;

	int row = 0;

	double raceStart = -1;

	public GUIStyle countDownStyle;


//	private CharacterUI charUI;


	// Use this for initialization
	void Start () 
	{


		conversation.Add ("Connected.");
		instance_ = this;

		if(!isOnline)
		{
			GameObject myCar = (GameObject)Instantiate(carPrefab, carPrefab.transform.position, carPrefab.transform.rotation);
			
			Camera.main.transform.parent = myCar.transform;
			Camera.main.transform.localPosition = new Vector3(0,1.4f,-2.15f);
		}

		//MasterServer.ipAddress = YOUR OWN SERVER IP
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!countDownStarted && Network.isServer)
		{
			if(clients.Count + 1 >= minPlayers)
			{
				SendRaceStartTime();
				Debug.Log ("START!");
				countDownStarted = true;
			}
		}
	}

	public static NetworkManager GetInstance()
	{
		if (instance_ == null) 
		{
			GameObject go = new GameObject("_NetworkManager");
			instance_ = (NetworkManager)go.AddComponent<NetworkManager>();
			go.AddComponent<NetworkView>();
		}

		return instance_;
	}

	void OnGUI()
	{
		if(isOnline)
		{
			if (!Network.isClient && !Network.isServer) 
			{
				GUI.Label ( new Rect(100,25, 100, 25), "Game Name:");
				gameName = GUI.TextField( new Rect(100, 50, 200, 25), gameName);
				if(GUI.Button (new Rect(100, 100, 250, 100), "Create Game"))
				{
					StartServer();
				}

				//CLIENT ONLY
				if(GUI.Button (new Rect(100, 250, 250, 100), "Join Game"))
				{
					RefreshHostList();
				}

				if(hostList != null)
				{
					scrollPosition = GUI.BeginScrollView(new Rect(400, 110, 320, 400), scrollPosition, new Rect(0, 0, 300, 110 *hostList.Length));
					for(int i = 0; i < hostList.Length; i++)
					{
						if(GUI.Button (new Rect(0, 0 + (110*i), 300, 100), hostList[i].gameName))
						{
							JoinServer(hostList[i]);
							Debug.Log ("Trying to connect to " + hostList[i].gameName);
						}
					}

					GUI.EndScrollView();
				}
				//CLIENT END
			}
//			if(Network.isClient)
//			{
//
//			}


			if(!gameStarted && raceStart > Network.time)
			{
				int countDown = (int)(raceStart - Network.time) + 1;
				GUI.Label (new Rect(Screen.width/2-100, 0, 200, 200),  ""+countDown, countDownStyle);
			}
			else if(!gameStarted && raceStart != -1)
			{
				gameStarted = true;
			}
			else if(raceStart + 1 > Network.time)
			{

				GUI.Label (new Rect(Screen.width/2-100, 0, 200, 200),  "GO!", countDownStyle);
			}
		}
	}

	private void StartServer()
	{
							  //(int connections, int listenPort, bool useNat) -- 20 used in FTP, 80 used for Internet traffic
		Network.InitializeServer (32, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (typeName, gameName);
	}

	// LOWER METHODS ONLY USED FOR CLIENT ONLY!!
	private void JoinServer(HostData hostData)
	{
		Network.Connect (hostData);
	}

	public void LeaveServer()
	{
		if(Network.isServer)
		{
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}
		else
		{
			Network.CloseConnection(Network.player, true);
		}
	}

	void OnNetworkInstantiate(NetworkMessageInfo info)
	{

	}

	void OnServerInitialized()
	{
		//SpawnPlayer ();
		//CharacterUI.GetInstance ().EnableGUI();
//		CharacterUI.GetInstance ().NetworkInstantiateCharacter ();
//		GameStateManager.GetInstance ().ConnectedToNetwork ();
		Debug.Log (Network.player);
		GameObject myCar = (GameObject)Network.Instantiate(carPrefab, carPrefab.transform.position, carPrefab.transform.rotation, 0);

		Camera.main.transform.parent = myCar.transform;
		Camera.main.transform.localPosition = new Vector3(0,1.4f,-2.15f);

		Debug.Log ("Server Initialized");
	}

	void OnConnectedToServer()
	{
		Debug.Log ("Joined Server!");
		JoinClientListOnServer ();
		int playerPos = int.Parse(Network.player.ToString());
		Debug.Log (playerPos);
		int row = playerPos/4;

		GameObject myCar = (GameObject)Network.Instantiate(carPrefab, carPrefab.transform.position + new Vector3(carPrefab.transform.localScale.x, 0, 0) * 1.5f * playerPos + 
		                                                   new Vector3(0, 0, carPrefab.transform.localScale.z) * 1.5f * row , carPrefab.transform.rotation, 0);

		Camera.main.transform.parent = myCar.transform;
		Camera.main.transform.localPosition = new Vector3(0,1.4f,-2.15f);
		//CharacterUI.GetInstance ().NetworkInstantiateCharacter ();
		//GameStateManager.GetInstance ().ConnectedToNetwork ();
		//SpawnPlayer();
	}

	void RefreshHostList()
	{
		MasterServer.RequestHostList (typeName);
	}

	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if (mse == MasterServerEvent.HostListReceived) 
		{
			hostList = MasterServer.PollHostList();
		}
	}

	[RPC] void JoinClientListOnServer()
	{
		networkView.RPC ("AddClientToList", RPCMode.Server, Network.player);
	}

	[RPC] void AddClientToList(NetworkPlayer player)
	{
		clients.Add (player);
	}


	[RPC] void SendRaceStartTime()
	{
		float delay = 10.0f;
		raceStart = Network.time + delay;
		networkView.RPC ("ReceiveRaceStartTime", RPCMode.Others, delay);

	}

	[RPC] void ReceiveRaceStartTime(float delay)
	{
		raceStart = Network.time + delay;
	}
	

//	// [RPC] = Remote Procedure Call needed for sending across network
//	[RPC] public void SendCharacterXMLToServer(string s, NetworkViewID networkViewID)
//	{
//
//		networkView.RPC ("ReceiveCharacterXMLFromClient", RPCMode.OthersBuffered, s, networkViewID);
//
//	}
//
//	// SERVER ONLY
//	[RPC] void ReceiveCharacterXMLFromClient(string charXML, NetworkViewID id)
//	{
//
//		GameObject[] avatars = GameObject.FindGameObjectsWithTag("Avatar");
//
//		for(int i = 0; i < avatars.Length; i++)
//		{
//			NetworkViewID idCheck = avatars[i].networkView.viewID;
//			Debug.Log(idCheck);
//
//			if(idCheck == id)
//			{
//				Debug.Log("Got here!");
//				CharacterUI.GetInstance().BuildCharacter (charXML, avatars[i]);
//			}
//		}
//	}

}
