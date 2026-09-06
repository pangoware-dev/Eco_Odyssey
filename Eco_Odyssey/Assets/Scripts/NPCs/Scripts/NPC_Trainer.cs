using UnityEngine;

public class NPC_Trainer : MonoBehaviour
{
        // PARTY DE ECOS
    public scrEcoFather[] ecoParty = new scrEcoFather[7];

    // HP ATUAL DE CADA ECO
    public float[] vidaAtualParty = new float[7];

    // ECO ATUALMENTE SELECIONADO
    public int currentEcoIndex = 0;
    public scrEcoFather currentEco;
    private levelCheck levelCheck;
    private NPC npc;
    private NPC_Patrol npc_Patrol;
    private NPC_Talking npc_Talking;
    public Animator anim;
    public CircleCollider2D ecoCol;
    public CircleCollider2D dialogueCol;
    public CapsuleCollider2D trainCol;
    private scrChase chase;
    private scrEHealth eHealth;
    private scrEnemyKB eKB;
    private scrEnemyEco eEco;
    private scrAnimationControl animControl;

    // STATUS DO ECO ATUAL
    public float vidaAtual;
    public float vidaC;
    public float atkC;
    public float defC;
    public float veloC;
    public int levelC;
    public int quantEco;

    public RuntimeAnimatorController animControllerC;
    public RuntimeAnimatorController animControllerNPC;
    public bool usingEco = false;

    public void Start()
    {
        levelCheck = GetComponent<levelCheck>();
        npc = GetComponent<NPC>();
        npc_Patrol = GetComponent<NPC_Patrol>();
        npc_Talking = GetComponent<NPC_Talking>();
        chase = GetComponent<scrChase>();
        eHealth = GetComponent<scrEHealth>();
        eKB = GetComponent<scrEnemyKB>();
        eEco = GetComponent<scrEnemyEco>();
        animControl = GetComponent<scrAnimationControl>();

        quantEco = 0;

        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null)
            {
                quantEco++;

                vidaAtualParty[i] = CalcularVidaMaxima(i);
            }
        }

        // Equipa o primeiro Eco vivo
        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                EquipEco(i);
                break;
            }
        }

        usingEco = false;
    }

    void Update()
    {
        if (trainCol.enabled==false && (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow)))
        {
            dialogueCol.enabled=false;
        }

        if (chase.enabled==true)
        {
            npc_Patrol.enabled=false;
        }
    }

    public void StartBattle()
    {
        usingEco = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        //Cura a Party
        HealParty();

        //Seleciona o Eco
        currentEcoIndex = 0;
        EquipEco(currentEcoIndex);
        ChangeAnimator();

        //Altera sua vida para a vida máxima
        eHealth.currentHP=eHealth.maxHP;

        //Ativar eco
        ecoCol.enabled=true;
        animControl.enabled=true;
        chase.enabled=true;
        

        //Desativar NPC
        trainCol.enabled=false;
        npc.enabled=false;
        npc_Patrol.enabled=false;
    }

    public void EndBattle()
    {
        usingEco = false;
        gameObject.layer = LayerMask.NameToLayer("Default");

        //Desativar eco
        ecoCol.enabled=false;
        chase.enabled=false;
        animControl.enabled=false;
        

        //Ativar NPC
        trainCol.enabled=true;
        dialogueCol.enabled=true;
        npc.enabled=true;
        npc_Patrol.enabled=true;

        ChangeAnimator();
        currentEcoIndex = 0;
        EquipEco(currentEcoIndex);
        HealParty();
    }
    

    public void ChangeAnimator()
    {
        if (usingEco==true)
        {
            anim.runtimeAnimatorController = currentEco.animControllerEco;
        }
        else if(usingEco==false)
        {
            anim.runtimeAnimatorController = animControllerNPC;

        }
    }


    // =========================
    // EQUIPAR ECO
    // =========================

    public bool EquipEco(int index)
    {
        if (index < 0 || index >= ecoParty.Length)
        {
            Debug.Log("Índice de Eco inválido.");
            return false;
        }

        if (ecoParty[index] == null)
        {
            Debug.Log("Slot vazio.");
            return false;
        }

        if (vidaAtualParty[index] <= 0)
        {
            Debug.Log("Eco morto.");
            return false;
        }

        currentEcoIndex = index;
        currentEco = ecoParty[index];
        eEco.ecoData = currentEco;

        // Calcula os atributos diretamente para ESTE Eco
        vidaC = CalcularVidaMaxima(index);
        atkC = (currentEco.Ataque + 2 * Mathf.Sqrt(levelCheck.level)) * 10;
        defC = (currentEco.Defesa + 2 * Mathf.Sqrt(levelCheck.level)) * 10;
        veloC = levelCheck.speed;

        // Recupera o HP salvo
        vidaAtual = vidaAtualParty[index];

        animControllerC = currentEco.animControllerEco;

        // Se o sistema de vida já estiver inicializado
        if (eHealth != null)
        {
            eHealth.maxHP = vidaC;
            eHealth.currentHP = vidaAtual;
        }

        Debug.Log(
            "Eco equipado: " + currentEco.name +
            " | HP: " + vidaAtual + "/" + vidaC +
            " | Índice: " + currentEcoIndex
        );

        return true;
    }


    // =========================
    // SALVAR HP DO ECO ATUAL
    // =========================

    public void SaveCurrentEcoHealth(float hp)
    {
        if (currentEco == null)
            {
                return;
            }

        // Salva o HP no slot do Eco atualmente equipado
        vidaAtualParty[currentEcoIndex] = hp;

        // Atualiza também o HP atual
        vidaAtual = hp;
    }

    private float CalcularVidaMaxima(int index)
    {
        if (ecoParty[index] == null)
            return 0;

        // Ajuste esta fórmula conforme seu sistema
        return (ecoParty[index].Vida + 2 * Mathf.Sqrt(levelCheck.level)) * 10;
    }


    // =========================
    // PEGAR HP DO ECO ATUAL
    // =========================

    public float GetCurrentEcoHealth()
    {
        if (currentEco == null)
        {
            return 0;
        }

        return vidaAtualParty[currentEcoIndex];
    }


    // =========================
    // ENCONTRAR PRÓXIMO ECO VIVO
    // =========================

    public int GetNextAliveEco()
    {
        // Primeiro procura nos slots seguintes
        for (int i = currentEcoIndex + 1; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                return i;
            }
        }

        // Depois procura nos slots anteriores
        for (int i = 0; i < currentEcoIndex; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                return i;
            }
        }
    
        Debug.Log("Todos os Ecos morreram!");
        EndBattle();

        // Nenhum Eco vivo
        return -1;
    }

    /* void ApplyEcoStats(){

        eHealth.maxHP = levelCheck.hp;

        eHealth.currentHP = NPC_Trainer.vidaAtual;

        anim.runtimeAnimatorController = NPC_Trainer.animControllerC;
    } */

    public void HealParty()
{
    for (int i = 0; i < ecoParty.Length; i++)
    {
        if (ecoParty[i] != null)
        {
            vidaAtualParty[i] = CalcularVidaMaxima(i);
        }
    }
}
}
