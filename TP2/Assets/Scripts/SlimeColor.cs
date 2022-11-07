using System;
using System.Collections.Generic;
using UnityEngine;

public static class SlimeOrbsGenerator
{
    public static Dictionary<SlimeColor, SlimeOrb> GenerateOrbs()
    {
        return new Dictionary<SlimeColor, SlimeOrb>()
        {
            {
                SlimeColor.Green,
                new SlimeOrb(SlimeColor.Green, "Typical Green", Color.green, 0,
                    (Material) Resources.Load("Materials/green"),
                    (PhysicMaterial) Resources.Load("PhysicMaterials/SlimeMaterials/DefaultGreen"))
            },
            {
                SlimeColor.Yellow,
                new SlimeOrb(SlimeColor.Yellow, "Sticky Yellow", Color.yellow, 0,
                    (Material) Resources.Load("Materials/yellow"),
                    (PhysicMaterial) Resources.Load("PhysicMaterials/SlimeMaterials/StickyYellow"))
            },
            {
                SlimeColor.Orange,
                new SlimeOrb(SlimeColor.Orange, "Piercing Orange", new Color(1.0f, 0.64f, 0.0f), 0,
                    (Material) Resources.Load("Materials/orange"),
                    (PhysicMaterial) Resources.Load("PhysicMaterials/SlimeMaterials/DefaultGreen"))
            },
            {
                SlimeColor.Blue,
                new SlimeOrb(SlimeColor.Blue, "Piercing Blue", Color.cyan, 0,
                    (Material) Resources.Load("Materials/blue"),
                    (PhysicMaterial) Resources.Load("PhysicMaterials/SlimeMaterials/DefaultGreen"))
            },
            {
                SlimeColor.Pink,
                new SlimeOrb(SlimeColor.Pink, "Bouncy Pink", Color.magenta, 0,
                    (Material) Resources.Load("Materials/pink"),
                    (PhysicMaterial) Resources.Load("PhysicMaterials/SlimeMaterials/BouncyPink"))
            },
        };
    }
}

public class SlimeOrb
{
    public SlimeColor slimeColor;
    public string Name;
    public Color Color;
    public uint Amount;
    public Material Material;
    public PhysicMaterial PhysicMaterial;

    public SlimeOrb(SlimeColor slimeColor, string name, Color color, uint amount, Material material, PhysicMaterial physicMaterial)
    {
        this.slimeColor = slimeColor;
        Name = name;
        Color = color;
        Amount = amount;
        Material = material;
        PhysicMaterial = physicMaterial;
    }
}

public enum SlimeColor
{
    Green,
    Yellow,
    Orange,
    Blue,
    Pink,
    None
}


