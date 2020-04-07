using System.IO;
using System.Text;
using UnityEngine;
using TMPro;
using System;

public class TriviaIO : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI warningMessage;
    [SerializeField] TMP_InputField questionIndex;
    [SerializeField] TMP_InputField questionField;
    [SerializeField] TMP_InputField[] answerFields;
    [SerializeField] TMP_Dropdown customQuestionsDropdown;
    string path = "Assets/test.txt";
    string[] questions;

    private void Start()
    {
        ReadQuestionBank();
    }

    #region Editing Actions
    //Completely erases all data in custom questions file
    public void ClearBank()
    {
        GameManager.ClearBank();
        path = "Assets/test.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write("");

        writer.Close();
    }
    //Reads text from file separated by [Q] tags
    public void ReadQuestionBank()
    {
        StreamReader reader = new StreamReader(path);
        //StringBuilder builder = new StringBuilder(reader.ReadToEnd());
        string readText = reader.ReadToEnd();
        string[] separator = { "[Q]" };
        questions = readText.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
        //Debug.Log("Number of questions: " + questions.Length);

    }
    public void DEBUG_DisplayQuestionByIndex()
    {
        int index = Int32.Parse(questionIndex.text);
        if (index > questions.Length)
        {
            warningMessage.text = "index higher than question bank!";
            warningMessage.gameObject.SetActive(true);
            return;
        }
        string[] separator = { "[A]" };
        string[] fullQnA = questions[index].Split(separator, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder questionBuilder = new StringBuilder();

        foreach (string answer in fullQnA)
        {
            questionBuilder.Append(answer);
        }
        warningMessage.text = questionBuilder.ToString();
        warningMessage.gameObject.SetActive(true);
    }

    //Writes input fields into text file as a set of question and answers
    public void SubmitQuestionAndAnswers()
    {
        if (questionField.text.Equals(""))
        {
            warningMessage.text = "No question submitted!";
            warningMessage.gameObject.SetActive(true);
            return;
        }
        if (isThereEmptyAnswerField())
        {
            warningMessage.text = "An answer field is empty!";
            warningMessage.gameObject.SetActive(true);
            return;
        }

        if (warningMessage.gameObject.activeSelf)
        {
            warningMessage.gameObject.SetActive(false);

        }
        string formattedQuestion = BuildQuestionAndAnswerString();
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(formattedQuestion + "\n\n");

        writer.Close();
    }
    public void UpdateBank()
    {
        AddQuestionsToBank();
    }
    #endregion

    //Creates new Question object from input fields and adds them to GameManagers bank
    void AddQuestionsToBank()
    {
        GameManager.ClearBank();
        Question questionToAdd;
        string[] separators = { "[A]" };
        string[] QnA;
        string[] answers;
        foreach (string unsplitQuestion in questions)
        {
            answers = new string[4];
            QnA = unsplitQuestion.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            Array.Copy(QnA, 1, answers, 0, 4);
            questionToAdd = new Question(QnA[0], answers);
            GameManager.AddQuestion(questionToAdd);
        }
    }
    void UpdateDropdownMenu()
    {
        
    }
    bool isThereEmptyAnswerField()
    {
        foreach (TMP_InputField answerField in answerFields)
        {
            if (answerField.text.Equals(""))
                return true;
        }
        return false;
    }
    string BuildQuestionAndAnswerString()
    {
        StringBuilder QnA = new StringBuilder("[Q]" + questionField.text);

        foreach (TMP_InputField answerField in answerFields)
        {
            QnA.Append("\n[A]" + answerField.text);
        }
        return QnA.ToString();

    }

}
