﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tridion.Extensions.ECL.FotoWare.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tridion.Extensions.ECL.FotoWare.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-16&quot;?&gt;
        ///&lt;Configuration xmlns=&quot;http://www.sdltridion.com/ExternalContentLibrary/Configuration&quot;&gt;
        ///  &lt;l:Logging Level=&quot;Warning&quot; xmlns:l=&quot;http://www.sdltridion.com/Infrastructure/LoggingConfiguration&quot;&gt;
        ///    &lt;l:Folder&gt;C:\Tridion\Log&lt;/l:Folder&gt;
        ///  &lt;/l:Logging&gt;
        ///  &lt;CoreServiceUrl&gt;net.tcp://localhost:2660/CoreService/2011/netTcp&lt;/CoreServiceUrl&gt;
        ///  &lt;MountPoints&gt;
        ///    &lt;MountPoint type=&quot;FotoWareProvider&quot; id=&quot;fwx&quot; rootItemName=&quot;FotoWare&quot;&gt;
        ///      &lt;StubFolders&gt;
        ///        &lt;StubFolder id=&quot;t [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExternalContentLibrary_FotoWare {
            get {
                return ResourceManager.GetString("ExternalContentLibrary_FotoWare", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap logo_fotoweb_48x48 {
            get {
                object obj = ResourceManager.GetObject("logo_fotoweb_48x48", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}