using UnityEngine;

public class Computer : Singleton<Computer>
{
    [SerializeField]
    private Ram ram;
    [SerializeField]
    private FileSystem fileSystem;
    [SerializeField]
    private ComputerSettings computerSettings;

    public ComputerSettings ComputerSettings { get => computerSettings; set => computerSettings = value; }
    public FileSystem FileSystem { get => fileSystem; set => fileSystem = value; }
    public Ram Ram { get => ram; set => ram = value; }

}
