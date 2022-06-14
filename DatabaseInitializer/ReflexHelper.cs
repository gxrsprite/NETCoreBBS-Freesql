using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class ReflexHelper
{
    public static List<Type> GetEntityTypes(Type interf)
    {
        Assembly ass = Assembly.GetAssembly(interf);

        List<Type> list = new List<Type>();
        Type[] types = ass.GetTypes();
        foreach (Type item in types)
        {
            if (item.IsInterface) continue;//判断是否是接口
            Type[] ins = item.GetInterfaces();
            foreach (Type ty in ins)
            {
                if (ty == interf)
                {
                    list.Add(item);
                }
            }
        }
        return list;

    }
}
