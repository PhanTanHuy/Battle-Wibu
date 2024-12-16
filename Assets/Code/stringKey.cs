using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class stringKey
{
    public static class States
    {
        public const string Run = "run";
        public const string Walk = "walk";
        public const string Jump = "jump";
    }
    public static class Attack
    {
        public const string AttackJ = "atj";
        public const string AttackU = "atu";
        public const string AttackI = "ati";
        public const string AttackWU = "atwu";
        public const string AttackWI = "atwi";
        public static readonly string[] Skills = { AttackU, AttackWU, AttackI, AttackWI };
    }
}
