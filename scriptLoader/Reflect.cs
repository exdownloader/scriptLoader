using System.Reflection;

namespace scriptLoader
{
    public static class Reflect
    {
        private const BindingFlags _bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        public static FieldInfo FetchField(object o, string f, BindingFlags bf = _bf)
        {
            return o.GetType().GetField(f, bf);
        }

        public static T FetchValue<T>(object o, string f, BindingFlags bf = _bf)
        {
            return (T)o.GetType().GetField(f, bf).GetValue(o);
        }
    }
}
