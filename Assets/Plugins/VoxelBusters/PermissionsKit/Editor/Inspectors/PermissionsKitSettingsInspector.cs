using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;
using VoxelBusters.PermissionsKit.Core.Simulator;

namespace VoxelBusters.PermissionsKit.Editor
{
    [CustomEditor(typeof(PermissionsKitSettings))]
    public class PermissionsKitSettingsInspector : SettingsObjectInspector
    {
        #region Fields
        
        private     ButtonMeta[]                m_resourceButtons;

        #endregion

        #region Base class methods

        protected override void OnEnable()
        {
            // Set properties
            m_resourceButtons   = new ButtonMeta[]
            {
                new ButtonMeta(label: "Documentation",  onClick: PermissionsKitEditorUtility.OpenDocumentation),
                new ButtonMeta(label: "Tutorials",      onClick: PermissionsKitEditorUtility.OpenTutorials),
                new ButtonMeta(label: "Forum",          onClick: PermissionsKitEditorUtility.OpenForum),
                new ButtonMeta(label: "Discord",        onClick: PermissionsKitEditorUtility.OpenSupport),
                new ButtonMeta(label: "Write Review",	onClick: () => PermissionsKitEditorUtility.OpenAssetStorePage()),
            };

            base.OnEnable();
        }

        protected override UnityPackageDefinition GetOwner()
        {
            return PermissionsKitSettings.Package;
        }

        protected override string[] GetTabNames()
        {
            return new string[]
            {
                DefaultTabs.kGeneral,
                DefaultTabs.kMisc,
            };
        }

        protected override EditorSectionInfo[] GetSectionsForTab(string tab)
        {
            return null;
        }

        protected override bool DrawTabView(string tab)
        {
            switch (tab)
            {
                case DefaultTabs.kGeneral:
                    DrawGeneralTabView();
                    return true;

                case DefaultTabs.kMisc:
                    DrawMiscTabView();
                    return true;

                default:
                    return false;
            }
        }

        protected override void DrawFooter(string tab)
        {
            base.DrawFooter(tab);
        }

        #endregion

        #region Section methods

        private void DrawGeneralTabView()
        {

            EditorGUILayout.BeginVertical(GroupBackgroundStyle);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_logLevel"));
            EditorGUILayout.EndVertical();
            
            var     permissionSection   = new EditorSectionInfo(displayName: "Permissions Configuration",
                property: serializedObject.FindProperty("m_permissionsConfiguration"),
                drawStyle: EditorSectionDrawStyle.Expand,
                drawDetailsCallback: DrawPermissionsConfigurationSection);
            
            LayoutBuilder.DrawSection(permissionSection,
                                      showDetails: true,
                                      selectable: false);
        }
        private void DrawPermissionsConfigurationSection(EditorSectionInfo section)
        {
            EditorLayoutBuilder.DrawChildProperties(property: section.Property,
                ignoreProperties: section.IgnoreProperties);
            
            GUILayout.BeginVertical();
            if (GUILayout.Button("Reset Simulator"))
            {
                PermissionsKitInterface.Reset();
            }
            
            //GUILayout.Box("List here enabled permissions!");
            
            GUILayout.EndVertical();
        }

        private void DrawMiscTabView()
        {
            DrawButtonList(m_resourceButtons);
        }

        #endregion
    }
}