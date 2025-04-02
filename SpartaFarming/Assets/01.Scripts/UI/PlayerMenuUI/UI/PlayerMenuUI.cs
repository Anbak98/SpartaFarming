using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuUI : MonoBehaviour
{
    [SerializeField] private PlayerMenuAnimationData animationData;
    private Animator animator;
    [SerializeField] private List<GameObject> tabs;
    public int CurrentTab {get; private set;} = 0;
    public int PreviousTab {get; private set;} = 0;

    public bool isTransition {get; private set;} = false ;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        OpenBook();
    }

    public void OpenBook(){
        animator.enabled = true;
        animator.Play(animationData.BookOpenHash);
    }

    public void CloseBook(){
        animator.enabled = true;
        animator.Play(animationData.BookCloseHash);
    }

    public void ChangeTab(int index)
    {
        if(index == CurrentTab) return;

        animator.enabled = true;
        isTransition = true;

        if(index < CurrentTab){
            animator.Play(animationData.PageFlipLeftHash);
        }else{
            animator.Play(animationData.PageFlipRightHash);
        }

        PreviousTab = CurrentTab;

        CurrentTab = index;
    }

    public void OnAnimationTransitionEvent(){
        tabs[PreviousTab].SetActive(false);
        tabs[CurrentTab].SetActive(true);
    }

    public void OnAnimationEndEvent(){
        animator.enabled = false;
        isTransition = false;
    }
}