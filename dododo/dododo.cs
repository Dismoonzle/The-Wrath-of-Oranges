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
        // ������ģ������
        // ���������ģ���ʼ������
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
        // ������������Զ�����Ϣ����
    }
}
