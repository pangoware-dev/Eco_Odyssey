using UnityEngine;
using UnityEngine.UI;

public class scrEHealth : MonoBehaviour{
    public float currentHP;
    public float maxHP;
    public int level=5;
    private float def;

    private scrEnemyEco ecoComp;
    private BlinkingSprite blink;
    public GameObject EnemyHP;
    public scrGlobalStatus globalStatus;
    private Slider HPBar;

    public float D, Damage;

    void Start(){
        ecoComp = GetComponent<scrEnemyEco>();
        D = 1f;
        level=5;
        def=(ecoComp.ecoData.Defesa+2*Mathf.Sqrt(level))*10;

        if (ecoComp != null && ecoComp.ecoData != null){
            maxHP = (ecoComp.ecoData.Vida+2*Mathf.Sqrt(level))*10;
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
        level=globalStatus.levelC;

        if (ecoComp != null && ecoComp.ecoData != null){
            maxHP = (ecoComp.ecoData.Vida+2*Mathf.Sqrt(level))*10;
        }
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
        if (HPBar.gameObject==false)
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

        Damage = amount-def*D/3;
        Damage = Mathf.Max(Damage, 1);
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
