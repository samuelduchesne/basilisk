﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Basilisk.Controls.Attributes;

namespace Basilisk.Controls.InterfaceModels
{
    public class LibraryComponentSet : LibraryComponent
    {
        private readonly Lazy<IReadOnlyCollection<SimulationSetting>> settings;

        internal LibraryComponentSet(IEnumerable<LibraryComponent> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }
            else if (!components.Any())
            {
                throw new ArgumentException("A library component set must contain at least one component", nameof(components));
            }
            Components = components;
            settings = new Lazy<IReadOnlyCollection<SimulationSetting>>(CreateSimulationSettings);
        }

        public IEnumerable<LibraryComponent> Components { get; }

        public override string Category
        {
            get { return "(multiple categories)"; }
            set { throw new NotSupportedException(); }
        }

        public override string Comments
        {
            get { return "(multiple comments)"; }
            set { throw new NotSupportedException(); }
        }

        public override string DataSource
        {
            get { return "(multiple sources)"; }
            set { throw new NotSupportedException(); }
        }

        public override bool IsCategoryNameMutable => false;
        public override bool IsNameMutable => false;

        public override string Name
        {
            get { return "(multiple names)"; }
            set { throw new NotSupportedException(); }
        }

        public override bool DirectlyReferences(LibraryComponent component) =>
            Components.Any(c => c.DirectlyReferences(component));

        public override LibraryComponent Duplicate() =>
            new LibraryComponentSet(Components.Select(c => c.Duplicate()));

        public override IEnumerable<SimulationSetting> SimulationSettings =>
            settings.Value;

        private IReadOnlyCollection<SimulationSetting> CreateSimulationSettings()
        {
            System.Diagnostics.Debug.Assert(Components.Any());
            var sourceTypes = Components.Select(c => c.GetType()).Distinct().ToArray();
            if (sourceTypes.Length > 1)
            {
                throw new InvalidOperationException("Simulation settings cannot be generated for library component sets with components of multiple types");
            }
            var typeOrderer = SimulationSettingsCreator.HierarchyComparer.Build(sourceTypes[0]);
            return
                sourceTypes[0]
                .GetProperties()
                .OrderBy(prop => prop.DeclaringType, typeOrderer)
                .Select(prop => new { Prop = prop, Att = prop.GetCustomAttribute<SimulationSettingAttribute>() })
                .Where(x => x.Att != null)
                .Select(x =>
                {
                    var displayName = x.Att.DisplayName == null ? x.Prop.Name : x.Att.DisplayName;
                    var setting = new SimulationSetting(this, x.Prop, displayName);
                    setting.PropertyChanged += (s, e) => RaisePropertyChanged(setting.PropertyName);
                    return setting;
                })
                .ToArray();
        }
    }
}