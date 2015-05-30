using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playerlog.Utils
{
    class Registry
    {
        public static RegistryKey Key_mode = RegistryKey.New(@"HKEY_CURRENT_USER\SOFTWARE\Altis Life\Playerlog", "Mode");
        public static RegistryKey Key_firstuse = RegistryKey.New(@"HKEY_CURRENT_USER\SOFTWARE\Altis Life\Playerlog", "Firstuse");

        public static void SetValue(RegistryKey key, string value)
        {
            Microsoft.Win32.Registry.SetValue(key.reg_key, key.reg_value, value, Microsoft.Win32.RegistryValueKind.String);
        }
        public static string GetValue(RegistryKey key)
        {
            return (string)Microsoft.Win32.Registry.GetValue(key.reg_key, key.reg_value, null);
        }
    }

    public struct RegistryKey
    {
        public string reg_key { get; set; }
        public string reg_value { get; set; }
        public static RegistryKey New(string key, string value)
        {
            return new RegistryKey() {reg_key = key, reg_value = value};
        }
    }


}
