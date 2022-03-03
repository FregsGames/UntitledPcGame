using System.Threading.Tasks;
using UnityEngine;

public class ToolBar : MonoBehaviour
{
    public async void Save()
    {
        ComputerScreen.Instance.BlockPanel.EnableBlock(true);
        await SaveManager.Instance.SaveChanges();
        await Task.Delay(1000);
        ComputerScreen.Instance.BlockPanel.EnableBlock(false);
    }
}
