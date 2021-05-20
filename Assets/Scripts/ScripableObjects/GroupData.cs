using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines the data for a given level group
[CreateAssetMenu]
public class GroupData : ScriptableObject
{
    public Sprite panelSprite;
    public Color textColor;
    public GroupData next;

}
