using UnityEngine;
using UnityEngine.UI;

public class scrEHealth : MonoBehaviour
{
    public float currentHP;
    public float maxHP;

    private scrEnemyEco ecoComp;
    private BlinkingSprite blink;
    public GameObject EnemyHP;
    private Slider HPBar;

    public float D, Damage;

    void Start()
    {
        ecoComp = GetComponent<scrEnemyEco>();
        D = 0.5f;

        if (ecoComp != null && ecoComp.ecoData != null)
        {
            maxHP = ecoComp.ecoData.Vida;
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
       Debug.Log("HP Bar Visible");
    }

    public void SetHealthBarInvisible()
    {
        HPBar.gameObject.SetActive(false);
        Debug.Log("HP Bar Invisible");
    }

    public void changeHP(float amount)
    {
<<<<<<< Updated upstream
        Damage = amount/(ecoComp.ecoData.Defesa*D)/2+1;
        currentHP -= Mathf.CeilToInt(Damage);
=======
        float effectiveness = scrEcoFather.ElementEffectiveness(
        attackerEco.Element1,
        attackerEco.Element2,
        ecoComp.ecoData.Element1,
        ecoComp.ecoData.Element2);

        Damage = amount/(ecoComp.ecoData.Defesa*D)/5*10;
        currentHP -= Mathf.CeilToInt(Mathf.CeilToInt(Damage)*effectiveness);
>>>>>>> Stashed changes

        Debug.Log("HP Inimigo: " + currentHP);
        blink.Blink();

        if(currentHP > maxHP || currentHP <= 0)
        {
            currentHP = maxHP;
        }
    }
}