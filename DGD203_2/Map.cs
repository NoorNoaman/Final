using DGD203_2;
using System.Numerics;

public class Map
{
    private Game _theGame;

    private Vector2 _coordinates;

    private int[] _widthBoundaries;
    private int[] _heightBoundaries;

    private Location[] _locations;
    public _NPC _npc;
    public Game Game;


    public Map(Game game, int width, int height)
    {
        _theGame = game;

        // Setting the width boundaries
        int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
        _widthBoundaries[1] = widthBoundary;

        // Setting the height boundaries
        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
        _heightBoundaries[0] = -heightBoundary;
        _heightBoundaries[1] = heightBoundary;

        // Setting starting coordinates
        _coordinates = new Vector2(0, 0);

        GenerateLocations();
    }

    #region Coordinates

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public void SetCoordinates(Vector2 newCoordinates)
    {
        _coordinates = newCoordinates;
    }

    #endregion

    #region Movement

    public void MovePlayer(int x, int y)
    {
        int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

        if (!CanMoveTo(newXCoordinate, newYCoordinate))
        {
            Console.WriteLine("Can't go that way");
            return;
        }

        _coordinates[0] = newXCoordinate;
        _coordinates[1] = newYCoordinate;

        CheckForLocation(_coordinates);
    }

    private bool CanMoveTo(int x, int y)
    {
        return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
    }

    #endregion

    #region Locations

    private void GenerateLocations()
    {
        _locations = new Location[5];

        Vector2 goblinLocation = new Vector2(-2, 2);
        Location goblin = new Location("Goblin's HideOut", "\nwhere he hides from justice and law", LocationType.Combat, goblinLocation);
        _locations[0] = goblin;

        Vector2 storeLocation = new Vector2(-1, -1);
        Location Store = new Location("Sherlock's ", "You walk in to find a sign that says he is in the street south west smoking his pipe, as usual", LocationType.Office, storeLocation);
        _locations[1] = Store ;

        Vector2 SherlockLocation = new Vector2(-2, -2);
        List<Item> sherlockItem = new List<Item>();
        sherlockItem.Add(Item.Bomb);
        Location Sherlock = new Location("\nSherlock\n", "busy puffing clouds, I think his arrogance will trouble our quest, I hope not...", LocationType.npc, SherlockLocation, sherlockItem);
        _locations[2] = Sherlock;

        Vector2 bakerStreetLocation = new Vector2(-1, 1);
        List<Item> streetItem = new List<Item>();
        streetItem.Add(Item.Charm);
        Location Baker_Street = new Location("Baker ", "The street that overheard every crime to ever exist,", LocationType.Street, bakerStreetLocation, streetItem);
        _locations[3] = Baker_Street;

        Vector2 mycroftLocation = new Vector2(1, -2);
        List<Item> mycroftItem = new List<Item>();
        mycroftItem.Add(Item.Rune);
        Location Mycroft = new Location("Mycroft's", "Surprisingly the saner of the two", LocationType.Office, mycroftLocation, mycroftItem);
        _locations[4] = Mycroft;
    }

    public void CheckForLocation(Vector2 coordinates)
    {
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
            {
                Console.WriteLine("Here is the goblin!! If you have your weapon type 'bomb'!!");
                Combat combat = new Combat(_theGame);
            }
            else if (location.Type == LocationType.Office)
            {
                Console.WriteLine($"You are in {location.Name} {location.Type}");
                Console.WriteLine(location.Discription);

                if (HasItem(location))
                {
                    Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here");
                }
            }
            else if (location.Type == LocationType.Street)
            {
                Console.WriteLine($"You are on {location.Name} {location.Type}");
                Console.WriteLine(location.Discription);

                if (HasItem(location))
                {
                    Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here");
                }
            }
            else if (location.Type == LocationType.npc)
            {
                Console.WriteLine("You found Sherlock");
                Console.WriteLine("Type 'talk' if you want to know what he has in store for you");

            }
        }
    }

    private bool IsOnLocation(Vector2 coords, out Location foundLocation)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].Coordinates == coords)
            {
                foundLocation = _locations[i];
                return true;
            }
        }

        foundLocation = null;
        return false;
    }

    private bool HasItem(Location location)
    {
        return location.ItemsOnLocation.Count != 0;

        // ---- THE LONG FORM ----
        //if (location.ItemsOnLocation.Count == 0)
        //{
        //	return false;
        //} else
        //{
        //	return true;
        //}
    }

    public void TakeItem(Location location)
    {

    }

    public void TakeItem(Player player, Vector2 coordinates)
    {
        if (IsOnLocation(coordinates, out Location location))
        {
            if (HasItem(location))
            {
                Item itemOnLocation = location.ItemsOnLocation[0];

                player.TakeItem(itemOnLocation);
                location.RemoveItem(itemOnLocation);

                Console.WriteLine($"You took the {itemOnLocation}");

                return;
            }
        }

        Console.WriteLine("There is nothing to take here!");
    }

    public void RemoveItemFromLocation(Item item)
    {
        for (int i = 0; i < _locations.Length; i++)
        {
            if (_locations[i].ItemsOnLocation.Contains(item))
            {
                _locations[i].RemoveItem(item);
            }
        }
    }

    #endregion
}