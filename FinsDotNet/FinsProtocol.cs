using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;
 

namespace FinsDotNet
{
    public static class FinsProtocol
    {
        public enum MemoryArea
        {
            CIO_Bit = 0x30,
            W_Bit = 0x31,
            H_Bit = 0x32,
            A_Bit = 0x33,
            D_Bit = 0x02,
            CIO_Word = 0xB0,
            W_Word = 0xB1,
            H_Word = 0xB2,
            A_Word = 0xB3,
            D_Word = 0x82
        }

        public enum ErrorCodes
        {
            errSocketCreation = 0x00000001,
            errConnectionTimeout = 0x00000002,
            errConnectionFailed = 0x00000003,
            errReceiveTimeout = 0x00000004,
            errDataReceive = 0x00000005,
            errSendTimeout = 0x00000006,
            errDataSend = 0x00000007,
            errConnectionReset = 0x00000008,
            errNotConnected = 0x00000009,
            errUnreachableHost = 0x0000000a,

            //fins execution error codes
            finsErrUndefined = 0x00010000,
            finsErrLocalNode = 0x00010001,
            finsErrDestNode = 0x00010002,
            finsErrCommController = 0x00010003,
            finsErrNotExec = 0x00010004,
            finsErrRouting = 0x00010005,
            finsErrCmdFormat = 0x00010006,
            finsErrParam = 0x00010007,
            finsErrCannotRead = 0x00010008,
            finsErrCannotWrite = 0x00010009
        }

