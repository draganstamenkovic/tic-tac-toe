using System;

namespace Model
{
    [Serializable]
    public class IpApiResponse
    {
        public Timezone timezone;
    }

    [Serializable]
    public class Timezone
    {
        public string id;
    }
}