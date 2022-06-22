using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCPCacheServer.Builders;

namespace TCPCacheServer
{
    public class StringParser
    {
        const string newLineIdentifier = "\\r\\n";
        CommandBuilder commandBuilder;




        public StringParser(string stringToParse)
        {
            Parse(stringToParse);
        }

        private void Parse(string stringToParse)
        {

            var words = stringToParse
                    .Split(newLineIdentifier)[0]
                    .Split(' ');

            switch (words[0].ToLower())
            {
                case "set":
                    commandBuilder = new SetCommandBuilder();

                    //waiting here only "set", cache_key and size_in_bytes
                    if (words.Length == 3)
                    {
                        commandBuilder.BuildCommandKey(words[1]);
                    }
                    else throw new ArgumentException("Set command should contain key and value");
                    break;

                case "get":
                    commandBuilder = new GetCommandBuilder();

                    if (words.Length == 2)
                    {
                        commandBuilder.BuildCommandKey(words[1]);
                    }
                    else throw new ArgumentException("Get command should contain only key");
                    break;

                default: throw new ArgumentException("Unknown command type");

            }

        }

        public CommandBuilder GetCommandBuilder() => commandBuilder;



    }
}