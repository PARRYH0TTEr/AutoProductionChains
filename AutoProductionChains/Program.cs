

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
            string blueprintJsonFile = File.ReadAllText("G:\\Programmer (x86)\\Datalogi - Bachelors Project\\AutoProductionChains\\AutoProductionChains\\blueprint.json");
            Console.WriteLine(blueprintJsonFile);
            Blueprint testBlueprint = JsonSerializer.Deserialize<Blueprint>(blueprintJsonFile);




            string test_blueprintJsonFile_filePath = "G:\\Programmer (x86)\\Datalogi - Bachelors Project\\AutoProductionChains\\AutoProductionChains\\blueprintTesting.json";

            string jsonString = JsonSerializer.Serialize<Blueprint>(testBlueprint);
            
            //FileStream testFileStream = File.Open(test_blueprintJsonFile_filePath, FileMode.Open);

            File.WriteAllText(test_blueprintJsonFile_filePath, jsonString);
            
            Console.WriteLine(testBlueprint.label);

            string testBlueprintString = BlueprintHandler.EncodeBP(blueprintJsonFile);

            Console.WriteLine(testBlueprintString);
        }
    }

}

namespace AutoProductionChains.BlueprintJSONObjects
{
    public class BlueprintWrapper
    {
        public Blueprint blueprint { get; set; }
    }

    public class Blueprint
    {
        public string item {  get; set; }
        public string label { get; set; }
        public Entity[]? entities { get; set; }
        public int version {  get; set; }
    }

    public class Entity
    {
        public int entity_number {  get; set; }
        public string name {  get; set; }
        public Position position {  get; set; }
    }

    public class Position
    {
        public float x {  get; set; }
        public float y {  get; set; }
    }

}
