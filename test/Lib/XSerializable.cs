using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Lib
{
    public class XSerializable<T> : IXunitSerializable
    {
        public T Object { get; private set; }

        public XSerializable()
        {
        }

        public XSerializable(T objectToSerialize)
        {
            Object = objectToSerialize;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Object = JsonConvert.DeserializeObject<T>(info.GetValue<string>("objValue"));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            string json = JsonConvert.SerializeObject(Object);
            info.AddValue("objValue", json);
        }
    }
}
