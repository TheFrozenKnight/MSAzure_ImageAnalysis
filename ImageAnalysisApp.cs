
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;

class ImageAnalysisApp
{
    static bool CheckImageExists(string imageUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(imageUrl).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
                if(IsImageFile(imageUrl))
                    return true;
            return false;
        }
        return false;
    }
    static bool IsImageFile(string filePath)
    {
        string extension = Path.GetExtension(filePath);

        if (extension != null)
        {
            string[] validExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

            // Compare the file extension (case-insensitive)
            return validExtensions.Contains(extension.ToLower());
        }

        return false;
    }

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

            SelectionLoopStart:
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
                        ImageLoopStart:
                        Console.WriteLine(" Enter File Name");
                        filePath = "input\\" + Console.ReadLine();
                        if (!IsImageFile(filePath))
                        {
                            Console.WriteLine(" --- Select an Image ---");
                            goto ImageLoopStart;
                        }
                        if (!File.Exists(filePath))
                        {
                            Console.WriteLine(" --- File Does not Exist  ---");
                            goto ImageLoopStart;
                        }
                        break;
                    case '2':
                        URLLoopStart:
                        Console.WriteLine(" Enter File URL");
                        filePath = Console.ReadLine();

                        if (!CheckImageExists(filePath))
                        {
                            Console.WriteLine(" --- Image does not exist at URL: " + filePath);
                            goto URLLoopStart;
                        }
                        break;
                    case '0':
                        Console.WriteLine(" Exiting...");
                        DisplayCodeLinesFromFile();
                        return;
                    default:
                        Console.WriteLine(" Invalid selection, choose again.");
                        goto SelectionLoopStart;
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
