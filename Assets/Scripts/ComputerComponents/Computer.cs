using UnityEngine;

public class Computer : Singleton<Computer>
{
    [SerializeField]
    private Ram ram;
    [SerializeField]
    private ComputerSettings computerSettings;
    [SerializeField]
    private Desktop desktop;

    public ComputerSettings ComputerSettings { get => computerSettings; set => computerSettings = value; }
    public Ram Ram { get => ram; set => ram = value; }
    public Desktop Desktop { get => desktop; set => desktop = value; }
}
