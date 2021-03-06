﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UDPMessenger.Commands;
using UDPMessenger.Events;
using UDPMessenger.Packets;
using UDPMessenger.Utils;

namespace UDPMessenger
{
    public class Application
    {
        private static Application _app;

        public static Application Instance => _app;

        public static void Start(params String[] args)
        {
            if (_app == null)
            {
                _app = new Application();
                _app.Init();
                _app.Loop();
            }
            else
            {
                throw new InvalidOperationException("既にインスタンスが作成されています。");
            }
        }

        public UDPManager NetworkManager { get; private set; }
        public PacketHandler Handler { get; private set; }
        public RsaKeys Key { get; internal set; }
        public string UserName { get; set; }

        public Dictionary<string, Session> Sessions { get; } = new Dictionary<string, Session>();

        private readonly Dictionary<int, Packet> _packets = new Dictionary<int, Packet>();
        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();

        private void Init()
        {
            Console.CancelKeyPress += (e, a) => Disconnect();

            Handler = new PacketHandler();
            Key = EncryptionManager.GenerateKeys();

            RegisterPackets();
            RegisterCommands();

            NetworkManager = new UDPManager();
            NetworkManager.Start();
            NetworkManager.ReceiveEvent += NetworkManager_ReceiveEvent;
        }

        private void Loop()
        {
            UserName = InteractiveManager.InteractiveMessage("名前を入力してください。");
            Console.WriteLine("コマンドを入力してください。(helpで確認出来ます。)");

            while (true)
            {
                string cmd = InteractiveManager.InteractiveMessage();
                if (string.IsNullOrWhiteSpace(cmd))
                {
                    continue;
                }

                string[] args = cmd.Split(' ');
                if (_commands.ContainsKey(args[0]))
                {
                    try
                    {
                        ExecuteResult result = _commands[args[0]]
                            .ExecuteCommand(args[0], args.Skip(1).ToArray());
                        if (result == ExecuteResult.Failed)
                        {
                            Console.WriteLine("{0} コマンドの実行に失敗しました。", args[0]);
                            Console.WriteLine();
                        }
                        else if (result == ExecuteResult.Error)
                        {
                            Console.WriteLine("{0} コマンドの実行中にエラーが発生しました。", args[0]);
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("コマンド実行中に例外が発生しました。");
                        Console.WriteLine(e.ToString());
                        Console.WriteLine();
                    }
                }
                else
                {
                    if (args[0] == "exit")
                    {
                        Disconnect();
                        break;
                    }

                    Console.WriteLine("{0} というコマンドはありません。", args[0]);
                    Console.WriteLine();
                }
            }

            Environment.Exit(0);
        }

        private void RegisterCommands()
        {
            RegisterCommand(new HelpCommand());
            RegisterCommand(new ConnectionCommand());
            RegisterCommand(new ChatCommand());
            RegisterCommand(new ScriptCommand());
            RegisterCommand(new ConfigCommand());
        }

        public void RegisterCommand(Command cmd)
        {
            _commands.Add(cmd.Name, cmd);
        }

        private void RegisterPackets()
        {
            _packets.Add(0x01, new ConnectionPacket());
            _packets.Add(0x03, new ChatPacket());

            _packets.Add(0x10, new DisconnectPacket());
        }

        public Command GetCommand(string name)
        {
            return _commands[name];
        }

        public Command[] GetCommands()
        {
            return _commands.Values.ToArray();
        }

        private Packet GetPacket(byte id)
        {
            if (_packets.ContainsKey(id))
            {
                return (Packet) _packets[id].Clone();
            }

            return null;
        }

        private Packet GetPacket(byte id, byte[] buffer)
        {
            if (_packets.ContainsKey(id))
            {
                Packet packet = (Packet) _packets[id].Clone();
                packet.SetBuffer(buffer);
                return packet;
            }

            return null;
        }

        private void NetworkManager_ReceiveEvent(object sender, PacketReceiveEventArgs e)
        {
            BinaryStream stream = new BinaryStream(e.Buffer);
            bool isEncrypt = stream.ReadBool();
            if (isEncrypt)
            {
                stream = new BinaryStream(EncryptionManager.Decrypt(stream.ReadBytes(), Key.PrivateKey));
            }

            byte[] magic = stream.ReadBytes(12);
            if (StructuralComparisons.StructuralEqualityComparer.Equals(magic, Packet.Magic))
            {
                byte id = stream.ReadByte();
                stream.Offset--;

                Packet packet = GetPacket(id, stream.ReadBytes());
                packet.Decode();

                Handler.HandlePacket(e.EndPoint, packet);
                packet.Dispose();
            }
        }

        public void SendPacket(IPEndPoint endPoint, Packet packet, bool isEncrypt = false)
        {
            packet.Encode();

            BinaryStream stream = new BinaryStream();
            stream.WriteBool(isEncrypt);
            if (isEncrypt)
            {
                stream.WriteBytes(EncryptionManager.Encrypt(packet.ToArray(), GetSession(endPoint).PublicKey));
                this.NetworkManager.Send(endPoint, stream.ToArray());
                stream.Dispose();
                packet.Dispose();
            }
            else
            {
                stream.WriteBytes(packet.ToArray());
                this.NetworkManager.Send(endPoint, stream.ToArray());
                stream.Dispose();
                packet.Dispose();
            }
        }

        public void AddSession(IPEndPoint endPoint, Session session)
        {
            this.Sessions[endPoint.ToString()] = session;
        }

        public void RemoveSession(IPEndPoint endPoint)
        {
            this.Sessions.Remove(endPoint.ToString());
        }

        public Session GetSession(IPEndPoint endPoint)
        {
            if (this.Sessions.ContainsKey(endPoint.ToString()))
            {
                return this.Sessions[endPoint.ToString()];
            }

            return null;
        }

        public Session GetSession(string name)
        {
            return this.Sessions.FirstOrDefault((session => session.Value.Name == name)).Value;
        }

        public void Disconnect()
        {
            DisconnectPacket packet = new DisconnectPacket();
            packet.Reason = "Exit Application";
            foreach (Session session in Sessions.Values)
            {
                SendPacket(session.EndPoint, packet);
            }

            Sessions.Clear();
        }
    }
}
