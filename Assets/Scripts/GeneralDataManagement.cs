using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;


public class GeneralDataManagement : MonoBehaviour
{
    public string out_filename;


    public void SetFilename(string data_dir, string dataname, int SubId)
    {

        string dir = data_dir + "/" + SubId + "/";
        Directory.CreateDirectory(dir);

        DateTime dt = DateTime.Now;
        out_filename = string.Format(dir + dataname + "_{0}-{1}-{2}-{3}-{4}-{5}.txt", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }


    public void LoadStringFromFile(INIFile iniFileCtl, ref string var, string group, string var_name)
    {
        try
        {
            var = iniFileCtl[group, var_name].Trim();
        }
        catch
        {
            Debug.Log(var_name + " is not string in the Init file ");
        }

    }

    public void LoadIntFromFile(INIFile iniFileCtl, ref int var, string group, string var_name)
    {
        try
        {
            var = Int32.Parse(iniFileCtl[group, var_name].Trim());
        }
        catch
        {
            Debug.Log(var_name + " is not int in the Init file ");
        }

    }

    public void LoadFloatFromFile(INIFile iniFileCtl, ref float var, string group, string var_name)
    {
        try
        {
            var = float.Parse(iniFileCtl[group, var_name].Trim(), System.Globalization.CultureInfo.InvariantCulture);
        }
        catch
        {
            Debug.Log(var_name + " is not int in the Init file ");
        }

    }


}
