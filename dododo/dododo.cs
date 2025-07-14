using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace dododo
{
    public class dododo : Mod
    {
        // 这里是模组主类
        // 在这里添加模组初始化代码
    }
    public enum YourModMessageType : byte
    {
        SpawnOranger,
        SyncNPCData,
        PlayerDataRequest,
        PlayerDataResponse,
        PlayerDataUpdate,
        PlayerDataSync,
        PlayerDataSyncRequest,
        PlayerDataSyncResponse,
        PlayerDataSyncUpdate,
        PlayerDataSyncComplete,
        PlayerDataSyncError,
        PlayerDataSyncCancel,
        PlayerDataSyncStart,
        // 可以添加其他自定义消息类型
    }
}
