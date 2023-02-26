using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
    public class SunVar
    {

        public string hour;


        public float Elevation; //energy consumption in each month

        public float Azimuth; //energy consumption in each month

        public Vector3 SunPos;


    }


    // public GameObject sunRotation;
    public GameObject Sun;
    public static List<SunVar> quests = new List<SunVar>();

    public static float sunIntensity;




    // string RaycastRet;
    Vector3 org;

    public static float max_enCon = 0f;

    // public GameObject FloatingTextPrefab;
    public static List<GameObject> myTextList = new List<GameObject>();

    bool selected;



    void Start()
    {
        //the path of sun during a day in 12/4/2022 for Alesund, Norway
        TextAsset questdata = Resources.Load<TextAsset>("SunPositionMay");
        string[] data = questdata.text.Split(new char[] { '\n' });
        selected = false;
        float distance = 2000f;
        float x, y, z;

        // float max_enCon_temp = 0f;
        // Vector3 UnityCo;



        for (int i = 4; i < data.Length - 1; i++)
        {
            //Define variables
            // RaycastHit hit;
            SunVar q = new SunVar();



            //seperate data from CSV and put the in the q list
            string[] row = data[i].Split(new char[] { ',' });

            q.hour = row[0];
            //year
            float.TryParse(row[1], out q.Elevation);
            // Debug.Log(q.year);
            float.TryParse(row[2], out q.Azimuth);

            x = distance * Mathf.Cos(q.Elevation * Mathf.PI / 180) * Mathf.Sin(q.Azimuth * Mathf.PI / 180);
            y = distance * Mathf.Sin(q.Elevation * Mathf.PI / 180);
            z = distance * Mathf.Cos(q.Elevation * Mathf.PI / 180) * Mathf.Cos(q.Azimuth * Mathf.PI / 180);

            q.SunPos = new Vector3(x, y, z);

            quests.Add(q);

        }
        transform.GetChild(0).position = quests[0].SunPos;
        // x = distance * Mathf.Cos(quests[0].Elevation * Mathf.PI / 180) * Mathf.Sin(quests[0].Azimuth * Mathf.PI / 180);
        // y = distance * Mathf.Sin(quests[0].Elevation * Mathf.PI / 180);
        // z = distance * Mathf.Cos(quests[0].Elevation * Mathf.PI / 180) * Mathf.Cos(quests[0].Azimuth * Mathf.PI / 180);

        // float anglex = 180 - Mathf.Acos(x / distance) * 180 / Mathf.PI;
        // float angley = 180 - quests[0].Azimuth * 180 / Mathf.PI;
        // float anglez = 180 - Mathf.Acos(z / distance) * 180 / Mathf.PI;
        Vector3 dir = transform.GetChild(0).position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        transform.GetChild(0).rotation = Quaternion.RotateTowards(lookRotation, Quaternion.LookRotation(Vector3.zero), 10f);
        Quaternion VD = Quaternion.Euler(180, 0, 0);
        // transform.GetChild(0).rotation = transform.GetChild(0).rotation + VD ;

        Sun.transform.eulerAngles = new Vector3(Sun.transform.eulerAngles.x + 180, Sun.transform.eulerAngles.y, Sun.transform.eulerAngles.z);
    }

    void Update()
    {

        if (selected)
        {
            // sun.transform.position = quests[0].SunPos;
            for (int i = 1; i < quests.Count; i++)
            {

                // transform.position = new Vector3(x, y, z);
                // sun.transform.position = quests[i].SunPos;
                transform.RotateAround(Vector3.zero, GetNormal(quests[i].SunPos, quests[i - 1].SunPos), 0.2f * Time.deltaTime);
                sunIntensity = quests[i].Elevation;
                // Debug.Log(sunIntensity);

                // float xx = Mathf.Cos(Mathf.PI / 2);//* Mathf.PI / 180);
                // Debug.Log(item.SunPos);
                // Debug.Log(y);
                // Debug.Log(z);
                // sun.transform.position = Vector3.MoveTowards(quests[i - 1].SunPos, quests[i].SunPos, Time.deltaTime * 8);

            }
        }


    }
    Vector3 GetNormal(Vector3 a, Vector3 b)
    {
        //Calculate the normal vector of palne included two consecutive points of sun path
        // Cross the vectors to get a perpendicular vector, then normalize it.
        return Vector3.Cross(a, b).normalized;
    }

    public void clickButton()
    {
        selected = !selected;
    }
}
