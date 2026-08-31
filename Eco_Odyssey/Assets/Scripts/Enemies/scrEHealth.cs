using UnityEngine;
using UnityEngine.UI;

public class scrEHealth : MonoBehaviour{
    public float currentHP;
    public float maxHP;
    private float def;

    private scrEnemyEco ecoComp;
    private levelCheck levelCheck;
    private BlinkingSprite blink;
    public GameObject EnemyHP;
    private Slider HPBar;

    public float D, Damage;

    void Start(){
        ecoComp = GetComponent<scrEnemyEco>();
        levelCheck = GetComponent<levelCheck>();
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
        if (!HPBar.gameObject.activeSelf)
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

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if (currentHP <= 0)
        {
            currentHP=1;
        }
    }
}
