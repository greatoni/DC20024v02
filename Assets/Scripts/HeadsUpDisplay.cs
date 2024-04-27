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
using static PlayerDamageSequences;
using static SequencerUserStates;
using static Elements;

/*
.top
.healthTotal
.healthCurrent
.portrait
.inventory
.slot
.slot-selected
.slot1
    // background-image: var(--slot1)
.slot2
    // background-image: var(--slot2)
.slot3
    // background-image: var(--slot3)
.slot4
    // background-image: var(--slot4)
.slot5
    // background-image: var(--slot5)
.slot6
    // background-image: var(--slot6)
.bottom
.sequence-input
.sequencers
.sequence
.sequence-neutral
.sequence-fire
.sequence-ice
.sequence-earth
.sequence-air
.interval
.fret
.interval_1
.interval_2
.interval_3
.interval_4
.interval_5
.interval_6
.interval_7
.interval_8
.pip
.hidden
.sequence-commander
.btn
.sequence-run
.sequence-save
Label
#unity-text-input
.runesong-alias
.library
.catalog

*/
namespace HeadsUpDisplay
{
    public class HUD : MonoBehaviour
    /*
    @namespace HUD
    @accessor namespace HUD
        using HUD;
    */
    {
        private readonly QSelector QSelector;
        private readonly PlayerDamageSequences player;
        private readonly HealthMeter health;
        private readonly VisualElement portrait;
        private readonly VisualElement inventory;
        private readonly VisualElement sequencer;
        private readonly VisualElement library;

        readonly Dictionary<string, int> attributes = new();

        public HUD()
        {
            foreach(GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if(obj.name == "Player") player = obj.GetComponent<PlayerDamageSequences>();
            }

            QSelector = new QSelector(GetComponent<UIDocument>());
            health = new HealthMeter(player.healthMax, QSelector.First(".health-current"));
            // portrait = new Portrait(QSelector.First(".portrait"));
            inventory = QSelector.First(".inventory");
            sequencer = QSelector.First(".sequence-input");
            library = QSelector.First(".library");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            health.Update(player.GetCurrentHealth());
        }
        
        public class HealthMeter 
        {
            private readonly int healthTotal;
            private readonly VisualElement healthBarCurrent;

            public HealthMeter(int healthTotal, VisualElement healthBarCurrent)
            {
                this.healthTotal = healthTotal;
                this.healthBarCurrent = healthBarCurrent;
            }

         /* public void Start()
            {

            } */

            public void Update(int healthCurrent)
            {
                int factor = 100 * healthCurrent / this.healthTotal;
                float noChange = 1.0f;
                healthBarCurrent.transform.scale.Set(factor/100.0f, noChange, noChange);
            }

        }

        public class Inventory
        {
            private readonly Item[] items;
            private readonly int selectedIndex;

            public Inventory()
            {
                this.items = new Item[5];
                this.selectedIndex = 0;
            }
        }

        public class Item
        {
            private readonly string alias;
            private readonly string description;
            private readonly string imagePath;

            public Item(string alias, string description, string imagePath)
            {
                this.alias = alias;
                this.description = description;
                this.imagePath = imagePath;
            }

            public string GetAlias()
            {
                return this.alias;
            }

            public string GetDescription()
            {
                return this.description;
            }

            public string GetImagePath()
            {
                return this.imagePath;
            }
        }

        public class InventoryUI : MonoBehaviour
        {


            void Start()
            {

            }

            void Update()
            {

            }
        }
        
        public class Sequence 
    /*
        @class Sequence
        @property {int} Capacity
            The maximum number of values is always 8
        @property {bool[]} sequence
            holds the sequence of boolean values to determine if a pip is present
        @property {Elements} type 
            label to identify the elemental type of the sequence
        @default {Elements} _type
            default value for the type property is Neutral
    */
        {
            public Elements type;
            private readonly Elements _type = Elements.Neutral;
            private readonly bool[] sequence = new bool[8];

            public Sequence(Elements type)
            {
                this.type = type;
            }

            public Sequence(Elements type, bool[] sequence)
            {
                if(sequence.LongLength != 8)
                {
                    throw new Exception("Sequence length must be 8");
                }
                this.type = type;
                this.sequence = sequence;
            }

            public bool[] GetSequence()
            {
                return this.sequence;
            }

            public void ToggleIndex(int index)
            {
                this.sequence[index] = !this.sequence[index];
            }

            public void SetIndex(int index, bool value)
            {
                this.sequence[index] = value;
            }
        }

        public class Runesong
        {
            private readonly string alias = "";
            private Sequence neutral = new(Elements.Neutral);
            private Sequence fire = new(Elements.Fire);
            private Sequence ice = new(Elements.Ice);
            private Sequence earth = new(Elements.Earth);
            private Sequence air = new(Elements.Air);

            public Runesong(string alias)
            {
                this.alias = alias;
            }

            public Runesong(string alias, Sequence neutral, Sequence fire, Sequence ice, Sequence earth, Sequence air)
            {
                this.alias = alias;
                this.neutral = neutral;
                this.fire = fire;
                this.ice = ice;
                this.earth = earth;
                this.air = air;
            }

            public Runesong(Runesong temp){
                this.alias = temp.GetAlias();
                this.neutral = temp.GetSequence(Elements.Neutral);
                this.fire = temp.GetSequence(Elements.Fire);
                this.ice = temp.GetSequence(Elements.Ice);
                this.earth = temp.GetSequence(Elements.Earth);
                this.air = temp.GetSequence(Elements.Air);
            }

            public string GetAlias()
            {
                return this.alias;
            }

            public Sequence GetSequence(Elements type)
            {
                if (type == Elements.Neutral) return neutral;
                else if (type == Elements.Fire) return fire;
                else if (type == Elements.Ice) return ice;
                else if (type == Elements.Earth) return earth;
                else if (type == Elements.Air) return air;
                else throw new Exception("Invalid sequence type");
            }

            public void SetSequence(Elements type, Sequence sequence)
            {
                if (type == Elements.Neutral) this.neutral = sequence;
                else if (type == Elements.Fire) this.fire = sequence;
                else if (type == Elements.Ice) this.ice = sequence;
                else if (type == Elements.Earth) this.earth = sequence;
                else if (type == Elements.Air) this.air = sequence;
                else throw new Exception("Invalid sequence type");
            }
        
            public void placePip(Elements type, int index)
            {
                Sequence sequence = this.GetSequence(type);
                sequence.SetIndex(index, true);
            }

            public void removePip(Elements type, int index)
            {
                Sequence sequence = this.GetSequence(type);
                sequence.SetIndex(index, false);
            }
        }

        public class Sequencer : MonoBehaviour
        {

            /*** The Runesong the corresponds to the sequencer on the HUD ***/
            Runesong runeSongInProgress;

            /*** The listener for the clicks on each interval ***/
            ClickEvent m_Click;

            void Start()
            {
                runeSongInProgress = new("Runesong");

                if(m_Click == null){
                    m_Click = new();
                }

            }

            void Update()
            {
                if(Input.GetMouseButtonDown(0))
                {

                }
            }
        }

        public class RunesongLibrary : MonoBehaviour
        {
            private readonly Dictionary<string, Runesong> library;
            private readonly int selectedIndex;

            public RunesongLibrary()
            {
                this.library = new Dictionary<string, Runesong>();
            }

            public RunesongLibrary(Runesong runesong)
            {
                this.library = new Dictionary<string, Runesong>();
                this.library.Add(runesong.GetAlias(), runesong);
            }

            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }


        }

        public class EnemyTarget : MonoBehaviour
        {
            
        }


    }
}
