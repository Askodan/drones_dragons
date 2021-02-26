using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerRebinder : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput PlayerInput
    {
        get { return playerInput; }
        set
        {
            playerInput = value;
            int i = 0;
            int j = 0;
            foreach (var action_map in playerInput.actions.actionMaps)
            {
                var map_label = Instantiate(map_label_prefab, transform.position + new Vector3(i * 200, 0, 0), Quaternion.identity, transform);
                map_label.text = action_map.name;
                map_label.name = action_map.name;
                j = 1;
                foreach (var action in action_map.actions)
                {
                    var rebinder = Instantiate(rebinding_prefab, map_label.transform.position + new Vector3(0, -j * 30, 0), Quaternion.identity, map_label.transform);
                    rebinder.Action = action;
                    rebinder.name = action.name;
                    j++;
                }
                i++;
            }
        }
    }
    [SerializeField] Rebinding rebinding_prefab;
    [SerializeField] Text map_label_prefab;

}
