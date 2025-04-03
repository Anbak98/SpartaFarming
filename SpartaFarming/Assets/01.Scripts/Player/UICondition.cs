using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    public Condition farmingProficiency;
    public Condition fishingProficiency;

    void Start()
    {
        GameManager.Instance.Player.Condition.uiCondition = this;
    }
}