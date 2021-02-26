using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Rebinding : MonoBehaviour
{
    private InputAction action;
    public InputAction Action
    {
        get { return action; }
        set
        {
            action = value;
            label.text = action.name;
            GetCurrentKey();
        }
    }
    [SerializeField] private Text label;
    [SerializeField] private Text receiver;
    public void GetCurrentKey()
    {
        receiver.text = InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath);
    }
    public void Rebind()
    {
        receiver.text = "-";
        action.PerformInteractiveRebinding()
                .WithControlsExcluding("<Mouse>/position")
                .WithControlsExcluding("<Mouse>/delta")
                .OnMatchWaitForAnother(0.1f).OnComplete(callback => GetCurrentKey());
    }
}
