using System;
using UnityEngine;

public interface IStatus
{
    public Sprite Icon { get; }
    public bool IsView { get; }
    public Action OnDelete { get; set; }
    public Action<float> OnTick { get; set; }
    public void Work();
}
