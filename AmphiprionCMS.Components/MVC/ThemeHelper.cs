using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace AmphiprionCMS.Helpers
{
    public class ThemeHelper
    {
        public ThemeHelper(ViewContext viewContext,
          IViewDataContainer viewDataContainer)
            : this(viewContext, viewDataContainer, RouteTable.Routes)
        {
        }

        public ThemeHelper(ViewContext viewContext,
           IViewDataContainer viewDataContainer, RouteCollection routeCollection)
        {
            ViewContext = viewContext;
            ViewData = new ViewDataDictionary(viewDataContainer.ViewData);
        }

        public ViewDataDictionary ViewData
        {
            get;
            private set;
        }

        public ViewContext ViewContext
        {
            get;
            private set;
        }

        public  string Name
        {
            get
            {
               return Theme.Name;
            }
        }
        public  string BasePath
        {
            get
            {
                return Theme.BasePath;
            }
        }
    }

    public static class Theme
    {
        public static string Name
        {
            get
            {
                return WebConfigurationManager.AppSettings["defaultThemeName"];
            }
        }
        public static string BasePath
        {
            get
            {
                return VirtualPathUtility.ToAbsolute("~/themes/" + Name);
            }
        }
    }

    public class ThemeConfiguration
    {
        private static readonly object _lock = new object();
        private static readonly object _initLock = new object();

        private static ThemeConfiguration _config = null;
        private bool _isInitialized = false;
        private bool _isInitializing = false;
        private ThemeConfiguration()
        {
            _isInitialized = false;
            Initialize();
        }

        public static ThemeConfiguration Instance()
        {
            lock (_lock)
            {
                if(_config == null)
                    _config = new ThemeConfiguration();

                return _config;
            }
        }

        private void Initialize()
        {
            if (!_isInitialized && !_isInitializing)
            {
                _isInitializing = true;
            }
        }
    }
}