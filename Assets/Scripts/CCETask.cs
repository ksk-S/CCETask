using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CCETask : MonoBehaviour
{
    public float restSec = 0.5f;
    public float soaSec = 0.033f;

    Arduino arduino;

    public enum Index
    {
        LB, RB, LO, RO
    }

    public List<Light> lights = new List<Light>();
    public List<Material> materials = new List<Material>();

    public static Stopwatch sw;

    bool button_pressed;
    public KeyCode pressedKey;

    public long lightOnset;
    public long vibOnset;
    public long keyOnset;

    // Start is called before the first frame update
    void Awake()
    {
        sw = new Stopwatch();

        arduino = GetComponent<Arduino>();

        lights.Add(GameObject.Find("LEDLight_LB").GetComponent<Light>());
        lights.Add(GameObject.Find("LEDLight_RB").GetComponent<Light>());
        lights.Add(GameObject.Find("LEDLight_LO").GetComponent<Light>());
        lights.Add(GameObject.Find("LEDLight_RO").GetComponent<Light>());

        materials.Add(GameObject.Find("LED_LB").GetComponent<Renderer>().material);
        materials.Add(GameObject.Find("LED_RB").GetComponent<Renderer>().material);
        materials.Add(GameObject.Find("LED_LO").GetComponent<Renderer>().material);
        materials.Add(GameObject.Find("LED_RO").GetComponent<Renderer>().material);
    }

    void Start()
    {
        ResetLED();
    }

    public IEnumerator runCCE(CCEData.Laterality laterality, CCEData.Congruency congruency, CCEData.Side side)
    {
        Index lightId = Index.LB;
        int vibId = 0;

        if (laterality == CCEData.Laterality.LEFT)
        {
            vibId = 0;
        }
        else
        {
            vibId = 1;
        }

        if (side == CCEData.Side.SAME)
        {
            if ((laterality == CCEData.Laterality.LEFT) ^ (congruency == CCEData.Congruency.CONGRUENT))
            {
                lightId = Index.RB;
            }
            else
            {
                lightId = Index.LB;
            }
        }
        else
        {
            if ((laterality == CCEData.Laterality.LEFT) ^ (congruency == CCEData.Congruency.CONGRUENT))
            {
                lightId = Index.RO;
            }
            else
            {
                lightId = Index.LO;
            }
        }
        yield return StartCoroutine(CoCCE(lightId, vibId));
    }

    IEnumerator CoCCE(Index lightId, int vibId)
    {
        sw.Reset(); sw.Start();

        yield return new WaitForSeconds(restSec);

        lightOnset = sw.ElapsedMilliseconds;

        TurnOn(lightId);

        yield return new WaitForSeconds(soaSec);

        vibOnset = sw.ElapsedMilliseconds;

        TurnOff(lightId);

        arduino.Vibrate(vibId);


        yield return StartCoroutine(WaitForKeysDown(KeyCode.LeftArrow, KeyCode.RightArrow));

        keyOnset = sw.ElapsedMilliseconds;


        UnityEngine.Debug.Log(lightOnset + " " + (vibOnset - lightOnset) + " " + (keyOnset - vibOnset) + " " + pressedKey);


        //get reaction time etc

    }

    public void ResetLED()
    {
        for (int i = 0; i < 4; i++)
        {
            lights[i].enabled = false;
        }
        for (int i = 0; i < 4; i++)
        {
            materials[i].DisableKeyword("_EMISSION");

        }
    }

    public void TurnOn(Index index)
    {
        lights[(int)index].enabled = true;
        materials[(int)index].EnableKeyword("_EMISSION");
    }

    public void TurnOff(Index index)
    {
        lights[(int)index].enabled = false;
        materials[(int)index].DisableKeyword("_EMISSION");
    }



    // Update is called once per frame
    void Update()
    {
        button_pressed = Input.anyKeyDown;


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOn(Index.LB);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOff(Index.LB);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
               runCCE(CCEData.Laterality.RIGHT, CCEData.Congruency.CONGRUENT, CCEData.Side.SAME);
        }
    }

    IEnumerator WaitForKeysDown(KeyCode keyCode1, KeyCode keyCode2)
    {
        while ((!Input.GetKey(keyCode1) && !Input.GetKey(keyCode2)) || !button_pressed)
        {
            yield return new WaitForEndOfFrame();
        }
        if (Input.GetKey(keyCode1))
        {
            pressedKey = keyCode1;
        }
        if (Input.GetKey(keyCode2))
        {
            pressedKey = keyCode2;
        }
        yield return 0;
    }


}
