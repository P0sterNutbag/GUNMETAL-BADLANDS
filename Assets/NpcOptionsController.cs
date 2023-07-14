using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcOptionsController : MonoBehaviour
{
    public string[] options = { "CONTACT", "ENGAGE" };

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("NPC Contacted");
        }
        else if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
