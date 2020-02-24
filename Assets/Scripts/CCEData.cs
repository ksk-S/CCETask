using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

[System.Serializable]
public class CCEData : GeneralDataManagement
{
    public int numRepetition = 1;

    public System.Random rnd;

    public int subId;
    public string data_dir = "Results/";
    string config_file = "config.ini";
    int nonce = 200000;

    public enum Laterality
    {
        LEFT, RIGHT
    }

    public enum Congruency
    {
        CONGRUENT, INCONGRUENT
    }

    public enum Side
    {
        SAME, OTHER
    }

    
    [System.Serializable]
    public struct Result
    {
        public int subId;
        public int trialId;
        public int repId;
        public int conditionId;
        public Laterality laterality;
        public Congruency congruency;
        public Side side;
        public long SoA;
        public long rt;
        public int response;
        public bool correct;

    }

    public List<Result> resultList = new List<Result>();


    void Awake()
    {


        string initfile = Application.dataPath + "/../" + config_file;
        INIFile iniFileCtl = new INIFile(initfile);

        LoadIntFromFile(iniFileCtl, ref subId, "Experiment", "subId");
        //        LoadStringFromFile(iniFileCtl, ref subId, "Experiment", "subId");

        rnd = new System.Random(subId + nonce);

        CreateConditions();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void CreateConditions()
    {
        int condition = 0;
        for (int repId = 0; repId < numRepetition; repId++)
        {
            for (int latId = 0; latId < Enum.GetNames(typeof(Laterality)).Length; latId++)
            {
                for (int congId = 0; congId < Enum.GetNames(typeof(Congruency)).Length; congId++)
                {
                    for (int sideId = 0; sideId < Enum.GetNames(typeof(Side)).Length; sideId++)
                    {
                        Result param = new Result();
                        param.subId = subId;
                        param.repId = repId;
                        param.conditionId = condition;
                        param.laterality = (Laterality)latId;
                        param.congruency = (Congruency)congId;
                        param.side = (Side)sideId;
                        param.rt = -1;
                        param.SoA = -1;
                        param.response = -1;
                        resultList.Add(param);
                    }
                }
            }
        }

        Shuffle(resultList);

        for (int i = 0; i < resultList.Count(); i++)
        {
            Result r = resultList[i];
            r.trialId = i;
            resultList[i] = r;
        }
    }

    public void SaveHeaderToFile()
    { 
        string text = "";
        text += "#subId, trialId, conditionId, laterality, congruency, side, SoA, rt, response, correct";
        text += Environment.NewLine;
        File.AppendAllText(out_filename, text);
    }


    public void SaveDataToFile(Result p)
    {
        string text = "";
        text += p.subId + ", ";
        text += p.trialId + ", ";
        text += p.conditionId + ", ";
        text += p.laterality + ", ";
        text += p.congruency + ", ";
        text += p.side + ", ";
        text += p.SoA + ", ";
        text += p.rt + ", ";
        text += p.response + ", ";
        text += p.correct + ", ";
        text += Environment.NewLine;

        File.AppendAllText(out_filename, text);
    }


    public void Shuffle<T>(List<T> list)
    {

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
