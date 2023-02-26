using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Heatmap : MonoBehaviour
{
    public Dropdown modeDropdownMenu;
    public InputField InputX;
    public InputField InputZ;

    public Dropdown InputMode;
    public GameObject Cylinder;

    public GameObject mySun;
    public GameObject sun;
    // public static float aa;

    // Start is called before the first frame update
    public class barList
    {
        public GameObject myCylinder;
        public Vector3 myCylinderLoc;
        public float mySunIntensity;
        public float myLightIntensity;
        public float myOverallIntensity;
        public Color myColor;
        public List<float> myLightsValue = new List<float>();

    }
    public static List<barList> cylinderGroup = new List<barList>();


    // Update is called once per frame
    public void Update()
    {
        float MaxLights = 0.0f;
        float MinLights = Mathf.Infinity;

        // float sumIntensity = 0f;

        // foreach (barList item in cylinderGroup)
        // {
        //     item.myLightsValue.Clear();

        // }



        //########################Instantiate Heat Map ##########################
        if ((Input.GetMouseButtonDown(0)) && (modeDropdownMenu.value == 4))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            RaycastHit hitmesh;


            if (Physics.Raycast(ray, out hit, 100))
            {

                Vector3 org;
                int x, z;



                x = 3;
                z = 3;
                for (int i = -4 * (x - 1); i <= 4 * (x - 1); i++)
                {
                    for (int j = -4 * (z - 1); j <= 4 * (z - 1); j++)
                    {
                        org = new Vector3(hit.point.x + (float)i * 0.125f, hit.point.y + 20, hit.point.z + (float)j * 0.125f); //The origine of Raycasting
                        if (Physics.Raycast(org, Vector3.down, out hitmesh, 1000f))
                        {
                            barList instantiatedcylinder = new barList();
                            instantiatedcylinder.myCylinderLoc = hitmesh.point;
                            instantiatedcylinder.myCylinder = Instantiate(Cylinder, hitmesh.point, Quaternion.identity);
                            cylinderGroup.Add(instantiatedcylinder);

                        }

                    }

                }

            }
            else
            {
                Debug.Log("not found");
            }

        }
        // Debug.Log(StreetLight.listOfLights.Count);
        //#####################Setting the property of each node of heatmap#######################
        foreach (barList item in cylinderGroup)
        {

            // ############################ Light Intensity ############################

            foreach (Quest mylight in StreetLight.listOfLights)
            {
                RaycastHit hit2;
                if (Physics.Raycast(item.myCylinderLoc, mylight.lightSourceLoc, out hit2, 1000f))
                {
                    float distance = (mylight.lightSourceLoc - item.myCylinderLoc).magnitude;
                    item.myLightsValue.Add(mylight.lightIntensity1 / (distance * distance));

                }

            }
            //Finding accumulated light intensity in each point
            if (item.myLightsValue.Sum() > MaxLights)
            {
                MaxLights = item.myLightsValue.Sum();
            }
            else if (item.myLightsValue.Sum() < MinLights)
            {
                MinLights = item.myLightsValue.Sum();
            }

        }


        //######################## accumulated lights intensity ########################
        foreach (barList item in cylinderGroup)
        {
            RaycastHit hit;

            if (InputMode.value == 1)
            {
                if (StreetLight.listOfLights.Any())
                {

                    foreach (Quest mylight in StreetLight.listOfLights)
                    {
                        Debug.DrawRay(item.myCylinder.transform.position, mylight.lightSourceLoc - item.myCylinderLoc, Color.green);
                    }

                    item.myOverallIntensity = (item.myLightsValue.Sum() - MinLights) / (MaxLights - MinLights);
                    // Debug.Log(item.myOverallIntensity);
                }
                else
                {
                    item.myOverallIntensity = 0;
                    // Debug.Log(item.myOverallIntensity);
                }

            }




            // ######################### Sun Intensity ######################## 
            if (Physics.Raycast(item.myCylinderLoc, sun.transform.position, out hit, 1000f))// & (sun.transform.position.y > 0))
            {
                if (InputMode.value == 0)
                {
                    Debug.DrawRay(item.myCylinder.transform.position, sun.transform.position - item.myCylinder.transform.position, Color.blue);
                }
                item.mySunIntensity = Mathf.Abs(Mathf.Sin(mySun.transform.rotation.y));// between 0 and 1 //sun.GetComponent<Light>().intensity *

                // Debug.Log(item.mySunIntensity);
            }
            else
            {
                if (InputMode.value == 0)
                {
                    item.mySunIntensity = 0;
                    Debug.DrawRay(item.myCylinder.transform.position, sun.transform.position - item.myCylinder.transform.position, Color.red);
                }
            }



            //Change the color of Heatmap
            if (InputMode.value == 0)
            {

                item.myOverallIntensity = item.mySunIntensity;
                // Debug.Log(item.myOverallIntensity);

                // Change the color of heat map based on light intensity in an point 
                item.myColor = new Color(1 - item.myOverallIntensity, 0, item.myOverallIntensity);
                item.myCylinder.GetComponent<MeshRenderer>().material.color = item.myColor;
            }
            else
            {

                item.myColor = new Color(item.myOverallIntensity, 1 - item.myOverallIntensity, 0);
                item.myCylinder.GetComponent<MeshRenderer>().material.color = item.myColor;
            }




        }
    }

}