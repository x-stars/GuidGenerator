// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// This minimal API is modified to not get the interface properties
// for much better performance when only getting the MAC address of the interface.

#pragma warning disable
#nullable enable

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

// SystemNetworkInterface.cs
namespace System.Net.NetworkInformation
{
    internal sealed class SystemNetworkInterface : NetworkInterface
    {
        private readonly string _name;
        private readonly string _id;
        private readonly string _description;
        private readonly byte[] _physicalAddress;
        private readonly NetworkInterfaceType _type;
        private readonly OperationalStatus _operStatus;
        private readonly long _speed;

        // Any interface can have two completely different valid indexes for ipv4 and ipv6.
        private readonly uint _index;
        private readonly uint _ipv6Index;
        private readonly Interop.IpHlpApi.AdapterFlags _adapterFlags;
        //private readonly SystemIPInterfaceProperties _interfaceProperties;

        public static new unsafe NetworkInterface[] GetAllNetworkInterfaces()
        {
            AddressFamily family = AddressFamily.Unspecified;
            uint bufferSize = 0;

            List<SystemNetworkInterface> interfaceList = new List<SystemNetworkInterface>();

            Interop.IpHlpApi.GetAdaptersAddressesFlags flags =
                Interop.IpHlpApi.GetAdaptersAddressesFlags.IncludeGateways
                | Interop.IpHlpApi.GetAdaptersAddressesFlags.IncludeWins;

            // Figure out the right buffer size for the adapter information.
            uint result = Interop.IpHlpApi.GetAdaptersAddresses(
                family, (uint)flags, (nint)0, (nint)0, &bufferSize);

            while (result == Interop.IpHlpApi.ERROR_BUFFER_OVERFLOW)
            {

                // Allocate the buffer and get the adapter info.
                nint buffer = Marshal.AllocHGlobal((int)bufferSize);
                try
                {
                    result = Interop.IpHlpApi.GetAdaptersAddresses(
                        family, (uint)flags, (nint)0, buffer, &bufferSize);

                    // If succeeded, we're going to add each new interface.
                    if (result == Interop.IpHlpApi.ERROR_SUCCESS)
                    {
                        // Linked list of interfaces.
                        Interop.IpHlpApi.IpAdapterAddresses* adapterAddresses = (Interop.IpHlpApi.IpAdapterAddresses*)buffer;
                        while (adapterAddresses != null)
                        {
                            // Traverse the list, marshal in the native structures, and create new NetworkInterfaces.
                            interfaceList.Add(new SystemNetworkInterface(in *adapterAddresses));
                            adapterAddresses = adapterAddresses->next;
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }

            // If we don't have any interfaces detected, return empty.
            if (result == Interop.IpHlpApi.ERROR_NO_DATA || result == Interop.IpHlpApi.ERROR_INVALID_PARAMETER)
            {
                return Array.Empty<SystemNetworkInterface>();
            }

            // Otherwise we throw on an error.
            if (result != Interop.IpHlpApi.ERROR_SUCCESS)
            {
                throw new NetworkInformationException((int)result);
            }

            return interfaceList.ToArray();
        }

        internal SystemNetworkInterface(in Interop.IpHlpApi.IpAdapterAddresses ipAdapterAddresses)
        {
            // Store the common API information.
            _id = ipAdapterAddresses.AdapterName;
            _name = ipAdapterAddresses.FriendlyName;
            _description = ipAdapterAddresses.Description;
            _index = ipAdapterAddresses.index;

            _physicalAddress = ipAdapterAddresses.Address;

            _type = ipAdapterAddresses.type;
            _operStatus = ipAdapterAddresses.operStatus;
            _speed = unchecked((long)ipAdapterAddresses.receiveLinkSpeed);

            // API specific info.
            _ipv6Index = ipAdapterAddresses.ipv6Index;

            _adapterFlags = ipAdapterAddresses.flags;
            //_interfaceProperties = new SystemIPInterfaceProperties(ipAdapterAddresses); // Veeeery slow...
        }

        public override string Id => _id;

        public override string Name => _name;

        public override string Description => _description;

        public override PhysicalAddress GetPhysicalAddress()
        {
            return new PhysicalAddress(_physicalAddress);
        }

        public override NetworkInterfaceType NetworkInterfaceType => _type;

        // We cache this to be consistent across all platforms.
        public override OperationalStatus OperationalStatus => _operStatus;

        public override long Speed => _speed;

        public override bool IsReceiveOnly =>
            ((_adapterFlags & Interop.IpHlpApi.AdapterFlags.ReceiveOnly) > 0);

        /// <summary>The interface doesn't allow multicast.</summary>
        public override bool SupportsMulticast =>
            ((_adapterFlags & Interop.IpHlpApi.AdapterFlags.NoMulticast) == 0);
    }
}

// IpHlpApi/Interop.ErrorCodes.cs
namespace Interop
{
    internal static partial class IpHlpApi
    {
        public const uint ERROR_SUCCESS = 0;
        public const uint ERROR_INVALID_FUNCTION = 1;
        public const uint ERROR_NO_SUCH_DEVICE = 2;
        public const uint ERROR_INVALID_DATA = 13;
        public const uint ERROR_INVALID_PARAMETER = 87;
        public const uint ERROR_BUFFER_OVERFLOW = 111;
        public const uint ERROR_INSUFFICIENT_BUFFER = 122;
        public const uint ERROR_NO_DATA = 232;
        public const uint ERROR_IO_PENDING = 997;
        public const uint ERROR_NOT_FOUND = 1168;
    }
}

// IpHlpApi/Interop.NetworkInformation.cs
namespace Interop
{
    internal static partial class IpHlpApi
    {
        [Flags]
        internal enum AdapterFlags
        {
            DnsEnabled = 0x01,
            RegisterAdapterSuffix = 0x02,
            DhcpEnabled = 0x04,
            ReceiveOnly = 0x08,
            NoMulticast = 0x10,
            Ipv6OtherStatefulConfig = 0x20,
            NetBiosOverTcp = 0x40,
            IPv4Enabled = 0x80,
            IPv6Enabled = 0x100,
            IPv6ManagedAddressConfigurationSupported = 0x200,
        }

        [Flags]
        internal enum AdapterAddressFlags
        {
            DnsEligible = 0x1,
            Transient = 0x2
        }

        [Flags]
        internal enum GetAdaptersAddressesFlags
        {
            SkipUnicast = 0x0001,
            SkipAnycast = 0x0002,
            SkipMulticast = 0x0004,
            SkipDnsServer = 0x0008,
            IncludePrefix = 0x0010,
            SkipFriendlyName = 0x0020,
            IncludeWins = 0x0040,
            IncludeGateways = 0x0080,
            IncludeAllInterfaces = 0x0100,
            IncludeAllCompartments = 0x0200,
            IncludeTunnelBindingOrder = 0x0400,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct IpSocketAddress
        {
            internal nint address;
            internal int addressLength;
        }

        // IP_ADAPTER_ANYCAST_ADDRESS
        // IP_ADAPTER_MULTICAST_ADDRESS
        // IP_ADAPTER_DNS_SERVER_ADDRESS
        // IP_ADAPTER_WINS_SERVER_ADDRESS
        // IP_ADAPTER_GATEWAY_ADDRESS
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct IpAdapterAddress
        {
            internal uint length;
            internal AdapterAddressFlags flags;
            internal IpAdapterAddress* next;
            internal IpSocketAddress address;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct IpAdapterUnicastAddress
        {
            internal uint length;
            internal AdapterAddressFlags flags;
            internal IpAdapterUnicastAddress* next;
            internal IpSocketAddress address;
            internal PrefixOrigin prefixOrigin;
            internal SuffixOrigin suffixOrigin;
            internal DuplicateAddressDetectionState dadState;
            internal uint validLifetime;
            internal uint preferredLifetime;
            internal uint leaseLifetime;
            internal byte prefixLength;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct IpAdapterAddresses
        {
            internal const int MAX_ADAPTER_ADDRESS_LENGTH = 8;

            internal uint length;
            internal uint index;
            internal IpAdapterAddresses* next;

            private nint _adapterName; // ANSI string
            internal string AdapterName => Marshal.PtrToStringAnsi(_adapterName)!;

            internal nint firstUnicastAddress;
            internal nint firstAnycastAddress;
            internal nint firstMulticastAddress;
            internal nint firstDnsServerAddress;

            private nint _dnsSuffix;
            internal string DnsSuffix => Marshal.PtrToStringUni(_dnsSuffix)!;

            private nint _description;
            internal string Description => Marshal.PtrToStringUni(_description)!;

            private nint _friendlyName;
            internal string FriendlyName => Marshal.PtrToStringUni(_friendlyName)!;

            private fixed byte _address[MAX_ADAPTER_ADDRESS_LENGTH];
            private uint _addressLength;
            internal byte[] Address
            {
                get
                {
#if NETCOREAPP3_0_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    return MemoryMarshal.CreateReadOnlySpan<byte>(ref _address[0], (int)_addressLength).ToArray();
#else
                    if (_addressLength == 0)
                    {
                        return Array.Empty<byte>();
                    }

                    byte[] result = new byte[(int)_addressLength];
                    fixed (byte* pAddress = &_address[0], pResult = &result[0])
                    {
                        Buffer.MemoryCopy(pAddress, pResult, (int)_addressLength, (int)_addressLength);
                    }
                    return result;
#endif
                }
            }

            internal AdapterFlags flags;
            internal uint mtu;
            internal NetworkInterfaceType type;
            internal OperationalStatus operStatus;
            internal uint ipv6Index;

            private fixed uint _zoneIndices[16];
            internal uint[] ZoneIndices
            {
                get
                {
#if NETCOREAPP3_0_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    return MemoryMarshal.CreateReadOnlySpan<uint>(ref _zoneIndices[0], 16).ToArray();
#else
                    uint[] result = new uint[16];
                    fixed (uint* pZoneIndices = &_zoneIndices[0], pResult = &result[0])
                    {
                        Buffer.MemoryCopy(pZoneIndices, pResult, 16 * sizeof(uint), 16 * sizeof(uint));
                    }
                    return result;
#endif
                }
            }

            internal nint firstPrefix;

            internal ulong transmitLinkSpeed;
            internal ulong receiveLinkSpeed;
            internal nint firstWinsServerAddress;
            internal nint firstGatewayAddress;
            internal uint ipv4Metric;
            internal uint ipv6Metric;
            internal ulong luid;
            internal IpSocketAddress dhcpv4Server;
            internal uint compartmentId;
            internal fixed byte networkGuid[16];
            internal InterfaceConnectionType connectionType;
            internal InterfaceTunnelType tunnelType;
            internal IpSocketAddress dhcpv6Server; // Never available in Windows.
            internal fixed byte dhcpv6ClientDuid[130];
            internal uint dhcpv6ClientDuidLength;
            internal uint dhcpV6Iaid;

            /* Windows 2008 +
                  PIP_ADAPTER_DNS_SUFFIX             FirstDnsSuffix;
             * */
        }

        internal enum InterfaceConnectionType : int
        {
            Dedicated = 1,
            Passive = 2,
            Demand = 3,
            Maximum = 4,
        }

        internal enum InterfaceTunnelType : int
        {
            None = 0,
            Other = 1,
            Direct = 2,
            SixToFour = 11,
            Isatap = 13,
            Teredo = 14,
            IpHttps = 15,
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        internal static unsafe extern uint GetAdaptersAddresses(
            AddressFamily family, uint flags, nint pReserved, nint adapterAddresses, uint* outBufLen);
    }
}
