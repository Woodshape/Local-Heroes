using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LH.Data;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private List<Item> _items;
    [SerializeField]
    private List<Item> _equippedItems;
    [SerializeField]
    private List<Item> _consumables;

    public void EquipItem(IEquippable item) {
        item.EquipItem();

        try {
            Item i = (Item) item;
            _equippedItems.Add(i);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public void TakeHealthPotion(Entity entity) {
        Debug.Log("Taking health potion...");
        int index = -1;
        foreach (Item item in _consumables) {
            if (item is Potion potion) {
                index++;
            }
        }

        if (index != -1) {
            ConsumeItem(entity, (Potion)_consumables.ElementAt(index));
        }
    }

    public void ConsumeItem(Entity consumer, IConsumable consumable) {
        if (consumable.Consume(consumer)) {
            RemoveConsumable(consumable);
        }
    }

    public void AddConsumable(IConsumable consumable) {
        try {
            Item i = (Item) consumable;
            _consumables.Add(i);
            
            Debug.Log($"Consumable {consumable} added to inventory!");
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void RemoveConsumable(IConsumable consumable) {
        try {
            Item i = (Item) consumable;
            _consumables.Remove(i);

            Debug.Log($"Consumable {consumable} removed from inventory!");
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool HasPotionOfType(PotionType type) {
        foreach (Item item in _consumables) {
            if (item is Potion && ((Potion) item).type == type) {
                return true;
            }
        }

        return false;
    }

    public List<Item> GetEquippedItems() {
        return _equippedItems;
    }

    public List<Item> GetConsumables() {
        return _consumables;
    }
}
