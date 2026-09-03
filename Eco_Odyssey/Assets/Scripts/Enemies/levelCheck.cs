using UnityEngine;

public class levelCheck : MonoBehaviour
{
    private scrEnemyEco ecoComp;
    private scrGlobalStatus globalStatus;
    public int level;
    public float speed, def, atk, hp;

    void Start()
    {
        ecoComp = GetComponent<scrEnemyEco>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            globalStatus = player.GetComponent<scrGlobalStatus>();
        }
    }
    void FixedUpdate()
    {
        level=globalStatus.levelC;
        speed = Mathf.CeilToInt((ecoComp.ecoData.Velocidade + 15 * Mathf.Sqrt(level)) / 10);
        def=Mathf.CeilToInt((ecoComp.ecoData.Defesa+2*Mathf.Sqrt(level))*10);
        atk=Mathf.CeilToInt((ecoComp.ecoData.Ataque+2*Mathf.Sqrt(level))*10);
        hp=Mathf.CeilToInt((ecoComp.ecoData.Vida+2*Mathf.Sqrt(level))*10);
    }
}
