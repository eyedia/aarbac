﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eyedia.Aarbac.Framework {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Eyedia.Aarbac.Framework.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to thispasswordtobechnagedtoexact32.
        /// </summary>
        internal static string PassKey {
            get {
                return ResourceManager.GetString("PassKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select 
        ///       rc.constraint_name,         
        ///       rcu.table_name &apos;ReferencingTable&apos;, 
        ///       rcu.column_name &apos;ReferencingColumn&apos;,
        ///       rcu1.table_name &apos;ReferencedTable&apos;,
        ///       rcu1.column_name &apos;ReferencedColumn&apos;
        ///    from
        ///       information_schema.referential_constraints rc
        ///    inner join information_schema.constraint_column_usage rcu 
        ///		on rc.constraint_catalog = rcu.constraint_catalog 
        ///        and rc.constraint_name = rcu.constraint_name
        ///    inner join information_schema.constraint_column_us [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Query_ForeignKeys {
            get {
                return ResourceManager.GetString("Query_ForeignKeys", resourceCulture);
            }
        }
    }
}