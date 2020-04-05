using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]List<Question> questionBank;
    List<Question> questionsUsed;

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
}
