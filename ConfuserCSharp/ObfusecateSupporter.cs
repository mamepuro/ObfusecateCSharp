using System;
using System.Collections.Generic;
using System.Text;

namespace ConfuserCSharp
{
    /// <summary>
    /// 難読化処理に関する機能を提供するクラス
    /// </summary>
    static class ObfusecateSupporter
    {
        /// <summary>
        /// アクセス修飾子
        /// </summary>
        public static readonly HashSet<string> accesser = new HashSet<string>()
        {"private", "public", "internal", "abstract", "static", "sealed", "protected"};
        /// <summary>
        /// C#の予約後(文脈のキーワードも含む)
        /// </summary>
        public static readonly HashSet<string> reservedWords = new HashSet<string>()
        {
            "abstract", "as", "async", "await", "base", "bool", "break", "byte", "case", "catch",
            "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do",
            "double", "else", "enum", "event","explicit", "exteren", "false", "finarlly", "fixed",
            "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
            "paramas", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
            "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw",
            "true", "try", "typeof", "unit", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual",
            "volatile", "void", "while", "add", "dynamic", "get", "partial", "remove", "set", "value", "var",
            "where", "yield", "when"
        };

        public static readonly HashSet<string> DefaultType = new HashSet<string>()
        {
            "char", "char[]", "int", "int[]", "string", "string[]", "float", "float[]", "double", "double[]",
            "decimal", "decimal[]", "byte", "byte[]", "uint", "uint[]", "short", "short[]", "ushort", "struct",
            "object", "object[]", "enum", "event", "long", "long[]", "ulong", "ulong[]", "List", "List<", "HashSet", "Dictionary", 
            "var", "in", "class"
        };

        private static readonly HashSet<string> NotIdentifiers = new HashSet<string>()
        {
            ";", "{", "}", "==", ">=", "<=", "!=", "^", "+", "-", "+=", "-=", "*", "/", "*=", "/="
        };
        /// <summary>
        /// 名詞リスト
        /// </summary>
        private static readonly List<string> Noun = new List<string>()
        {
            "Apple", "Alpha", "Argent", "April", "Asgore", "Ape", "Apex", "Aqua", "Award", "Bone", "Bob", "Baby",
            "Bajil", "Bay", "Board", "Beta", "Bravo", "Charlie", "Chocolate", "ClassMate", "Curry", "Chalotte", "Clumn",
            "Delta", "DK", "Domino", "Doom", "Double", "Dog", "Deck", "Echo", "Emergence", "Electricity", "Foxtrot", "Fortune",
            "Fortress", "Golf", "George", "Government", "Hotel", "Home", "House", "India", "Imperial", "Internet", "Juliet", 
            "Juice", "Kilo", "Kiss", "King", "Lima", "Lava", "Leo", "Mike", "Milk", "Moon", "November", "Neo", "NitroCell", "Oscar",
            "Octopus", "Papa", "Pudding", "Phoenix", "Quebec", "Queen", "Queue", "Romeo", "Room", "Sierra", "Siege", "Six", "Tango",
            "Tortue", "Toe", "Uniform", "Universe", "Usage", "Victor", "Vector", "Virus", "Whiskey", "Winter", "X-ray", "Yankee", "Yawn",
            "Zulu", "Zoo", "Zack"
        };

        /// <summary>
        /// 動詞リスト
        /// </summary>
        private static readonly List <string> Verb = new List<string>()
        {
            "Do", "Run", "Occur", "Please", "Write", "Jump", "Hop", "Print", "Move", "Use", "Yell"
        };

        /// <summary>
        /// 前置詞リスト
        /// </summary>
        private static List<string> Preposition = new List<string>() 
        {
            "To", "Of", "In", "From", "Next_to", "On", "Into", "Around", "Among" 
        };

        /// <summary>
        /// 副詞リスト
        /// </summary>
        private static List<string> Adverb = new List<string>()
        {
            "Always", "Then", "There", "Maybe", "Tomorrow", "Yesterday", "Seriously", "Happily", "Personally",
            "Usually", "Hardly", "Sometimes", "Immediately"
        };

