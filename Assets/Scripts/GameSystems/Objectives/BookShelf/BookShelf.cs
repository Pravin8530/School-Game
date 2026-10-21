using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour
{
    [Header("Shelf Setup")]
    public Transform[] bookSlots;
    public List<Book> currentBooks = new List<Book>();

    [Header("Passward relarted shit")]
    public List<Book> currentBookSequence;
    private bool isSolved = false;


    private int lastHoverIndex = -1;

    private void Start()
    {
        ShuffleBooks();
        GenrateRandomPassward();
        UpdateBookPositions(currentBooks);

    }

   

    private void GenrateRandomPassward()
    {
        currentBookSequence = new List<Book>(currentBooks);

        for (int i = 0; i < currentBookSequence.Count; i++)
        {
            Book temp = currentBookSequence[i];
            int randomNum = Random.Range(i, currentBookSequence.Count);

            currentBookSequence[i] = currentBookSequence[randomNum];
            currentBookSequence[randomNum] = temp;


        }
        string solution = "solution is " ;

        foreach (Book book in currentBookSequence)
        {
            solution += book.bookColor + "-> ";
        }
        Debug.Log(solution);
    }


    public void ShuffleBooks()
    {
        for (int i = 0; i < currentBooks.Count; i++)
        {

            Book temp = currentBooks[i];
            int randomNum = Random.Range(i, currentBooks.Count);
            currentBooks[i] = currentBooks[randomNum];
            currentBooks[randomNum] = temp;


        }

    }
    
    private void CheackPassward()
    {
        if (isSolved)
            return;

        for (int i = 0; i < currentBooks.Count; i++)
        {
            if (currentBooks[i] != currentBookSequence[i])
                return;

        }

        isSolved = true;
        Debug.Log("ye We won");


    }


    
    public void OnBookPickedUp(Book pickedBook)
    {
        if (currentBooks.Contains(pickedBook))
        {
            currentBooks.Remove(pickedBook);
            lastHoverIndex = -1;

            UpdateBookPositions(currentBooks);
        }
    }

   
    public void OnBookHover(Vector3 hoverWorldPosition)
    {
        int hoverIndex = GetClosestSlotIndex(hoverWorldPosition);

        if (hoverIndex == lastHoverIndex) return;
        lastHoverIndex = hoverIndex;

        
        List<Book> previewList = new List<Book>(currentBooks);

        hoverIndex = Mathf.Clamp(hoverIndex, 0, previewList.Count);
        previewList.Insert(hoverIndex, null);

        UpdateBookPositions(previewList);
    }

    
    public void OnBookDropped(Book book, Vector3 dropPosition)
    {
        int dropIndex = GetClosestSlotIndex(dropPosition);
        dropIndex = Mathf.Clamp(dropIndex, 0, currentBooks.Count);

        currentBooks.Insert(dropIndex, book);
        lastHoverIndex = -1;

        UpdateBookPositions(currentBooks);

        CheackPassward();
    }

    
    private void UpdateBookPositions(List<Book> bookList)
    {
        for (int i = 0; i < bookList.Count; i++)
        {
            if (bookList[i] != null && i < bookSlots.Length)
            {
                bookList[i].SetTargetSlot(bookSlots[i]);
            }
        }
    }

   
    private int GetClosestSlotIndex(Vector3 worldPoint)
    {
        int closestIndex = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < bookSlots.Length; i++)
        {
            float dist = Vector3.Distance(worldPoint, bookSlots[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }


    public Vector3 ClampPositionToShelf(Vector3 targetPos)
    {
        if (bookSlots == null || bookSlots.Length == 0) return targetPos;

        Transform firstSlot = bookSlots[0];
        Transform lastSlot = bookSlots[bookSlots.Length - 1];

        Vector3 start = firstSlot.position;
        Vector3 end = lastSlot.position;

        Vector3 shelfDir = (end - start);
        float shelfLength = shelfDir.magnitude;
        shelfDir.Normalize();

        Vector3 localForward = transform.forward;
        Vector3 depthDir = (localForward - Vector3.Dot(localForward, shelfDir) * shelfDir).normalized;
        if (depthDir == Vector3.zero) depthDir = Vector3.forward;

        Vector3 toTarget = targetPos - start;
        float horizontalDist = Vector3.Dot(toTarget, shelfDir);
        float depthDist = Vector3.Dot(toTarget, depthDir);

        float clampedHorizontal = Mathf.Clamp(horizontalDist, 0f, shelfLength);

        float clampedDepth = Mathf.Clamp(depthDist, 0f, 1.2f);

        if (horizontalDist < 0f || horizontalDist > shelfLength)
        {
            clampedDepth = Mathf.Min(clampedDepth, 0.5f); 
        }

        Vector3 finalPos = start + (shelfDir * clampedHorizontal) + (depthDir * clampedDepth);
        finalPos.y = targetPos.y;

        return finalPos;
    }


}