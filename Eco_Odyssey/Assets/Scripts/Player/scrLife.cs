using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrLife : MonoBehaviour
{
    public float MaxHealth, CurrentHealth;
    public TMP_Text HPText;
    public Animator HPTextAnim;
    private scrGlobalStatus globalStatus;

    public GameObject HealthBar;
    private Slider healthBarSlider;

    private BlinkingSprite blink;
    public int Defesa;
    public float D, Damage;

    public void Start()
    {
        globalStatus = GetComponent<scrGlobalStatus>();

        if(globalStatus != null && globalStatus.currentEco != null)
        {
            MaxHealth = (int)globalStatus.vidaC;
            CurrentHealth = MaxHealth;
        }

        HPText.text = "HP: " + CurrentHealth + "/" + MaxHealth;
        
        if (blink == null)
        {
            blink = gameObject.AddComponent<BlinkingSprite>();
        }
        blink = GetComponent<BlinkingSprite>();

        GameObject bar = Instantiate(HealthBar);
        healthBarSlider = HealthBar.GetComponentInChildren<Slider>();
        bar.transform.localPosition = new Vector3(0, 1.5f, 0);
    }

    public void FixedUpdate()
    {
        healthBarSlider.maxValue = MaxHealth;
        healthBarSlider.value = CurrentHealth;
        Debug.Log("Pai da barra: " + HealthBar.transform.parent.name);
    }
    
    public void ChangeHealth(int amount)
    {
        D = 0.4f;
        Defesa = (int)globalStatus.defC;
        
        Damage = (amount/(Defesa*D)/2)+1;
        CurrentHealth-=Mathf.CeilToInt(Damage);
        HPTextAnim.Play("Text_Pop");
        blink.Blink();
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