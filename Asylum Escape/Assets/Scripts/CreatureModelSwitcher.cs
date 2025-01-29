using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureModelSwitcher : MonoBehaviour
{
    public Transform model1;
    public Transform model2;
    public Transform model3;
    public Transform monster;

    private void Start()
    {
        if(monster != null)
        {
            var meshRenderer = monster.GetComponent<MeshRenderer>();
            if(meshRenderer != null )
            {
                meshRenderer.enabled = false;
            }
        }
        SwitchModel(1);
    }
    public void SwitchModel(int modelIndex)
    {

        model1.gameObject.SetActive(false);
        model2.gameObject.SetActive(false);
        model3.gameObject.SetActive(false);

        if (modelIndex == 1 && model1 != null)
        {
            model1.gameObject.SetActive(true);
            SyncWithCapsule(model1);
        } else if (modelIndex == 2 && model2 != null)
        {
            model2.gameObject.SetActive(true);
            SyncWithCapsule(model2);
        } else if (modelIndex == 1 && model2 != null)
        {
            model2.gameObject.SetActive(true);
            SyncWithCapsule(model2);
        }

        if (modelIndex == 1) model1.gameObject.SetActive(true);
        if (modelIndex == 2) model2.gameObject.SetActive(true);
        if (modelIndex == 3) model3.gameObject.SetActive(true);
    }

    private void SyncWithCapsule(Transform model)
    {
        if (monster != null && model != null)
        {
            model.position = monster.transform.position;
            model.rotation = monster.transform.rotation;
        }
    }
}
