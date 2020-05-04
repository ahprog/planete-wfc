﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EntropyTile : IComparable<EntropyTile>
{
    public float entropy;
    public Tile tile;

    public int CompareTo(EntropyTile other)
    {
        if (entropy > other.entropy) return 1;
        if (entropy < other.entropy) return -1;
        else return 0;
    }
}