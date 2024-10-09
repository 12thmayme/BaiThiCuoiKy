using System;
using System.Collections.Generic;
using System.Linq;

namespace DieuMy
{
    public class Todo
    {
        public string NameOfWork { get; set; }
        public int Priority { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; } // 0: hủy; 1: hoàn thành; 2: chờ

        public Todo(string nameOfWork, int priority, string detail, int status)
        {
            NameOfWork = nameOfWork;
            Priority = priority;
            Detail = detail;
            Status = status;
        }

        private string StatusRef(int status)
        {
            return status switch
            {
                0 => "huy",
                1 => "hoan thanh",
                2 => "cho",
                _ => "khong xac dinh"
            };
        }

        public void InRaTodo()
        {
            Console.WriteLine("\tten: {0}", NameOfWork);
            Console.WriteLine("\tdo uu tien: {0}", Priority);
            Console.WriteLine("\tchi tiet: {0}", Detail);
            Console.WriteLine("\ttrang thai: {0}", StatusRef(Status));
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Todo> todoList = new List<Todo>
            {
                new Todo("Complete project report", 1, "Finish the monthly report and submit it to the manager", 2),
                new Todo("Grocery shopping", 3, "Buy vegetables, fruits, and dairy products", 1),
                new Todo("Schedule team meeting", 2, "Set up a meeting with the project team for Monday", 0),
                new Todo("Pay electricity bill", 4, "Pay the monthly electricity bill before the due date", 0),
                new Todo("Exercise", 2, "Complete a 30-minute home workout", 2),
                new Todo("Plan vacation itinerary", 3, "Research and plan destinations for the upcoming vacation", 1)
            };

            bool quit = false;
            while (!quit)
            {
                int chon = InMenu();
                switch (chon)
                {
                    case '1': // Add new task
                        todoList.Add(AddWork());
                        break;
                    case '2': // Delete task
                        DeleteWork(ref todoList);
                        break;
                    case '3': // Update task status
                        ChangeStatus(ref todoList);
                        break;
                    case '4': // Search tasks
                        SearchWork(todoList);
                        break;
                    case '5': // Show tasks by priority order
                        ShowAllWorkByPriorityOrder(todoList);
                        break;
                    case '6': // Show all tasks
                        ShowAllWork(todoList);
                        break;
                    case 27: // Escape
                    case 'q':
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection");
                        break;
                }
            }
        }

        public static int InMenu()
        {
            Console.WriteLine("----To do List----");
            Console.WriteLine("1: them cong viec");
            Console.WriteLine("2: xoa cong viec");
            Console.WriteLine("3: sua trang thai");
            Console.WriteLine("4: tim kiem cong viec");
            Console.WriteLine("5: hien thi theo do uu tien giam dan");
            Console.WriteLine("6: hien thi toan bo");
            Console.WriteLine("q hoac escape: thoat");
            Console.Write("lua chon: ");
            return (int)Console.ReadKey(false).KeyChar;
        }

        static Todo AddWork()
        {
            string name;
            do
            {
                Console.Write("\nNhap ten Cong Viec: ");
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                    Console.WriteLine("Ten viec khong duoc de trong.");
            } while (string.IsNullOrEmpty(name));

            int priority;
            do
            {
                Console.Write("Do uu tien (1-5): ");
                if (!int.TryParse(Console.ReadLine(), out priority) || priority < 1 || priority > 5)
                    Console.WriteLine("chi nhan gia tri tu 1 - 5");
            } while (priority < 1 || priority > 5);

            Console.Write("chi tiet cong viec: ");
            string detail = Console.ReadLine();

            int status;
            do
            {
                Console.Write("trang thai (0: Huy, 1: hoan thanh, 2: Cho): ");
                if (!int.TryParse(Console.ReadLine(), out status) || status < 0 || status > 2)
                    Console.WriteLine("chi nhan gia tri tu 0 den 2");
            } while (status < 0 || status > 2);

            Console.WriteLine("Da them");
            return new Todo(name, priority, detail, status);
        }

        public static void DeleteWork(ref List<Todo> todoList)
        {
            if (todoList.Count == 0)
            {
                Console.WriteLine("Danh sach rong, khong the xoa.");
                return;
            }

            int idx;
            do
            {
                Console.Write("\nnhap vi tri cua cong viec muon xoa: ");
                if (!int.TryParse(Console.ReadLine(), out idx) || idx < 0 || idx >= todoList.Count)
                    Console.WriteLine("khong tim thay cong viec tai vi tri {0}", idx);
            } while (idx < 0 || idx >= todoList.Count);

            todoList.RemoveAt(idx);
            Console.WriteLine("Da xoa cong viec tai vi tri {0}.", idx);
        }

        public static void ChangeStatus(ref List<Todo> todoList)
        {
            if (todoList.Count == 0)
            {
                Console.WriteLine("Danh sach rong, khong the cap nhat.");
                return;
            }

            int idx;
            do
            {
                Console.Write("\nnhap vi tri cua cong viec muon cap nhat trang thai: ");
                if (!int.TryParse(Console.ReadLine(), out idx) || idx < 0 || idx >= todoList.Count)
                    Console.WriteLine("khong tim thay cong viec tai vi tri {0}", idx);
            } while (idx < 0 || idx >= todoList.Count);

            int status;
            do
            {
                Console.Write("trang thai (0: Huy, 1: hoan thanh, 2: Cho): ");
                if (!int.TryParse(Console.ReadLine(), out status) || status < 0 || status > 2)
                    Console.WriteLine("chi nhan gia tri tu 0 den 2");
            } while (status < 0 || status > 2);

            todoList[idx].Status = status;
            Console.WriteLine("Cap nhat trang thai thanh cong cho vi tri {0}:", idx);
            todoList[idx].InRaTodo();
        }

        static void SearchWork(List<Todo> todoList)
        {
            Console.Write("\nten viec can tim: ");
            string searchtxt = Console.ReadLine();

            var result = todoList.Where(t => t.NameOfWork.Contains(searchtxt, StringComparison.OrdinalIgnoreCase)).ToList();
            Console.WriteLine("Ket qua tim kiem: ");
            if (result.Count > 0)
                result.ForEach(t => t.InRaTodo());
            else
                Console.WriteLine("Khong tim thay");
        }

        static void ShowAllWorkByPriorityOrder(List<Todo> todoList)
        {
            Console.WriteLine("");
            List<Todo> result = todoList.OrderByDescending(t => t.Priority).ToList();
            ShowAllWork(result);
        }

        static void ShowAllWork(List<Todo> todoList)
        {
            Console.WriteLine("");
            for (int i = 0; i < todoList.Count; i++)
            {
                Console.WriteLine("vi tri: {0}", i);
                todoList[i].InRaTodo();
            }
        }
    }
}
