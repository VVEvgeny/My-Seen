using System;
using System.Reflection;

namespace MySeenWeb.Add_Code
{
    public class VveAuth
    {
        private bool _loaded;
        private bool _loadError;

        private Assembly _assembly;
        private Type _type;
        private object _instance;

        private string _path;

        public VveAuth(string path)
        {
            _path = path;
        }

        private bool Load()
        {
            if (_loadError) return false;
            if (_loaded) return true;

            try
            {
                _assembly = Assembly.LoadFile(_path + "\\VVEAuths.dll");
                _type = _assembly.GetType("VVEAuths");
                _instance = Activator.CreateInstance(_type);
            }
            catch
            {
                _loadError = true;
                return false;
            }
            _loaded = true;
            return true;
        }

        public string Splitter
        {
            get { return ";;;"; }
        }

        public string GetId(string appName, string serviceName)
        {
            return GetId(appName, serviceName, Splitter);
        }

        public string GetId(string appName, string serviceName, string splitter)
        {
            var str = Get(appName, serviceName, splitter);
            if (str != string.Empty)
            {
                if (str.Contains(splitter))
                {
                    return str.Substring(0, str.IndexOf(splitter, StringComparison.Ordinal));
                }
                return str;
            }
            return str;
        }

        public string GetSecret(string appName, string serviceName)
        {
            return GetSecret(appName, serviceName, Splitter);
        }

        public string GetSecret(string appName, string serviceName, string splitter)
        {
            var str = Get(appName, serviceName, splitter);
            if (str != string.Empty)
            {
                if (str.Contains(splitter))
                {
                    str = str.Remove(0, str.IndexOf(splitter, StringComparison.Ordinal) + splitter.Length);
                    if (str.Contains(splitter))
                    {
                        return str.Substring(0, str.IndexOf(splitter, StringComparison.Ordinal));
                    }
                    return str;
                }
                return string.Empty;
            }
            return str;
        }

        public string GetOptions(string appName, string serviceName)
        {
            return GetOptions(appName, serviceName, Splitter);
        }

        public string GetOptions(string appName, string serviceName, string splitter)
        {
            var str = Get(appName, serviceName, splitter);
            if (str != string.Empty)
            {
                if (str.Contains(splitter))
                {
                    str = str.Remove(0, str.IndexOf(splitter, StringComparison.Ordinal) + splitter.Length);

                    if (str.Contains(splitter))
                    {
                        str = str.Remove(0, str.IndexOf(splitter, StringComparison.Ordinal) + splitter.Length);

                        if (str.Contains(splitter))
                        {
                            return str.Substring(0, str.IndexOf(splitter, StringComparison.Ordinal));
                        }
                        return str;
                    }
                    return string.Empty;
                }
                return string.Empty;
            }
            return str;
        }

        public string Get(string appName, string serviceName)
        {
            return Get(appName, serviceName, Splitter);
        }

        private string Get(string appName, string serviceName, string splitter)
        {
            if (Load())
            {
                try
                {
                    return
                        _type.InvokeMember("Get",
                            BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                            null, _instance, new object[] {appName, serviceName, splitter}).ToString();
                }
                catch
                {
                    _loadError = true;
                }
            }
            return "Not loaded " + (_loadError ? "_loadError" : "") + (_loaded ? "_loaded" : "");
        }

        public bool Have(string appName, string serviceName)
        {
            if (Get(appName, serviceName) == string.Empty
                ||
                _loadError
                ||
                !_loaded)
            {
                return false;
            }
            return true;
        }

        ~VveAuth()
        {
            _assembly = null;
            _type = null;
            _instance = null;
        }
    }

    public class Auths
    {
        private static VveAuth _vveAuth;
        private static string _appName;

        public bool Have(string name)
        {
            return _vveAuth.Have(_appName, name);
        }

        public string Id(string name)
        {
            return _vveAuth.GetId(_appName, name);
        }

        public string Secret(string name)
        {
            return _vveAuth.GetSecret(_appName, name);
        }

        public string Options(string name)
        {
            return _vveAuth.GetOptions(_appName, name);
        }

        public Auths(string appName,string path)
        {
            _vveAuth = new VveAuth(path);
            _appName = appName;
        }
    }
}