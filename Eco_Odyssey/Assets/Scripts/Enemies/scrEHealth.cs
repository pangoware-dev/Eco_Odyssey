using UnityEngine;
using UnityEngine.UI;

public class scrEHealth : MonoBehaviour{
    public float currentHP;
    public float maxHP;
    private float def;

    private scrEnemyEco ecoComp;
    private levelCheck levelCheck;
    private BlinkingSprite blink;
    private NPC_Trainer npc_Trainer;
    public GameObject EnemyHP;
    private Slider HPBar;

    public float D, Damage;

    void Start(){
        ecoComp = GetComponent<scrEnemyEco>();
        levelCheck = GetComponent<levelCheck>();
        npc_Trainer = GetComponent<NPC_Trainer>();
        D = 1f;

        maxHP = levelCheck.hp;
        currentHP = maxHP;

        if (blink == null)
        {
            blink = gameObject.AddComponent<BlinkingSprite>();
        }
        blink = GetComponent<BlinkingSprite>();

        GameObject bar = Instantiate(EnemyHP, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
        HPBar = bar.GetComponentInChildren<Slider>();
        HPBar.gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        maxHP = levelCheck.hp;
        def=levelCheck.def;
        HPBar.maxValue = maxHP;
        HPBar.value = currentHP;
        
        MaxHPLevel();
    }

    public void SetHealthBarVisible()
    {
       HPBar.gameObject.SetActive(true);
    }

    public void SetHealthBarInvisible()
    {
        HPBar.gameObject.SetActive(false);
    }

    public void MaxHPLevel()
    {
        if (!HPBar.gameObject.activeSelf&&ecoComp.isTamed==false)
        {
            currentHP = maxHP;
        }
    }

    public void changeHP(float amount, scrEcoFather attackerEco)
    {
        float effectiveness = scrEcoFather.ElementEffectiveness(
        attackerEco.Element1,
        attackerEco.Element2,
        ecoComp.ecoData.Element1,
        ecoComp.ecoData.Element2);

        Damage = amount/(def/10)*D;
        Damage = Mathf.Max(Damage, 1);
        currentHP -= Mathf.CeilToInt(Mathf.CeilToInt(Damage)*effectiveness);

        Debug.Log("HP Inimigo: " + currentHP);
        Debug.Log("Defesa Inimigo: "+def);
        blink.Blink();

        if (ecoComp.isTamed==true)
        {
            npc_Trainer.SaveCurrentEcoHealth(currentHP);
        }

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }else if (currentHP <= 0&&ecoComp.isTamed==false)
        {
            currentHP=1;
        }
        else if(currentHP <= 0&&ecoComp.isTamed==true)
        {
            currentHP=0;
            npc_Trainer.SaveCurrentEcoHealth(0);
            int nextEco = npc_Trainer.GetNextAliveEco();
            HPBar.gameObject.SetActive(false);

            if (nextEco != -1)
            {
                // Equipa o próximo Eco
                npc_Trainer.EquipEco(nextEco);

                // Atualiza a vida do novo Eco
                maxHP = levelCheck.hp;
                currentHP = npc_Trainer.vidaAtual;

                // Atualiza a animação
                npc_Trainer.ChangeAnimator();

                Debug.Log("Próximo Eco: " + npc_Trainer.currentEco.name);
            }
            else
            {
                Debug.Log("Todos os Ecos do treinador foram derrotados!");

                npc_Trainer.EndBattle();
            }
        }
    }
}
