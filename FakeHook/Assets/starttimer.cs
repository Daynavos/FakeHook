using System;
using _Scripts;
using UnityEngine;

public class starttimer : MonoBehaviour
{
   public GameObject gameCOntroller;
   public GameObject tutorial;
   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.tag == "Player")
      {
         gameCOntroller.GetComponent<GameController>().timerStarted = true;
         tutorial.SetActive(false);
      }
   }
}
