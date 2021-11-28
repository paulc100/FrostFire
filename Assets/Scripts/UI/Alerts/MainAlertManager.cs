using UnityEngine;
using UnityEngine.UI;

public class MainAlertManager : MonoBehaviour
{
    [SerializeField]
    private Text headingText = null;
    [SerializeField]
    private Text subheadingText = null;

    private Animator animator = null;

    private void Awake() => animator = gameObject.GetComponent<Animator>();

    public void DisplayCampfireAlert_50()
    {
        animator.Play("MainAlert", -1, 0f);

        headingText.text = AlertDatabase.HEADING_CAMPFIRE_50;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_CAMPFIRE_ALERTS);

        // FindObjectOfType<AudioManager>().Play("Alert");
        animator.Play("MainAlert");
    }

    public void DisplayCampfireAlert_25()
    {
        animator.Play("MainAlert", -1, 0f);

        headingText.text = AlertDatabase.HEADING_CAMPFIRE_25;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_CAMPFIRE_ALERTS);

        // FindObjectOfType<AudioManager>().Play("Alert");
        animator.Play("MainAlert");
    }

    public void DisplayCampfireAlert_10()
    {
        animator.Play("MainAlert", -1, 0f);

        headingText.text = AlertDatabase.HEADING_CAMPFIRE_10;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_CAMPFIRE_ALERTS);

        // FindObjectOfType<AudioManager>().Play("Alert");
        animator.Play("MainAlert");
    }
}
