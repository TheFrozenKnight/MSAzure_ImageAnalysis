//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
// Azure AI Vision SDK -- C# Image Analysis Samples
//

using Azure;
using Azure.AI.Vision.Common.Input;
using Azure.AI.Vision.Common.Options;
using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class AzureServices
{

    public static void ImageAnalysis_Analyze(string endpoint, string key, char imgType, string filePath)
    {
        var serviceOptions = new VisionServiceOptions(endpoint, new AzureKeyCredential(key));
        VisionSource imageSource;
        if (imgType == '1')
        {
            imageSource = VisionSource.FromFile("input\\" + filePath);
        }
        else
        {
            imageSource = VisionSource.FromUrl(new Uri(filePath));
        }

        var analysisOptions = new ImageAnalysisOptions()
        {
            Features =
                  ImageAnalysisFeature.CropSuggestions
                | ImageAnalysisFeature.Caption
                | ImageAnalysisFeature.DenseCaptions
                | ImageAnalysisFeature.Objects
                | ImageAnalysisFeature.People
                | ImageAnalysisFeature.Text
                | ImageAnalysisFeature.Tags,

            CroppingAspectRatios = new List<double>() { 0.9, 1.33 },

            Language = "en",

            // Optional. Default is "latest".
            ModelVersion = "latest",

            GenderNeutralCaption = true
        };

        using var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);

        Console.WriteLine(" Please wait for image analysis results...\n");

        var result = analyzer.Analyze();

        if (result.Reason == ImageAnalysisResultReason.Analyzed)
        {
            Console.WriteLine($" Image height = {result.ImageHeight}");
            Console.WriteLine($" Image width = {result.ImageWidth}");
            Console.WriteLine($" Model version = {result.ModelVersion}");

            if (result.Caption != null)
            {
                Console.WriteLine(" Caption:");
                Console.WriteLine($"   \"{result.Caption.Content}\", Confidence {result.Caption.Confidence:0.0000}");
            }

            if (result.DenseCaptions != null)
            {
                Console.WriteLine(" Dense Captions:");
                foreach (var caption in result.DenseCaptions)
                {
                    Console.WriteLine($"   \"{caption.Content}\", Bounding box {caption.BoundingBox}, Confidence {caption.Confidence:0.0000}");
                }
            }

            if (result.Objects != null)
            {
                Console.WriteLine(" Objects:");
                foreach (var detectedObject in result.Objects)
                {
                    Console.WriteLine($"   \"{detectedObject.Name}\", Bounding box {detectedObject.BoundingBox}, Confidence {detectedObject.Confidence:0.0000}");
                }
            }

            if (result.Tags != null)
            {
                Console.WriteLine($" Tags:");
                foreach (var tag in result.Tags)
                {
                    Console.WriteLine($"   \"{tag.Name}\", Confidence {tag.Confidence:0.0000}");
                }
            }

            if (result.People != null)
            {
                Console.WriteLine($" People:");
                foreach (var person in result.People)
                {
                    Console.WriteLine($"   Bounding box {person.BoundingBox}, Confidence {person.Confidence:0.0000}");
                }
            }

            if (result.CropSuggestions != null)
            {
                Console.WriteLine($" Crop Suggestions:");
                foreach (var cropSuggestion in result.CropSuggestions)
                {
                    Console.WriteLine($"   Aspect ratio {cropSuggestion.AspectRatio}: "
                        + $"Crop suggestion {cropSuggestion.BoundingBox}");
                };
            }

            if (result.Text != null)
            {
                Console.WriteLine($" Text:");
                foreach (var line in result.Text.Lines)
                {
                    string pointsToString = "{" + string.Join(',', line.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                    Console.WriteLine($"   Line: '{line.Content}', Bounding polygon {pointsToString}");

                    foreach (var word in line.Words)
                    {
                        pointsToString = "{" + string.Join(',', word.BoundingPolygon.Select(pointsToString => pointsToString.ToString())) + "}";
                        Console.WriteLine($"     Word: '{word.Content}', Bounding polygon {pointsToString}, Confidence {word.Confidence:0.0000}");
                    }
                }
            }

            var resultDetails = ImageAnalysisResultDetails.FromResult(result);
            Console.WriteLine($" Result details:");
            Console.WriteLine($"   Image ID = {resultDetails.ImageId}");
            Console.WriteLine($"   Result ID = {resultDetails.ResultId}");
            Console.WriteLine($"   Connection URL = {resultDetails.ConnectionUrl}");
            Console.WriteLine($"   JSON result = {resultDetails.JsonResult}");
        }
        else // result.Reason == ImageAnalysisResultReason.Error
        {
            var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
            Console.WriteLine(" Analysis failed.");
            Console.WriteLine($"   Error reason : {errorDetails.Reason}");
            Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
            Console.WriteLine($"   Error message: {errorDetails.Message}");
            Console.WriteLine(" Did you set the computer vision endpoint and key?");
        }
    }

    public static void ImageAnalysis_BG_Remover(string endpoint, string key, char imgType, string filePath)
    {
        var serviceOptions = new VisionServiceOptions(endpoint, new AzureKeyCredential(key));

        VisionSource imageSource;

        if (imgType == '1')
        {
            imageSource = VisionSource.FromFile("input\\" + filePath);
        }
        else
        {
            imageSource = VisionSource.FromUrl(new Uri(filePath));
        }

        var analysisOptions = new ImageAnalysisOptions()
        {
            SegmentationMode = ImageSegmentationMode.BackgroundRemoval
        };

        using var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);

        Console.WriteLine(" Please wait for image analysis results...\n");

        var result = analyzer.Analyze();

        if (result.Reason == ImageAnalysisResultReason.Analyzed)
        {
            using var segmentationResult = result.SegmentationResult;

            // Get the resulting output image buffer (PNG format)
            var imageBuffer = segmentationResult.ImageBuffer;
            Console.WriteLine($" Segmentation result:");
            Console.WriteLine($"   Output image buffer size (bytes) = {imageBuffer.Length}");

            // Get output image size
            Console.WriteLine($"   Output image height = {segmentationResult.ImageHeight}");
            Console.WriteLine($"   Output image width = {segmentationResult.ImageWidth}");

            // Write the buffer to a file
            string outputImageFile =filePath+ ".png";
            using (var fs = new FileStream("output\\"+outputImageFile, FileMode.Create))
            {
                fs.Write(imageBuffer.Span);
            }
            Console.WriteLine($"   File {outputImageFile} written to disk");
        }
        else // result.Reason == ImageAnalysisResultReason.Error
        {
            var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
            Console.WriteLine(" Analysis failed.");
            Console.WriteLine($"   Error reason : {errorDetails.Reason}");
            Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
            Console.WriteLine($"   Error message: {errorDetails.Message}");
            Console.WriteLine(" Did you set the computer vision endpoint and key?");
        }
    }
}
