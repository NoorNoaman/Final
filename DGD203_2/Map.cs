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
            Console.WriteLine("You can't go that way");
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
        Location goblin = new Location("Dobby's Cave", "\njackpot!", LocationType.Combat, goblinLocation);
        _locations[0] = goblin;

        Vector2 tavernLocation = new Vector2(-1, -1);
        Location tavern = new Location("The Giant's ", "I advice you not to stay here for long, it's where the big hairy dudes fight", LocationType.Tavern, tavernLocation);
        _locations[1] = tavern ;

        Vector2 ravenLocation = new Vector2(-2, -2);
        List<Item> ravenItem = new List<Item>();
        ravenItem.Add(Item.Bomb);
        Location raven = new Location("\nRaven\n", "This Raven looks exactly like the one in Teen Titans, lucky you ...", LocationType.npc, ravenLocation, ravenItem);
        _locations[2] = raven;

        Vector2 goreStreetLocation = new Vector2(-1, 1);
        List<Item> goreItem = new List<Item>();
        goreItem.Add(Item.Kidney);
        Location gore_Street = new Location("Gore", "You might want to hold your breath, people here take pleasure from watching dead bodies rot.\nHowever, if you go down three blocks then west, you will find what you are looking for.", LocationType.Street, goreStreetLocation, goreItem);
        _locations[3] = gore_Street;

        Vector2 osamaLocation = new Vector2(1, -2);
        List<Item> osamaItem = new List<Item>();
        osamaItem.Add(Item.Rune);
        Location osama = new Location("O'Sam Ben Ladin's", "He says he is a pilot.", LocationType.Office, osamaLocation, osamaItem);
        _locations[4] = osama;
    }

    public void CheckForLocation(Vector2 coordinates)
    {
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
            {
                Console.WriteLine("You found Dobby!! Help him out of his misery, type 'bomb' if you have your weapon!!");
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
            else if (location.Type == LocationType.Tavern)
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
                Console.WriteLine("You found Raven");
                Console.WriteLine("Type 'talk'");

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

        Console.WriteLine("There is nothing to take here.");
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
