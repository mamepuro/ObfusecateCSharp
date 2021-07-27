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
            int[] nextTokenIndex = new int[10];
            using (var writer = new StreamWriter(obClassName + ".cs"))
            {
                foreach(var line in codes)
                {
                    //string newLine = "";
                    if(line.IndexOf("\"//\"") == -1 && line.IndexOf("\"///\"") == -1 && line.IndexOf("\"*/\"") == -1 && line.IndexOf("\"/*\"") == -1)
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
                    for(int i = 0;i < line.Length;　i++)
                    {
                        //Console.WriteLine(line.Length);
                        string tmpLine = line.Substring(i);
                        if(line[i] == ' ' || line[i] == '\t' || line[i] == '\n' || line[i] == '\r')
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
                }
            }
        }
    }
}
