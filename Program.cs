﻿using System;
using System.Collections.Generic;

namespace TrainMathsPuzzle
{
    class Program
    {
        private static int Capacity = 500;
        private static int TrainPosition = 0;
        private static int FuelOnBoard = Capacity;
        private static int FuelUsed = Capacity;
        private static int StepAmount = 100;
        private static Dictionary<int, int> FuelOnSideOfTrack = new Dictionary<int, int>();
        private static List<string> StepsTaken = new List<string>();

        static void Main(string[] args)
        {
            FuelOnSideOfTrack = InitialiseFuelOnSideOfTrack();
            ShowStatus();

            while(true)
            {
                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        //Move foward
                        if (TrainPosition < 800 && FuelOnBoard >= StepAmount)
                        {
                            TrainPosition += StepAmount;
                            FuelOnBoard -= StepAmount;
                            StepsTaken.Add($"Move forward {StepAmount} miles");
                        }

                        break;

                    case ConsoleKey.LeftArrow:
                        //Move backward
                        if (TrainPosition > 0 && FuelOnBoard >= StepAmount)
                        {
                            TrainPosition -= StepAmount;
                            FuelOnBoard -= StepAmount;
                            StepsTaken.Add($"Move backward {StepAmount} miles");

                            if (TrainPosition == 0)
                            {
                                //We're at the depot - automatically refuel to 500
                                Refuel(Capacity - FuelOnBoard);
                            }
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        //Drop fuel
                        Console.Write("How much fuel to drop? ");
                        var amountToDrop = int.Parse(Console.ReadLine());
                        if (amountToDrop <= FuelOnBoard)
                        {
                            FuelOnBoard -= amountToDrop;
                            FuelOnSideOfTrack[TrainPosition] += amountToDrop;
                            StepsTaken.Add($"Drop {amountToDrop} miles of fuel");
                        }
                        else
                        {
                            Console.WriteLine("You don't have that much fuel to drop");
                        }

                        break;

                    case ConsoleKey.UpArrow:
                        //Pick up fuel
                        Console.Write("How much fuel to pick up? ");
                        var amountToPickUp = int.Parse(Console.ReadLine());
                        if (amountToPickUp <= FuelOnSideOfTrack[TrainPosition] && FuelOnBoard + amountToPickUp <= Capacity)
                        {
                            FuelOnBoard += amountToPickUp;
                            FuelOnSideOfTrack[TrainPosition] -= amountToPickUp;
                            StepsTaken.Add($"Pick up {amountToPickUp} miles of fuel");
                        }
                        else
                        {
                            Console.WriteLine("You can't pick up that much fuel here");
                        }

                        break;

                    case ConsoleKey.R:
                        //Refuel at depot
                        Console.Write("How much fuel to add? ");
                        var amountToAdd = int.Parse(Console.ReadLine());
                        Refuel(amountToAdd);
                        break;

                    case ConsoleKey.Escape:
                        //Start over
                        TrainPosition = 0;
                        FuelOnBoard = Capacity;
                        FuelUsed = 500;
                        FuelOnSideOfTrack = InitialiseFuelOnSideOfTrack();
                        StepsTaken.Clear();
                        break;
                }

                ShowStatus();
            }
        }

        private static void Refuel(in int amountToAdd)
        {
            if (amountToAdd + FuelOnBoard <= Capacity)
            {
                FuelOnBoard += amountToAdd;
                FuelUsed += amountToAdd;
                StepsTaken.Add($"Added {amountToAdd} miles of fuel at the depot");
            }
            else
            {
                Console.WriteLine($"That maximum fuel you can add is {Capacity - FuelOnBoard}");
            }
        }

        private static Dictionary<int, int> InitialiseFuelOnSideOfTrack()
        {
            var dictionary = new Dictionary<int, int>();
            for (var position = 0; position < 800; position += StepAmount)
            {
                dictionary[position] = 0;
            }

            return dictionary;
        }

        private static void ShowStatus()
        {
            Console.Clear();
            Console.WriteLine($"Fuel Used: {FuelUsed}");

            //Headings
            WritePositionMessage("Depot");
            for (var position = StepAmount; position <= (800 - StepAmount); position += StepAmount)
            {
                WritePositionMessage($"|{position}","-");
            }
            WritePositionMessage("Party Town!");
            Console.Write(Environment.NewLine);


            //Display Train Position and fuel on board
            for (var position = 0; position <= 800; position += StepAmount)
            {
                var message = "";
                if (TrainPosition == position)
                {
                    message = $"choo\xB2 ({FuelOnBoard})";
                }
                WritePositionMessage(message);
            }
            Console.WriteLine(Environment.NewLine);


            //Display Fuel on Side of Track
            for (var position = 0; position < 800; position += StepAmount)
            {
                var message = " ";
                if (FuelOnSideOfTrack[position] > 0)
                {
                    message = $"{FuelOnSideOfTrack[position]}";
                }

                WritePositionMessage(message);
            }


            Console.Write(Environment.NewLine);

            if (TrainPosition == 800)
            {
                Console.WriteLine("You made it to party town!!!");
            }

            //Console.WriteLine();
            //Console.WriteLine("Steps taken:");
            //var stepCount = 1;
            //foreach (var step in StepsTaken)
            //{
            //    Console.WriteLine($"{stepCount}. {step}");

            //    stepCount++;
            //}
        }

        private static void WritePositionMessage(string message, string paddingCharacter = " ")
        {
            var output = $"{message,-12}".Replace(" ", paddingCharacter);
            Console.Write(output);
        }
    }
}
