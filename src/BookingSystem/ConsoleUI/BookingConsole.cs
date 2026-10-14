using System.Globalization;
using BookingSystem.Models;
using BookingSystem.Services;

namespace BookingSystem.ConsoleUI;

public class BookingConsole(BookingService service)
{
    public void Run()
    {
        var isRunning = true;

        while (isRunning)
        {
            ShowMenu();
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowBookings(service.GetActiveBookings());
                    break;
                case "2":
                    SearchBookings();
                    break;
                case "3":
                    CreateBooking();
                    break;
                case "4":
                    CancelBooking();
                    break;
                case "0":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Select a valid menu option.");
                    break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Meeting room bookings");
        Console.WriteLine("1. Show active bookings");
        Console.WriteLine("2. Search bookings");
        Console.WriteLine("3. Create booking");
        Console.WriteLine("4. Cancel booking");
        Console.WriteLine("0. Exit");
        Console.Write("Select: ");
    }

    private void SearchBookings()
    {
        Console.Write("Customer name: ");
        var searchTerm = Console.ReadLine() ?? string.Empty;
        ShowBookings(service.Search(searchTerm));
    }

    private void CreateBooking()
    {
        var customerName = InputCustomerName();

        var roomName = InputRoomName();
        Console.WriteLine($"Du valde {roomName}");

        var date = ReadDate("Date (yyyy-MM-dd): ");
        var startTime = ReadTime("Start time (HH:mm): ");
        var endTime = ReadTime("End time (HH:mm): ", startTime);

        var participants = InputParticipants(roomName);

        var booking = new Booking
        {
            CustomerName = customerName,
            RoomName = roomName,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Participants = participants
        };

        var result = service.Create(booking);
        Console.WriteLine(result.Message);
    }



    private void CancelBooking()
    {
        Console.Write("Booking number: ");

        if (!int.TryParse(Console.ReadLine(), out var bookingId))
        {
            Console.WriteLine("Enter a valid booking number.");
            return;
        }

        Console.WriteLine(service.Cancel(bookingId).Message);
    }

    private static void ShowBookings(IReadOnlyList<Booking> bookings)
    {
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings were found.");
            return;
        }

        foreach (var booking in bookings)
        {
            Console.WriteLine();
            Console.WriteLine($"Booking {booking.Id}");
            Console.WriteLine($"Customer: {booking.CustomerName}");
            Console.WriteLine($"Room: {booking.RoomName}");
            Console.WriteLine($"Date: {booking.Date:yyyy-MM-dd}");
            Console.WriteLine($"Time: {booking.StartTime:HH\\:mm}-{booking.EndTime:HH\\:mm}");
            Console.WriteLine($"Participants: {booking.Participants}");
        }
    }

    private static DateOnly ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (DateOnly.TryParseExact(
                    Console.ReadLine(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            Console.WriteLine("Enter the date using yyyy-MM-dd.");
        }
    }

    private static TimeOnly ReadTime(string prompt, TimeOnly? meetingStartTime = null)
    {
        while (true)
        {
            Console.Write(prompt);

            if (TimeOnly.TryParseExact(
                    Console.ReadLine(),
                    "HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var time))
            {

                if (meetingStartTime.HasValue && time <= meetingStartTime)
                {
                    Console.WriteLine($"End time must be set after start time ({meetingStartTime}) ");
                    continue;
                }
                return time;
            }

            Console.WriteLine("Enter the time using HH:mm.");
        }
    }

    private int InputParticipants(string roomName)
    {
        int participants;
        var maxNumberInRoom = service.MaxParticipantsInRoom(roomName);

        while (true)
        {
            Console.WriteLine($"(For {roomName} enter a number between 1 and {maxNumberInRoom})");
            Console.WriteLine("Enter number of participants: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out participants) && participants > 0 && participants <= maxNumberInRoom)
                break;

            Console.WriteLine($"Participants must be a number greater than 0 and maximum {maxNumberInRoom}.");
        }

        return participants;
    }

    private static string InputCustomerName()
    {
        string customerName = string.Empty;

        while (string.IsNullOrWhiteSpace(customerName))
        {
            Console.Write("Customer name: ");
            customerName = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(customerName))
            {
                Console.WriteLine("Customer name cannot be empty.");
                continue;
            }

            if (customerName.Length < 2 || customerName.Length > 30)
            {
                Console.WriteLine("Customer name must be between 2 and 30 characters");
                customerName = string.Empty;
            }
        }

        return customerName;
    }

    private static string InputRoomName()
    {
        Console.WriteLine("Select booking Room (Aspen, Birken or Eken): ");
        Console.WriteLine("1. Aspen (1-4)");
        Console.WriteLine("2. Birken (1-8)");
        Console.WriteLine("3. Eken (1-12)");

        ConsoleKey option;

        do
        {
            option = Console.ReadKey(true).Key;
        }
        while (option != ConsoleKey.D1 &&
        option != ConsoleKey.D2 &&
        option != ConsoleKey.D3);

        return option switch
        {
            ConsoleKey.D1 => "Aspen",
            ConsoleKey.D2 => "Birken",
            ConsoleKey.D3 => "Eken",
            _ => "Ogiltigt val"
        };
    }
}
