using UnityEngine;

public class TimeParserForPlayfab
{

    public int ParseTime(float time)
    {
        int timeModified = 0;

        timeModified = (int)(time * 1000); //Multiplicamos por un numero alto para no perder tanta precision del float
        timeModified = -timeModified; //Invertimos el valor para que el orden este al reves en la base de datos.
            
        return timeModified;
    }

}
