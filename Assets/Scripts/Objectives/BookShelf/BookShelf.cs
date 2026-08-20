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
        // Align starting books to initial slots
        UpdateBookPositions(currentBooks);

    }

    // we create new book list 
    //we sort bookIds 
    // genrate random sequnce out of it  like suffle shit 

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
        string solution = "solution is ";

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
            Debug.Log(temp);
            int randomNum = Random.Range(i, currentBooks.Count);
            Debug.Log(randomNum);
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



    /// <summary>
    /// Called when the player picks up a book off the shelf.
    /// </summary>
    public void OnBookPickedUp(Book pickedBook)
    {
        if (currentBooks.Contains(pickedBook))
        {
            currentBooks.Remove(pickedBook);
            lastHoverIndex = -1;

            // Shift remaining books left to fill the gap
            UpdateBookPositions(currentBooks);
        }
    }

    /// <summary>
    /// Called every frame while hovering a held book over the shelf.
    /// Opens up a physical gap dynamically right under the player's view/cursor.
    /// </summary>
    public void OnBookHover(Vector3 hoverWorldPosition)
    {
        int hoverIndex = GetClosestSlotIndex(hoverWorldPosition);

        // Avoid re-calculating if hovering over the same slot
        if (hoverIndex == lastHoverIndex) return;
        lastHoverIndex = hoverIndex;

        // Build a temporary list with a blank gap inserted at hoverIndex
        List<Book> previewList = new List<Book>(currentBooks);

        // Clamp index to valid bounds
        hoverIndex = Mathf.Clamp(hoverIndex, 0, previewList.Count);
        previewList.Insert(hoverIndex, null); // null represents the empty gap

        // Move books according to the preview gap
        UpdateBookPositions(previewList);
    }

    /// <summary>
    /// Called when the player drops/places the held book back onto the shelf.
    /// </summary>
    public void OnBookDropped(Book book, Vector3 dropPosition)
    {
        int dropIndex = GetClosestSlotIndex(dropPosition);
        dropIndex = Mathf.Clamp(dropIndex, 0, currentBooks.Count);

        // Insert into real list
        currentBooks.Insert(dropIndex, book);
        lastHoverIndex = -1;

        // Lock all books to their final slot assignments
        UpdateBookPositions(currentBooks);

        //cheking passward after 
        CheackPassward();
    }

    /// <summary>
    /// Smoothly assigns target slot transforms to books according to an ordering list.
    /// </summary>
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

    /// <summary>
    /// Finds which slot index is closest to the given world point.
    /// </summary>
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






}