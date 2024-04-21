using System.Collections.Generic;
using UnityEngine.UIElements;



namespace QuerySelector
{
    public class QSelector
    {
        // UI Document caller
        private readonly UIDocument ui;
        // Root Visual Element
        private readonly VisualElement root;
        // Query Builder
        private readonly UQueryBuilder<VisualElement> builder;
        private readonly List<VisualElement> elements;

        public QSelector(UIDocument UIDoc)
        /*
        @constructor QSelector
        @param {UIDocument} ui
            Pass GameObject(implicitly) GetComponent<UIDocument>() in UI component
        */
        {
            // Constructor
            ui = UIDoc;
            root = ui.rootVisualElement;
            builder = new UQueryBuilder<VisualElement>(root);
        }
        
        public List<VisualElement> List(string str)
        {
            // Exception Block
            if(str == null)
            {
                throw new QuerySelectorException($"NullInputException: in QSelector(string), string is null");
            }
            else if (str.Length == 0)
            {
                throw new QuerySelectorException($"EmptyInputException: in QSelector(string), string is empty");
            } else if (str.StartsWith('@') && str.Length > 1)
            {
                throw new QuerySelectorException($"InvalidInputException: in QSelector(string), string starts with '@' to select active elements but is not '@'");
            }

            List<VisualElement> elements;

            if( str.StartsWith('.'))
            {
                // by class
                str = str.Substring(1);
                elements = builder.Class(str).ToList();
            }
            else if(str.StartsWith('#'))
            {
                // by name (roughly equiv to ID)
                str = str.Substring(1);
                elements = builder.Name(str).ToList();
            }
            else if(str.StartsWith('@') && str.Length == 1)
            {
                // focused
                elements = builder.Focused().ToList();
            }
            else
            {
                // by type (roughly equiv to tag)
                elements = builder.Name(str).ToList();
            }
            
            return elements;
        }

        public VisualElement First(string str)
        {
            return List(str)[0];
        }

        public VisualElement Last(string str)
        {
            List<VisualElement> elements = List(str);
            return elements[elements.Count - 1];
        }
    }

    [System.Serializable]
    public class QuerySelectorException : System.Exception
    {
        public QuerySelectorException() { }
        public QuerySelectorException(string message) : base(message) { }
        public QuerySelectorException(string message, System.Exception inner) : base(message, inner) { }
        protected QuerySelectorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
