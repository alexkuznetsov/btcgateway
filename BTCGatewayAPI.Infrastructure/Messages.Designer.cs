﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BTCGatewayAPI.Infrastructure {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BTCGatewayAPI.Infrastructure.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ссылка: .
        /// </summary>
        internal static string LoggerHelpLnkMsg {
            get {
                return ResourceManager.GetString("LoggerHelpLnkMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Внутреннее исключение: .
        /// </summary>
        internal static string LoggerInnerExcMsg {
            get {
                return ResourceManager.GetString("LoggerInnerExcMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Источник: .
        /// </summary>
        internal static string LoggerSourceMsg {
            get {
                return ResourceManager.GetString("LoggerSourceMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Трассировка: .
        /// </summary>
        internal static string LoggerStackTrcMsg {
            get {
                return ResourceManager.GetString("LoggerStackTrcMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TargetSite: .
        /// </summary>
        internal static string LoggerTargetSiteMsg {
            get {
                return ResourceManager.GetString("LoggerTargetSiteMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Необработанное исключение. Uri: {0}, метод: {1}. Исключение: {2}.
        /// </summary>
        internal static string UnhandledMessageStringTpl {
            get {
                return ResourceManager.GetString("UnhandledMessageStringTpl", resourceCulture);
            }
        }
    }
}