        #region "HELPER GET METHODS"
        /// <summary>
        /// Returns an Int from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static Int16 GetIntAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            Int16 outVal = 0;
            byte[] tmpByte = new Byte[2];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 2)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];

            outVal = BitConverter.ToInt16(tmpByte, 0);

            return outVal;
        }

        /// <summary>
        /// Returns an UInt from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static UInt16 GetUIntAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            UInt16 outVal = 0;
            byte[] tmpByte = new Byte[2];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 2)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];

            outVal = BitConverter.ToUInt16(tmpByte, 0);

            return outVal;
        }

        /// <summary>
        /// Returns a DInt from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static Int32 GetDintAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            Int32 outVal = 0;
            byte[] tmpByte = new Byte[4];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 4)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];
            tmpByte[2] = buffer[startByte + 3];
            tmpByte[3] = buffer[startByte + 2];

            outVal = BitConverter.ToInt32(tmpByte, 0);

            return outVal;
        }

        /// <summary>
        /// Returns an UDint from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static UInt32 GetUDintAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            UInt32 outVal = 0;
            byte[] tmpByte = new Byte[4];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 4)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];
            tmpByte[2] = buffer[startByte + 3];
            tmpByte[3] = buffer[startByte + 2];

            outVal = BitConverter.ToUInt32(tmpByte, 0);

            return outVal;
        }

        /// <summary>
        /// Returns a Real from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static Single GetRealAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            Single outVal = 0f;
            byte[] tmpByte = new Byte[4];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 4)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];
            tmpByte[2] = buffer[startByte + 3];
            tmpByte[3] = buffer[startByte + 2];

            outVal = BitConverter.ToSingle(tmpByte, 0);

            return outVal;
        }

        /// <summary>
        /// Returns a LReal from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static Double GetLRealAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            Double outVal = 0f;
            byte[] tmpByte = new Byte[8];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 8)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];
            tmpByte[2] = buffer[startByte + 3];
            tmpByte[3] = buffer[startByte + 2];
            tmpByte[4] = buffer[startByte + 5];
            tmpByte[5] = buffer[startByte + 4];
            tmpByte[6] = buffer[startByte + 7];
            tmpByte[7] = buffer[startByte + 6];

            outVal = BitConverter.ToDouble(tmpByte, 0);

            return outVal;
        }

        /// <summary>
        /// Returns a LInt from Omron PLC
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static Int64 GetLIntAt(byte[] buffer, int start) {
            int startByte = start * 2;//WORD offset
            Int64 outVal = 0;
            byte[] tmpByte = new Byte[8];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 8)) return outVal;

            tmpByte[0] = buffer[startByte + 1];
            tmpByte[1] = buffer[startByte + 0];
            tmpByte[2] = buffer[startByte + 3];
            tmpByte[3] = buffer[startByte + 2];
            tmpByte[4] = buffer[startByte + 5];
            tmpByte[5] = buffer[startByte + 4];
            tmpByte[6] = buffer[startByte + 7];
            tmpByte[7] = buffer[startByte + 6];

            outVal = BitConverter.ToInt64(tmpByte, 0);

            return outVal;
        }
        #endregion

        #region "HELPER SET METHODS"
        /// <summary>
        /// Sets an Omron PLC Int  
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetIntAt(ref byte[] buffer, int start, Int16 value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[2];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 2)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1] = tmpByte[0];
            buffer[startByte + 0] = tmpByte[1];
        }

        /// <summary>
        /// Sets an Omron PLC UInt
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetUIntAt(ref byte[] buffer, int start, UInt16 value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[2];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 2)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1] = tmpByte[0];
            buffer[startByte + 0] = tmpByte[1];
        }

        /// <summary>
        /// Sets an Omron PLC DInt
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetDintAt(ref byte[] buffer, int start, Int32 value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[4];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 4)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1]= tmpByte[0];
            buffer[startByte + 0]= tmpByte[1];
            buffer[startByte + 3]= tmpByte[2];
            buffer[startByte + 2]= tmpByte[3];
        }

        /// <summary>
        /// Sets an Omron PLC UDint
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetUDintAt(ref byte[] buffer, int start, UInt32 value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[4];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 4)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1] = tmpByte[0];
            buffer[startByte + 0] = tmpByte[1];
            buffer[startByte + 3] = tmpByte[2];
            buffer[startByte + 2] = tmpByte[3];
        }

        /// <summary>
        /// Sets an Omron PLC Real 
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetRealAt(ref byte[] buffer, int start, Single value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[4];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 4)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1] = tmpByte[0];
            buffer[startByte + 0] = tmpByte[1];
            buffer[startByte + 3] = tmpByte[2];
            buffer[startByte + 2] = tmpByte[3];
        }

        /// <summary>
        /// Sets an Omron PLC LReal
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetLRealAt(ref byte[] buffer, int start, Double value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[8];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 8)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1]= tmpByte[0];
            buffer[startByte + 0]= tmpByte[1];
            buffer[startByte + 3]= tmpByte[2];
            buffer[startByte + 2]= tmpByte[3];
            buffer[startByte + 5]= tmpByte[4];
            buffer[startByte + 4]= tmpByte[5];
            buffer[startByte + 7]= tmpByte[6];
            buffer[startByte + 6]= tmpByte[7];
        }

        /// <summary>
        /// Sets an Omron PLC LInt
        /// </summary>
        /// <param name="buffer">The byte buffer of data.</param>
        /// <param name="start">The start offset for the selected variable. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <returns></returns>
        public static void SetLIntAt(byte[] buffer, int start, Int64 value) {
            int startByte = start * 2;//WORD offset
            byte[] tmpByte = new Byte[8];

            if ((buffer.Length == 0) || (buffer.Length < startByte + 8)) return;

            tmpByte = BitConverter.GetBytes(value);

            buffer[startByte + 1] = tmpByte[0];
            buffer[startByte + 0] = tmpByte[1];
            buffer[startByte + 3] = tmpByte[2];
            buffer[startByte + 2] = tmpByte[3];
            buffer[startByte + 5] = tmpByte[4];
            buffer[startByte + 4] = tmpByte[5];
            buffer[startByte + 7] = tmpByte[6];
            buffer[startByte + 6] = tmpByte[7];
        }
        #endregion
    }

    public class FinsClient
    {
        #region "PRIVATE FINS CLIENT PROPERTIES"
        private byte destNetAddr = 0;
        private byte destNodeNum = 0;
        private byte destUnitNum = 0;
        private byte srcNetAddr = 0;
        private byte srcNodeNum = 0;
        private byte srcUnitNum = 0;
        #endregion

        #region "SOCKET MANAGEMENT VARIABLES"
        // IP address
        IPAddress ipAddress;
        IPEndPoint remoteEP;

        // Create a socket.  
        Socket finsClient;

        // The port number for the remote device, default is 9600.  
        private const int port = 9600;

        // Clients count
        public static int finsTCPClientCount = 0;

        // Client id
        private int finsTCPClientId = 0;

        // Size of receive buffer.  
        private int ReceiveBufferSize = 0;

        #endregion

        #region "ERROR HANDLING VARABLES"
        private int currentStatus = 0;
        #endregion

        #region "PUBLIC FINS CLIENT PROPERTIES"
        public byte DestinationNetworkAddress { get => destNetAddr; set => destNetAddr = value; }
        public byte DestinationNodeNumber { get => destNodeNum; set => destNodeNum = value; }
        public byte DestinationUnitNumber { get => destUnitNum; set => destUnitNum = value; }
        public byte SourceNetworkAddress { get => srcNetAddr; set => srcNetAddr = value; }
        public byte SourceNodeNumber { get => srcNodeNum; set => srcNodeNum = value; }
        public byte SourceUnitNumber { get => srcUnitNum; set => srcUnitNum = value; }
        #endregion

        public struct CommandDataStructure
        {
            public byte ICF;//INFORMATION CONTROL FIELD, SET TO 0x80
            public byte RSV;//RESERVED, SET TO 0x00
            public byte GCT;//GATEWAY COUNT, SET TO 0x02
            public byte DNA;//DESTINATION NETWORK, 0x01 IF THERE ARE NOT NETWORK INTERMEDIARIES
            public byte DA1;//DESTINATION NODE NUMBER, IF SET TO DEFAULT THIS IS THE SUBNET BYTE OF THE IP OF THE PLC (EX. 192.168.0.1 -> 0x01)
            public byte DA2;//DESTINATION UNIT NUMBER, THE UNIT NUMBER, SEE THE HW CONIFG OF PLC, GENERALLY 0x00
            public byte SNA;//SOURCE NETWORK, GENERALLY 0x01 
            public byte SA1;//SOURCE NODE NUMBER, LIKE THE DESTINATION NODE NUMBER, YOU COULD SET A FIXED NUMBER INTO PLC CONFIG
            public byte SA2;//SOURCE UNIT NUMBER, LIKE THE DESTINATION UNIT NUMBER
            public byte SID;//COUNTER FOR THE RESEND, GENERALLY 0x00
            public byte MR;
            public byte SR;
            public byte[] text;
            public byte[] ToByteArray() {
                byte[] b = new Byte[12 + text.Length];
                b[0] = ICF;
                b[1] = RSV;
                b[2] = GCT;
                b[3] = DNA;
                b[4] = DA1;
                b[5] = DA2;
                b[6] = SNA;
                b[7] = SA1;
                b[8] = SA2;
                b[9] = SID;
                b[10] = MR;
                b[11] = SR;
                for (int i = 0; i < text.Length; i++) {
                    b[12 + i] = text[i];
                }
                return b;
            }
        }

        public struct ResponseDataStructure
        {
            public byte ICF;
            public byte RSV;
            public byte GCT;
            public byte DNA;
            public byte DA1;
            public byte DA2;
            public byte SNA;
            public byte SA1;
            public byte SA2;
            public byte SID;
            public byte MR;
            public byte SR;
            public byte rspCodeH;
            public byte rspCodeL;
            public byte[] text;
            public void Compile(byte[] data) {
                text = new byte[data.Length - 14];
                ICF = data[0];
                RSV = data[1];
                GCT = data[2];
                DNA = data[3];
                DA1 = data[4];
                DA2 = data[5];
                SNA = data[6];
                SA1 = data[7];
                SA2 = data[8];
                SID = data[9];
                MR = data[10];
                SR = data[11];
                rspCodeH = data[12];
                rspCodeL = data[13];
                for (int i = 0; i < data.Length - 14; i++) {
                    text[i] = data[14 + i];
                }
            }
        }

        #region "CONSTRUCTORS"
        /// <summary>
        /// Instantiates a new Fins Client
        /// </summary>
        public FinsClient() {
            finsTCPClientId = finsTCPClientCount++;//assign id and so increments the clients count

            //destination properties
            DestinationNetworkAddress = 0x01;   //DNA
            DestinationNodeNumber = 0x01;       //DA1
            DestinationUnitNumber = 0x00;       //DA2

            //source properties
            SourceNetworkAddress = 0x01;   //SNA
            SourceNodeNumber = 0x01;   //SA1
            SourceUnitNumber = 0x01;   //SA2
        }

        /// <summary>
        /// Instantiates a new Fins Client
        /// </summary>
        /// <param name="DNA">Destination Network, look at the PLC configuration</param>
        /// <param name="DA1">Destination Node Number, if set to default this is the last IP byte (subnet) of the plc. <para /> Example: 192.168.0.1 -> 0x01 <para /> You can set a fixed number into PLC configuration</param>
        /// <param name="DA2">Destination Unit Number, the unit number, see the hw conifg of plc, generally 0x00.</param>
        /// <param name="SNA">Source Network, if in the same network of PLC, it is like DNA.</param>
        /// <param name="SA1">Source Node Number, like the destination node number. <para /> You can set a fixed number into PLC configuration</param>
        /// <param name="SA2">Source Unit Number, like the destination unit number.</param>
        public FinsClient(byte DNA, byte DA1, byte DA2, byte SNA, byte SA1, byte SA2) : this() {
            //destination properties
            DestinationNetworkAddress = DNA;   //DNA
            DestinationNodeNumber = DA1;       //DA1
            DestinationUnitNumber = DA2;       //DA2

            //source properties
            SourceNetworkAddress = SNA;        //SNA
            SourceNodeNumber = SA1;            //SA1
            SourceUnitNumber = SA2;            //SA2
        }

        #endregion

        #region "FINS PROTOCOL PUBLIC METHODS"
        //PING THE REMOTE HOST
        public bool PingPlc(string address)
        {
            Ping p = new Ping();
            PingReply r = p.Send(address);

            if (r.Status == IPStatus.Success) return true;
           
            return false;
        }

        //CONNECT UDP: synchronously connects to a UDP/IP endpoint via socket
        public int ConnectUdp(string address) {
            currentStatus = 0;

            //first try to ping the local endpoint
            if (!PingPlc(address)) return (int)FinsProtocol.ErrorCodes.errUnreachableHost;

            //try to create a socket client
            try {
                ipAddress = IPAddress.Parse(address);
                remoteEP = new IPEndPoint(ipAddress, port);

                finsClient = new Socket(ipAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                currentStatus = (int)FinsProtocol.ErrorCodes.errSocketCreation;
                return currentStatus;
            }

            //then try to connect to host
            try
            {
                finsClient.Connect(remoteEP);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                currentStatus = (int)FinsProtocol.ErrorCodes.errConnectionFailed;
            }

            return currentStatus;
        }

        //DISCONNECT: shutdown and close the socket connection
        public int Disconnect() {
            currentStatus = 0;
            try {
                finsClient.Shutdown(SocketShutdown.Both);
                finsClient.Close();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return currentStatus;
        }

        /// <summary>
        /// Write an Omron PLC area: the area must be defined as CJ like area
        /// </summary>
        /// <param name="area">The area type: use the MemoryArea struct in the FinsProtocol static class.</param>
        /// <param name="start">The start offset for the write process. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <param name="size">The size of the area to write. <para /> IMPORTANT: The size is expressed in WORD (2 byte)</param>
        /// <param name="buffer">The byte array buffer which will be write in the PLC.</param>
        /// <returns>The status integer</returns>
        public int WriteArea(FinsProtocol.MemoryArea area, int start, int size, byte[] buffer) {
            currentStatus = 0;
            ReceiveBufferSize = 14 + size * 2;
            byte[] returnBuffer;
            byte[] text = new Byte[6 + buffer.Length];

            //memory area code
            text[0] = (byte)area;

            //beginning address
            UInt16 startAddr = (UInt16)start;
            text[1] = (byte)(startAddr >> 8);
            text[2] = (byte)startAddr;
            text[3] = 0x00;

            //number of items
            UInt16 readSize = (UInt16)size;
            text[4] = (byte)(readSize >> 8);
            text[5] = (byte)readSize;

            for (int i = 0; i < buffer.Length; i++) {
                text[6 + i] = buffer[i];
            }

            //sending the fins command and storing the response
            currentStatus = SendFinsCommand(finsClient, 0x01, 0x02, text, out returnBuffer);

            return currentStatus;
        }

        /// <summary>
        /// Read an Omron PLC area: the area must be defined as CJ like area
        /// </summary>
        /// <param name="area">The area type: use the MemoryArea struct in the FinsProtocol static class.</param>
        /// <param name="start">The start offset for the read process. <para /> IMPORTANT: The offset has the value of a WORD (2 byte)</param>
        /// <param name="size">The size of the area to read. <para /> IMPORTANT: The size is expressed in WORD (2 byte)</param>
        /// <param name="buffer">The byte array buffer in which will be store the PLC readed area.</param>
        /// <returns></returns>
        public int ReadArea(FinsProtocol.MemoryArea area, int start, int size, out byte[] buffer) {
            currentStatus = 0;
            ReceiveBufferSize = 14 + size * 2;
            byte[] text = new Byte[6];

            //memory area code
            text[0] = (byte)area;
            
            //beginning address
            UInt16 startAddr = (UInt16)start;
            text[1] = (byte)(startAddr >> 8);
            text[2] = (byte)startAddr;
            text[3] = 0x00;

            //number of items
            UInt16 readSize = (UInt16)size;
            text[4] = (byte)(readSize >> 8);
            text[5] = (byte)readSize;

            //sending the fins command and storing the response
            byte[] tmpBuf;
            buffer = new byte[readSize * 2];
            currentStatus = SendFinsCommand(finsClient, 0x01, 0x01, text, out tmpBuf);

            for (int i = 0; i < buffer.Length; i++) {
                buffer[i] = tmpBuf[i];
            }

            return currentStatus;
        }
        #endregion

        private int SendFinsCommand(Socket client, byte MR, byte SR, byte[] comText, out byte[] rspText) {
            // FINS COMMAND FRAME

            // Frame building
            CommandDataStructure commandFrame = new CommandDataStructure();
            commandFrame.ICF = 0x80;
            commandFrame.RSV = 0x00;
            commandFrame.GCT = 0x02;
            commandFrame.DNA = DestinationNetworkAddress;
            commandFrame.DA1 = DestinationNodeNumber;
            commandFrame.DA2 = DestinationUnitNumber;
            commandFrame.SNA = SourceNetworkAddress;
            commandFrame.SA1 = SourceNodeNumber;
            commandFrame.SA2 = SourceUnitNumber;
            commandFrame.SID = 0x00;
            commandFrame.MR = MR;
            commandFrame.SR = SR;
            commandFrame.text = comText;

            // Frame send via UDP
            byte[] sendFrame = commandFrame.ToByteArray();
            UdpSend(client, sendFrame);
            if (currentStatus != 0) {
                rspText = new Byte[0];
                return (int)FinsProtocol.ErrorCodes.errDataSend;
            }

            // FINS RESPONSE FRAME

            // Frame Receive via UDP
            byte[] receiveFrame = new Byte[ReceiveBufferSize];
            UdpReceive(client, ref receiveFrame);
            if (currentStatus != 0) {
                rspText = new Byte[0];
                return (int)FinsProtocol.ErrorCodes.errDataReceive;
            }

            //Frame deconstruct
            ResponseDataStructure responseFrame = new ResponseDataStructure();
            responseFrame.Compile(receiveFrame);

            //response execution code
            if (responseFrame.rspCodeH != 0) {
                switch (responseFrame.rspCodeH) {
                    case 1:
                        currentStatus = (int)FinsProtocol.ErrorCodes.finsErrLocalNode;
                        break;
                    case 2:
                        currentStatus = (int)FinsProtocol.ErrorCodes.finsErrDestNode;
                        break;
                    default:
                        currentStatus = (int)FinsProtocol.ErrorCodes.finsErrUndefined;
                        break;
                }
            }

            // RESPONSE BYTE ARRAY OUTPUT
            rspText = responseFrame.text;

            return currentStatus;
        }

        #region "SYNC UDP SOCKET COMMUNICATION METHODS"
        //SYNC UDP SEND
        private void UdpSend(Socket client, byte[] data) {
            try {
                client.Send(data);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                currentStatus = (int)FinsProtocol.ErrorCodes.errDataSend;
            }
        }

        //SYNC UDP RECEIVE
        private void UdpReceive(Socket client, ref byte[] data) {
            Stopwatch t = new Stopwatch();
            t.Start();

            try {
                int byteRead;
                byteRead = client.Receive(data);

                if (t.ElapsedMilliseconds > 5000) {
                    t.Stop();
                    t = null;

                    currentStatus = (int)FinsProtocol.ErrorCodes.errReceiveTimeout;
                    return;
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                currentStatus = (int)FinsProtocol.ErrorCodes.errDataReceive;
            }
        }
        #endregion 
    }
}
