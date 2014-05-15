using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro.Multi
{
    public interface IEventSignificator
    {
        IList<Type> EventTypeList { get; }
        string ToSignificant(string eventName);
        Type ToSignificantType(string eventName);
    }

    [Serializable]
    public class Significator : IEventSignificator
    {
        [Serializable]
        private class EnumInfo
        {
            public string Name;
            public Type EnumType;

            public EnumInfo(Type enumType, string name)
            {
                Name = name;
                EnumType = enumType;
            }
        }
        public IList<Type> EventTypeList { get; private set; }
        private IDictionary<string, EnumInfo> EventTypeDict { get; set; }

        public Significator()
        {
            EventTypeList = new List<Type>();
            EventTypeList.Add(typeof(SignificantProfileEvent));
            EventTypeList.Add(typeof(SignificantNetworkEvent));
            EventTypeList.Add(typeof(SignificantDiskIoEvent));
            EventTypeList.Add(typeof(SignificantRegistryEvent));
            EventTypeList.Add(typeof(SignificantFileIoEvent));
            InitTypeDict();
        }

        private void InitTypeDict()
        {
            EventTypeDict = new Dictionary<string, EnumInfo>();

            EventTypeDict.Add("DiskIoRead", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoRead.ToString()));
            EventTypeDict.Add("DiskIoWrite", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoWrite.ToString()));
            EventTypeDict.Add("DiskIoReadInit", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoRead.ToString()));
            EventTypeDict.Add("DiskIoWriteInit", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoWrite.ToString()));
            EventTypeDict.Add("DiskIoFlushInit", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoFlush.ToString()));
            EventTypeDict.Add("DiskIoFlushBuffers", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoFlush.ToString()));
            EventTypeDict.Add("DiskIoDriverMajorFunctionCall", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoNonSignificant.ToString()));
            EventTypeDict.Add("DiskIoDriverMajorFunctionReturn", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoNonSignificant.ToString()));
            EventTypeDict.Add("DiskIoDriverCompletionRoutine", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoNonSignificant.ToString()));
            EventTypeDict.Add("DiskIoDriverCompleteRequest", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoNonSignificant.ToString()));
            EventTypeDict.Add("DiskIoDriverCompleteRequestReturn", new EnumInfo(typeof(SignificantDiskIoEvent), SignificantDiskIoEvent.DiskIoNonSignificant.ToString()));

            EventTypeDict.Add("RegistryCreate", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegCreate.ToString()));
            EventTypeDict.Add("RegistryOpen", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegOpen.ToString()));
            EventTypeDict.Add("RegistryDelete", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegModify.ToString()));
            EventTypeDict.Add("RegistryQuery", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegRead.ToString()));
            EventTypeDict.Add("RegistrySetValue", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegModify.ToString()));
            EventTypeDict.Add("RegistryDeleteValue", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegModify.ToString()));
            EventTypeDict.Add("RegistryQueryValue", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegRead.ToString()));
            EventTypeDict.Add("RegistryEnumerateKey", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegEnumerate.ToString()));
            EventTypeDict.Add("RegistryEnumerateValueKey", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegEnumerate.ToString()));
            EventTypeDict.Add("RegistryQueryMultipleValue", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegRead.ToString()));
            EventTypeDict.Add("RegistrySetInformation", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegModify.ToString()));
            EventTypeDict.Add("RegistryFlush", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegModify.ToString()));
            EventTypeDict.Add("RegistryRunDown", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegRead.ToString()));
            EventTypeDict.Add("RegistryKCBCreate", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegCreate.ToString()));
            EventTypeDict.Add("RegistryKCBDelete", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegModify.ToString()));
            EventTypeDict.Add("RegistryKCBRundownBegin", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegRead.ToString()));
            EventTypeDict.Add("RegistryKCBRundownEnd", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegRead.ToString()));
            EventTypeDict.Add("RegistryVirtualize", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegNonSignificant.ToString()));
            EventTypeDict.Add("RegistryClose", new EnumInfo(typeof(SignificantRegistryEvent), SignificantRegistryEvent.RegNonSignificant.ToString()));

            EventTypeDict.Add("FileIoName", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoNonSignificant.ToString()));
            EventTypeDict.Add("FileIoFileCreate", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoCreate.ToString()));
            EventTypeDict.Add("FileIoFileDelete", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoDelete.ToString()));
            EventTypeDict.Add("FileIoFileRundown", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoRead.ToString()));
            EventTypeDict.Add("FileIoCreate", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoCreate.ToString()));
            EventTypeDict.Add("FileIoCleanup", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoDelete.ToString()));
            EventTypeDict.Add("FileIoClose", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoWrite.ToString()));
            EventTypeDict.Add("FileIoFlush", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoWrite.ToString()));
            EventTypeDict.Add("FileIoRead", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoRead.ToString()));
            EventTypeDict.Add("FileIoWrite", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoWrite.ToString()));
            EventTypeDict.Add("FileIoSetInfo", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoWrite.ToString()));
            EventTypeDict.Add("FileIoDelete", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoDelete.ToString()));
            EventTypeDict.Add("FileIoRename", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoDelete.ToString()));
            EventTypeDict.Add("FileIoQueryInfo", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoDelete.ToString()));
            EventTypeDict.Add("FileIoFSControl", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoWrite.ToString()));
            EventTypeDict.Add("FileIoDirEnum", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoRead.ToString()));
            EventTypeDict.Add("FileIoDirNotify", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoRead.ToString()));
            EventTypeDict.Add("FileIoOperationEnd", new EnumInfo(typeof(SignificantFileIoEvent), SignificantFileIoEvent.FileIoNonSignificant.ToString()));

            EventTypeDict.Add("TcpIpSend", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetSend.ToString()));
            EventTypeDict.Add("TcpIpRecv", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetRecv.ToString()));
            EventTypeDict.Add("TcpIpConnect", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpDisconnect", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetDisconnect.ToString()));
            EventTypeDict.Add("TcpIpRetransmit", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetSend.ToString()));
            EventTypeDict.Add("TcpIpAccept", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpReconnect", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpFail", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetNonSignificant.ToString()));
            EventTypeDict.Add("TcpIpTCPCopy", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetNonSignificant.ToString()));
            EventTypeDict.Add("TcpIpARPCopy", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetNonSignificant.ToString()));
            EventTypeDict.Add("TcpIpFullACK", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpPartACK", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpDupACK", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpSendIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetSend.ToString()));
            EventTypeDict.Add("TcpIpRecvIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetRecv.ToString()));
            EventTypeDict.Add("TcpIpDisconnectIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetDisconnect.ToString()));
            EventTypeDict.Add("TcpIpRetransmitIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetSend.ToString()));
            EventTypeDict.Add("TcpIpReconnectIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpTCPCopyIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetNonSignificant.ToString()));
            EventTypeDict.Add("TcpIpConnectIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("TcpIpAcceptIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetConnect.ToString()));
            EventTypeDict.Add("UdpIpSend", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetSend.ToString()));
            EventTypeDict.Add("UdpIpRecv", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetRecv.ToString()));
            EventTypeDict.Add("UdpIpFail", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetNonSignificant.ToString()));
            EventTypeDict.Add("UdpIpSendIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetSend.ToString()));
            EventTypeDict.Add("UdpIpRecvIPV6", new EnumInfo(typeof(SignificantNetworkEvent), SignificantNetworkEvent.NetRecv.ToString()));

            EventTypeDict.Add("PerfInfoSampleProf", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoSampleProf.ToString()));
            EventTypeDict.Add("PerfInfoBatchedSampleProf", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoSampleProf.ToString()));
            EventTypeDict.Add("PerfInfoSetInterval", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoSetInterval.ToString()));
            EventTypeDict.Add("PerfInfoCollectionStart", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoCollection.ToString()));
            EventTypeDict.Add("PerfInfoCollectionEnd", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoCollection.ToString()));
            EventTypeDict.Add("PerfInfoSysClEnter", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoSysCl.ToString()));
            EventTypeDict.Add("PerfInfoSysClExit", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoSysCl.ToString()));
            EventTypeDict.Add("PerfInfoISR", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoISR.ToString()));
            EventTypeDict.Add("PerfInfoThreadedDPC", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoDPC.ToString()));
            EventTypeDict.Add("PerfInfoDPC", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoDPC.ToString()));
            EventTypeDict.Add("PerfInfoTimerDPC", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoDPC.ToString()));
            EventTypeDict.Add("PerfInfoDebuggerEnabled", new EnumInfo(typeof(SignificantProfileEvent), SignificantProfileEvent.PerfInfoNonSignificant.ToString()));
        }

        public string ToSignificant(string eventName)
        {
            try
            {
                EnumInfo enumInfo = EventTypeDict[eventName];
                return Enum.Parse(enumInfo.EnumType, enumInfo.Name).ToString();
            }
            catch
            {
                return null;
            }
        }

        public Type ToSignificantType(string eventName)
        {
            try
            {
                return EventTypeDict[eventName].EnumType;
            }
            catch
            {
                return null;
            }
        }
    }
}
