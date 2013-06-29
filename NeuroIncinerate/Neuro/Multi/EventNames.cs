using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro.Multi
{
    public enum EventTraceEvent
    {
        EventTraceHeader = 0,
        EventTraceExtension,
        EventTraceEndExtension,
        EventTraceRundownComplete
    }
    public enum ProcessEvent
    {
        ProcessStart = 0,
        ProcessStartGroup,
        ProcessEnd,
        ProcessEndGroup,
        ProcessDCStart,
        ProcessDCEnd,
        ProcessDefunct,
        ProcessPerfCtr,
        ProcessPerfCtrRundown
    }
    public enum ThreadEvent
    {
        ThreadStart = 0,
        ThreadStartGroup,
        ThreadEnd,
        ThreadEndGroup,
        ThreadDCStart,
        ThreadDCEnd,
        ThreadCSwitch,
        ThreadCompCS,
        ThreadWorkerThread,
        ThreadReserveCreate,
        ThreadReserveDelete,
        ThreadReserveJoinThread,
        ThreadReserveDisjoinThread,
        ThreadReserveState,
        ThreadReserveBandwidth,
        ThreadReserveLateCount,
    }
    public enum DiskIoEvent
    {
        DiskIoRead = 0,
        DiskIoWrite,
        DiskIoReadInit,
        DiskIoWriteInit,
        DiskIoFlushInit,
        DiskIoFlushBuffers,
        DiskIoDriverMajorFunctionCall,
        DiskIoDriverMajorFunctionReturn,
        DiskIoDriverCompletionRoutine,
        DiskIoDriverCompleteRequest,
        DiskIoDriverCompleteRequestReturn,
    }
    public enum RegistryEvent
    {
        RegistryCreate = 0,
        RegistryOpen,
        RegistryDelete,
        RegistryQuery,
        RegistrySetValue,
        RegistryDeleteValue,
        RegistryQueryValue,
        RegistryEnumerateKey,
        RegistryEnumerateValueKey,
        RegistryQueryMultipleValue,
        RegistrySetInformation,
        RegistryFlush,
        RegistryRunDown,
        RegistryKCBCreate,
        RegistryKCBDelete,
        RegistryKCBRundownBegin,
        RegistryKCBRundownEnd,
        RegistryVirtualize,
        RegistryClose
    }
    public enum FileIoEvent
    {
        FileIoName = 0,
        FileIoFileCreate,
        FileIoFileDelete,
        FileIoFileRundown,
        FileIoCreate,
        FileIoCleanup,
        FileIoClose,
        FileIoFlush,
        FileIoRead,
        FileIoWrite,
        FileIoSetInfo,
        FileIoDelete,
        FileIoRename,
        FileIoQueryInfo,
        FileIoFSControl,
        FileIoDirEnum,
        FileIoDirNotify,
        FileIoOperationEnd,
    }
    public enum TcpUdpEvent
    {
        TcpIpSend = 0,
        TcpIpRecv,
        TcpIpConnect,
        TcpIpDisconnect,
        TcpIpRetransmit,
        TcpIpAccept,
        TcpIpReconnect,
        TcpIpFail,
        TcpIpTCPCopy,
        TcpIpARPCopy,
        TcpIpFullACK,
        TcpIpPartACK,
        TcpIpDupACK,
        TcpIpSendIPV6,
        TcpIpRecvIPV6,
        TcpIpDisconnectIPV6,
        TcpIpRetransmitIPV6,
        TcpIpReconnectIPV6,
        TcpIpTCPCopyIPV6,
        TcpIpConnectIPV6,
        TcpIpAcceptIPV6,
        UdpIpSend,
        UdpIpRecv,
        UdpIpFail,
        UdpIpSendIPV6,
        UdpIpRecvIPV6,
    }
    public enum ImageEvent
    {
        ImageLoad = 0,
        ImageLoadGroup,
        ImageUnload,
        ImageUnloadGroup,
        ImageDCStart,
        ImageDCEnd,
    }
    public enum PageFaultEvent
    {
        PageFaultTransitionFault = 0,
        PageFaultDemandZeroFault,
        PageFaultCopyOnWrite,
        PageFaultGuardPageFault,
        PageFaultHardPageFault,
        PageFaultAccessViolation,
        PageFaultHardFault,
        PageFaultHeapRangeRundown,
        PageFaultHeapRangeCreate,
        PageFaultHeapRangeReserve,
        PageFaultHeapRangeRelease,
        PageFaultHeapRangeDestroy,
        PageFaultImageLoadBacked,
    }
    public enum PerfInfoEvent
    {
        PerfInfoSampleProf = 0,
        PerfInfoBatchedSampleProf,
        PerfInfoSetInterval,
        PerfInfoCollectionStart,
        PerfInfoCollectionEnd,
        PerfInfoSysClEnter,
        PerfInfoSysClExit,
        PerfInfoISR,
        PerfInfoThreadedDPC,
        PerfInfoDPC,
        PerfInfoTimerDPC,
        PerfInfoDebuggerEnabled,
    }
    public enum ALPCEvent
    {
        ALPCSendMessage = 0,
        ALPCReceiveMessage,
        ALPCWaitForReply,
        ALPCWaitForNewMessage,
        ALPCUnwait,
    }
    public enum SystemConfigEvent
    {
        SystemConfigCPU = 0,
        SystemConfigPhyDisk,
        SystemConfigLogDisk,
        SystemConfigNIC,
        SystemConfigVideo,
        SystemConfigServices,
        SystemConfigPower,
        SystemConfigIRQ,
        SystemConfigPnP,
        SystemConfigNetwork,
        SystemConfigIDEChannel,
    }
    public enum OtherEvent
    {
        SplitIoVolMgr = 0,
        StackWalk,
        LostEvent,
        VirtualAlloc,
        VirtualFree,
        ReadyThread,
        Mark
    }
}
