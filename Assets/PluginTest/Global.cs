using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;


namespace PluginTest
{
    internal class LineCsv
    {
        public readonly Dictionary<string, string> Termos = new();

        public LineCsv()
        {
            // Termos["timestamp"] = "";
            // Termos["_life"] = "";
            // Termos["_isDead"] = "";
            // Termos["money"] = "";
            // Termos["isOnSpecial"] = "";
            // Termos["totalbulletSync"] = "";
            // Termos["spawnedEnemies"] = "";
            // Termos["waveCounter"] = "";
            
            //var lista = string.Join(";", Global.itens);
            Termos["timestamp"] = "";
            foreach (var item in Global.itens)
            {
                Termos[item] = "";
            }

        }

        public void Clear()
        {
            Termos.Clear();
            Termos["timestamp"] = "";
            foreach (var item in Global.itens)
            {
                Termos[item] = "";
            }
            // var chaves = Termos.Keys;
            // foreach (var key in Termos.Keys)
            // {
            //     Termos[key] = "";
            // }
            // Termos["timestamp"] = "";
            // Termos["_life"] = "";
            // Termos["_isDead"] = "";
            // Termos["money"] = "";
            // Termos["isOnSpecial"] = "";
            // Termos["totalbulletSync"] = "";
            // Termos["spawnedEnemies"] = "";
            // Termos["waveCounter"] = "";
            
        }
        
        public override string ToString()
        {
            var resultado = "";
            // resultado += termos["timestamp"] == "" ? "NULL" : termos["timestamp"]; 
            //
            var chaves = Termos.Keys;
            var first = true;
            foreach (var chave in chaves)
            {
                if (!first)
                    resultado += ";";
                first = false;
                if (Termos[chave] != "")
                    resultado += Termos[chave];
                else
                    resultado += "-";
            }

            return resultado;
        }

        public static string ReturnHeader()
        {
            string lista = "timeStamp;";
            lista += string.Join(";", Global.itens);

            return lista; //"timestamp;_life;_isDead;money;isOnSpecial;totalbulletSync;spawnedEnemies;waveCounter";
        }
    }
    
    
    //[Serializable]
    public struct Input  : IEqualityComparer<Input>
    {
        public string Nome;
        public DateTime Tempo;
        public Dictionary<FieldInfo,object> Conteudo; // = new Dictionary<FieldInfo, Object>();

        public override string ToString()
        {
            // Provisório
            return base.ToString() + " - Provisório";
        }

        public bool Equals(Input x, Input y)
        {
            var a = x.Tempo == y.Tempo;
            var b = x.Conteudo == y.Conteudo;
            return a && b;
        }

        public int GetHashCode(Input obj)
        {
            return obj.Tempo.GetHashCode() + obj.Conteudo.GetHashCode();
        }
    }

