using System.Net;
using System.Net.Sockets;

namespace HWTester;

// COMMANDS

public interface IMessage<T>
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public T Payload { get; set; }
}

public abstract class Message : IMessage<string>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    public string Payload { get; set; }
    protected Message(string payload)
    {
        Payload = payload;
    }
}

public class ShutdownMessage : Message
{
    public const string SHUTDOWN_TEXT = "SHUTDOWN"; 
    
    public ShutdownMessage(string payload) : base(SHUTDOWN_TEXT)
    {
    }
}

// DEvICES

public interface IDevice
{
    public Guid Id { get; set; }
    public HashSet<IDevice> ConnectedDevices { get; set; }
    public IPEndPoint IpEndPoint { get; set; }
    public Task<bool> Connect(IDevice device, bool bidirectional);
    public Task<bool> Disconnect(IDevice device, bool bidirectional);
    public void Send(IDevice device, Message message);
    public void Receive(IDevice device, Message message);
    public void ShutDown();
}


public abstract class AbstractEthernetDevice : IDevice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Stack<Message> MessagesStack { get; set; } = new();

    public Dictionary<IDevice, Message> DeviceMessagePair { get; set; } = new();

    public HashSet<IDevice> ConnectedDevices { get; set; }
    public IPEndPoint IpEndPoint { get; set; }

    protected AbstractEthernetDevice() : this(new IPEndPoint(IPAddress.Loopback, 12345))
    {
    }
    
    protected AbstractEthernetDevice(IPEndPoint ipEndPoint)
    {
        IpEndPoint = ipEndPoint;
        ConnectedDevices = new HashSet<IDevice>();
    }
    
    protected AbstractEthernetDevice(IPEndPoint ipEndPoint, List<IDevice> devices, HashSet<IDevice> connectedDevices)
    {
        IpEndPoint = ipEndPoint;
        ConnectedDevices = devices.ToHashSet();
    }
    
    public async Task<bool> Connect(IDevice device, bool bidirectional)
    {
        if (bidirectional)
        {
            ConnectedDevices.Add(device);
            return await device.Connect(this, false);
        }

        ConnectedDevices.Add(device);
        return await Task.FromResult(true);
    }

    public async Task<bool> Disconnect(IDevice device, bool bidirectional)
    {
        if (bidirectional)
        {
            ConnectedDevices.Remove(device);
            return await device.Disconnect(this, false);
        }

        ConnectedDevices.Remove(device);
        return await Task.FromResult(true);
    }

    public void Send(IDevice device, Message message)
    {
        device.Receive(this, message);
    }

    public void Receive(IDevice device, Message message)
    {
        MessagesStack.Push(message);
        DeviceMessagePair.Add(device, message);
    }

    public void ShutDown()
    {
        foreach (var connectedDevice in ConnectedDevices)
        {
            connectedDevice.Disconnect(this, true);
        }
    }
}


public class CamEthernetDevice : AbstractEthernetDevice
{
    public const string NAME = "cam";
}

public class MicEthernetDevice : AbstractEthernetDevice
{
    public const string NAME = "mic";
}


//--------------------------------------------------------------------------------

// TEST_CASE



public interface ITestCase
{
    public bool Result { get; set; }
    
    public Task<bool> Run();
}


public class TrueResultTestCase : ITestCase
{
    public bool Result { get; set; }

    public async Task<bool> Run()
    {
        Result = true;
        return await Task.FromResult(Result);
    }
}

public class FalseResultTestCase : ITestCase
{
    public bool Result { get; set; }

    public async Task<bool> Run()
    {
        Result = false;
        return await Task.FromResult(Result);
    }
}

public interface ITestSuite
{
    public List<ITestCase> TestCases { get; }

    public Task<bool> Run();
}


public abstract class GeneralTestSuite : ITestSuite
{
    public List<ITestCase> TestCases { get; }
    public bool Result { get; set; }
    
    private readonly ILogger<GeneralTestSuite> _logger;

    protected GeneralTestSuite(ILogger<GeneralTestSuite> logger, List<ITestCase> testCases)
    {
        _logger = logger;
        TestCases = testCases;
    }
    
    public async Task<bool> Run()
    {
        foreach (var testCase in TestCases) 
            await testCase.Run();

        return TestCases.All(x => x.Result);
    }
}


