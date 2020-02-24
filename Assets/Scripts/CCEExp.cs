using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCEExp : MonoBehaviour
{
    CCEData Data;
    CCETask Task;

    // Start is called before the first frame update
    void Awake()
    {
        Task = GetComponent<CCETask>();
        Data = GetComponent<CCEData>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(runExp());
        }

    }

    IEnumerator runExp()
    {
        Data.SetFilename(Data.data_dir, "CCE", Data.subId);
        Data.SaveHeaderToFile();

        yield return null;

        for (int trialId = 0; trialId < Data.resultList.Count; trialId++)
        {
            yield return new WaitForSeconds(1f);

            CCEData.Result param = Data.resultList[trialId];

            yield return StartCoroutine( Task.runCCE(param.laterality, param.congruency, param.side));

            param.SoA = Task.vibOnset - Task.lightOnset;
            param.rt = Task.keyOnset - Task.vibOnset;
            param.response = Task.pressedKey == KeyCode.LeftArrow ? 0 : 1;
            param.correct = !(Task.pressedKey == KeyCode.LeftArrow ^ param.laterality == CCEData.Laterality.LEFT);

            Data.SaveDataToFile(param);
        }

    }


    IEnumerator runTrial()
    {

        yield return null;
    }


}
