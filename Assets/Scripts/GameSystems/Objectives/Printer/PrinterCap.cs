using System.Collections;
using UnityEngine;

public class PrinterCap : MonoBehaviour, IInteractables
{
    public int Priority => 1;

    [SerializeField] private Transform pivot;
    [SerializeField] private Vector3 closedRotation;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float speed = 500f;

    [SerializeField] private Transform paperSlot;
    [SerializeField] private GameObject printedPaperPrefab;
    [SerializeField] private Transform printExitLocation;



    private bool isOpen;
    private bool isPrinting;

    private void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(isOpen ? openRotation : closedRotation);
        pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, targetRotation, speed * Time.deltaTime);
    }

    public void Interact(InventoryNew inventory)
    {
        if (isPrinting) return;
//----for picables
   if (inventory != null)
        {
            GameObject paperObj = inventory.HeldObject;

            if (paperObj != null)
            {
                WorldItem worldItemScript = paperObj.GetComponent<WorldItem>();
                if (worldItemScript == null)
                {
                    worldItemScript = paperObj.GetComponentInChildren<WorldItem>();
                }

                if (worldItemScript != null && worldItemScript.itemData != null && worldItemScript.itemData.itemName == "Paper")
                {
                    Paper paperScript = paperObj.GetComponent<Paper>();
                    if (paperScript == null)
                    {
                        paperScript = paperObj.GetComponentInChildren<Paper>();
                    }

                    if (paperScript == null)
                    {
                        paperScript = paperObj.AddComponent<Paper>();
                    }

                    inventory.RemoveHeldItem(paperObj);

                    paperObj.SetActive(true);
                    paperObj.transform.localScale = Vector3.one; 

                    Rigidbody rb = paperObj.GetComponent<Rigidbody>();
                    if (rb != null) Destroy(rb);

                    Collider col = paperObj.GetComponent<Collider>();
                    if (col != null) col.enabled = false;

                    StartCoroutine(FullyAutomatedPrintSequence(paperScript));
                    return; 
                }
            }
        }
//-----
        if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.HasPaper)
        {
            Paper heldPaper = PlayerStateManager.Instance.HeldPaper;

            if (heldPaper != null ){

                StartCoroutine(FullyAutomatedPrintSequence(heldPaper));
            }
        }
        else
        {
            isOpen = !isOpen;
        }

       
    }
       
    private IEnumerator FullyAutomatedPrintSequence(Paper placedPaper)
    {
        isPrinting = true;
        string identityName = placedPaper.GetPaperName();

        isOpen = true;

        yield return new WaitForSeconds(0.01f);

        placedPaper.transform.SetParent(paperSlot);
        placedPaper.transform.localPosition = Vector3.zero;
        placedPaper.transform.localRotation = Quaternion.identity;
        placedPaper.isOnPrinter = true;       // blocking it from picking again while printing
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);

        Debug.Log("Paper dropped into slot.");

        yield return new WaitForSeconds(0.8f);

        isOpen = false;
        Debug.Log("Printer Lid closing ");

        yield return new WaitForSeconds(0.8f);

        Debug.Log("Copying document...");
        yield return new WaitForSeconds(3.0f);

        Destroy(placedPaper.gameObject);

        isOpen = true;

        GameObject printedPage = Instantiate(printedPaperPrefab, printExitLocation.position, printExitLocation.rotation);
         Destroy(printedPage, 1.0f);                   

        Paper newPaperScript = printedPage.GetComponent<Paper>();
        if (newPaperScript != null)
        {
            Debug.Log($"Boom! {identityName} Printed!");
        }

        isPrinting = false;
    }
}
