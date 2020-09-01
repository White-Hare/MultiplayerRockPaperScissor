using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;


public class Web : MonoBehaviour
{
    public static Web Instance;

    private string baselink = "http://localhost/RockPaperScrissor/";

    [SerializeField] private Text result;
    [SerializeField] private float timeout = 10f;

    public string player1Name;
    public string card1;

    public string player2Name;
    public string card2;

    public int roomID;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);



        WWWForm form = new WWWForm();
        form.AddField("playername", player1Name);


        StartCoroutine(SendRequest(baselink + "Connect.php", form, (string data) =>
        {
            JSONObject o = (JSONObject) JSON.Parse(data);
            roomID = Int32.Parse(o["roomid"]);
            player2Name = player1Name == o["player1name"] ? o["player2name"] : o["player1name"];
        }));
    }

    public void SendCard(string cardname)
    {
        card1 = cardname;

        WWWForm form = new WWWForm();
        form.AddField("playername", player1Name);
        form.AddField("card", cardname);

        StartCoroutine(SendRequest(baselink + "SendCard.php", form, (string data) =>
        {
            if (String.IsNullOrEmpty(data))
                GetCard();
            else
                Debug.Log(data);

        }));

    }

    private void GetCard()
    {

        WWWForm form = new WWWForm();
        form.AddField("player2name", player2Name);

        StartCoroutine(SendRequest(baselink + "GetCard.php", form, (data) =>
        {
            float ctime = Time.time;
            do
            {
                card2 = data;
            } while (String.IsNullOrEmpty(card2) && Time.time - ctime < timeout);



            if (!String.IsNullOrEmpty(card2))
            {
                if (card1 == "Rock" && card2 == "Scissor")
                    result.text = "YOU WIN";

                else if (card1 == "Scissor" && card2 == "Paper")
                    result.text = "YOU WIN";

                else if (card1 == "Paper" && card2 == "Rock")
                    result.text = "YOU WIN";

                else if (card1 == card2)
                    result.text = "DRAW";

                else
                    result.text = "YOU LOSE";
            }
            else
                result.text = "DISCONNECTED";

            result.gameObject.SetActive(true);

        }));

    }

    private IEnumerator SendRequest(string link, WWWForm form, Action<string> func)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(link, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("ERROR: " + www.error + "\nDATA: " + www.downloadHandler.text);
                yield break;
            }


            func(www.downloadHandler.text);
        }
    }

    //private void OnApplicationQuit()
    //{
    //    //Disconnect
    //    WWWForm form = new WWWForm();
    //    form.AddField("roomid", roomID);
    //    using (UnityWebRequest www = UnityWebRequest.Post(baselink + "Disconnect.php", form))
    //    {
    //        www.SendWebRequest();
    //        while(!www.isDone)
    //            System.Threading.Thread.Sleep(100);

    //        if (www.isNetworkError || www.isHttpError)
    //            Debug.Log("ERROR: " + www.error + "\nDATA: " + www.downloadHandler.text);
            
    //    }
    //}
}
