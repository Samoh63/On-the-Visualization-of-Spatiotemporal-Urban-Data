using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;

using Mapbox.Examples;
using Mapbox.Unity.MeshGeneration.Modifiers;


public class importCSV : MonoBehaviour
{
    public static List<Quest> streetLights = new List<Quest>();

    string path;
    Vector2d _Locations = new Vector2d();

    [SerializeField]
    AbstractMap _map;
    public GameObject[] prefabs;




    public void OpenFileExplorer()
    {

        // path = EditorUtility.OpenFilePanel("Select a CSV file ...", "", "CSV");
        TextAsset questdata = Resources.Load<TextAsset>("ListOfLights");
        string[] data = questdata.text.Split(new char[] { '\n' });
        // Debug.Log(data[0]);
        Vector3 UnityCo;

        for (int i = 1; i < data.Length - 1; i++)
        {
            //Define variables
            // RaycastHit hit;
            Quest q = new Quest();
            GameObject myLight;


            //seperate data from CSV and put the in the q list
            string[] row = data[i].Split(new char[] { ',' });

            q.lightType = row[0];
            //year
            double.TryParse(row[1], out q.GPSLatitude);
            double.TryParse(row[2], out q.GPSLongitude);
            q.lightColor = row[3];
            float.TryParse(row[4], out q.lightIntensity1);

            //Convert Global location to Unity Location
            _Locations = Conversions.StringToLatLon(row[1] + ',' + row[2]);
            UnityCo = _map.GeoToWorldPosition(_Locations, true);
            q.UnityPosition = UnityCo;
            StreetLight.listOfLights.Add(q);
            Debug.Log(q.lightType);
            switch (q.lightType)
            {


                case "StreetLamp2_Tall (Black)(Clone)":
                    Debug.Log(0);
                    myLight = Instantiate(prefabs[0], q.UnityPosition, Quaternion.identity);

                    break;
                case "StreetLamp2_TallDouble (Black)(Clone)":
                    Debug.Log(1);
                    myLight = Instantiate(prefabs[1], q.UnityPosition, Quaternion.identity);
                    break;
                case "StreetLamp2_Short (Black)(Clone)":
                    Debug.Log(2);
                    myLight = Instantiate(prefabs[2], q.UnityPosition, Quaternion.identity);
                    break;
                case "StreetLamp2_ShortDouble (Black)(Clone)":
                    Debug.Log(3);
                    myLight = Instantiate(prefabs[2], q.UnityPosition, Quaternion.identity);
                    break;

            }




        }


    }
}
