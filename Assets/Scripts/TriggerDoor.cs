using System;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void OggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.Play("DoorOpen", 0, 0.0f);
                gameObject.SetActive(false);
                Console.Write("Trigger succesful");
            }else if(closeTrigger){
                myDoor.Play("DoorClose", 0, 0.0f);
                gameObject.SetActive(false);
                Console.Write("Trigger succesful");

            }
        }
    }
}
//C:\Users\alexc\OneDrive\Documentos\GitHub\CrunchTime\Assets