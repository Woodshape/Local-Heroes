﻿using System.Collections.Generic;
using UnityEngine;

namespace LH.GOAP
{
    public class GInventory
    {
        // Store our items in a List
        public List<GameObject> items = new List<GameObject>();

        // Method to add items to our list
        public void AddItem(GameObject i) {
            Debug.Log("Adding item to inventory: " + i.name);
            items.Add(i);
        }

        // Method to search for a particular item
        public GameObject FindItemWithTag(string tag) {
            // Iterate through all the items
            foreach (GameObject i in items) {
                if (i == null) {
                    break;
                }
            
                // Found a match
                if (i.CompareTag(tag)) {
                    return i;
                }
            }
            // Nothing found
            return null;
        }

        // Remove an item from our list
        public void RemoveItem(GameObject i) {

            int indexToRemove = -1;
            // Search through the list to see if it exists
            foreach (GameObject g in items) {
                // Initially set indexToRemove to 0. The first item in the List
                indexToRemove++;
                // Have we found it?
                if (g == i) {
                    break;
                }
            }
            // Do we have something to remove?
            if (indexToRemove != -1) {
                Debug.Log("Removing item from inventory: " + i.name);
                // Yes we do.  So remove the item at indexToRemove
                items.RemoveAt(indexToRemove);
            }
        }
    }
}