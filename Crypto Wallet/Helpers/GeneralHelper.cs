using System;
namespace Crypto_Wallet.Helpers
{
	public static class GeneralHelper
	{
        public static void PrintGeneralSectionSeparator(string text)
        {
            Console.WriteLine($"\n<<<---------- {text} ---------->>>\n");
        }

        public static string CapitalizeAndTrim(string input)
        {
            input = input.Trim().ToLower();
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static bool ConfirmChoice(string message = "Are you sure?")
        {
            Console.Write($"\n{message} (y/n): ");
            return Console.ReadLine().Trim().ToLower() == "y";
        }

        public static int GetMenuChoiceFromUser(string menuText, string menuTitle)
        {
            var success = false;
            var choice = 0;

            do
            {
                PrintGeneralSectionSeparator(menuTitle);

                Console.WriteLine(menuText);
                Console.Write("Input your choice: ");
                success = int.TryParse(Console.ReadLine(), out choice);

                if (!success)
                {
                    Console.Clear();
                    Console.WriteLine("Value must be a number!\n");
                }

            } while (!success);

            return choice;
        }

        public static Guid GetGuidFromUserInput(string inputMessage)
        {
            var success = false;

            Console.Write(inputMessage);
            success = Guid.TryParse(Console.ReadLine(), out Guid userInput);

            if (!success)
            {
                Console.WriteLine("Input value need to be a guid!");
                return Guid.Empty;
            }

            return userInput;
        }
    }
}

