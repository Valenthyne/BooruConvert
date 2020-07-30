/*
 * This program should be able to take as input images directly downloaded from Danbooru
 * and, following the site's file naming conventions, convert the file to a more sensible
 * and legible name, following this pattern:
 *	[artist] character hash.extension
 *	
 *	Example:
 *	Original filename: __flandre_scarlet_touhou_drawn_by_nagisa_shizuku__dba3735641df0127c18b23417bf0ba3f.jpg
 *	Modified filename: [nagisa shizuku] flandre scarlet touhou dba37.jpg
 *	
 *	There is probably any number of ways to do this more efficiently, but it works.
 *	
 *	Authored by Drachen, July 2020
 */

using System;
using System.IO;

namespace BooruConvert
{
	class RenameImg
	{
		static void Main(string[] args)
		{
			Console.WriteLine("\tWelcome to the Booru Converter!");
			Console.WriteLine("\tConvert Danbooru filenames with ease.");

			Console.Write("\nPlease enter a path to read: ");
			String curPath = Console.ReadLine();

			if (Directory.Exists(curPath)) {
				Console.WriteLine("");
				String[] filesInDirectory = Directory.GetFiles(curPath);

				double fileCount = 0;
				double successCount = 0;

				String path = curPath + "\\";
				String convPath = path + "Converted\\";

				if (filesInDirectory.Length > 0)
				{
					Directory.CreateDirectory(convPath);
				}

				foreach (String str in filesInDirectory)
				{
					// Clip the string down until the filename only
					String image = str.Substring(str.LastIndexOf('\\') + 1);

					// Determine if it's a relevant booru file
					if (image[0] == '_' && image[1] == '_')
					{
						// Valid booru file detected; increment counter
						fileCount++;

						// Clip off the double underscore
						image = image.Substring(2);
						try
						{
							int len = image.Length;
							int start = image.IndexOf("by_") + 3;
							int end = image.IndexOf("__");

							String newCharacter = "";

							String artist = image.Substring(start, end - start);

							// Determine if artist is mentioned by present of "drawn" substring
							if (image.Substring(0, 5).Equals("drawn"))
							{
								newCharacter = "unknown";
							} 
							else
							{
								newCharacter = image.Substring(0, image.IndexOf("_drawn")); 
							}
							
							String newHash = image.Substring(image.LastIndexOf("__") + 2, 5);
							String newExt = image.Substring(image.IndexOf('.'));

							String newFile = "[" + artist + "] " + newCharacter + " " + newHash + newExt;

							Console.WriteLine(convPath + newFile);

							successCount++;
							newFile = RemoveUnderscores(newFile);
							Console.WriteLine(newFile);
							System.IO.File.Move(path + "__" + image, convPath + newFile);
						}

						catch (Exception ex)
						{
							Console.WriteLine("This file cannot be processed: " + image);
							Console.WriteLine(ex.Message + "\n");
						}

					}
				}

				if (successCount != 0)
				{
					Console.WriteLine("Of a total of " + fileCount + " files, " + successCount + " were converted for a " +
					"success rate of " + (((successCount) / fileCount) * 100) + "%.");
				}
				else
				{
					Console.WriteLine("No files were successfully converted.");
				}
			} 
			else
			{
				Console.WriteLine("\nNon-existing path. Closing...");
			}

			Console.WriteLine();
			Console.ReadKey();
		}

		static String RemoveUnderscores (String s)
		{
			String n = "";
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '_')
				{
					n += " ";
				}
				else
				{
					n += s[i];
				}	
			}

			return n;
		}
	}
}
