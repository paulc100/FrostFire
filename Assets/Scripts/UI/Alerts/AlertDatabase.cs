using System.Collections.Generic;
using UnityEngine;

public class AlertDatabase 
{
    // Campfire Alerts
    public static string HEADING_CAMPFIRE_50 = "Campfire is at 50%"; 
    public static string HEADING_CAMPFIRE_25 = "Campfire is at 25%"; 
    public static string HEADING_CAMPFIRE_10 = "Campfire is at 10%"; 

    public static List<string> SUBHEADINGS_CAMPFIRE_ALERTS = new List<string> 
    {
        "Gather some wood!",
        "Did anyone bring marshmallows?",
        "Ouch.",
        "Is everyone doing okay?",
        "It could be worse, right?",
        "Uh, you probably shouldn't let that happen",
    };

    // Wave Alerts
    public static string HEADING_WAVE_INCOMING = "Wave Incoming!"; 
    public static string HEADING_WAVE_COMPLETE = "Wave Complete!"; 

    public static List<string> SUBHEADINGS_WAVE_ALERTS = new List<string> 
    {
        "Get the gang together!",
        "Avengers, assemble!",
        "Why did the snowman cross the road?",
        "Go team!",
        "Will they ever stop coming?",
        "Defend the fire!",
    };

    // Player Alerts
    public static string HEADING_PLAYER_DOWNED = "Player is down!";
    public static string HEADING_PLAYER_REVIVED = "Player revived!";

    public static string GenerateRandomSubheadingFromList(List<string> subheadingList) => subheadingList[Random.Range(0, subheadingList.Count)];
}
