using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace ConfuserCSharp
{
    /// <summary>
    /// ファイルに関する操作を行う
    /// </summary>
    static class FileOperater
    {
        /// <summary>
        /// ダミーのメンバでサポートする型
        /// </summary>
        private static List<string> Dammytype = new List<string>()
        {
            "bool", "string", "int", "long", "short"
        };

        /// <summary>
        /// ありそうな文字列リスト
        /// </summary>
        private static List<string> DecoyString = new List<string>()
        {
            "Content :", "Now Content in this List: ", "Custom Id Setting :", "You can Remove this from:", "Id", "Type Name is ",
            "Enemy Hp","Score:", "UI DisplayMode :", "Sending Http Request...", "TimeOut. Try Again", "Smash Bros is the Best Game in the World",
            "DateTime = ", "Location is ", "Your ID:", "Next:", "Current Index is ", "Debug Mode", "Releace Mode", "SQL request is",
            "Error :", "Error!: Correct Usage is", "Fatal! Your Operation is dangerous!", "Enemy spotted!", "Popping a Battery",
            "Your Dinner is", "The Weather is", "Time is", "Current list", "Yes", "No", "Playing", "Val is ", "Apple", "Next",
            "Margin is ", "Testing case..."
        };


        /// <summary>
        /// 現在のブロックで宣言されているダミーメンバの辞書
        /// 名前と型が格納される
        /// </summary>
        private static Dictionary<string, string> tmpDammyVal = new Dictionary<string, string>();

        /// <summary>
        /// 宣言したダミーのメンバリスト
        /// </summary>
        private static List<string> DammyVals = new List<string>()
        {

        };
        /// <summary>
        /// クラスファイル名を取得する
        /// </summary>
        /// <returns>クラスファイル名のリスト</returns>
        static public List<string> GetClassFileNames(string path)
        {
            var lengthOfPath = path.Length;
            var classFileName = new List<string>();
            string[] fileNames = Directory.GetFiles(path);
            foreach (var name in fileNames)
            {
                if (name.IndexOf(".cs") != -1 && name.IndexOf("csproj") == -1)
                {
                    string fileName = name.Substring(lengthOfPath, name.Length - lengthOfPath);
                    classFileName.Add(fileName);
                    Console.WriteLine(fileName);
                }
            }
            return classFileName;
        }

        /// <summary>
        /// プログラムのコード全文を取得する
        /// </summary>
        /// <param name="filename">クラスファイルの名前</param>
        /// <returns>プログラム全文</returns>
        public static string[] GetProgramCodes(string filename)
        {
            var codes = File.ReadAllLines("../../../" + filename);
            return codes;
        }

        private static void AddDammyMemberVal(StreamWriter writer, Dictionary<string, string> identifier)
        {
            var r = new Random();
            string type = Dammytype[r.Next(0, Dammytype.Count)];
            string dammyValName = ObfusecateSupporter.GetRandomName();
            object dammyLiteral;
            var dt = new DateTime();
            while(identifier.ContainsValue(dammyValName) || DammyVals.Contains(dammyValName))
            {
                dammyValName = ObfusecateSupporter.GetRandomName();
            }
            switch (type)
            {
                case "string":
                    dammyLiteral = DecoyString[r.Next(0, DecoyString.Count)];
                    writer.WriteLine("public "+type + " " + dammyValName + " = " + "\"" + dammyLiteral + "\";");
                    break;
                case "bool":
                    if(dt.Ticks % 2 == 0)
                    {
                        dammyLiteral = "false";
                    }
                    else
                    {
                        dammyLiteral = "true";
                    }
                    writer.WriteLine("public " + type + " " + dammyValName + " = " + dammyLiteral + ";");
                    break;
                default:
                    dammyLiteral = r.Next(0, 10000);
                    writer.WriteLine("public " +type + " " + dammyValName + " = " + dammyLiteral + ";");
                    break;
            }
            DammyVals.Add(dammyValName);
            tmpDammyVal.Add(dammyValName, type);
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="identifierDict">識別子のリスト</param>
        public static void WriteObfusecateCodes(string filename, Dictionary<string, string> identifierDict)
        {
            var codes = GetProgramCodes(filename);
            Console.WriteLine(codes.Length);
            var className = filename.Substring(0, filename.Length - 3);
            //Console.WriteLine(className);
            var obClassName = identifierDict[className];
            bool doIgnore = false;
            int[] nextTokenIndex = new int[11];
            bool isStaticClass = false;
            //クラス宣言が始まったかどうか
            bool DidClassDecStart = false;
            int flag = 0;
            var r = new Random();
            using (var writer = new StreamWriter("../../../" + obClassName + ".cs"))
            {
                foreach(var line in codes)
                {
                    var lineTokens = line.Split(new char[] { ' ', '\t', '(', ')', ',', '.', ';', }, StringSplitOptions.RemoveEmptyEntries);

                    if(Array.IndexOf(lineTokens, "class") != -1)
                    {
                        DidClassDecStart = true;
                        if(Array.IndexOf(lineTokens, "static") != -1)
                        {
                            isStaticClass = true;
                        }
                    }

                    if(lineTokens.Length > 0)
                    {
                        if (ObfusecateSupporter.reservedWords.Contains(lineTokens[0]) && flag != 0)
                        {
                            isStaticClass = true;
                        }
                    }
                    if (DidClassDecStart)
                    {
                        flag++;
                    }
                    if (line.IndexOf("\"//\"") == -1 && line.IndexOf("\"///\"") == -1 && line.IndexOf("\"*/\"") == -1 && line.IndexOf("\"/*\"") == -1)
                    {
                        if (line.IndexOf("*/") != -1)
                        {
                            doIgnore = false;
                            continue;
                        }

                        if (line.IndexOf("/*") != -1)
                        {
                            doIgnore = true;
                            continue;
                        }

                        //「//」または「///」がある行を全て削除する
                        if (line.IndexOf("//") != -1 || line.IndexOf("///") != -1 || doIgnore)
                        {
                            continue;
                        }
                    }
                    for(int i = 0;i < line.Length; i++)
                    {
                        //Console.WriteLine(line.Length);
                        string tmpLine = line.Substring(i);
                        if(line[i] == ' ' || line[i] == '\t' || line[i] == '\n' || line[i] == '\r' || line[i] == '　')
                        {
                            writer.Write(line[i]);
                            continue;
                        }
                        //トークンの区切り文字となりうるモノを全て調査する
                        nextTokenIndex[0] = tmpLine.IndexOf(' ');
                        nextTokenIndex[1] = tmpLine.IndexOf('.');
                        nextTokenIndex[2] = tmpLine.IndexOf(',');
                        nextTokenIndex[3] = tmpLine.IndexOf(';');
                        nextTokenIndex[4] = tmpLine.IndexOf('+');
                        nextTokenIndex[5] = tmpLine.IndexOf('-');
                        nextTokenIndex[6] = tmpLine.IndexOf('[');
                        nextTokenIndex[7] = tmpLine.IndexOf('(');
                        nextTokenIndex[8] = tmpLine.IndexOf(')');
                        nextTokenIndex[9] = tmpLine.IndexOf(']');
                        nextTokenIndex[10] = tmpLine.IndexOf('!');

                        int minNextTokenIndex = 1000000;
                        for(int index = 0; index < nextTokenIndex.Length; index++)
                        {
                            if(nextTokenIndex[index] == -1)
                            {
                                nextTokenIndex[index] = 1000000;
                            }
                            if(nextTokenIndex[index] < minNextTokenIndex)
                            {
                                minNextTokenIndex = nextTokenIndex[index];
                            }
                        }
                        if(minNextTokenIndex == 0)
                        {
                            minNextTokenIndex = 1;
                        }
                        else if(minNextTokenIndex == 1000000)
                        {
                            minNextTokenIndex = tmpLine.Length;
                        }
                        string token = tmpLine.Substring(0, minNextTokenIndex);
                        if(identifierDict.ContainsKey(token))
                        {
                            writer.Write(identifierDict[token]);
                            //Console.WriteLine(token);
                        }
                        else
                        {
                            writer.Write(token);
                            //Console.WriteLine(token);
                        }
                        //文字数分進める(但し、インクリメント部分を考慮する)
                        i += token.Length - 1;
                        //Console.WriteLine(i);
                    }
                    writer.WriteLine("");

                    if (flag >= 2&& !isStaticClass && DidClassDecStart)
                    {
                        int times = r.Next(0, 10);
                        for (int j = 0; j < times; j++)
                        {
                            AddDammyMemberVal(writer, identifierDict);
                        }
                    }
                }
            }
        }
    }
}
