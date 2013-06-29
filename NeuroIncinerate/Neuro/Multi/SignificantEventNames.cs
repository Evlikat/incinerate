using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro.Multi
{
    public enum SignificantNetworkEvent : int
    {
        NetSend = 0,
        NetRecv,
        NetConnect,
        NetDisconnect,
        NetNonSignificant
    }

    public enum SignificantDiskIoEvent : int
    {
        DiskIoRead = 0,
        DiskIoWrite,
        DiskIoNonSignificant
    }

    public enum SignificantRegistryEvent : int
    {
        RegCreate = 0,
        RegRead,
        RegModify,
        RegNonSignificant
    }

    public enum SignificantFileIoEvent : int
    {
        FileIoRead = 0,
        FileIoWrite,
        FileIoCreate,
        FileIoDelete,
        FileIoNonSignificant
    }
}
