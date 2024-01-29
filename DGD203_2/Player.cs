using DGD203_2;
using System;

public class Player
{
    private int playerMaxHealth = 2;

    public string Name { get; private set; }
    public int Health { get; set; }
    public string _Npcname;

    public Inventory Inventory { get; private set; }
    public Game Game { get; set; }

    public Player(string name, List<Item> inventoryItems)
    {
        Name = name;
        Health = playerMaxHealth;
        Inventory = new Inventory();
        if (inventoryItems != null)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                Inventory.AddItem(inventoryItems[i]);
            }
        }
        else
        {
            return;
        }
    }

    public void TakeItem(Item item)
    {
        Inventory.AddItem(item);
    }

    public bool HasBomb(Item item)
    {
        if (Inventory.Items.Contains(item))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void DropItem(Item item)
    {
        if (Inventory.Items.Contains(item))
        {
            Inventory.Items.Remove(item);
        }
    }

    public void CheckInventory()
    {
        for (int i = 0; i < Inventory.Items.Count; i++)
        {
            Console.WriteLine($"You have a {Inventory.Items[i]}");
        }
    }
}