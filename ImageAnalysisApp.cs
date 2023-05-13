//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// Azure AI Vision SDK -- C# Image Analysis Samples
//
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

class ImageAnalysisApp
{
    static void DisplayCodeLinesFromFile()
    {
        string[] lines = File.ReadAllLines("Outro.txt");

        foreach (string line in lines)
        {
            foreach (char c in line)
            {
                Console.Write(c);
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }

            Console.WriteLine();
        }
        Thread.Sleep(TimeSpan.FromSeconds(1));
    }
    static void Main(string[] args)
    {
        char keyChar;
        char KeyImageType;
        String filePath = " ";

        string EnvKey = "144afe8c6c7a41c08beda3fd857b97d6";
        string EnvEndpoint = "https://ncuazure-imageanalysis.cognitiveservices.azure.com/";

        Console.WriteLine("");
        Console.WriteLine(" Ncu Azure project - Image Analysis and Background removal");
        Console.WriteLine(" By- 18CSU072 || Gaurav Pratap Singh");
        Console.WriteLine("     19CSU218 || Prashant Gupta");
        Console.WriteLine("     19CSU302 || Shubham Drolia");
        Console.WriteLine("");

        do
        {

            Console.WriteLine(" Please choose one of the following: ");
            Console.WriteLine(" 1. Select image from local path");
            Console.WriteLine(" 2. Select image from URL");
            Console.WriteLine(" 0. Exit");
            KeyImageType = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");

            try
            {
                switch (KeyImageType)
                {
                    case '1':
                        Console.WriteLine(" Enter File Name");
                        filePath = Console.ReadLine();
                        break;
                    case '2':
                        Console.WriteLine(" Enter File URL");
                        filePath = Console.ReadLine();
                        break;
                    case '0':
                        Console.WriteLine(" Exiting...");
                        DisplayCodeLinesFromFile();
                        return;
                    default:
                        Console.WriteLine(" Invalid selection, choose again.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            do
            {
                Console.WriteLine("");
                Console.WriteLine(" Please choose one of the following samples:");
                Console.WriteLine("");
                Console.WriteLine(" 1. Analyze Image");
                Console.WriteLine(" 2. Remove Background");
                Console.WriteLine(" 3. Back");
                Console.WriteLine(" 0. Exit");

                keyChar = Console.ReadKey().KeyChar;
                Console.WriteLine("\n");

                try
                {
                    switch (keyChar)
                    {
                        case '1':
                            AzureServices.ImageAnalysis_Analyze(EnvEndpoint, EnvKey, KeyImageType, filePath);
                            break;
                        case '2':
                            AzureServices.ImageAnalysis_BG_Remover(EnvEndpoint, EnvKey, KeyImageType, filePath);
                            break;
                        case '3':
                            break;
                        case '0':
                            Console.WriteLine(" Exiting...");
                            DisplayCodeLinesFromFile();
                            return;
                        default:
                            Console.WriteLine(" Invalid selection, choose again.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            } while (keyChar != '3' );
        } while (KeyImageType != '0');
    }
}
