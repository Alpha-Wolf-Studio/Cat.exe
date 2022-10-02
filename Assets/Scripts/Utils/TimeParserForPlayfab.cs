using UnityEngine;

public class TimeParserForPlayfab
{

    public int ParseGameTimeToDatabaseTime(float time)
    {
        int timeModified = 0;

        timeModified = (int)(time * 1000); //Multiplicamos por un numero alto para no perder tanta precision del float
        timeModified = -timeModified; //Invertimos el valor para que el orden este al reves en la base de datos.
            
        return timeModified;
    }

    public float ParseDatabaseTimeToGameTime(int time)
    {
        float timeModified = (float)(-time);
        timeModified /= 1000;
        return timeModified;
    }
    
}
