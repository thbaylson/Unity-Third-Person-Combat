using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDecoratorRule : ScriptableObject
{
    // These show up in the Editor because of the EnumFlagsAttribute and EnumFlagsAttributeDrawer classes.
    [SerializeField, EnumFlags] private RoomType roomType;
    public RoomType RoomTypes => roomType;

    internal abstract bool CanBeApplied(TileType[,] levelDecorated, Room room);
    internal abstract void Apply(TileType[,] levelDecorated, Room room, Transform parent);
}
