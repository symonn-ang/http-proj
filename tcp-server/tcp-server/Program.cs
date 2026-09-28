using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices.Marshalling;
using tcp_server;
using tcp_server.Routing;
using System.Runtime.CompilerServices;

var connection = new Connection();

Task check = connection.StartConnection();

await Task.Delay(1000);
int option = 0;
int id = 0;
do
{
    Console.WriteLine(
        "1. Show messages\n" +
        "2. Create a message\n" +
        "3. Edit a message\n" +
        "4. Delete a message\n" +
        "5. Exit"
        );
    Console.Write("Choose an option: ");
    if (!int.TryParse(Console.ReadLine(), out option))
    {
        Console.WriteLine("Please enter a valid number.");
    }

    switch (option)
    {
        case 1:
            await Endpoints.GetMessages();
            break;

        case 2:
            Console.Write("Enter your message: ");
            string message = Console.ReadLine() ?? "";

            await Endpoints.PostMessage(message);
            break;

        case 3:
            Console.Write("Enter MessageID to edit: ");
            if (int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine();
                Console.Write("Enter new message: ");
                string editedMessage = Console.ReadLine() ?? "";
                await Endpoints.EditMessage(editedMessage, id);
            }
            else
            {
                Console.WriteLine("Please enter a valid ID.");
            }
            break;

        case 4:
            Console.Write("Enter MessageID to delete: ");
            if (int.TryParse(Console.ReadLine(), out id))
            {
                await Endpoints.DeleteMessage(id);
            }
            else
            {
                Console.WriteLine("Please enter a valid ID.");
            }
            break;

        case 5:
            Console.WriteLine("Good Bye!");
            break;

        default:
            Console.WriteLine("Please choose among the options.");
            break;
    }

} while (option != 5);

await check;



