using System;
using TMPro;
using UnityEngine;

public class TEMP_TalkingTo : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public GameObject Dialogue;
    public Boolean talking;



    public void Start()
    {
        talking = false;
        ExitDialogue();
    }

    public void Update()
    {
        if (talking == true)
        {
            //te.text = "hello";
            //Debug.Log("is talking rn yeah");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //texts.text = "hello";
                ExitDialogue();
                Debug.Log("leaving the conversation");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            EnterDialogue();
            Debug.Log("lets talk");
        }
    }

    public void EnterDialogue()
    {
        Dialogue.SetActive(true);
        talking = true;
        Debug.Log("takling rn");
    }

    public void ExitDialogue()
    {
        Dialogue.SetActive(false);
        talking = false;
        Debug.Log("not talking at the moment");
    }
}
