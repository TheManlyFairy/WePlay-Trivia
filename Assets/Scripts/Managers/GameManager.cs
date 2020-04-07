using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Utilities;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static GameManager instance;
    [SerializeField] List<Question> questionBank;
    List<Question> questionsUsed;
    List<Player> players;
    Question currentQuestion;
    int amountOfPlayersThatAnswered;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            questionBank = new List<Question>();
            questionsUsed = new List<Question>();
        }

    }

    public static void AddQuestion(Question questionToAdd)
    {
        instance.questionBank.Add(questionToAdd);
    }
    public static void ClearBank()
    {
        instance.questionBank.Clear();
    }

    
    //INCOMPLETE METHOD
    void CheckPlayerAnswer(string answer)
    {
        if (answer.Equals(currentQuestion.answers[0]))
        {
            //Give player score
        }
    }

    void GenerateNextQuestion()
    {
        amountOfPlayersThatAnswered = 0;
        currentQuestion = questionBank[Random.Range(0, questionBank.Count)];
        string[] scrambledAnswers = ScrambleAnswers(currentQuestion.answers);
        object[] answers = { scrambledAnswers[0], scrambledAnswers[1], scrambledAnswers[2], scrambledAnswers[3] };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.DoNotCache
        };
        SendOptions sendOptions = new SendOptions() { Reliability = false };

        PhotonNetwork.RaiseEvent((byte)CustomEventCodes.SendQnAToPlayers, answers, raiseEventOptions, sendOptions);
    }
    string[] ScrambleAnswers(string[] answers)
    {
        string[] scrambledAnswers = answers;
        string tempHolder;
        int randomIndex;
        for (int i = 0; i < answers.Length; i++)
        {
            randomIndex = Random.Range(0, answers.Length);
            tempHolder = scrambledAnswers[randomIndex];
            scrambledAnswers[randomIndex] = scrambledAnswers[i];
            scrambledAnswers[i] = tempHolder;
        }

        return scrambledAnswers;
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch (eventCode)
        {
            case (byte)CustomEventCodes.SendPlayerAnswer:
                object[] data = (object[])photonEvent.CustomData;
                string answer = (string)data[0];
                CheckPlayerAnswer(answer);
                amountOfPlayersThatAnswered++;
                break;
        }
    }
}
