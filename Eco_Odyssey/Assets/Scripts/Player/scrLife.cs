using UnityEngine;
using TMPro;

public class scrLife : MonoBehaviour
{
    public float MaxHealth, CurrentHealth;
    public TMP_Text HPText;
    public Animator HPTextAnim;
    scrGlobalStatus globalStatus;
    public int Defesa;
    public float D;

    public void Start()
    {
        globalStatus = GetComponent<scrGlobalStatus>();

        if(globalStatus != null && globalStatus.currentEco != null)
        {
            MaxHealth = (int)globalStatus.vidaC;
            CurrentHealth = MaxHealth;
        }

        HPText.text = "HP: " + CurrentHealth + "/" + MaxHealth;
    }
    
    public void ChangeHealth(int amount)
    {
        D = 0.4f;
        Defesa = (int)globalStatus.defC;
        
        CurrentHealth-=(amount/(Defesa*D)/2)+1;
        HPTextAnim.Play("HP_Animation");
        HPText.text="HP: "+CurrentHealth+"/"+MaxHealth;

        if(CurrentHealth>MaxHealth)
        {
            CurrentHealth=MaxHealth;
        }
        else if(CurrentHealth<=0)
        {
            gameObject.SetActive(false);
        }
    }
}