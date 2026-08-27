using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrLife : MonoBehaviour
{
    public float MaxHealth, CurrentHealth;
    public TMP_Text HPText;
    public Animator HPTextAnim;
    private scrGlobalStatus globalStatus;

    public GameObject PlayerHP;
    private Slider HPBar;

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

        GameObject bar = Instantiate(PlayerHP, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
        HPBar = bar.GetComponentInChildren<Slider>();
        HPBar.gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        HPBar.maxValue = MaxHealth;
        HPBar.value = CurrentHealth;
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

<<<<<<< Updated upstream
        if(CurrentHealth>MaxHealth)
        {
            CurrentHealth=MaxHealth;
=======
        float effectiveness = scrEcoFather.ElementEffectiveness(
        attackerEco.Element1,
        attackerEco.Element2,
        globalStatus.currentEco.Element1,
        globalStatus.currentEco.Element2);

        float damage = amount / (Defesa * D)/5*10;

        CurrentHealth -= Mathf.CeilToInt(damage)*effectiveness;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);


        // Salva o HP no slot do Eco
        globalStatus.SaveCurrentEcoHealth(CurrentHealth);


        /* if (HPTextAnim != null){
            HPTextAnim.Play("HP_Animation");
        } */


        UpdateHPText();


        // ECO MORREU
        if (CurrentHealth <= 0){
            CurrentHealth = 0;

            globalStatus.SaveCurrentEcoHealth(0);

            scrPlayer player = GetComponent<scrPlayer>();

            if (player != null){
                player.EcoDied();
            }
>>>>>>> Stashed changes
        }
        else if(CurrentHealth<=0)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetHealthBarVisible()
    {
        HPBar.gameObject.SetActive(true);
        Debug.Log("HP Bar Visible");
    }

    public void SetHealthBarInvisible()
    {
        HPBar.gameObject.SetActive(false);
        Debug.Log("HP Bar Invisible");
    }
}