public class BasicTestSuite : GeneralTestSuite 
{
    public BasicTestSuite(ILogger<GeneralTestSuite> logger, List<ITestCase> testCases) : base(logger, testCases)
    {
        
    }
}


public class TestSuiteBuilder
{
    private readonly ILogger<TestSuiteBuilder> _logger;

    public TestSuiteBuilder(ILogger<TestSuiteBuilder> logger)
    {
        _logger = logger;
    }
}


public class TestCaseValidator
{
    private readonly ILogger<TestCaseValidator> _logger;

    public TestCaseValidator(ILogger<TestCaseValidator> logger)
    {
        _logger = logger;
    }

    public Task<bool> Validate()
    {
        throw new NotImplementedException();
    }
}




public class CommandHandler
{
    public const string SPAWN = "s";
    public const string KILL = "k";
    public const string CONNECT = "c";
    public const string DISCONNECT = "d";
    public const string PRINT_TIME = "pt";
    public const string SPAWN_ADN_CONNECT = "sac";

    private readonly ILogger<CommandHandler> _logger;
    private readonly ZooKeeper _zooKeeper;

    public CommandHandler(ILogger<CommandHandler> logger, ZooKeeper zooKeeper)
    {
        _logger = logger;
        _zooKeeper = zooKeeper;
    }
    
    public async Task HandleCommands()
    {
        _logger.LogInformation("Handler is up and running");
        while (true)
        {
            var commands =  Console.ReadLine()!.Split(" ").ToList();

            commands = commands.Select(x => x.Trim()).ToList();
            
            _logger.LogInformation("Command received: {0}",string.Join(" ", commands ));

            switch (commands[0])
            {
                case SPAWN:
                    switch (commands[1])
                    {
                        case CamEthernetDevice.NAME:
                            _zooKeeper.Add(new CamEthernetDevice());
                            break;
                        case MicEthernetDevice.NAME:
                            _zooKeeper.Add(new MicEthernetDevice());
                            break;
                    }
                    break;
                case KILL:
                    var tmp =_zooKeeper.Devices.FirstOrDefault(x => x.Id == Guid.Parse(commands[1]));
                    if (tmp is not null)
                        tmp.ShutDown();
                    break;
                case CONNECT:
                {
                    var d1 = _zooKeeper.Devices.FirstOrDefault(x => x.Id == Guid.Parse(commands[1]));
                    var d2 = _zooKeeper.Devices.FirstOrDefault(x => x.Id == Guid.Parse(commands[2]));
                    var bi = Convert.ToBoolean(commands[3]);

                    if (d1 is not null && d2 is not null)
                    {
                        d1.Connect(d2, bi);
                    }
                    break;
                }
                case DISCONNECT:
                {
                    var d1 = _zooKeeper.Devices.FirstOrDefault(x => x.Id == Guid.Parse(commands[1]));
                    var d2 = _zooKeeper.Devices.FirstOrDefault(x => x.Id == Guid.Parse(commands[2]));
                    var bi = Convert.ToBoolean(commands[3]);

                    if (d1 is not null && d2 is not null)
                    {
                        d1.Disconnect(d2, bi);
                    }
                    break;
                }
                case SPAWN_ADN_CONNECT:
                {
                    var c = new CamEthernetDevice();
                    var m = new MicEthernetDevice();

                    await c.Connect(m, true);
                    _zooKeeper.Add(m);
                    _zooKeeper.Add(c);
                }
                    break;
                case PRINT_TIME:
                    _logger.LogInformation(DateTime.UtcNow.ToString());
                    break;
                default:
                    _logger.LogInformation("Command wasn't recognized");
                    break;
                    
            }
        }
    }
}



public class ZooKeeper
{
    public HashSet<IDevice> Devices{ get; } = new();

    private readonly ILogger<ZooKeeper> _logger;

    public ZooKeeper(ILogger<ZooKeeper> logger)
    {
        _logger = logger;
    }

    public void Add(IDevice device)
    {
        Devices.Add(device);
        _logger.LogInformation("Device: {0} was added to zoo", device.Id);
    }
    
    public void Remove(IDevice device)
    {
        Devices.Remove(device);
        _logger.LogInformation("Device: {0} was removed from zoo", device.Id);
    }

    public void PrintZoo()
    {
        _logger.LogInformation("There are: {0} in the zoo", Devices.Count);
        foreach (var device in Devices)
        {
            _logger.LogInformation(" -- {0} in the zoo", device.Id);
        }
    }
}








