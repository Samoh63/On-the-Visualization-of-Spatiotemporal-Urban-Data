using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;

using Mapbox.Examples;
using Mapbox.Unity.MeshGeneration.Modifiers;


public class exportCSV : MonoBehaviour
{
    string filename = "";




    public void WriteCSV()
    {
        filename = Application.dataPath + "/Resources/ListOfLights_export.csv";
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Light Type, Latitude, Longitude, Color, Intensity");
        tw.Close();

        tw = new StreamWriter(filename, true);
        foreach (Quest item in StreetLight.listOfLights)
        {
            tw.WriteLine(item.lightType + "," + item.GPSLatitude + "," + item.GPSLongitude + "," +
                        item.lightColor + "," + item.lightIntensity1);

        }
        tw.Close();
        Debug.Log("File is saved");

    }






}


