using SomeGame.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeGame.Server
{
    internal class Client
    {
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        public Client(Stream stream, string name)
        {
            _writer = new StreamWriter(stream, Encoding.ASCII);
            _reader = new StreamReader(stream, Encoding.ASCII);
            Name = name;
        }

        public string Name { get; }

        public void Run()
        {
            var line = _reader.ReadLine();
            while (line is not null)
            {
                Console.WriteLine(line);
                line = _reader.ReadLine();
            }
        }

        public void StartGame(Game game)
        {
            _writer.WriteLine($"Player 1: {game.Player1.Name}");
            _writer.WriteLine($"Player 2: {game.Player2.Name}");
            _writer.Flush();
        }
    }
}
