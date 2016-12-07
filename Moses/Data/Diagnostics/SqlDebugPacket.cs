#if TRACE

namespace Moses.Data.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    public struct SqlDebugPacket
    {
        private DateTime _packetSendTime;

        public DateTime PacketSendTime
        {
            get
            {
                return _packetSendTime;
            }

            set
            {
                _packetSendTime = value;
            }
        }

        private string _sqlCommandText;

        public string SqlCommandText
        {
            get { return _sqlCommandText; }
            set { _sqlCommandText = value; }
        }

    }
}

#endif
