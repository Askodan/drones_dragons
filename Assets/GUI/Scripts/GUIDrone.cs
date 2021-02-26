using UnityEngine;
using System.Collections;

public class GUIDrone : MonoBehaviour
{
    //[HideInInspector] 
    public AltitudeMeter altmeter;
    public DigitsDisplayer altDisplayer;
    public LightsPanel lightPanel;
    public RotateInfo rotateInfo;
    public bool altitude_or_height;
    void OnGUI()
    {
        if (altitude_or_height)
        {
            altDisplayer.value = altmeter.altitude;
        }
        else
        {
            altDisplayer.value = altmeter.height;
        }
    }

}
