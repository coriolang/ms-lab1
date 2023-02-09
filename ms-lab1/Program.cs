class Barbershop
{
    // рабочее время в минутах
    private int working;

    // клиенты в парикмахерской
    private List<Client> clients = new List<Client>();

    // средняя длина очереди
    private double avgQueue = 0;

    // среднее время ожидания в очереди
    // коэффициент загруженности парикмахера

    public Barbershop(int hours)
    {
        working = hours * 60;
    }

    // сгенерирует и добавит в список
    // всех клиентов, пришедших в парикмахерскую
    public void Open()
    {
        Random random = new Random();

        int arrival = 0;
        while (true)
        {
            int service;

            int withBeard = random.Next(0, 2);
            if (withBeard == 1)
            {
                service = random.Next(40, 81);
            }
            else
            {
                service = random.Next(25, 46);
            }

            arrival = random.Next(
                arrival,
                arrival + service
                );

            //Client last = clients.Last();

            //if (last != null && )

            if (arrival + service < working)
            {
                Client client 
                    = new Client(arrival, service);

                clients.Add(client);
            }
            else break;
        }
    }

    // подсчет средней длины очереди в парикмахерской
    // вычисляется длина очереди во время обслуживания каждого клиента,
    // суммируется, затем сумма делится на количество клиентов
    public void CountAvgQueue()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            Predicate<Client> inQueue = (client) => 
            client.Arrival >= clients[i].Arrival
            && 
            client.Arrival <= clients[i].Arrival + clients[i].Service
            &&
            client != clients[i];

            avgQueue += clients.Count(client => inQueue(client));

            /*for (int j = 0; j < clients.Count; j++)
            {
                if (
                    clients[j].Arrival >= clients[i].Arrival
                    &&
                    clients[j].Arrival <= clients[i].Arrival + clients[i].Service
                    &&
                    clients[j] != clients[i]
                    )
                {
                    avgQueue++;
                }
            }*/
        }

        avgQueue /= clients.Count;
    }

    public int Working { get { return working; } }
    public List<Client> Clients { get { return clients; } }

    public double AvgQueue { get { return avgQueue; } }
}

class Client
{
    // минута прибытия в парикмахерскую
    private int arrival;

    // время обслуживания клиента в минутах
    private int service;
    private int start;

    public Client(int arrival, int service)
    {
        this.arrival = arrival;
        this.service = service;
    }

    public int Arrival { get { return arrival; } }
    public int Service { get { return service; } }
    public int Start 
    {
        get { return start; } 
        set { start = value; }
    }
}

class Program
{
    public static void Main()
    {
        // создание парикмахерских
        int hours = 8;
        int experiments = 100;

        List<Barbershop> barbershops = new List<Barbershop>();

        for (int i = 0; i < experiments; i++)
        {
            Barbershop barbershop = new Barbershop(hours);

            barbershop.Open();
            barbershop.CountAvgQueue();

            barbershops.Add(barbershop);
        }

        // подсчет общей статистики:
        // - средней длины очереди и ее дисперсии;
        // - среднего времени ожидания и ее дисперсии;
        // - коэффициента загруженности парикмахера и ее дисперсии.
        double avgQueueLength = 0.0; // среднее
        double varianceQueueLength = 0.0; // дисперсия

        for (int i = 0; i < experiments; i++)
        {
            avgQueueLength += barbershops[i].AvgQueue;

            double p = 1.0 / experiments;
            for (int j = i + 1; j < experiments; j++)
            {
                varianceQueueLength += p * p * Math.Pow(
                    barbershops[i].AvgQueue - barbershops[j].AvgQueue, 
                    2
                    );
            }
        }

        avgQueueLength /= experiments;

        // вывод результатов
        Console.WriteLine($"avg queue length = {avgQueueLength}");
        Console.WriteLine($"variance queue length = {varianceQueueLength}");
    }
}
