using UnityEngine;
using System.Collections;

public class JsonIO : MonoBehaviour
{
        string exeRuntimeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        

    public void WriteLevel (string level, string levelOutput)
    {
       
        string subDirectory = System.IO.Path.Combine(exeRuntimeDirectory, "Levels");

        if (!System.IO.Directory.Exists(subDirectory))
        {
            // Output directory does not exist, so create it.
            System.IO.Directory.CreateDirectory(subDirectory);
        }

        string fileName = System.IO.Path.Combine(subDirectory, string.Format("{0}", level));

        System.IO.File.WriteAllText(fileName, levelOutput);

    }

    public string ReadLevel (string level)
    {

        string subDirectory = System.IO.Path.Combine(exeRuntimeDirectory, "Levels");
        string fileName = System.IO.Path.Combine(subDirectory, string.Format("{0}", level));


        string levelInput = System.IO.File.ReadAllText(fileName);

        return levelInput;
    }


}