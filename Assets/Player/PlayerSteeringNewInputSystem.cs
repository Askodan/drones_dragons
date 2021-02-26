using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerSteeringNewInputSystem : PlayerSteering
{
    PlayerInput playerInput;

    float thrust;
    public void OnThrust(InputValue value)
    {
        thrust = value.Get<float>();
    }

    float pitch;
    public void OnPitch(InputValue value)
    {
        pitch = value.Get<float>();
    }

    float roll;
    public void OnRoll(InputValue value)
    {
        roll = value.Get<float>();
    }

    float yaw;
    public void OnYaw(InputValue value)
    {
        yaw = value.Get<float>();
    }
    float prototypeturbo;
    public void OnPrototypeturbo(InputValue value)
    {
        prototypeturbo = value.Get<float>();
    }

    bool motors;
    public void OnMotors(InputValue value)
    {
        motors = true;
    }
    bool stabilize;
    public void OnStabilize(InputValue value)
    {
        stabilize = true;
    }

    bool keepaltitude;
    public void OnKeepaltitude(InputValue value)
    {
        keepaltitude = true;
    }

    bool selfleveling;
    public void OnSelfleveling(InputValue value)
    {
        selfleveling = true;
    }

    bool lights;
    public void OnLights(InputValue value)
    {
        lights = true;
    }

    float vertical;
    public void OnVertical(InputValue value)
    {
        vertical = value.Get<float>();
    }

    float horizontal;
    public void OnHorizontal(InputValue value)
    {
        horizontal = value.Get<float>();
    }

    float trolley;
    public void OnTrolley(InputValue value)
    {
        trolley = value.Get<float>();
    }

    bool geardown;
    public void OnGeardown(InputValue value)
    {
        geardown = true;
    }

    bool gearup;
    public void OnGearup(InputValue value)
    {
        gearup = true;
    }

    float widthwheels;
    public void OnWidthwheels(InputValue value)
    {
        widthwheels = value.Get<float>();
    }

    float heightwheels;
    public void OnHeightwheels(InputValue value)
    {
        heightwheels = value.Get<float>();
    }

    float camera_x;
    public void OnCameraX(InputValue value)
    {
        camera_x = value.Get<float>();
    }

    float camera_y;
    public void OnCameraY(InputValue value)
    {
        camera_y = value.Get<float>();
    }

    float cameradistance;
    public void OnCameradistance(InputValue value)
    {
        cameradistance = value.Get<float>();
    }

    bool turninginplace;
    public void OnTurninginplace(InputValue value)
    {
        turninginplace = true;
    }

    bool previousitem;
    public void OnPreviousitem(InputValue value)
    {
        previousitem = true;
    }

    bool nextitem;
    public void OnNextitem(InputValue value)
    {
        nextitem = true;
    }

    bool handsup;
    public void OnArmup(InputValue value)
    {
        handsup = true;
    }

    bool changevehicle;
    bool fire;
    bool break_down;
    bool break_up;
    bool itemoutsideup;
    bool itemoutside;
    bool iteminsideup;
    bool iteminside;
    bool aim;

    InputActionMap droneActionMap;
    InputActionMap robotActionMap;
    InputActionMap tractorActionMap;
    override protected void SteeringStart()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.FindActionMap("Universal").Enable();
        droneActionMap = playerInput.actions.FindActionMap("Drone");
        robotActionMap = playerInput.actions.FindActionMap("Robot");
        tractorActionMap = playerInput.actions.FindActionMap("Tractor");

        var ac = playerInput.actions.FindAction("Change vehicle");
        ac.started += ctx => changevehicle = true;

        ac = playerInput.actions.FindAction("Fire");
        ac.started += ctx => fire = true;
        ac.canceled += ctx => fire = false;

        ac = playerInput.actions.FindAction("Break");
        ac.started += ctx => break_down = true;
        ac.canceled += ctx => break_up = true;

        ac = playerInput.actions.FindAction("Arm front");
        ac.started += ctx => itemoutside = true;
        ac.canceled += ctx => { itemoutsideup = true; itemoutside = false; };

        ac = playerInput.actions.FindAction("Arm carrier");
        ac.started += ctx => iteminside = true;
        ac.canceled += ctx => { iteminsideup = true; iteminside = false; };

        ac = playerInput.actions.FindAction("Aim");
        ac.started += ctx => aim = true;
        ac.canceled += ctx => aim = false;
    }
    override protected void SteeringUpdate()
    {

        if (changevehicle)
        {
            changevehicle = false;
            if (specs.actualVehicle != specs.mainVehicle)
            {
                specs.OutofVehicle();
                specs.IntoVehicle(specs.mainVehicle);
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, mask))
                {
                    specs.OutofVehicle();
                    specs.IntoVehicle(hit.collider.gameObject.transform.parent.GetComponent<VehicleTypeDefiner>());

                }
                else
                {
                    if (specs.inTrigger.Count > 0)
                    {
                        specs.OutofVehicle();
                        specs.IntoVehicle(specs.inTrigger[0]);
                    }
                }
            }

            SetSteering(specs.actualVehicle.vehicleType);
        }

        if (specs.actualVehicle != null)
        {
            switch (specs.actualVehicle.vehicleType)
            {
                case VehicleType.Drone:
                    steeringDrone.Steer(thrust, pitch, roll, yaw, prototypeturbo,
                        lights, motors, stabilize, keepaltitude, selfleveling);
                    motors = false;
                    stabilize = false;
                    keepaltitude = false;
                    selfleveling = false;
                    lights = false;
                    break;
                case VehicleType.Tractor:
                    tractorScriptEasy.Steer(break_down, break_up, vertical, horizontal,
                    geardown, gearup);
                    trailerTrolley.Steer(trolley);
                    break_down = false;
                    break_up = false;
                    geardown = false;
                    gearup = false;
                    break;
                case VehicleType.Robot:
                    robotWheelsSteering.Steer(vertical, horizontal, widthwheels,
                        heightwheels, turninginplace);
                    robotGrabber.Steer(itemoutsideup, iteminsideup, itemoutside,
                        iteminside, nextitem, previousitem, handsup);
                    turninginplace = false;
                    nextitem = false;
                    previousitem = false;
                    handsup = false;
                    iteminsideup = false;
                    itemoutsideup = false;
                    break;
            }
        }
        if (specs.weaponManager)
        {
            foreach (Shooter weapon in specs.weaponManager.weapons_Shooter)
            {
                weapon.shoot = fire;
            }
        }
    }
    override protected void SteeringLateUpdate()
    {
        if (specs.actualVehicle != null)
        {
            switch (specs.actualVehicle.vehicleType)
            {
                case VehicleType.Drone:
                    break;
                case VehicleType.Tractor:
                    simpleCamera.Steer(aim, camera_x, camera_y, cameradistance);
                    break;
                case VehicleType.Robot:
                    smoothHelper.Steer(camera_x, camera_y);
                    break;
            }
        }
    }
    void SetSteering(VehicleType type)
    {
        switch (type)
        {
            case VehicleType.Drone:
                droneActionMap.Enable();
                robotActionMap.Disable();
                tractorActionMap.Disable();
                break;
            case VehicleType.Tractor:
                droneActionMap.Disable();
                robotActionMap.Disable();
                tractorActionMap.Enable();
                break;
            case VehicleType.Robot:
                droneActionMap.Disable();
                robotActionMap.Enable();
                tractorActionMap.Disable();
                break;
        }
    }
}
