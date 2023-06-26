using System;
using System.Collections.Generic;
using System.Drawing;
//using Unity.Mathematics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor.Timeline.Actions;

namespace PluginTest.Editor
{
    [CustomEditor(typeof(InObject))]
    public class InObject_Inspector : UnityEditor.Editor
    {
        private VisualElement root { get; set; }

        private SerializedProperty propertyPerido { get; set; }
        private SerializedProperty propertyFilterItems { get; set; }

        private PropertyField fieldPeriodo { get; set; }
        private PropertyField fieldFilterItems { get; set; }


        private SerializedProperty _listaFilter;
        public VisualTreeAsset Root;
        public VisualTreeAsset Item;

        private List<string> components = new ();

        private InObject obj;
        //private VisualElement root;
        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            InitializeEditor();
            Composer();
            return root;
            obj = serializedObject.targetObject as InObject;
            var comp = obj.gameObject.GetComponents<Component>();
            components.Clear();
            foreach (var c in comp)
            {
                components.Add(c.GetType().ToString());
            }
            root = new VisualElement();
            Root.CloneTree(root);
            root.Q("botoes").Q("menos").RegisterCallback<ClickEvent>(RemoveItem);
            root.Q("botoes").Q("mais").RegisterCallback<ClickEvent>(AddItem);
            return root;
        }

        private List<VisualElement> componentsList = new();

        void AddItem(ClickEvent e)
        {
            var test = new VisualElement();
            Item.CloneTree(test);
            componentsList.Add(test);
            test.Q<DropdownField>().choices = components;
            root.Q("container").Add(test);
            var filterItem = new InObject.FilterItem();
            //test.Bind(serializedObject.FindProperty());
            //filterItem.script
            //obj.filterItems.Add();
            //Debug.Log("mais");
        }
        void RemoveItem(ClickEvent e)
        {
            componentsList.Remove(root.Q("container").ElementAt(root.Q("container").Children().Count() - 1));
            root.Q("container").RemoveAt(root.Q("container").Children().Count() - 1);
            //Debug.Log("menos");
        }
        
        
        
        
        

        private void FindProperties()
        {
            propertyPerido = serializedObject.FindProperty(nameof(InObject.t));
            propertyFilterItems = serializedObject.FindProperty(nameof(InObject.filterItems));
        }

        private void InitializeEditor()
        {
            root = new VisualElement();

            fieldPeriodo = new PropertyField(propertyPerido, "Periodo");
            //periodoField.BindProperty(propertiePerido);

            fieldFilterItems = new PropertyField(propertyFilterItems);
            //filterItemsField.BindProperty(propertieFilterItems);
        }

        private void Composer()
        {
            root.Add(fieldPeriodo);
            root.Add(fieldFilterItems);
        }
    }


    [CustomPropertyDrawer(typeof(InObject.FilterItem))]
    public class FilterItem_PD : PropertyDrawer
    {
        private VisualElement root { get; set; }


        private SerializedProperty propertyScript { get; set; }
        private SerializedProperty propertyProperties { get; set; }

        private PopupField<string> fieldScript { get; set; }
        //private ListView fieldProperties { get; set; }
        private PropertyField fieldProperties { get; set; }


        private List<string> possibleScripts = new();


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SetupVariables(property);
            FindProperties(property);
            InitializeEditor();
            Composer();
            return root;
        }

        private void SetupVariables(SerializedProperty property)
        {
            var obj = property.serializedObject.targetObject as InObject;

            var listObjs = obj.gameObject.GetComponents<Component>();

            foreach (var component in listObjs)
            {
                if (component.GetType() == typeof(InObject))
                    continue;
                possibleScripts.Add(component.GetType().ToString());
            }
        }

        private void FindProperties(SerializedProperty property)
        {
            propertyScript = property.FindPropertyRelative(nameof(InObject.FilterItem.script));
            propertyProperties = property.FindPropertyRelative(nameof(InObject.FilterItem.properties));
        }

        private void InitializeEditor()
        {
            root = new VisualElement();

            fieldScript = new PopupField<string>(possibleScripts, 0);
            fieldScript.BindProperty(propertyScript);

            //fieldProperties = new ListView(propertyProperties.,10);
            fieldProperties = new PropertyField(propertyProperties);
        }

        private void Composer()
        {
            root.Add(fieldScript);
            root.Add(fieldProperties);
            //Debug.Log("AAAAAAAAAAAAAQUI"+fieldScript.name);
        }
    }

    [CustomPropertyDrawer(typeof(InObject.FilterItem.PropertyContainer))]
    public class PropertyContainer : PropertyDrawer
    {
        private VisualElement root { get; set; }

        public SerializedProperty propertyValue { get; set; }

        //public PopupField<string> fieldValue { get; set; }
        public TextField fieldValue { get; set; }

        private List<string> possibilidades = new();
        private int id = -1;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SetupVariables(property);
            FindProperties(property);
            InitializeEditor();
            Composer();
            return root;
        }

        private int idParent = -1;
        private SerializedProperty _property;
        private void SetupVariables(SerializedProperty property)
        {
            _property = property;
            AtualizaPossibilidades();
        }

        void AtualizaPossibilidades()
        {
            var name = _property.propertyPath;
            var number = Regex.Matches(name, "data\\[([0-9]*)\\]")[1].Groups[1].Value;
            id = int.Parse(number);

            var parentName = _property.propertyPath;
            var parentID = Regex.Match(parentName, "data\\[([0-9]*)\\]").Groups[1].Value;
            idParent = int.Parse(parentID);
            //Debug.Log("ID: " + id);

            var inObject = _property.serializedObject.targetObject as InObject;
            var fields = inObject.gameObject.GetComponent(inObject.filterItems[idParent].script).GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            possibilidades.Clear();
            foreach (var item in fields)
            {
                possibilidades.Add(item.Name);
            }

            //return possibilidades;
        }

        private void FindProperties(SerializedProperty property)
        {
            propertyValue = property.FindPropertyRelative(nameof(InObject.FilterItem.PropertyContainer.value));
        }

        private void InitializeEditor()
        {
            root = new VisualElement();
            fieldValue = new TextField();
            fieldValue.BindProperty(propertyValue);
            
        }

        private void Composer()
        {
            root.Add(fieldValue);
        }
    }
}