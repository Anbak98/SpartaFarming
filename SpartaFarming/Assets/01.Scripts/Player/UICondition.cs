using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;    

    void Start()
    {
        GameManager.Instance.Player.Condition.uiCondition = this;
    }
}