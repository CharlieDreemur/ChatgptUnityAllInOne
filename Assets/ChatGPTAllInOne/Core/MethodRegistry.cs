using System.Collections.Generic;
namespace ChatgptAllInOne
{
    public static class MethodRegistry
    {
        public static Dictionary<string, MethodMetadata> Methods = new Dictionary<string, MethodMetadata>();
    }
    

    public class MethodMetadata
    {
        public string Description { get; set; }
        public Dictionary<string, ParameterMetadata> Parameters { get; set; }
    }

    public class ParameterMetadata
    {
        public string Description { get; set; }
        public List<string> EnumValues { get; set; }
        public bool IsRequired { get; set; }
    }


}