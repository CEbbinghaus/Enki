namespace LotterySimulator
{
	using System;

	/// <summary>This class contains the lottery simulator.</summary>
	public class Program
	{
		/// <summary>Starts the simulator and initializes the parameters..</summary>
		/// <param name="args">The arguments.</param>
		public static void Main(string[] args)
		{
			int count = 6;
			int lowerBoundary = 1;
			int upperBoundary = 45;

			Random random = new Random();

			MainMenu(count, lowerBoundary, upperBoundary, random);
		}

		/// <summary>This is the main menu. All functions can be accessed from here.</summary>
		/// <param name="count">The number of picks.</param>
		/// <param name="lowerBoundary">The lower boundary.</param>
		/// <param name="upperBoundary">The upper boundary.</param>
		/// <param name="random">The random variable.</param>
		public static void MainMenu(int count, int lowerBoundary, int upperBoundary, Random random)
		{
			ConsoleKey decision;

			try
			{
				while (true)
				{
					DrawMainMenu();

					decision = Console.ReadKey().Key;

					switch (decision)
					{
						case ConsoleKey.M:
							Console.Clear();
							ManualPick(count, lowerBoundary, upperBoundary, random);
							break;
						case ConsoleKey.Q:
							Console.Clear();
							QuickPick(count, lowerBoundary, upperBoundary, random);
							break;
						case ConsoleKey.G:
							Console.Clear();
							GraphicalPick();
							break;
						case ConsoleKey.S:
							Console.Clear();
							JackpotSimulation(count, lowerBoundary, upperBoundary, random);
							break;
						case ConsoleKey.H:
							Console.Clear();
							FrequencyCalculator();
							break;
						case ConsoleKey.B:
							Console.Clear();
							CloseApplication();
							break;
						default:
							Console.WriteLine();
							Console.WriteLine();
							Console.ForegroundColor = ConsoleColor.DarkRed;
							Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
							Console.ResetColor();
							Console.ReadKey();
							Console.Clear();
							break;
					}
				}
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (ArgumentNullException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (Exception)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: Something went wrong!");
				Console.ReadKey();
			}
		}

		/// <summary>Manuals the pick.</summary>
		/// <param name="count">The number of picks.</param>
		/// <param name="lowerBoundary">The lower boundary.</param>
		/// <param name="upperBoundary">The upper boundary.</param>
		/// <param name="random">The random variable.</param>
		public static void ManualPick(int count, int lowerBoundary, int upperBoundary, Random random)
		{
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"The specified value must be equal or greater than 1!");
			}

			if (count > upperBoundary - lowerBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"Can't pick {count} unique numbers between {lowerBoundary} and {upperBoundary}!");
			}

			if (lowerBoundary > upperBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than {lowerBoundary}!");
			}

			if (lowerBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lowerBoundary), $"The specified value must be greater than 0!");
			}

			if (upperBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than 0!");
			}

			if (random == null)
			{
				throw new ArgumentNullException(nameof(random), $"Random can't be 0!");
			}

			int[] userNumbers = new int[count];
			int[] randomNumbers = new int[count + 1];

			DrawManualPick();

			try
			{
				userNumbers = ReadUniqueNumbersBetweenBoundaries(count, lowerBoundary, upperBoundary);
				randomNumbers = GenerateUniqueNumbersBetweenBoundaries(count, lowerBoundary, upperBoundary, random);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (ArgumentNullException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (Exception)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: Something went wrong!");
				Console.ReadKey();
			}

			Console.Clear();

			while (true)
			{
				DrawManualPick();

				Console.Write("   Ihre Zahlen waren: ");
				for (int i = 0; i < userNumbers.Length; i++)
				{
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write($"{userNumbers[i]}");
					Console.ResetColor();

					if (i + 1 < userNumbers.Length)
					{
						Console.Write(", ");
					}
				}

				Console.ResetColor();

				Console.WriteLine();
				Console.Write("   Die gezogenen Zahlen sind: ");
				for (int i = 0; i < 6; i++)
				{
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write($"{randomNumbers[i]}");
					Console.ResetColor();

					if (i + 1 < randomNumbers.Length)
					{
						Console.Write(", ");
					}
				}

				Console.ResetColor();
				Console.Write($"Zusatzzahl: ");
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.Write($"{randomNumbers[6]}");
				Console.ResetColor();

				CheckAndDisplayOutcome(userNumbers, randomNumbers);

				Console.WriteLine();
				Console.Write("   Bitte drücken Sie Enter um zum Hauptmenü zurückzukehren! ");

				ConsoleKey decision = Console.ReadKey().Key;

				switch (decision)
				{
					case ConsoleKey.Enter:
						Console.Clear();
						break;
					default:
						Console.WriteLine();
						Console.WriteLine();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
						Console.ResetColor();
						Console.ReadKey();
						Console.Clear();
						continue;
				}

				break;
			}
		}

		/// <summary>This method is almost identical to ManualPick. The difference is, that the user numbers are also randomly generated.</summary>
		/// <param name="count">  The number of picks.</param>
		/// <param name="lowerBoundary">The lower boundary.</param>
		/// <param name="upperBoundary">The upper boundary.</param>
		/// <param name="random">The random variable.</param>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if ...
		/// ... the count is lower than 1.
		/// ... the count is bigger than the difference of upper boundary and lower boundary.
		/// ... the lower boundary is greater than the upper boundary.
		/// ... the lower boundary is smaller than 0.
		/// ... the upper boundary is smaller than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">Is thrown if ...
		/// ... random is null.
		/// </exception>
		public static void QuickPick(int count, int lowerBoundary, int upperBoundary, Random random)
		{
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"The specified value must be equal or greater than 1!");
			}

			if (count > upperBoundary - lowerBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"Can't pick {count} unique numbers between {lowerBoundary} and {upperBoundary}!");
			}

			if (lowerBoundary > upperBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than {lowerBoundary}!");
			}

			if (lowerBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lowerBoundary), $"The specified value must be greater than 0!");
			}

			if (upperBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than 0!");
			}

			if (random == null)
			{
				throw new ArgumentNullException(nameof(random), $"Random can't be 0!");
			}

			int[] randomUserNumbers = new int[count];
			int[] randomNumbers = new int[count + 1];

			try
			{
				randomUserNumbers = GenerateUniqueNumbersBetweenBoundaries(count - 1, lowerBoundary, upperBoundary, random);
				randomNumbers = GenerateUniqueNumbersBetweenBoundaries(count, lowerBoundary, upperBoundary, random);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (ArgumentNullException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (Exception)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: Something went wrong!");
				Console.ReadKey();
			}

			while (true)
			{
				DrawQuickPick();

				Console.Write("   Ihre Zahlen waren: ");
				for (int i = 0; i < randomUserNumbers.Length; i++)
				{
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write($"{randomUserNumbers[i]}");
					Console.ResetColor();

					if (i + 1 < randomUserNumbers.Length)
					{
						Console.Write(", ");
					}
				}

				Console.ResetColor();

				Console.WriteLine();
				Console.Write("   Die gezogenen Zahlen sind: ");
				for (int i = 0; i < 6; i++)
				{
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write($"{randomNumbers[i]}");
					Console.ResetColor();

					if (i + 1 < randomNumbers.Length)
					{
						Console.Write(", ");
					}
				}

				Console.ResetColor();
				Console.Write($"Zusatzzahl: ");
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.Write($"{randomNumbers[6]}");
				Console.ResetColor();

				CheckAndDisplayOutcome(randomUserNumbers, randomNumbers);

				Console.WriteLine();
				Console.Write("   Bitte drücken Sie Enter um zum Hauptmenü zurückzukehren! ");

				ConsoleKey decision = Console.ReadKey().Key;

				switch (decision)
				{
					case ConsoleKey.Enter:
						Console.Clear();
						break;
					default:
						Console.WriteLine();
						Console.WriteLine();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
						Console.ResetColor();
						Console.ReadKey();
						Console.Clear();
						continue;
				}

				break;
			}
		}

		/// <summary>This function was not implemented.</summary>
		public static void GraphicalPick()
		{
			while (true)
			{
				DrawGraphicalPick();

				Console.WriteLine();
				Console.Write("   Bitte drücken Sie Enter um zum Hauptmenü zurückzukehren! ");

				ConsoleKey decision = Console.ReadKey().Key;

				switch (decision)
				{
					case ConsoleKey.Enter:
						Console.Clear();
						break;
					default:
						Console.WriteLine();
						Console.WriteLine();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
						Console.ResetColor();
						Console.ReadKey();
						Console.Clear();
						continue;
				}

				break;
			}
		}

		/// <summary>Jackpots the simulation.</summary>
		/// <param name="count">  The number of picks.</param>
		/// <param name="lowerBoundary">The lower boundary.</param>
		/// <param name="upperBoundary">The upper boundary.</param>
		/// <param name="random">The random variable.</param>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if ...
		/// ... the count is lower than 1.
		/// ... the count is bigger than the difference of upper boundary and lower boundary.
		/// ... the lower boundary is greater than the upper boundary.
		/// ... the lower boundary is smaller than 0.
		/// ... the upper boundary is smaller than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">Is thrown if ...
		/// ... random is null.
		/// </exception>
		public static void JackpotSimulation(int count, int lowerBoundary, int upperBoundary, Random random)
		{
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"The specified value must be equal or greater than 1!");
			}

			if (count > upperBoundary - lowerBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"Can't pick {count} unique numbers between {lowerBoundary} and {upperBoundary}!");
			}

			if (lowerBoundary > upperBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than {lowerBoundary}!");
			}

			if (lowerBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lowerBoundary), $"The specified value must be greater than 0!");
			}

			if (upperBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than 0!");
			}

			if (random == null)
			{
				throw new ArgumentNullException(nameof(random), $"Random can't be 0!");
			}

			int[] userNumbers = new int[count];
			int[] randomNumbers = new int[count];

			DrawJackpotSimualtor();

			try
			{
				userNumbers = ReadUniqueNumbersBetweenBoundaries(count, lowerBoundary, upperBoundary);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (ArgumentNullException e)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: {e.Message}");
				Console.ReadKey();
			}
			catch (Exception)
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write($"Error: Something went wrong!");
				Console.ReadKey();
			}

			Console.Clear();
			DrawJackpotSimualtor();

			int arrayCounter = 0;
			int checksumm = 0;

			while (true)
			{
				for (int i = 0; i < count; i++)
				{
					if (Array.IndexOf(randomNumbers, userNumbers[i]) >= 0)
					{
						checksumm++;
						continue;
					}
				}

				if (checksumm < count)
				{
					checksumm = 0;
					arrayCounter++;

					try
					{
						randomNumbers = GenerateUniqueNumbersBetweenBoundaries(count - 1, lowerBoundary, upperBoundary, random);
					}
					catch (ArgumentOutOfRangeException e)
					{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"Error: {e.Message}");
						Console.ReadKey();
					}
					catch (ArgumentNullException e)
					{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"Error: {e.Message}");
						Console.ReadKey();
					}
					catch (Exception)
					{
						Console.Clear();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"Error: Something went wrong!");
						Console.ReadKey();
					}

					continue;
				}
				else
				{
					break;
				}
			}

			while (true)
			{
				Console.Clear();
				DrawJackpotSimualtor();

				Console.Write("   Ihre Zahlen waren: ");
				for (int i = 0; i < userNumbers.Length; i++)
				{
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write($"{userNumbers[i]}");
					Console.ResetColor();

					if (i + 1 < userNumbers.Length)
					{
						Console.Write(", ");
					}
				}

				Console.WriteLine();
				Console.Write("   Anzahl der benötigten Ziehungen bis zum 6er: ");
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.WriteLine($"{arrayCounter}");
				Console.ResetColor();

				Console.WriteLine();
				Console.Write("   Bitte drücken Sie Enter um zum Hauptmenü zurückzukehren! ");

				ConsoleKey decision = Console.ReadKey().Key;

				switch (decision)
				{
					case ConsoleKey.Enter:
						Console.Clear();
						break;
					default:
						Console.WriteLine();
						Console.WriteLine();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
						Console.ResetColor();
						Console.ReadKey();
						Console.Clear();
						continue;
				}

				break;
			}
		}

		/// <summary>This function was not implemented.</summary>
		public static void FrequencyCalculator()
		{
			while (true)
			{
				DrawFrequencyCalculator();

				Console.WriteLine();
				Console.Write("   Bitte drücken Sie Enter um zum Hauptmenü zurückzukehren! ");

				ConsoleKey decision = Console.ReadKey().Key;

				switch (decision)
				{
					case ConsoleKey.Enter:
						Console.Clear();
						break;
					default:
						Console.WriteLine();
						Console.WriteLine();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
						Console.ResetColor();
						Console.ReadKey();
						Console.Clear();
						continue;
				}

				break;
			}
		}

		/// <summary>This function let's the user decide if her truly wants to close the application. Pressing "J" closes the application and Pressing "N" returns the user to the main menu.</summary>
		public static void CloseApplication()
		{
			while (true)
			{
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("   Möchten Sie den Lotto-Simulator wirklich beenden?");
				Console.ResetColor();
				Console.WriteLine();
				Console.Write("   (N) Nein / (J) Ja ");

				ConsoleKey decision = Console.ReadKey().Key;

				switch (decision)
				{
					case ConsoleKey.J:
						Environment.Exit(0);
						break;
					case ConsoleKey.N:
						Console.Clear();
						break;
					default:
						Console.WriteLine();
						Console.WriteLine();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write($"   Eingabe ist ungültig! Drücken Sie eine beliebige Taste um fortzufahren. ");
						Console.ResetColor();
						Console.ReadKey();
						Console.Clear();
						continue;
				}

				break;
			}
		}

		/// <summary>Reads the unique numbers between boundaries.</summary>
		/// <param name="count">  The number of picks.</param>
		/// <param name="lowerBoundary">The lower boundary.</param>
		/// <param name="upperBoundary">The upper boundary.</param>
		/// <returns>Returns an array of unique numbers between the boundaries entered by the user.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if ...
		/// ... the count is lower than 1.
		/// ... the count is bigger than the difference of upper boundary and lower boundary.
		/// ... the lower boundary is greater than the upper boundary.
		/// ... the lower boundary is smaller than 0.
		/// ... the upper boundary is smaller than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">Is thrown if ...
		/// ... random is null.
		/// </exception>
		public static int[] ReadUniqueNumbersBetweenBoundaries(int count, int lowerBoundary, int upperBoundary)
		{
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"The specified value must be equal or greater than 1!");
			}

			if (count > upperBoundary - lowerBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"Can't pick {count} unique numbers between {lowerBoundary} and {upperBoundary}!");
			}

			if (lowerBoundary > upperBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than {lowerBoundary}!");
			}

			if (lowerBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lowerBoundary), $"The specified value must be greater than 0!");
			}

			if (upperBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than 0!");
			}

			int[] userNumbers = new int[count];
			int value;

			for (int i = 0; i < userNumbers.Length; ++i)
			{
				Console.Write($"   Bitte geben Sie die {i + 1}te Zahl ein: ");

				if (!int.TryParse(Console.ReadLine(), out value))
				{
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.WriteLine("   Bitte geben Sie nur ganze Zahlen ein!");
					Console.ResetColor();
					--i;
					continue;
				}
				else if (value < lowerBoundary || value > upperBoundary)
				{
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.WriteLine($"   Bitte geben Sie nur Zahlen im Bereich {lowerBoundary} bis {upperBoundary} ein!");
					Console.ResetColor();
					--i;
					continue;
				}
				else if (Array.IndexOf(userNumbers, value) >= 0)
				{
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.WriteLine("   Bitte geben Sie keine doppelten Zahlen ein!");
					Console.ResetColor();
					--i;
					continue;
				}

				userNumbers[i] = value;
			}

			return userNumbers;
		}

		/// <summary>Generates the unique numbers between boundaries.</summary>
		/// <param name="count">The number of picks.</param>
		/// <param name="lowerBoundary">The lower boundary.</param>
		/// <param name="upperBoundary">The upper boundary.</param>
		/// <param name="random">The random variable.</param>
		/// <returns>Returns an array with unique and randomly generated numbers between the boundaries.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if ...
		/// ... the count is lower than 1.
		/// ... the count is bigger than the difference of upper boundary and lower boundary.
		/// ... the lower boundary is greater than the upper boundary.
		/// ... the lower boundary is smaller than 0.
		/// ... the upper boundary is smaller than 0.
		/// </exception>
		/// <exception cref="ArgumentNullException">Is thrown if ...
		/// ... random is null.
		/// </exception>
		public static int[] GenerateUniqueNumbersBetweenBoundaries(int count, int lowerBoundary, int upperBoundary, Random random)
		{
			if (count < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"The specified value must be equal or greater than 1!");
			}

			if (count > upperBoundary - lowerBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"Can't pick {count} unique numbers between {lowerBoundary} and {upperBoundary}!");
			}

			if (lowerBoundary > upperBoundary)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than {lowerBoundary}!");
			}

			if (lowerBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(lowerBoundary), $"The specified value must be greater than 0!");
			}

			if (upperBoundary < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(upperBoundary), $"The specified value must be greater than 0!");
			}

			if (random == null)
			{
				throw new ArgumentNullException(nameof(random), $"Random can't be 0!");
			}

			int[] randomNumbers = new int[count + 1];
			int value;

			for (int i = 0; i < randomNumbers.Length; i++)
			{
				value = random.Next(lowerBoundary, upperBoundary + 1);

				if (Array.IndexOf(randomNumbers, value) >= 0)
				{
					--i;
					continue;
				}

				randomNumbers[i] = value;
			}

			return randomNumbers;
		}

		/// <summary>This function checks whether the user array and the random array contain the same numbers and displays the result.</summary>
		/// <param name="userArray">The user array.</param>
		/// <param name="randomArray">The random array.</param>
		public static void CheckAndDisplayOutcome(int[] userArray, int[] randomArray)
		{
			int checksumm = 0;
			bool bonusNumber = false;

			for (int i = 0; i < userArray.Length; ++i)
			{
				if (Array.IndexOf(userArray, randomArray[i]) >= 0)
				{
					checksumm++;
					continue;
				}

				if (Array.IndexOf(userArray, randomArray[6]) >= 0)
				{
					bonusNumber = true;
					continue;
				}
			}

			Console.WriteLine();
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.DarkGreen;

			if (checksumm == 3 && bonusNumber == false)
			{
				Console.WriteLine("   Sie haben einen Dreier!");
			}
			else if (checksumm == 4 && bonusNumber == false)
			{
				Console.WriteLine("   Sie haben einen Vierer!");
			}
			else if (checksumm == 5 && bonusNumber == false)
			{
				Console.WriteLine("   Sie haben einen Fünfer!");
			}
			else if (checksumm == 6 && bonusNumber == false)
			{
				Console.WriteLine("   Sie haben einen Sechser!");
			}
			else if (checksumm < 3 && bonusNumber == true)
			{
				Console.WriteLine("   Sie haben die Zusatzzahl getroffen!");
			}
			else if (checksumm == 3 && bonusNumber == true)
			{
				Console.WriteLine("   Sie haben einen Dreier mit Zusatzzahl!");
			}
			else if (checksumm == 4 && bonusNumber == true)
			{
				Console.WriteLine("   Sie haben einen Vierer mit Zusatzzahl!");
			}
			else if (checksumm == 5 && bonusNumber == true)
			{
				Console.WriteLine("   Sie haben einen Fünfer mit Zusatzzahl!");
			}
			else if (checksumm == 6 && bonusNumber == true)
			{
				Console.WriteLine("   Sie haben einen Sechser mit Zusatzzahl!");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("   Sie haben leider nicht gewonnen!");
			}

			Console.ResetColor();
		}

		/// <summary>This method draws the main menu.</summary>
		public static void DrawMainMenu()
		{
			string[] abbreviations = new string[6] { "M", "Q", "G", "S", "H", "B" };
			string[] options = new string[6] { "... Manueller Tipp", "... Quick Tipp", "... Grafischer Tipp", "... Simulation bis zum 6er", "... Häufigkeiten ermitteln", "... Applikation beenden" };

			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine();
			Console.WriteLine("   Willkommen zum Lotto-Simulator");
			Console.WriteLine("   ==============================");
			Console.WriteLine();
			Console.ResetColor();

			for (int i = 0; i < abbreviations.Length; ++i)
			{
				Console.ForegroundColor = ConsoleColor.DarkGreen;
				Console.Write($"   {abbreviations[i]}");
				Console.ResetColor();
				Console.WriteLine($" {options[i]}");
			}

			Console.WriteLine("                            ");
			Console.Write("   Bitte treffen Sie Ihre Wahl: ");
		}

		/// <summary>This method draws the "manual pick" - function.</summary>
		public static void DrawManualPick()
		{
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine();
			Console.WriteLine("   Manueller Tipp");
			Console.WriteLine("   ==============");
			Console.WriteLine();
			Console.ResetColor();
		}

		/// <summary>This method draws the "quick pick" - function.</summary>
		public static void DrawQuickPick()
		{
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine();
			Console.WriteLine("   Quick-Tipp");
			Console.WriteLine("   ==========");
			Console.WriteLine();
			Console.ResetColor();
		}

		/// <summary>This method should draw the graphical pick, but the function was not implemented.</summary>
		public static void DrawGraphicalPick()
		{
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine();
			Console.WriteLine("   Grafischer Tipp");
			Console.WriteLine("   ===============");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("   Funktion wurde nicht implementiert!");
			Console.ResetColor();
		}

		/// <summary>This method draws the "jackpot simulator" - function.</summary>
		public static void DrawJackpotSimualtor()
		{
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine();
			Console.WriteLine("   Simulation bis zum 6er");
			Console.WriteLine("   ======================");
			Console.WriteLine();
			Console.ResetColor();
		}

		/// <summary>This method should draw the frequency calculator, but the function was not implemented.</summary>
		public static void DrawFrequencyCalculator()
		{
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.WriteLine();
			Console.WriteLine("   Häufigkeiten ermitteln");
			Console.WriteLine("   ======================");
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("   Funktion wurde nicht implementiert!");
			Console.ResetColor();
		}
	}
}