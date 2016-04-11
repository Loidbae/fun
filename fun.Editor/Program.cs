﻿using System;
using fun.Editor.Commands;

namespace fun.Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandManager commands = new CommandManager(
                new CreateCommandParser(),
                new AddCommandParser());

            commands.Parse(args);
        }
    }
}
