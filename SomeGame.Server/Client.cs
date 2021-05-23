using SomeGame.Logic;
using SomeGame.TextCommands;
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
        private InputProcessor _inputProcessor;

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
                ProcessInput(line);
                line = _reader.ReadLine();
            }
        }

        private void ProcessInput(string input)
        {
            var output = _inputProcessor.Process(input);
            foreach (var line in output)
            {
                _writer.WriteLine(line);
            }
            _writer.Flush();
        }

        public void StartGame(PlayerGate gate, Game game)
        {
            gate.CurrentPlayerChanged += CurrendPlayerChanged;
            _writer.WriteLine($"Player 1: {game.Player1.Name}");
            _writer.WriteLine($"Player 2: {game.Player2.Name}");
            _writer.WriteLine();
            _writer.WriteLine($"{gate.GetCurrentPlayerName()} at play");
            _writer.Flush();

            _inputProcessor = new InputProcessor(gate, game);
        }

        private void CurrendPlayerChanged(object sender, CurrentPlayerChangedEventArgs e)
        {
            _writer.WriteLine($"{e.CurrentPlayerName} at play");
            _writer.Flush();
        }
    }
}
