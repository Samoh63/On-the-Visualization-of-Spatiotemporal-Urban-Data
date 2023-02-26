using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Map;

using Mapbox.Examples;
using Mapbox.Unity.MeshGeneration.Modifiers;


public class StreetLight : MonoBehaviour
{
    public static List<Quest> listOfLights = new List<Quest>();
    public static List<Quest> listOfCameras = new List<Quest>();
    public GameObject[] prefabs;
    public Dropdown modeDropdownMenu;
    public Dropdown typeDropdownMenu;
    public GameObject myImageLight;
    public GameObject myImageCamera;
    public InputField[] inputIntensity;
    public InputField[] myColor;

    [SerializeField]
    AbstractMap _map;
    GameObject selectedLight;

    GameObject selectedCamera;

    public Camera mainCam;

    // Update is called once per frame
    void start()
    {
        myImageLight.SetActive(false);
        myImageCamera.SetActive(false);

    }
    void Update()
    {

        // GameObject FoundObject;

        if (modeDropdownMenu.value == 1)
        {
            myImageCamera.SetActive(false);
            myImageCamera.SetActive(false);
            if (Input.GetMouseButtonDown(0)) //&& typeDropdownMenu.value == 2)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;


                if (Physics.Raycast(ray, out hit, 100))
                {
                    Quest q = new Quest();
                    // Debug.Log("hit: " + hit.point);
                    // int prefabs
                    GameObject instantiatedObject = Instantiate(prefabs[typeDropdownMenu.value], hit.point, Quaternion.identity) as GameObject;
                    // q.position =
                    Vector2d GPS = _map.WorldToGeoPosition(hit.point);

                    if (typeDropdownMenu.value != 4)
                    {
                        q.lightType = instantiatedObject.name;
                        q.GPSLatitude = GPS[0];
                        q.GPSLongitude = GPS[1];
                        q.lightColor = ColorUtility.ToHtmlStringRGB(instantiatedObject.transform.GetChild(1).GetComponent<Light>().color);
                        q.lightIntensity1 = instantiatedObject.transform.GetChild(1).GetComponent<Light>().intensity;
                        q.lightSourceLoc = instantiatedObject.transform.GetChild(1).position;
                        // Debug.Log(q.lightSourceLoc);
                        listOfLights.Add(q);
                    }
                    else
                    {
                        q.lightType = instantiatedObject.name;
                        q.GPSLatitude = GPS[0];
                        q.GPSLongitude = GPS[1];
                        listOfCameras.Add(q);

                    }
                }
                else
                {
                    Debug.Log("not found");
                }



            }



        }
        else if (modeDropdownMenu.value == 2)
        {
            myImageLight.SetActive(true);
            myImageCamera.SetActive(false);
            if (Input.GetMouseButtonDown(0)) //&& typeDropdownMenu.value == 2)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                GameObject FoundLight;



                if (Physics.Raycast(ray, out hit, 100))
                {
                    FoundLight = hit.transform.gameObject;
                    if ((FoundLight.name == "StreetLamp2_Tall (Black)(Clone)") || (FoundLight.name == "StreetLamp2_TallDouble (Black)(Clone)") ||
                        (FoundLight.name == "StreetLamp2_Short (Black)(Clone)") || (FoundLight.name == "StreetLamp2_ShortDouble (Black)(Clone)"))
                    {
                        selectedLight = FoundLight;
                        Debug.Log(selectedLight.name);

                    }
                }
            }

        }
        else if (modeDropdownMenu.value == 3)
        {
            myImageLight.SetActive(false);
            myImageCamera.SetActive(true);
            if (Input.GetMouseButtonDown(0)) //&& typeDropdownMenu.value == 2)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                GameObject FoundCamera;



                if (Physics.Raycast(ray, out hit, 100))
                {
                    FoundCamera = hit.transform.gameObject;
                    if (FoundCamera.name == "CCTV(Clone)")
                    {
                        selectedCamera = FoundCamera;
                        Debug.Log(selectedCamera.transform);

                    }
                }
            }
        }
        else
        {
            myImageLight.SetActive(false);
            myImageCamera.SetActive(false);
        }
    }


    public void modifyClick()
    {
        if (modeDropdownMenu.value == 2)
        {

            // Debug.Log(selectedLight.name);
            if (inputIntensity[0].text != "")
            {
                // transform.RotateAround(selectedLight.transform, transform.up, Time.deltaTime * 90f);
                selectedLight.transform.rotation = Quaternion.Euler(0.0f, float.Parse(inputIntensity[0].text), 0.0f);
            }
            if (inputIntensity[1].text != "")
            {
                selectedLight.transform.GetChild(1).GetComponent<Light>().intensity = float.Parse(inputIntensity[1].text);
            }
            if (inputIntensity[2].text != "")
            {
                selectedLight.transform.GetChild(1).GetComponent<Light>().spotAngle = float.Parse(inputIntensity[2].text);
            }
            if (inputIntensity[3].text != "")
            {
                selectedLight.transform.GetChild(1).GetComponent<Light>().range = float.Parse(inputIntensity[3].text);
            }
            if ((myColor[0].text != "") && (myColor[1].text != "") && (myColor[2].text != ""))
            {
                selectedLight.transform.GetChild(1).GetComponent<Light>().color = new Color(float.Parse(myColor[0].text), float.Parse(myColor[1].text), float.Parse(myColor[2].text));
                // Debug.Log(selectedLight.transform.GetChild(1).GetComponent<Light>().color);
            }


        }
        else if (modeDropdownMenu.value == 3)
        {
            if (inputIntensity[4].text != "")
            {

                selectedCamera.transform.rotation = Quaternion.Euler(0.0f, float.Parse(inputIntensity[4].text), 0.0f);
                Debug.Log(selectedCamera.transform.GetChild(1).name);
            }
        }
    }

    public void selectView()
    {

        mainCam.enabled = false;
        selectedCamera.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);

    }

    public void mainCamera()
    {
        selectedCamera.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);

        mainCam.enabled = true;



    }

}


