# On-the-Visualization-of-Spatiotemporal-Urban-Data

## Theory

In science, an inverse-square law is any scientific law stating that a specified physical quantity is inversely proportional to the square of the distance from the source of that physical quantity. In mathematical notation the inverse square law can be expressed as an intensity (I) varying as a function of distance (d) from some centre. The intensity is proportional to the multiplicative inverse of the square of the distance thus:
 
$$ intensity \propto \frac{1}{distance^2} $$

It can also be mathematically expressed as:
\begin{equation}
    \frac{intensity_1}{intensity_2} = \frac{distance_1^2}{distance_2^2} 
\end{equation}
% In real world, lighting attenuation is governed by the inverse-square law: 
% \begin{equation}
%     \frac{1}{x . x} 
% \end{equation}
To show this property, imagine a point light, like \texttt{S} in Fig. 1, while \texttt{r} represents the measured points. The lines represent the flux emitting from the sources and fluxes. The total number of flux lines depends on the strength of the light source and is constant with increasing distance, where a greater density of flux lines (lines per unit area) means a stronger energy field. The density of flux lines is inversely proportional to the square of the distance from the source because the surface area of a sphere increases with the square of the radius. Thus, the intensity of light from a point source (energy per unit of area perpendicular to the source) is inversely proportional to the square of the distance from the source, so an object twice as far away receives only one-quarter the energy (in the same time period).
\begin{figure}[H]
    \centering
    \includegraphics[width=0.5\textwidth]{Images/pointLight.png}
    \caption[Caption used in list of tables]{how a point light propagates}
    \label{fig:flow around cylinder}
    \source{https://en.wikipedia.org/wiki/Inverse-square_law}
\end{figure}
However, in computer graphics things are a little problem. As distance approaches zero, lighting intensity approaches infinity. To address this issue,there are several way but \texttt{Standard Unity rendering pipeline} uses a \texttt{fake falloff curve} derived from the following equation:
 \begin{equation}
    \frac{1}{((\frac{x}{r}) . 5)^2 + 1 } 
\end{equation}
which \texttt{r} is \texttt{Range parameter}. It does in fact resemble the inverse square curve, except fixed at distance of 5 and then scaled based on Range parameter.
Since it is supposed to show \texttt{heatmap} on a specific point in the scene, we can use the main equations, 1 and 2, because we do not need to compute the light intensity near the light source.
\begin{figure}[H]
    \centering
    \includegraphics[width=0.5\textwidth]{Images/sunPosition.png}
    \caption[Caption used in list of tables]{Sun position}
    \label{fig:flow around cylinder}
    % \source{https://en.wikipedia.org/wiki/Inverse-square_law}
\end{figure}

To implement sun rising and sun setting for \texttt{Alesund}, a \texttt{directional lighting} element has been used. To calculate the exact position of \texttt{sun} for a city, we need three parameters, including \texttt{GPS coordinate}, \texttt{time zone} and \texttt{time}. Based on these parameters, two angles shown the position of sun at that specific time is derived. These angles are called \texttt{azimuth} and \texttt{elevation}, Fig. 2. The position of sun for \texttt{Alesund} on 12 April 2022 was extracted from \textit{www.SunEarthTools.com} website, Fig. 3.  

\begin{figure}[H]
    \centering
    \includegraphics[width=0.5\textwidth]{Images/Untitled.png}
    \caption[Caption used in list of tables]{Azimuth and elevation angles for Alesund on 12 April 2022}
    \label{fig:flow around cylinder}
    \source{https://www.SunEarthTools.com}
\end{figure}

In this part, the Cartesian coordinate of sun can be calculated as
\begin{equation}
    X = R \ . \ Cos ( Elevation) \ . \ Sin(Azimuth)
\end{equation}
\begin{equation}
    Y = R \ . \ Sin \ (Elevation)
\end{equation}
\begin{equation}
    Z = R \ . \ Cos ( Elevation) \ . \ Cos (Azimuth)
\end{equation}

\texttt{R} is the distance of light source from the point. A large number is selected for this parameter. It is worth noting that the intensity of sun in any point changes related to \texttt{Elevation} angle. For instance, when this angle are 90 degree, the intensity of sun is in its maximum value. Thus, the amount of sun intensity in any point can be calculated as follow
\begin{equation}
    {Sun \ Intensity \ in \ any \ point  = Sun\ Intensity. \ Sin ( Elevation) \ }
\end{equation}
% The surface of the Sun has a temperature of about 5,800 Kelvin (about 5,500 degrees Celsius, or about 10,000 degrees Fahrenheit). At that temperature, most of the energy the Sun radiates is visible and near-infrared light. At Earth’s average distance from the Sun (about 150 million kilometers), the average intensity of solar energy reaching the top of the atmosphere directly facing the Sun is about 1,360 watts per square meter, according to measurements made by the most recent NASA satellite missions. This amount of power is known as the total solar irradiance. (Before scientists discovered that it varies by a small amount during the sunspot cycle, total solar irradiance was sometimes called “the solar constant.”)

% A watt is measurement of power, or the amount of energy that something generates or uses over time. How much power is 1,360 watts? An incandescent light bulb uses anywhere from 40 to 100 watts. A microwave uses about 1000 watts. If for just one hour, you could capture and re-use all the solar energy arriving over a single square meter at the top of the atmosphere directly facing the Sun—an area no wider than an adult’s outstretched arm span—you would have enough to run a refrigerator all day.
To find the accumulated effect of \texttt{spot} lights, \texttt{Raycast} function is applied to show whether the light sources reach to the points or not. From each point of \texttt{Heatmap}, several rays cast towards light sources. Based on the distance between each point and the sources, Eq. 1, the light intensity is calculated for each point. It in worth noting that \texttt{maximum} and \texttt{minimum} values of each light in \texttt{Heatmap} is calculated as well to normalize the values of that light intensity. In other words, light intensity in each point can be calculated as   
\begin{equation}
    Intensity\ in\ a\ point = \frac{(\frac{intensity_{1}}{x_{1}^2}+\frac{intensity_{2}}{x_{2}^2}+...)- intensity_{min}}{intensity_{max}-intensity_{min}} 
\end{equation}

n= 1, 2, ..., n show the number of light and min and max illustrate the maximum and minimum values of accumulated light intensity of points used for normalizing values. The code is written for this part is shown in the following figure.    
\begin{lstlisting}[style=htmlcssjs]
foreach (barList item in cylinderGroup)
        {

            // ################### Light Intensity #################

            foreach (Quest mylight in StreetLight.listOfLights)
            {
                RaycastHit hit2;
                if (Physics.Raycast(item.myCylinderLoc, mylight.lightSourceLoc, out hit2, 1000f))
                {
                    float distance = (mylight.lightSourceLoc - item.myCylinderLoc).magnitude;
                    item.myLightsValue.Add(mylight.lightIntensity1 / (distance * distance));

                }

            }
            //Finding maximum and minimum values of accumulated light intensity in Heatmap
            if (item.myLightsValue.Sum() > MaxLights)
            {
                MaxLights = item.myLightsValue.Sum();
            }
            else if (item.myLightsValue.Sum() < MinLights)
            {
                MinLights = item.myLightsValue.Sum();
            }

        }


        //######## Normalizing accumulated lights intensity ##############
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
                }
                else
                {
                    item.myOverallIntensity = 0;
                }

            }

            // ###################### Sun Intensity #################### 
            if (Physics.Raycast(item.myCylinderLoc, sun.transform.position, out hit, 1000f))
            {
                if (InputMode.value == 0)
                {
                    Debug.DrawRay(item.myCylinder.transform.position, sun.transform.position - item.myCylinder.transform.position, Color.blue);
                }
                item.mySunIntensity = Mathf.Abs(Mathf.Sin(mySun.transform.rotation.y));

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
    
\end{lstlisting}


\section{Result}
The overall view of city simulator is shown in Fig. 4. There is a floating side bar designed to add and modify lights. As can be seen, two \texttt{drop down} menus are located in the side bar, the first one, \texttt{Mode}, is used to select between four modes, \texttt{Add Light/Camera}, \texttt{Edit Light}, \texttt{Edit Camera} and \texttt{Heatmap}, Fig. 5 (Right). There are four different types of street lights and CCTV in the second menu, \texttt{Type} menu, which can be located in the city simulator, Fig. 5 (left). Furthermore, There are two buttons for importing and exporting data as a \texttt{CSV} file.
\begin{figure}[H]
    \centering
    \includegraphics[width=1\textwidth]{Assignment 2/Images/Fig1.jpg}
    \caption{Overall view of city simulator}
    \label{fig:flow around cylinder}
    
\end{figure}

\begin{figure}[H]
    \centering
    \includegraphics[width=0.6\textwidth]{Assignment 2/Images/Fig5.jpg}
    \caption{\texttt{Mode} and \texttt{Type} menus}
    \label{fig:flow around cylinder}

\end{figure}
After adding the lights by clicking on any point we want, Fig. 7,  or importing them from \texttt{CSV} file, the features of each light can be modified. For doing this, \texttt{Edit Light} mode must be selected from \texttt{Mode} menu. In this mode, some input fields related to the lights are shown, Fig. 6. Then, we can select a light and change the features of \texttt{Rotation}, \texttt{Intensity}, \texttt{Spot Angle}, \texttt{Range} and \texttt{Color}, Fig. 8.
\begin{figure}[H]
    \centering
    \includegraphics[width=1\textwidth]{Assignment 2/Images/Fig2.jpg}
    \caption[Overall view of city simulator in \texttt{Edit} mode]{Overall view of city simulator in \texttt{Edit} mode}
    \label{fig:flow around cylinder}
    
\end{figure}

\begin{figure}[H]
    \centering
    \includegraphics[width=1\textwidth]{Assignment 2/Images/Fig3.jpg}
    \caption[Overall view of city simulator]{Overall view of city simulator}
    \label{fig:flow around cylinder}
    
\end{figure}


\begin{figure}[H]
    \centering
    \includegraphics[width=1\textwidth]{Assignment 2/Images/Fig4.jpg}
    \caption{How to modify the features of the selected light}
    \label{fig:flow around cylinder}
    
\end{figure}
Figure 9 illustrate the heat map in a selected point. Given the sun and light intensity, the heat map can be calculated. As can be seen in the bottom of floating side bar, the dimensions of heat map can be changed to cover a bigger area.
\begin{figure}[H]
    \centering
    \includegraphics[width=1\textwidth]{Assignment 2/Images/Heatmap.png}
    \caption{Heatmap}
    \label{fig:flow around cylinder}

\end{figure}

The effect of light intensity on heat maps is show in Fig. 10. It can be seen that points with shorter distances from both street lights receive higher intensity compared to points with more distance.
\begin{figure}[H]
    \centering
    \includegraphics[width=1\textwidth]{Assignment 2/Images/Heatmap_light.png}
    \caption{Heatmap of street lights}
    \label{fig:flow around cylinder}

\end{figure}
