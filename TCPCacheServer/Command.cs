using System;
namespace TCPCacheServer
{ 
    public enum CommandType { Set, Get };

    public class Command : IDisposable
    {
        private readonly CommandType commandType;
        private readonly string key;
        private string value;

        public CommandType CommandType { get => commandType; }
        public string Key { get => key; }
        public string Value { get => value; set => this.value = value; }

        public Command(CommandType commandType, string key, string value)
        {
            this.commandType = commandType;
            this.key = key;
            this.value = value;
        }

        public Command(string stringCommand)
        {
            var words = stringCommand
                .Split("\r\n")[0]
                .Split(' ');

            switch (words[0].ToLower())
            {
                case "set":
                    commandType = CommandType.Set;

                    if (words.Length == 3)
                    {
                        key = words[1];
                        value = words[2];
                    }
                    else throw new ArgumentException("Set command should contain key and value");
                    break;

                case "get":
                    commandType = CommandType.Get;

                    if (words.Length == 2)
                    {
                        key = words[1];
                        value = string.Empty;
                    }
                    else throw new ArgumentException("Get command should contain only key");
                    break;

                default:
                    throw new ArgumentException("Unknown command type");
            }
        }        

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}


