using UnityEngine;
using UnityEngine.UI;

public class TEMP_Player_Health : MonoBehaviour
{
    //Variables
    public int health = 5;
    public int maxHealth = 5;

    public Image[] T_Health;
    public Image Heart1;
    public Image Heart2;
    public Image Heart3;
    public Image Heart4;
    public Image Heart5;


    public GameObject Death_Screen;

    public void Start()
    {
        Death_Screen.SetActive(false);
        T_Health = new Image[maxHealth];
        T_Health[0] = Heart1;
        T_Health[1] = Heart2;
        T_Health[2] = Heart3;
        T_Health[3] = Heart4;
        T_Health[4] = Heart5;
    }
    public void Update()
    {



        if (health <= 0)
        {
            Death_Screen.SetActive(true);
            Debug.Log("You dead");
        }
    }

    public void Damaged()
    {
        T_Health[health - 1].sprite = null;
        health -= 1;
        Debug.Log(health);
    }
}
