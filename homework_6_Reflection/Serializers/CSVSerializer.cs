using System.Reflection;
using System.Text;

namespace homework_6_Reflection.Serializers
{
    public static class CSVSerializer
    {
        private static readonly BindingFlags _flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static class TypeCache<T>
        {
            public static readonly PropertyInfo[] Properties = typeof(T).GetProperties(_flags);
        }

        public static string Serialize<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Obj can not be null");

            StringBuilder serialized = new();

            var type = typeof(T);

            var properties = TypeCache<T>.Properties;

            serialized.Append(string.Join(',', properties.Select(p => p.GetValue(obj))));

            return serialized.ToString();
        }

        public static string SerializeNotOptimized<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Obj can not be null");

            StringBuilder serialized = new();

            var type = typeof(T);

            PropertyInfo[] properties = type.GetProperties(_flags);

            serialized.Append(string.Join(',', properties.Select(p => p.GetValue(obj))));

            return serialized.ToString();
        }

        public static T Deserialize<T>(string csvRow) where T : class
        {
            if (csvRow == null)
                throw new ArgumentNullException(nameof(csvRow), "Row can not be null");

            var values = csvRow.Split(',');

            var type = typeof(T);

            var properties = TypeCache<T>.Properties;

            var obj = (T?)Activator.CreateInstance(type);

            for (int i = 0; i < values.Length; i++)
            {
                var convertedValue = Convert.ChangeType(values[i], properties[i].PropertyType);
                properties[i].SetValue(obj, convertedValue);
            }

            return obj ?? throw new OperationCanceledException($"Can not to create new instance of {typeof(T)}");
        }
    }
}
