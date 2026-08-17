using UnityEngine;
using TMPro;

public class scrLife : MonoBehaviour{
    public float MaxHealth;
    public float CurrentHealth;

    public TMP_Text HPText;
    public Animator HPTextAnim;

    private scrGlobalStatus globalStatus;

    public int Defesa;
    public float D;


    // START
  void Start(){
        globalStatus = GetComponent<scrGlobalStatus>();

        if (globalStatus != null && globalStatus.currentEco != null){
            MaxHealth = globalStatus.vidaC;

            CurrentHealth = globalStatus.GetCurrentEcoHealth();
        }

        UpdateHPText();
    }


    // RECEBER DANO
    public void ChangeHealth(int amount){
        if (globalStatus == null || globalStatus.currentEco == null){
            return;
        }

        D = 0.4f;

        Defesa = (int)globalStatus.defC;

        float damage = (amount / (Defesa * D) / 2) + 1;

        CurrentHealth -= damage;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);


        // Salva o HP no slot do Eco
        globalStatus.SaveCurrentEcoHealth(CurrentHealth);


        if (HPTextAnim != null){
            HPTextAnim.Play("HP_Animation");
        }


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
}