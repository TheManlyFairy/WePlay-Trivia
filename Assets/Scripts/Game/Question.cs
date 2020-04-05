﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question 
{
    public string question;
    public string[] answers;

    public Question(string question, string[] answers)
    {
        this.question = question;
        this.answers = answers;
    }
}
