
namespace TCPCacheServer.Builders
{    
    public class Command
    {
        private CommandType commandType;
        public CommandType CommandType { get => commandType; set => commandType = value; }     

        private string key;
        public string Key { get => key; set => key = value; }
        
        private string value;
        public string Value { get => value; set => this.value = value; }


    }
}