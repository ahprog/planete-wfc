using System;
using System.Collections.Generic;
using UnityEngine;

public class EntropyTile : IComparable<EntropyTile>
{
    public float entropy;
    public Tile tile;

    public EntropyTile(Tile tile, float entropy)
    {
        this.tile = tile;
        this.entropy = entropy;
    }

    public int CompareTo(EntropyTile other)
    {
        return entropy.CompareTo(other.entropy);
    }
}
