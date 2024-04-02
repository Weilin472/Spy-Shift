using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject tutorialBtn;

    public void ShowOrHideTutorialUI()
    {
        tutorialUI.SetActive(!tutorialUI.activeSelf);
        tutorialBtn.SetActive(!tutorialBtn.activeSelf);
    }
}
