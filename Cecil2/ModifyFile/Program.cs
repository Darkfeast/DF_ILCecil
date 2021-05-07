using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

namespace ModifyFile
{
    class Program
    {
        static void Main(string[] args)
        {
            FileModify fm = new FileModify();
            //fm.ModifyDirName();

            fm.ParseJson();
        }
    }

    class FileModify
    {
        string filePath = @"D:\UIRes\识字\动画优化\all";


        string jsonFileLink = "dinosaurData_link.json";
        CharacterData chataData;
        Dictionary<int, Character> dictCharacterData = new Dictionary<int, Character>();
        Dictionary<int, List<string>> dictId = new Dictionary<int, List<string>>();
        Dictionary<int, string> dictMap = new Dictionary<int, string>();

        public void ModifyDirName()
        {
            if( PathExist(filePath))
            {
                List<string>  listDirs= new List<string>( Directory.GetDirectories(filePath,"*",SearchOption.TopDirectoryOnly));

                for(int i=0;i<listDirs.Count;i++)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(listDirs[i]);
                    DFLog.Log($" {directoryInfo.Name}    fm  {directoryInfo.FullName}   {directoryInfo.FullName.Length}");

                    string newPath = directoryInfo.FullName.Length > 27 ? directoryInfo.FullName.Substring(0, directoryInfo.FullName.Length - 2) : directoryInfo.FullName;
                    if(directoryInfo.FullName.Length>27)
                        directoryInfo.MoveTo(newPath);
                     //= directoryInfo.Name
                    //string[] sps =listDirs[i].Split('\\');
                    //listDirs[i] = sps[sps.Length - 1].Length>6 ? sps[sps.Length-1].Substring(0,sps[sps.Length-1].Length-2):sps[sps.Length-1];
                }

                //foreach(var v in listDirs)
                //{
                //    DFLog.Log($"{v}   leng  {v.Length}");
                //}
            }
        }


        public void ParseJson()
        {
            ParseJson(jsonFileLink);
            PrintJsonLink(dictId);
            PrintJsonLink(dictMap);
        }


	    void ParseJson(string name)
        {
            if (!File.Exists(name))
                return;

            string ta=File.ReadAllText(name);
            //DFLog.Log(ta,E_ColorType.Green);
            chataData = JsonMapper.ToObject<CharacterData>(ta);
            for (int i = 0; i < chataData.characterData.Count;i++)
            {
                string mainId= chataData.characterData[i].mainCard.Split('#')[1];
                string[] subs= chataData.characterData[i].subCard.Split('|');
                List<string> listSub = new List<string>();

                for(int j =0; j<subs.Length;j++)
                {
                    string subId= subs[j].Split('#')[1];
                    listSub.Add(mainId + "_" + subId);
                }
                dictId[chataData.characterData[i].ID] = listSub;
                dictMap[chataData.characterData[i].ID] = chataData.characterData[i].mainCard.Split('#')[0];
		    }
        }

        void PrintJsonLink(object dict)
        {
            if(dict is IDictionary)
            {
                IDictionary dic = dict as IDictionary;
                foreach(var v in dic.Keys)
                {
                    if(dic[v] is IList)
                    {
                        IList lis= dic[v] as IList;
                        foreach(var vv in lis)
                        {
                            if(int.Parse(v.ToString()) <2)
                            {
                                DFLog.Log("key  " + v + "   val " + vv,E_ColorType.Cyan);
                            }
                        }
                    }
                    else
                    {
                        DFLog.Log("key  " + v + "   val "+dic[v] ,E_ColorType.Green);
                    }
                }
            }
   
        }


        public bool PathExist(string path)
        {
            if (Directory.Exists(path))
                return true;
            else
                return false;
        }
    }

}
