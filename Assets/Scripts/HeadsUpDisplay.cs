using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using QuerySelector;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using static Elements;



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
        public UIDocument HUD_UI;
        HealthMeter health;
        public HUD()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            this.health = new HealthMeter();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public class HealthMeter 
        {
            private int healthTotal;
            public readonly VisualElement healthBarCurrent;

            public HealthMeter()
            {
                
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
