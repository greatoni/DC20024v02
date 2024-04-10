using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using QuerySelector;
using UnityEditor.UIElements;



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
        public class PlayerCharacter
        /*
        @class PlayerCharacter
            stub class for the player character
        */
        {
            public int HPMax = 512;
            public int runeMaxCapacity = 64;
            private int HP = 0;

            public int GetHP()
            {
                return this.HP;
            }

            public void Start()
            {
                this.HP = HPMax;
            }

            public void Update()
            {
                this.DecrementHP(1);
            }

            public void DecrementHP(int damage)
            {
                this.HP -= damage;
            }
        }

        private readonly QSelector QSelector;
        private readonly HealthMeter health;
        private readonly VisualElement inventory;
        private readonly VisualElement sequencer;
        private readonly VisualElement library;
        private readonly PlayerCharacter player = new();

        readonly Dictionary<string, int> attributes = new();

        public HUD()
        {
            QSelector = new QSelector(GetComponent<UIDocument>());
            health = new HealthMeter(player.HPMax, QSelector.First(".health-total"), QSelector.First(".health-current"));
            inventory = QSelector.First(".inventory");
            sequencer = QSelector.First(".sequence-input");
            library = QSelector.First(".library");

            // player = GetComponent<PlayerCharacter>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public class HealthMeter 
        {
            private readonly VisualElement healthBar;
            private readonly VisualElement healthBarCurrent;

            public HealthMeter(int healthTotal, VisualElement healthBarCurrent, VisualElement healthBar)
            {
                this.healthBar = healthBar;
                this.healthBarCurrent = healthBarCurrent;
            }

            void Update(int healthCurrent, int healthTotal)
            {
                int factor = 100 * healthCurrent / healthTotal / 100;
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
        
        public enum SequenceType 
        /*
        @enum SequenceType
            used to keep track of the elemental type of a sequence
        */
        {
            Neutral,
            Fire,
            Ice,
            Earth,
            Air
        }

        public class Sequence 
    /*
        @class Sequence
        @property {int} Capacity
            The maximum number of values is always 8
        @property {bool[]} sequence
            holds the sequence of boolean values to determine if a pip is present
        @property {SequenceType} type 
            label to identify the elemental type of the sequence
        @default {SequenceType} _type
            default value for the type property is Neutral
    */
        {
            public SequenceType type;
            private readonly SequenceType _type = SequenceType.Neutral;
            private readonly bool[] sequence = new bool[8];

            public Sequence(SequenceType type)
            {
                this.type = type;
            }

            public Sequence(SequenceType type, bool[] sequence)
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
            private Sequence neutral = new(SequenceType.Neutral);
            private Sequence fire = new(SequenceType.Fire);
            private Sequence ice = new(SequenceType.Ice);
            private Sequence earth = new(SequenceType.Earth);
            private Sequence air = new(SequenceType.Air);

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
                this.neutral = temp.GetSequence(SequenceType.Neutral);
                this.fire = temp.GetSequence(SequenceType.Fire);
                this.ice = temp.GetSequence(SequenceType.Ice);
                this.earth = temp.GetSequence(SequenceType.Earth);
                this.air = temp.GetSequence(SequenceType.Air);
            }

            public string GetAlias()
            {
                return this.alias;
            }

            public Sequence GetSequence(SequenceType type)
            {
                if (type == SequenceType.Neutral) return neutral;
                else if (type == SequenceType.Fire) return fire;
                else if (type == SequenceType.Ice) return ice;
                else if (type == SequenceType.Earth) return earth;
                else if (type == SequenceType.Air) return air;
                else throw new Exception("Invalid sequence type");
            }

            public void SetSequence(SequenceType type, Sequence sequence)
            {
                if (type == SequenceType.Neutral) this.neutral = sequence;
                else if (type == SequenceType.Fire) this.fire = sequence;
                else if (type == SequenceType.Ice) this.ice = sequence;
                else if (type == SequenceType.Earth) this.earth = sequence;
                else if (type == SequenceType.Air) this.air = sequence;
                else throw new Exception("Invalid sequence type");
            }
        
            public void placePip(SequenceType type, int index)
            {
                Sequence sequence = this.GetSequence(type);
                sequence.SetIndex(index, true);
            }

            public void removePip(SequenceType type, int index)
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

        public enum StatusEffects
        {
            burn,
            frostbite,
            galvanization,
            frozen
        }        

        public class Enemy : MonoBehaviour
        {
            
        }


    }
}
