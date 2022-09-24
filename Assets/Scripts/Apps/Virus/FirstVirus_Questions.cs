using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FirstVirus_Questions : SerializedMonoBehaviour
{
    [SerializeField]
    private List<string> questions  = new List<string>();

    [SerializeField]
    private List<List<string>> answers = new List<List<string>>();

    public int QuestionCount()
    {
        return questions.Count;
    }

    public string GetQuestion(int i)
    {
        return questions[i];
    }

    public bool CheckAnwser(string ans, int index)
    {
        return answers[index].Contains(ans);
    }
}