    public struct JsonUtil
    {
        public const string IniObj = "{";
        public const string EndObj = "}";
        public const string IniArr = "[";
        public const string EndArr = "]";
        public const string ItemSeparator = ",";
        public const string KeyValueSeparator = ":";
    }
    
    
    public class Global : MonoBehaviour
    {
        private static string nomeArquivoBase = "saida";
        private static List<Input> itemBuffer = new List<Input>();
        private static readonly List<string> archivesCreated = new List<string>();
        private static readonly float Periodo = 5.0f;
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        private static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long) elapsedTime.TotalMilliseconds;
        }
        
        
        public static void AddInput(Input item)
        {
            //if (Filter(item.nome))
            //{
                itemBuffer.Add(item);
            //}
        }

        private static string[] variaveisEscolhidas = {"_life", "_isDead", "money", "isOnSpecial", "totalbulletSync", "spawnedEnemies", "waveCounter"};
        private static bool Filter(string nome)
        {
            return true;
            foreach (var variavel in variaveisEscolhidas)
            {
                if (nome == variavel)
                    return true;
            }
            
            return false;
        }
        
        public static IEnumerator UpdateNovo()
        {      
            
            while (true)
            {
                
                now = ConvertToTimestamp(DateTime.Now);
                while ((now - past) < (Periodo * 1000)) // Convertendo para mili-segundos
                {  
                    //Debug.Log(now - past);  
                    yield return new WaitForSeconds(0.25f); // valor inicial dps abstrair para variavel
                    now = ConvertToTimestamp(DateTime.Now);
                }
                //Debug.Log("AAAAAAAAQUI");
                past = now;
                PrintInArchive().Wait();
                
            }

        }

        private static bool first = true;
        public static Task PrintInArchive()
        {
            List<string> moldeJson = new List<string>();
            List<string> moldeCsv = new List<string>();
            if (Global.first)
            {
                moldeCsv.Add(LineCsv.ReturnHeader());
                Global.first = false;
            }
                
            moldeJson.Add(JsonUtil.IniArr);
            bool first = true;
            LineCsv linha = new LineCsv();
            bool novaLinhaCSV = true;
            foreach (var item in itemBuffer)
            {
                bool vaiImprimir = false;
                foreach (var par in item.Conteudo)
                {
                    if (Filter(par.Key.Name))
                    {
                        vaiImprimir = true;
                        break;
                    }
                }
                if(!vaiImprimir)
                    continue;
                if (!first)
                    moldeJson.Add(JsonUtil.ItemSeparator);

                if (linha.Termos["timestamp"] == ConvertToTimestamp(item.Tempo).ToString())
                    novaLinhaCSV = false;
                else
                {
                    moldeCsv.Add(linha.ToString());
                    linha.Clear();
                    novaLinhaCSV = true;
                }
                    
                linha.Termos["timestamp"] = ConvertToTimestamp(item.Tempo).ToString();
                first = false;
                moldeJson.Add(JsonUtil.IniObj);
                moldeJson.Add("\"objeto\" : " + "\"" + item.Nome + "\"" + JsonUtil.ItemSeparator);
                moldeJson.Add("\"tempo\" : " + "\"" + ConvertToTimestamp(item.Tempo).ToString() + "\"" + JsonUtil.ItemSeparator);
                moldeJson.Add("\"estado\" : " + JsonUtil.IniArr);
                bool firstInterno = true;
                foreach (var par in item.Conteudo)
                {
                    if (!Filter(par.Key.Name))
                        continue;
                    if(!firstInterno)
                        moldeJson.Add(JsonUtil.ItemSeparator);
                    firstInterno = false;
                    moldeJson.Add(JsonUtil.IniObj);
                    moldeJson.Add("\"nomeVariavel\" : \"" + par.Key.Name + "\"" + JsonUtil.ItemSeparator);
                    moldeJson.Add("\"valor\" : \"" + par.Value?.ToString() + "\"");
                    var lastPar = item.Conteudo.Last();
                    if ((par.Key != lastPar.Key) || (par.Value != lastPar.Value))
                        moldeJson.Add(JsonUtil.EndObj); //+ JSON_Util.ItemSeparator);
                    else
                        moldeJson.Add(JsonUtil.EndObj);

                    linha.Termos[par.Key.Name] = par.Value.ToString();
                }
                moldeJson.Add(JsonUtil.EndArr);
                var lastItem = itemBuffer.Last();

                if (Equals(lastItem, item))
                    moldeJson.Add(JsonUtil.EndObj);
                else
                    moldeJson.Add(JsonUtil.EndObj); //+ JSON_Util.ItemSeparator);
            }
            moldeJson.Add(JsonUtil.EndArr);
            moldeCsv.Add(linha.ToString());
            //await File.WriteAllLinesAsync(Application.dataPath + "\\" + nomeArquivoBase + DateTime.Now.ToShortDateString().ToString().Replace('/','-') + ".json",molde.ToArray());
            string archiveName =
                nomeArquivoBase + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + ".json";
            archivesCreated.Add(archiveName);
            
            File.WriteAllLines(Application.dataPath + "/Logs" + "\\" + archiveName,moldeJson.ToArray());
            
            File.AppendAllLines(csvPath, moldeCsv);
            itemBuffer.Clear();
            Debug.Log("Arquivo emitido");
            return Task.CompletedTask;
        }

        private static long now;
        private static long past;
        private static bool _first = true;
        private static string csvPath = "";
        internal static List<string> itens = new();
        public static void Init(List<string> variables)
        {
            if(_first)
            {
                itens.Clear();
                csvPath = Application.dataPath + "\\" + "saidaTotal" +
                          DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + ".csv";
                var aux = File.Create(csvPath);
                aux.Close();
                _first = false;
                now = ConvertToTimestamp(DateTime.Now);
                past = ConvertToTimestamp(DateTime.Now);
            }

            foreach (var variable in variables)
            {
                itens.Add(variable);
            }
            
        }
        private static bool _firstEnd = true;
        public static void End()
        {
            if (_firstEnd)
            {
                _firstEnd = false;
                // Tentativa falha de JSON 
                // string text = File.ReadAllText(Application.dataPath + "\\" + archivesCreated[0]);
                // var test = JsonHelper.FromJson<Input>(text);
                // Debug.Log("Json teste" + test[0]);


            }
        }
        

    }

}

