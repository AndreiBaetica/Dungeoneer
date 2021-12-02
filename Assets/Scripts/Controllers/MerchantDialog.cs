using System;
using UnityEngine;

namespace Controllers
{
    public class MerchantDialog : MonoBehaviour, IInteractable
    {
        private GameObject merchantDialog;
        private bool showingDialog = false;
        private void Start()
        {
            merchantDialog = GameObject.Find("MerchantDialog");
        }

        public void Interact()
        {
            if (showingDialog)
            {
                merchantDialog.transform.position += new Vector3(0, 10, 0);
                showingDialog = false;
            }
            else
            {
                merchantDialog.transform.position -= new Vector3(0, 10, 0);
                showingDialog = true;
            }
        }

        public void StopInteract()
        {
        }
    }
}