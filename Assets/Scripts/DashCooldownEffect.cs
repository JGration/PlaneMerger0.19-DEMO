using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldownEffect : MonoBehaviour {

    private bool isrunning = false;

    void Update () {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().shadowPower)
            Activate();
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().shadowPower && !isrunning)
            Disable();

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().emitDashCooldown)
        {
            StartCoroutine(DashCD());
        }
            
    }
    public void Disable()
    {
        GetComponent<PSMeshRendererUpdater>().IsActive = false;
    }
    public void Activate()
    {
        GetComponent<PSMeshRendererUpdater>().IsActive = true;
    }
    IEnumerator DashCD()
    {
        isrunning = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().emitDashCooldown = false;
        yield return new WaitForSeconds(3.5f);
        GetComponent<PSMeshRendererUpdater>().IsActive = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<PSMeshRendererUpdater>().IsActive = false;
        isrunning = false;
        yield return 0;
    }
}
