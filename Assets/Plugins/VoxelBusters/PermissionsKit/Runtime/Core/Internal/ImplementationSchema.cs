using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.PermissionsKit
{
    public static class AssemblyName
    {
        public  const   string      kMain                   = "VoxelBusters.PermissionsKit";
        
        public  const   string      kIos                    = "VoxelBusters.PermissionsKit.iOSModule";
        
        public  const   string      kAndroid                = "VoxelBusters.PermissionsKit.AndroidModule";
        
        public  const   string      kSimulator              = "VoxelBusters.PermissionsKit.SimulatorModule";
    }

    public static class NamespaceName
    {
        public  const   string      kRoot                   = "VoxelBusters.PermissionsKit.Core";
    }

    internal static class ImplementationSchema
    {
        #region Static fields

        private static readonly NativeFeatureRuntimeConfiguration    s_configuration;

        #endregion

        #region Static properties
        
        public static NativeFeatureRuntimeConfiguration Default 
        { 
            get => GetRuntimeConfiguration();
        }

        
        #endregion

        #region Constructors

        static ImplementationSchema()
        {
            s_configuration = new NativeFeatureRuntimeConfiguration(packages: new NativeFeatureRuntimePackage[]
                {
                    NativeFeatureRuntimePackage.iOS(assembly: AssemblyName.kIos,
                        ns: $"{NamespaceName.kRoot}.iOS",
                        nativeInterfaceType: "PermissionsKitInterface",
                        bindingTypes: new string[]
                        {
                            "PermissionsKitBinding"
                        }),
                    NativeFeatureRuntimePackage.Android(assembly: AssemblyName.kAndroid,
                        ns: $"{NamespaceName.kRoot}.Android",
                        nativeInterfaceType: "PermissionsKitInterface"),
                },
                simulatorPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kSimulator,
                    ns: $"{NamespaceName.kRoot}.Simulator",
                    nativeInterfaceType: "PermissionsKitInterface"),
                fallbackPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kMain,
                    ns: NamespaceName.kRoot,
                    nativeInterfaceType: "NullPermissionsKitInterface"));
        }
            
        #endregion

        #region Public static methods

        public static NativeFeatureRuntimeConfiguration GetRuntimeConfiguration()
        {
            return s_configuration;
        }

        #endregion
    }
}