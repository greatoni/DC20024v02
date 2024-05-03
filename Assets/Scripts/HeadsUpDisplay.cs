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
        public HUD()
        {
            this.ROOT = uxml.GetComponent<VisualElement>();
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
            private int healthTotal;
            public readonly VisualElement healthBarCurrent;

            public HealthMeter(VisualElement healthBarCurrent)
            {
                this.healthBarCurrent = healthBarCurrent;
            }

            public void Start(int healthTotal)
            {
                this.healthTotal = healthTotal;
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
