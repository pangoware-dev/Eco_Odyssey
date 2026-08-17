using UnityEngine;
using UnityEngine.UI;

public class scrEHealth : MonoBehaviour{
    public float currentHP;
    public float maxHP;

    private scrEnemyEco ecoComp;
    private BlinkingSprite blink;
    public GameObject EnemyHP;
    private Slider HPBar;

    public float D, Damage;

    void Start(){
        ecoComp = GetComponent<scrEnemyEco>();
        D = 1f;

        if (ecoComp != null && ecoComp.ecoData != null){
            maxHP = ecoComp.ecoData.Vida*2;
        }

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
        HPBar.maxValue = maxHP;
        HPBar.value = currentHP;
    }

    public void SetHealthBarVisible()
    {
       HPBar.gameObject.SetActive(true);
    }

    public void SetHealthBarInvisible()
    {
        HPBar.gameObject.SetActive(false);
    }

    public void changeHP(float amount, scrEcoFather attackerEco)
    {
        float effectiveness = scrEcoFather.ElementEffectiveness(
        attackerEco.Element1,
        attackerEco.Element2,
        ecoComp.ecoData.Element1,
        ecoComp.ecoData.Element2);

        Damage = amount-(ecoComp.ecoData.Defesa*D)/3;
        currentHP -= Mathf.CeilToInt(Mathf.CeilToInt(Damage)*effectiveness);

        Debug.Log("HP Inimigo: " + currentHP);
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
