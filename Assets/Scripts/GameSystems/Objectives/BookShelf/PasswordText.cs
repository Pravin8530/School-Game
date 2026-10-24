using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class PasswordText : MonoBehaviour
{ 
    public TextMeshProUGUI[] passwordText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void UpdatePasswordText(List<Book> currentBookSequence)
    { 
        for (int i = 0; i < currentBookSequence.Count; i++)
        {
            passwordText[i].text = currentBookSequence[i].bookColor.ToString();
        }
    }

}
