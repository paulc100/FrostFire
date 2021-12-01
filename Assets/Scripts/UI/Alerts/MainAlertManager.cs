using UnityEngine;
using UnityEngine.UI;

public class MainAlertManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text headingText = null;
    [SerializeField]
    private Text subheadingText = null;

    private Animator animator = null;

    private void Awake() => animator = gameObject.GetComponent<Animator>();

    private void Reset() => animator.Play("MainAlert", -1, 0f);

    private void Play() => animator.Play("MainAlert");

    public void DisplayCampfireAlert_50()
    {
        Reset();

        headingText.text = AlertDatabase.HEADING_CAMPFIRE_50;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_CAMPFIRE_ALERTS);

        FindObjectOfType<AudioManager>().Play("Alert");

        Play();
    }

    public void DisplayCampfireAlert_25()
    {
        Reset();

        headingText.text = AlertDatabase.HEADING_CAMPFIRE_25;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_CAMPFIRE_ALERTS);

        FindObjectOfType<AudioManager>().Play("Alert");

        Play();
    }

    public void DisplayCampfireAlert_10()
    {
        Reset();

        headingText.text = AlertDatabase.HEADING_CAMPFIRE_10;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_CAMPFIRE_ALERTS);

        FindObjectOfType<AudioManager>().Play("Alert");

        Play();
    }

    public void DisplayWaveAlert_Incoming()
    {
        Reset();

        headingText.text = AlertDatabase.HEADING_WAVE_INCOMING;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_WAVE_ALERTS);

        FindObjectOfType<AudioManager>().Play("WaveIncoming");

        Play();
    }

    public void DisplayWaveAlert_Complete()
    {
        Reset();

        headingText.text = AlertDatabase.HEADING_WAVE_COMPLETE;
        subheadingText.text = AlertDatabase.GenerateRandomSubheadingFromList(AlertDatabase.SUBHEADINGS_WAVE_ALERTS);

        FindObjectOfType<AudioManager>().Play("WaveComplete");

        Play();
    }
}