        /// <summary>
        /// クラス名をプログラムファイルから取得する。但し、クラス宣言の行は必ず1行で書かれなければならない。
        /// </summary>
        /// <param name="programLines">クラスファイルの全ての行</param>
        /// <returns>クラス名が格納されたリスト</returns>
        public static List<string> GetClassName(string[] programLines)
        {
            var classNames = new List<string>();
            foreach(var line in programLines)
            {
                string[] token = line.Split(new char[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries);
                int index = Array.IndexOf(token, "class");
                if(Array.IndexOf(token, "class") != -1)
                {
                    int classNameIndex = token.Length - 1;
                    string className = token[classNameIndex];
                    Console.WriteLine(className);
                    classNames.Add(className);
                }
            }
            return classNames;
        }

        /// <summary>
        /// 名前空間の名前を取得する
        /// (但し、全てのクラスファイルで名前空間は統一されている前提で処理を行う)
        /// </summary>
        /// <param name="programLine">プログラムの全文</param>
        /// <returns>namesapace句で定義されている名前</returns>
        public static string GetNamespace(string[] programLine)
        {
            string namespaceName = "null";
            foreach (var line in programLine)
            {
                string[] token = line.Split(new char[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries);
                int index = Array.IndexOf(token, "namespace");
                if(index != -1)
                {
                    int namespaceIndex = token.Length - 1;
                    namespaceName = token[namespaceIndex];
                    break;
                }
            }
            return namespaceName;
        }

        public static List<string> GetIdentifer(string[] programLine, List<string> ClassNames)
        {
            var IdentiferList = new List<string>();
            string type = "null";
            string identifier;
            foreach (var line in programLine)
            {
                string[] token = line.Split(new char[] { ' ', '\t', '(', ')', ',', '.', ';', }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var t in token)
                {
                    //適切にエスケープができていない("がはじけてない)
                    //コメントを除去する処理を事前に施さないと上手く行かない
                    if(ObfusecateSupporter.DefaultType.Contains(t) 
                        || ClassNames.Contains(t) 
                        || (t.IndexOf("<") != -1 && t.IndexOf("<summary") == -1)
                        )
                    {
                        type = t;
                        int typeIndex = Array.IndexOf(token, type);
                        //Console.WriteLine(token.Length + ", " + typeIndex);
                        if(token.Length > typeIndex + 1)
                        {
                            identifier = token[typeIndex + 1];
                            if (type != "null"
                                && token.Length > typeIndex + 1
                                && typeIndex != -1
                                && !ObfusecateSupporter.NotIdentifiers.Contains(identifier)
                                && identifier.IndexOf("\"") == -1
                                && type.IndexOf("Dictionary") == -1)
                            {
                                //Console.WriteLine(identifier);
                                IdentiferList.Add(identifier);
                            }
                            else if(type != "null"
                                && token.Length > typeIndex + 1
                                && typeIndex != -1
                                && !ObfusecateSupporter.NotIdentifiers.Contains(identifier)
                                && identifier.IndexOf("\"") == -1
                                && type.IndexOf("Dictionary") != -1)
                            {
                                IdentiferList.Add(token[typeIndex + 2]);
                            }
                        }
                    }
                }
            }
            return IdentiferList;
        }

        /// <summary>
        /// 識別子と難読化された識別子の対応関係の辞書を得る
        /// </summary>
        /// <returns>識別子と難読化後の識別子の対応関係の辞書</returns>
        public static Dictionary<string, string> GetObfuseCatedIdentifierDictionary(List<string> identifier)
        {
            var dict = new Dictionary<string, string>();
            foreach (var idnt in identifier)
            {
                //識別子が登録済みなら何もしない
                if(dict.ContainsKey(idnt))
                {
                    continue;
                }
                string obfident = GetRandomName();
                while(dict.ContainsValue(obfident))
                {
                    obfident = GetRandomName();
                }
                dict.Add(idnt, obfident);
            }
            return dict;
        }

        /// <summary>
        /// コメントアウトの行を全て削除する
        /// </summary>
        public static void RemoveCommetLines()
        {

        }
        /// <summary>
        /// 名詞+動詞+前置詞+名詞+副詞のランダム生成された文字列を返す。
        /// </summary>
        /// <returns>ランダムに生成された文字列</returns>
        public static string GetRandomName()
        {
            int nounLength = ObfusecateSupporter.Noun.Count;
            int verbLength = ObfusecateSupporter.Verb.Count;
            int prepositionLength = ObfusecateSupporter.Preposition.Count;
            int adverbLength = ObfusecateSupporter.Adverb.Count;
            Random r = new Random();
            return ObfusecateSupporter.Noun[r.Next(0, nounLength)] +
                ObfusecateSupporter.Verb[r.Next(0, verbLength)] +
                ObfusecateSupporter.Preposition[r.Next(0, prepositionLength)] +
                ObfusecateSupporter.Noun[r.Next(0, nounLength)] +
                ObfusecateSupporter.Adverb[r.Next(0, adverbLength)];
        }
    }
}
