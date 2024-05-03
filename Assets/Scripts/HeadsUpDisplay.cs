using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using static Elements;
using Unity.VisualScripting;



namespace HeadsUpDisplay
{
    public class HUD : MonoBehaviour
    /*
    @namespace HUD
    @accessor namespace HUD
        using HUD;
    */
    {
        static float noChange = 1.0f;
        [SerializeField]
        private VisualTreeAsset uxml;
        [SerializeField]
        private StyleSheet uss;
        private readonly VisualElement ROOT;
        HealthMeter health;
        public HUD(int healthTotal)
        {
            this.ROOT = uxml.GetComponent<VisualElement>();
            this.health = new HealthMeter(this.ROOT.Query<VisualElement>("HealthCurrent"), healthTotal);
        }

        // Start is called before the first frame update
        void Start()
        {
            VisualElement meterElement = this.ROOT.Query<VisualElement>("HealthCurrent");
            this.health = new HealthMeter(meterElement);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public class HealthMeter 
        {
            public int healthTotal;
            public readonly VisualElement healthBarCurrent;

            public HealthMeter(VisualElement healthBarCurrent, int healthTotal)
            {
                this.healthBarCurrent = healthBarCurrent;
                this.healthTotal = healthTotal;
            }

            public void Start()
            {                
                healthBarCurrent.transform.scale.Set(HUD.noChange, HUD.noChange, HUD.noChange);
            }

            public void SetHealthTotal(int healthTotal)
            {
                this.healthTotal = healthTotal;
            }

            public void DisplayCurrentHealth(int healthCurrent)
            {
                int factor = 100 * healthCurrent / this.healthTotal;
                float xScale = factor / 100.0f;
                healthBarCurrent.transform.scale.Set(xScale, HUD.noChange, HUD.noChange);
            }

        }

        private class Portrait
        {
            
        }

        public class Inventory
        {
            
        }

        public class Tracker
        {
            
        }

        public class Codex
        {
            
        }

        


    }
}
