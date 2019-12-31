using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class DamageDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0}", baseStats.GetStat(Stat.Damage));
        }
    }
}