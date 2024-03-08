

using AutoProductionChains.Accessories;
using AutoProductionChains.Accessories.Util;
using AutoProductionChains.BlueprintJSONObjects;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AutoProductionChains
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            string blueprintJsonFile = File.ReadAllText("G:\\Programmer (x86)\\Datalogi - Bachelors Project\\AutoProductionChains\\AutoProductionChains\\blueprint.json");

            Console.WriteLine(blueprintJsonFile);

            Blueprint testBlueprint = JsonSerializer.Deserialize<Blueprint>(blueprintJsonFile);

            Console.WriteLine(testBlueprint.label);

            string testBlueprintString = BlueprintHandler.EncodeBP(blueprintJsonFile);

            Console.WriteLine(testBlueprintString);

        }
    }

}

namespace AutoProductionChains.BlueprintJSONObjects
{
    public class Blueprint
    {
        public string item {  get; set; }
        public string label { get; set; }
        public Entity[]? entities { get; set; }
        public Tile[]? tiles { get; set; }
        public Icon[]? icons {  get; set; }
        public int version {  get; set; }
    }

    public class Icon
    {
        public int index {  get; set; }
        public SignalID signal {  get; set; }
    }

    public class SignalID
    {
        public string name {  get; set; }
        public string type {  get; set; }
    }

    public class Tile
    {
        public string name {  get; set; }
        public Position position {  get; set; }
    }

    public class Entity
    {
        public int entity_number {  get; set; }
        public string name {  get; set; }
        public Position position {  get; set; }
        public bool request_from_buffers {  get; set; }

    }

    public class Position
    {
        public float x {  get; set; }
        public float y {  get; set; }
    }

}
