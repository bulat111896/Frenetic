using UnityEngine;
using UnityEngine.UI;
using System;

public class Calculator : MonoBehaviour
{
    public Text text;
    public NotebookController notebook;
    string number_str, text_before;
    decimal number;
    double result;
    int count_start, count_finish;

    void Rounding()
    {
        if (text.text[text.text.Length - 1] == '.')
            text.text = result.ToString();
        if (text.text[text.text.Length - 1] != ')' && number_str != "")
            text.text = text_before + Convert.ToDouble(number_str);
    }

    void SetTextBefore()
    {
        number_str = "";
        text_before = text.text;
    }

    void Clear()
    {
        number_str = "";
        text_before = "";
        count_start = 0;
        count_finish = 0;
    }

    public void Add(char character)
    {
        if (text.text == "Error")
            text.text = "0";
        char last_char = text.text[text.text.Length - 1];
        switch (character)
        {
            case 'C':
                text.text = "0";
                Clear();
                break;
            case '=':
                try
                {
                    result = (double)Convert.ToDecimal(new System.Data.DataTable().Compute(text.text.Replace(',', '.'), ""));
                    text.text = result.ToString();
                    if (text.text.Length > 8)
                        text.text = text.text.Substring(0, 7) + "...";
                }
                catch (Exception)
                {
                    text.text = "Error";
                }
                Clear();
                break;
            case '(':
                text.text = text_before + character;
                SetTextBefore();
                count_start++;
                break;
            case ')':
                Rounding();
                if (count_start > count_finish)
                {
                    text.text += character;
                    SetTextBefore();
                    count_finish++;
                }
                break;
            case '/':
            case '*':
            case '-':
            case '+':
                Rounding();
                if (last_char == '/' || last_char == '*' || last_char == '-' || last_char == '+')
                    text.text = text.text.Substring(0, text.text.Length - 1);
                if (last_char == '(')
                {
                    number_str += '0';
                    number = Convert.ToDecimal(number_str);
                    text.text = text_before + number;
                }
                text.text += character;
                SetTextBefore();
                break;
            case ',':
                if (!number_str.Contains(",") && last_char != '/' && last_char != '*' && last_char != '-' && last_char != '+' && last_char != '(' && last_char != ')' && last_char != '.')
                {
                    text.text += character;
                    number_str += character;
                }
                break;
            default:
                number_str += character;
                number = Convert.ToDecimal(number_str);
                text.text = text_before + number;
                break;
        }
    }
}