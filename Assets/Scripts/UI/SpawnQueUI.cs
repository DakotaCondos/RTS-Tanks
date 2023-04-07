using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnQueUI : MonoBehaviour
{
    [SerializeField] TextBlock remainingUnits;
    [SerializeField] UIBlock2D timer;
    private float fillAmmount = 0;

    public float FillAmmount { get => fillAmmount; }

    public void SetRemainingUnits(int value)
    {
        remainingUnits.Text = value.ToString();
    }

    public void SetTimerValue(float value)
    {
        timer.RadialFill.FillAngle = (value * -360);
        fillAmmount = Mathf.Abs(timer.RadialFill.FillAngle / 360);
    }




}
