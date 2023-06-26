using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using PluginTest;
using UnityEditor;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace PluginTest
{
    
    
    public class InObject : MonoBehaviour
    {
        [Serializable]
        public class FilterItem 
        {
            // public List<string> scriptsPossibilities = new();
            [SerializeField]
            public string script;
            [SerializeField]
            public List<PropertyContainer> properties = new();
            //public List<string> properties = new();
            
            
            [Serializable]
            public class PropertyContainer
            {
                [SerializeField]
                public string value;
            }
        }
        

        [SerializeField]
        public List<FilterItem> filterItems = new();

        private List<Input> listaInputs = new();
        
        public List<string> PossibilidadesScripts
        {
            get {
                var components = gameObject.GetComponents<Component>();
                var lista = new List<string>();
                foreach (var co in components)
                {
                    lista.Add(co.GetType().Name);
                }
                return lista;
            }
        }
        
        
        private void AtulizaListaInput()
        {
            listaInputs.Clear();
            var teste  = gameObject.GetComponents<Component>();
            foreach (var component in teste)
            {
                var type = component.GetType();
                var campos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                
                var input = new Input
                {
                    Nome = component.name + "----" + component.GetType(),
                    Conteudo = new Dictionary<FieldInfo, object>()
                };
                //bool util = false;
                foreach (var fieldInfo in campos)
                {
                    //if()
                    //Debug.Log(fieldInfo);
                    input.Conteudo.Add(fieldInfo, fieldInfo.GetValue(component));
                }
                listaInputs.Add(input);
                //Debug.Log(input);
            }
        }

        private void AtulizaListaInput2()
        {
            listaInputs.Clear();
            foreach (var item in filterItems)
            {
                var teste = gameObject.GetComponent(item.script);
                
                if (teste == null)
                {
                    Debug.Log("O component " + item.script + " nao existe no objeto!");
                    return;
                }

                var type = teste.GetType();
                //var campo = type.GetField();
                var input = new Input
                {
                    Nome = teste.name + "----" + teste.GetType(),
                    Conteudo = new Dictionary<FieldInfo, object>()
                };
                foreach (var propriedade in item.properties)
                {
                    var campo = type.GetField(propriedade.value,BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (campo == null)
                        continue;
                    input.Conteudo.Add(campo, campo.GetValue(teste));
                }
                listaInputs.Add(input);
            }
        }

        private void Start()
        {
            //AtulizaListaInput();
            List<string> listaFilterItens = new();
            foreach (var filterItem in filterItems)
            {
                foreach (var property in filterItem.properties)
                {
                    listaFilterItens.Add(property.value);
                }
            }
            AtulizaListaInput2();
            Global.Init(listaFilterItens);
            var tempo = DateTime.Now;
            foreach (var item in Enumerable.Range(0,listaInputs.Count))
            {
                var input = listaInputs[item];
                input.Tempo = tempo;
                listaInputs[item] = input;
                Global.AddInput(listaInputs[item]);
            }
            StartCoroutine(Global.UpdateNovo());

        }
        [Range(0.1f,2.0f)]
        public float t = 1f;
        private float _contador;
        [SerializeField] private List<string> _possibilidadesScripts;

        private void Update()
        {
            _contador += Time.deltaTime;
            if (_contador > t)
            {
                _contador = 0;
                //AtulizaListaInput();
                AtulizaListaInput2();
                foreach (var item in Enumerable.Range(0,listaInputs.Count))
                {
                    var input = listaInputs[item];
                    input.Tempo = DateTime.Now;
                    listaInputs[item] = input;
                    Global.AddInput(listaInputs[item]);
                    
                }
                
            }
        }

        private static bool oneTime = true;
        private void OnApplicationQuit()
        {
            Global.PrintInArchive().Wait();
            if (oneTime)
            {
                oneTime = false;
                Global.End();
                
            }
        }
    }    
}

