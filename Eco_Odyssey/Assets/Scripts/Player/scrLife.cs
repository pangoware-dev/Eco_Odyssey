using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scrLife : MonoBehaviour{
    public float MaxHealth;
    public float CurrentHealth;

    public TMP_Text HPText;
    public Animator HPTextAnim;
    private scrGlobalStatus globalStatus;

    public GameObject PlayerHP;
    private Slider HPBar;

    private BlinkingSprite blink;
    public int Defesa;
    public float D, Damage;


    // START
  void Start(){
        globalStatus = GetComponent<scrGlobalStatus>();

        if (globalStatus != null && globalStatus.currentEco != null){
            MaxHealth = globalStatus.vidaC*2;

            CurrentHealth = globalStatus.GetCurrentEcoHealth();
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


    // RECEBER DANO
    public void ChangeHealth(int amount, scrEcoFather attackerEco){
        if (globalStatus == null || globalStatus.currentEco == null){
            return;
        }

        D = 1f;

        Defesa = (int)globalStatus.defC;

        float effectiveness = scrEcoFather.ElementEffectiveness(
        attackerEco.Element1,
        attackerEco.Element2,
        globalStatus.currentEco.Element1,
        globalStatus.currentEco.Element2);

        float damage = amount - (Defesa * D)/3;

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
        }
    }


    // ATUALIZAR TEXTO
    public void UpdateHPText(){
        if (HPText == null){
            return;
        }

        HPText.text = "HP: " + Mathf.Ceil(CurrentHealth) + "/" + Mathf.Ceil(MaxHealth);
    }

    public void SetHealthBarVisible()
    {
        HPBar.gameObject.SetActive(true);
    }

    public void SetHealthBarInvisible()
    {
        HPBar.gameObject.SetActive(false);
    }
}