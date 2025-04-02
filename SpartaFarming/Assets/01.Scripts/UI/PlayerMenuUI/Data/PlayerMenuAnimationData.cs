using System;
using UnityEngine;

[Serializable]
public class PlayerMenuAnimationData
{
    [SerializeField] private string bookOpenParameterName = "BookOpen";
    [SerializeField] private string bookCloseParameterName = "BookClose";
    [SerializeField] private string pageFlipLeftParameterName = "PageFlipLeft";
    [SerializeField] private string pageFlipRightParameterName = "PageFlipRight";

    public int BookOpenHash { get; }
    public int BookCloseHash { get; }
    public int PageFlipLeftHash { get; }
    public int PageFlipRightHash { get; }

    public PlayerMenuAnimationData()
    {
        BookOpenHash = Animator.StringToHash(bookOpenParameterName);
        BookCloseHash = Animator.StringToHash(bookCloseParameterName);
        PageFlipLeftHash = Animator.StringToHash(pageFlipLeftParameterName);
        PageFlipRightHash = Animator.StringToHash(pageFlipRightParameterName);
    }
}
