using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDie<T>
{
    public void IDieInit(T temp);
    public void IDie();
}

public interface IAttack<T>
{
    public void IAttackInit(T temp);
    public void IAttack_Enter();
    public void IAttack();
    public void IAttack_Exit();
}

public interface ITraking<T>
{
    public void ITrakingInit(T temp);
    public void ITraking_Enter();
    public void ITraking();
    public void ITraking_Exit();
}

public interface IProwl<T>
{
    public void IProwlInit(T temp);
    public void IProwl_Enter();
    public void IProwl();
    public void IProwl_Exit();
}

public class Interface
{

}
