namespace SmartMangement.Domain.Utilities
{
    public class SequentialGuid
    {
        static Guid _CurrentGuid = Guid.NewGuid();
        public Guid CurrentGuid
        {
            get
            {
                return _CurrentGuid;
            }
        }

        public override string ToString()
        {
            return _CurrentGuid.ToString("N").ToUpper();
        }
        public static string NewGuid()
        {
            byte[] bytes = _CurrentGuid.ToByteArray();
            for(int mapIndex = 0; mapIndex < 16; mapIndex++)
            {
                int bytesIndex = sqlOrderMap[mapIndex];
                bytes[bytesIndex]++;
                if (bytes[bytesIndex] != 0)
                {
                    break;
                }
            }
            _CurrentGuid = new Guid(bytes);
            return _CurrentGuid.ToString("N").ToUpper();
        }

        private static int[] _sslOrderMap = null;
        private static int[] sqlOrderMap

        {
            get {  _sslOrderMap ??= new int[16]
            {
                3,2,1,0,5,4,7,6,9,8,15,14,13,12,11,10
            };
                return _sslOrderMap;
            }
        }
    }
}
